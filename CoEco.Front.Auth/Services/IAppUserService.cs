using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Front.Auth.Domain;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Services
{
    public interface IAppUserService
    {
        Task<User> GetUser(string username);
        Task<Result<bool>> UserExists(string username);
    }
}
