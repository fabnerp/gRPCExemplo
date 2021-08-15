using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static gRPCClientBasic.Protos.Person;

namespace gRPCClientBasic
{
    public class ServiceConsumer : IServiceConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly GrpcChannel _channel;
        private readonly PersonClient _client;


        public ServiceConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            _channel = GrpcChannel.ForAddress(_configuration.GetSection("urlServiceHTTPS").Value);
            _client = new PersonClient(_channel);
        }

        public void GetUserById(int id)
        {
            var response = _client.GetPersonById(
                new Protos.PersonRequest
                {
                    BusinessEntityID = id
                });

            Console.WriteLine($"Person result: Id:{response.BusinessEntityID}, FirstName:{response.FirstName}, LastName:{response.LastName}");
        }

        public async void GetUserByIdAsync(int id)
        {
            var response = await _client.GetPersonByIdAsync(
                 new Protos.PersonRequest
                 {
                     BusinessEntityID = id
                 });

            Console.WriteLine($"Person result: Id:{response.BusinessEntityID}, FirstName:{response.FirstName}, LastName:{response.LastName}");
        }

    }
}
