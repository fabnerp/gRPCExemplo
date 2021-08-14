using gRPCBasico.Protos;
using System.Collections.Generic;

namespace gRPCBasico.Data.Repository
{
    public interface IPersonRepository
    {
        PersonResponse GetPersonById(int businessEntityID);
        IEnumerable<PersonResponse> GetPersonByLastname(string lastName);
        
    }
}