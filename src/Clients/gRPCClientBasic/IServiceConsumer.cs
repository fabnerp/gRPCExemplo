using System.Threading.Tasks;

namespace gRPCClientBasic
{
    public interface IServiceConsumer
    {
        void CloseClient();
        Task GetPeople();
        void GetUserById(int id);
        Task GetUserByIdAsync(int id);
    }
}