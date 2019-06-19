using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoEco.Data;

namespace CoEco.Services.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IDataService<AspNetUser> _aspNetUser;
        private readonly IDataService<AspNetUserClaim> _aspNetUserClaim;
        private readonly IDataService<Log> _log;
        private readonly IDataService<Unit> _unit;
        private readonly IDataService<PermissionsProfile> _permissionsProfile;

        public DataAccessService(IDataService<AspNetUser> aspNetUser, IDataService<AspNetUserClaim> aspNetUserClaim, IDataService<Log> log,
                                IDataService<PermissionsProfile> permissionsProfile, IDataService<Unit> unit)
        {
            _aspNetUser = aspNetUser;
            _aspNetUserClaim = aspNetUserClaim;
            _log = log;
            _unit = unit;
            _permissionsProfile = permissionsProfile;
        }
        public AspNetUser GetAspNetUserById(string aspNetUserId)
        {
            return _aspNetUser.GetById(aspNetUserId);
        }

        public IQueryable<AspNetUserClaim> GetAspNetUserClaimsByUserId(string aspNetUserId)
        {
            return _aspNetUserClaim.FindBy(x => x.UserId == aspNetUserId);
        }

        public void RemoveAspNetUser(AspNetUser aspNetUser)
        {
            _aspNetUser.Delete(aspNetUser);
        }

        public void RemoveAspNetUserClaim(List<AspNetUserClaim> aspNetUserClaim)
        {
            _aspNetUserClaim.DeleteRange(aspNetUserClaim);
        }
        public void InsertLogMessage(Log log)
        {
            _log.Insert(log);
        }

        public IQueryable<Unit> GetAllUnits()
        {
            return _unit.GetAll().Where(x => !x.Disable);
        }

        public IQueryable<PermissionsProfile> GetAllPermissionsProfile()
        {
            return _permissionsProfile.GetAll().Where(x => !x.Disable);
        }
    }
}
