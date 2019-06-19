using System.Threading.Tasks;

namespace CoEco.Front.Auth.Services
{
    public interface ILoginService
    {
        Task<int> GetNumFailed(string username);
        Task SaveResult(string username, bool success);
    }
}
