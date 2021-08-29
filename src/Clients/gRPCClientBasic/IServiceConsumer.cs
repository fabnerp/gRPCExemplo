using System.Threading.Tasks;

namespace gRPCClientBasic
{
    public interface IServiceConsumer
    {
        void CloseClient();
        Task GetPeopleByLastNameBiDirect(string lastName);
        Task GetPeopleByLastNameStreamResponseAsync(string lastName);
        void GetUserById(int id);
        Task GetUserByIdAsync(int id);
    }
}