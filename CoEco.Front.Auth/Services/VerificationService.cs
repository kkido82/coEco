using CoEco.Front.Auth.Data;
using CoEco.Front.Auth.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Services
{
    public class VerificationService : IVerificationService
    {
        private readonly IRepository<VerificationCode> repo;

        public VerificationService(IRepository<VerificationCode> repo)
        {
            this.repo = repo;
        }
        public async Task Create(string username, string code)
        {
            var prevCode = await repo.All.Where(c => !c.Disabled && c.Username == username).ToArrayAsync();
            foreach (var vd in prevCode)
            {
                vd.Disabled = true;
            }
            await repo.SaveChanges();

            await repo.Insert(new VerificationCode
            {
                Code = code,
                Username = username,
                VerifiedOn = null,
                ExpirationDate = DateTime.Now.AddMinutes(5)
            });
        }

        public async Task SetVerified(string username, string code)
        {
            var vcs = await repo.All
                .Where(c => !c.Disabled && c.Username == username && c.Code == code)
                .ToArrayAsync();

            foreach (var vc in vcs)
            {
                vc.VerifiedOn = DateTime.Now;
                vc.Disabled = true;
            }
            await repo.SaveChanges();
        }

        public Task<bool> Verify(string username, string code) =>
            repo.All
                .Where(c => !c.Disabled && c.VerifiedOn == null && c.ExpirationDate >= DateTime.Now)
                .OrderByDescending(c => c.CreatedOn)
                .AnyAsync(c => c.Username == username && c.Code == code);
    }
}
