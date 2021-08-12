using Grpc.Core;
using gRPCBasico.Data.Repository;
using gRPCBasico.Protos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gRPCBasico.Services
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

        public override Task<PersonResponse> GetPersonByLastName(PersonByLastNameRequest request, ServerCallContext context)
        {
            var response = _personRepository.GetPersonByLastname(request.LastName);

            return Task.FromResult(response);
                       
        }
    }
}
