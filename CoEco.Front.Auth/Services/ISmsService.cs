using System.Threading.Tasks;

namespace CoEco.Front.Auth.Services
{
    public interface ISmsService
    {
        Task<bool> SendMessage(string phone, string message);
    }
}
