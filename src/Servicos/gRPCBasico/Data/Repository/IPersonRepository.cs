using gRPCBasico.Protos;

namespace gRPCBasico.Data.Repository
{
    public interface IPersonRepository
    {
        PersonResponse GetPersonById(int businessEntityID);
        PersonResponse GetPersonByLastname(string lastName);
    }
}