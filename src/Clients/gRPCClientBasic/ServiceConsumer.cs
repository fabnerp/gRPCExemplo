using Grpc.Core;
using Grpc.Net.Client;
using gRPCProtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static gRPCProtos.Person;


namespace gRPCClientBasic
{
    public class ServiceConsumer : IServiceConsumer
    {
        private readonly IConfiguration _configuration;
        private GrpcChannel _channel;
        private PersonClient _client;

        public ServiceConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            var urlChannel = _configuration.GetSection("HTTPSConfig").Get<HTTPSConfig>().Url;
            _channel = GrpcChannel.ForAddress(urlChannel);
            _client = new PersonClient(_channel);
        }

        public void GetUserById(int id)
        {
            var response = _client.GetPersonById(
                new PersonRequest
                {
                    BusinessEntityID = id
                });

            Console.WriteLine($"Person result: Id:{response.BusinessEntityID}, FirstName:{response.FirstName}, LastName:{response.LastName}");
        }

        public async Task GetUserByIdAsync(int id)
        {
            var response = await _client.GetPersonByIdAsync(
                 new PersonRequest
                 {
                     BusinessEntityID = id
                 });

            Console.WriteLine($"Person result: Id:{response.BusinessEntityID}, FirstName:{response.FirstName}, LastName:{response.LastName}");
        }

        public async Task GetPeopleByLastNameStreamResponseAsync(string lastName)
        {
            try
            {
                var callOptions = new CallOptions().WithCancellationToken(CancellationToken.None);
                using var resp = _client.GetPeopleLastNameStreamResponseAsync(new PeopleLastNameResquest { LastName = "bai" }, callOptions);

                try
                {
                    await foreach (var item in resp.ResponseStream.ReadAllAsync())
                    {
                        Console.WriteLine($"Person result: Id:{item.BusinessEntityID}, FirstName:{item.FirstName}, LastName:{item.LastName}");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetPeopleByLastNameBiDirect(string lastName)
        {
            try
            {
                //preparing respose async
                using var response = _client.GetPeopleByLastNameAsync();

                var respProc = Task.Run(async () =>
                {
                    try
                    {
                        await foreach (var item in response.ResponseStream.ReadAllAsync())
                        {
                            Console.WriteLine($"Person result: Id:{item.BusinessEntityID}, FirstName:{item.FirstName}, LastName:{item.LastName}");
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });

                //preparing request

                foreach (var item in new[] { "bai", "lee" })
                {
                    await response.RequestStream.WriteAsync(
                        new PeopleByLastNameRequestStream
                        {
                            LastName = item
                        });
                }

                await response.RequestStream.CompleteAsync();

                await respProc;


            }
            catch (Exception)
            {

                throw;
            }

        }



        public void CloseClient()
        {
            _client = null;
            _channel.ShutdownAsync();
        }
    }
}
