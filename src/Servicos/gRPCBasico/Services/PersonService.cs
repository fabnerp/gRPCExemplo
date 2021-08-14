using Grpc.Core;
using gRPCServiceBasic.Data.Repository;
using gRPCServiceBasic.Protos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gRPCServiceBasic.Services
{
    

    public class PersonService: Person.PersonBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPersonRepository _personRepository;

        public PersonService(IConfiguration configuration, IPersonRepository personRepository)
        {
            _configuration = configuration;
            _personRepository = personRepository;
        }

        public override Task<PersonResponse> GetPersonById(PersonRequest request, ServerCallContext context)
        {
            var response = _personRepository.GetPersonById(request.BusinessEntityID);

            return Task.FromResult(response);
        }

        public override async Task<PersonResponse> GetPersonByIdAsync(PersonRequest request, ServerCallContext context)
        {
            var response = _personRepository.GetPersonById(request.BusinessEntityID);

            return await Task.FromResult(response);
        }

        public override async Task GetPersonByLastNameAsync(IAsyncStreamReader<PersonByLastNameRequest> requestStream, IServerStreamWriter<PersonResponse> responseStream, ServerCallContext context)
        {
            try
            {
                while (await requestStream.MoveNext())
                {
                    if (requestStream.Current.LastName.Length > 0)
                    {
                        var response = _personRepository.GetPersonByLastname(requestStream.Current.LastName);

                        foreach (var item in response)
                        {
                            await responseStream.WriteAsync(item);
                        }
                    }                   
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
