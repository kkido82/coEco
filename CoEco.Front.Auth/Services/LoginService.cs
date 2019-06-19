using CoEco.Front.Auth.Data;
using CoEco.Front.Auth.Domain;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Services
{

    public class LoginService : ILoginService
    {
        private readonly IRepository<LoginResultLog> repository;

        public LoginService(IRepository<LoginResultLog> repository)
        {
            this.repository = repository;
        }

        public async Task<int> GetNumFailed(string username)
        {

            var results = await repository.All.GetResults(username, 5)
                .ToArrayAsync();

            var numFailed = results
                .TakeWhile(b => !b)
                .Count();

            return numFailed;
        }

        public Task SaveResult(string username, bool success)
        {
            return repository.Insert(new LoginResultLog
            {
                Succes = success,
                Username = username
            });
        }
    }
}
