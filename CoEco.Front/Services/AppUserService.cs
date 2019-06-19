using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Data;
using CoEco.Front.Auth.Domain;
using CoEco.Front.Auth.Services;
using CoEco.Services.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CoEco.Front.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IDataService<Member> dataService;

        public AppUserService(IDataService<Member> dataService)
        {
            this.dataService = dataService;
        }
        public async Task<User> GetUser(string username)
        {
            var member = await dataService.GetAll().FirstOrDefaultAsync(a => a.TZ == username);
            return new User
            {
                Id = member.ID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Phone = member.PhoneNumber,
                Username = member.TZ,
                UnitId = member.UnitID,
                Permissions = new UserPermission
                {
                    CanConfirmOrder = member.PermissionsProfile.OrderConfirmation,
                    CanOpenOrder = member.PermissionsProfile.OpenAnOrder,
                    CanUpdateInventory = member.PermissionsProfile.UpdateInventory
                }
            };
        }

        public async Task<Result<bool>> UserExists(string username)
        {
            var b = await dataService.GetAll().AnyAsync(a => a.TZ == username);
            return b;
        }
    }
}