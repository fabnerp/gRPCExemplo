using Grpc.Net.Client;
using gRPCProtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public async Task GetPeople()
        {
            using var stream = _client.GetPersonByLastNameAsync();

            //input requests
            await stream.RequestStream.WriteAsync(new PersonByLastNameRequestStream { LastName = "lee" });
            await stream.RequestStream.WriteAsync(new PersonByLastNameRequestStream { LastName = "gar" });
            await stream.RequestStream.WriteAsync(new PersonByLastNameRequestStream { LastName = "bai" });

            var response = Task.Run(async () =>
            {
                while (await stream.ResponseStream.MoveNext(cancellationToken: System.Threading.CancellationToken.None))
                {
                    Console.WriteLine($"mensagem atual:{stream.ResponseStream.Current}");
                    Console.WriteLine($"Person result: " +
                        $"Id:{stream.ResponseStream.Current.BusinessEntityID}, " +
                        $"FirstName:{stream.ResponseStream.Current.FirstName}, " +
                        $"LastName:{stream.ResponseStream.Current.LastName}");
                }
            });
        }

        public void CloseClient()
        {
            _client = null;
            _channel.ShutdownAsync();
        }
    }
}
