using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Front.Auth.Domain;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Services
{
    public interface IAuthenticationService
    {
        Task<Result<User>> Authenticate(string username, string code);
        Task<Result<string>> CreateCode(string username);
    }
}