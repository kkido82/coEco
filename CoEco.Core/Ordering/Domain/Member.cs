using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Domain
{
    public class Member
    {
        public Member(int id, int unitId, params Permission[] permissions)
        {
            Id = id;
            UnitId = unitId;
            Permissions = permissions;
        }
        public int Id { get; }
        public int UnitId { get; }
        public Permission[] Permissions { get; }

        public bool HasPermission(Permission permission) =>
            Permissions.Contains(permission);
    }
}
