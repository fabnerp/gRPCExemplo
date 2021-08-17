using Grpc.Core;
using gRPCServiceBasic.Data.Repository;
using gRPCProtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace gRPCServiceBasic.Services
{

    public class PersonService : Person.PersonBase
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

        public override async Task<PersonResponseAsync> GetPersonByIdAsync(PersonRequestAsync request, ServerCallContext context)
        {
            var response = _personRepository.GetPersonById(request.BusinessEntityID);

            PersonResponseAsync person = new PersonResponseAsync
            {
                BusinessEntityID = response.BusinessEntityID,
                FirstName = response.FirstName,
                LastName = response.LastName
            };

            return await Task.FromResult(person);
        }

        public override async Task GetPersonByLastNameAsync(
            IAsyncStreamReader<PersonByLastNameRequestStream> requestStream,
            IServerStreamWriter<PersonByLastNameResponseStream> responseStream,
            ServerCallContext context)
        {
            if (requestStream == null)
            {
                if (!await requestStream.MoveNext())
                {
                    return;
                }
            }

            try
            {
                while (await requestStream.MoveNext())
                {
                    if (requestStream.Current.LastName.Length > 0)
                    {
                        var response = _personRepository.GetPersonByLastname(requestStream.Current.LastName);

                        foreach (var item in response)
                        {
                            await responseStream.WriteAsync(
                                new PersonByLastNameResponseStream
                                {
                                    BusinessEntityID = item.BusinessEntityID,
                                    FirstName = item.FirstName,
                                    LastName = item.LastName
                                });
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
