using gRPCServiceBasic.Protos;
using System.Collections.Generic;

namespace gRPCServiceBasic.Data.Repository
{
    public interface IPersonRepository
    {
        PersonResponse GetPersonById(int businessEntityID);
        IEnumerable<PersonResponse> GetPersonByLastname(string lastName);
        
    }
}