using gRPCProtos;
using System.Collections.Generic;

namespace gRPCServiceBasic.Data.Repository
{
    public interface IPersonRepository
    {
        IEnumerable<PersonResponse> GetPeopleByLastname(string lastName);
        PersonResponse GetPersonById(int businessEntityID);
    }
}