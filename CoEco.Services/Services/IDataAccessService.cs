using CoEco.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Services.Services
{
    public interface IDataAccessService
    {
        AspNetUser GetAspNetUserById(string aspNetUserId);
        IQueryable<AspNetUserClaim> GetAspNetUserClaimsByUserId(string aspNetUserId);
        void RemoveAspNetUser(AspNetUser aspNetUser);
        void RemoveAspNetUserClaim(List<AspNetUserClaim> aspNetUserClaim);
        void InsertLogMessage(Log log);
        IQueryable<Unit> GetAllUnits();
        IQueryable<PermissionsProfile> GetAllPermissionsProfile();
    }
}
