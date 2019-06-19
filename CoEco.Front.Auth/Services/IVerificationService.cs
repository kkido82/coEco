using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Services
{
    public interface IVerificationService
    {
        Task Create(string username, string code);
        Task<bool> Verify(string username, string code);
        Task SetVerified(string username, string code);
    }
}
