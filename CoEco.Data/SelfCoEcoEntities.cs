using CoEco.Core.Context;
using CoEco.Data.EntityTypes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Data
{
    public partial class CoEcoEntities
    {
        private readonly IUserContext _userContext;

        static CoEcoEntities()
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, ensures static reference to System.Data.Entity.SqlServer");
        }

        public CoEcoEntities(IUserContext userContext) : this()
        {
            _userContext = userContext;
        }

        public override int SaveChanges()
        {

            PrepareForSave();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            PrepareForSave();
            return base.SaveChangesAsync();
        }

        void PrepareForSave()
        {
            if (_userContext == null) return;

            var addedEntites = this.ChangeTracker.Entries<IBaseEntity>().Where(x => x.State == System.Data.Entity.EntityState.Added);

            foreach (var dbEntityEntry in addedEntites)
            {
                dbEntityEntry.Entity.CreatedBy = _userContext.UserName;
                dbEntityEntry.Entity.UpdatedBy = _userContext.UserName;
                dbEntityEntry.Entity.CreatedOn = DateTime.Now;
                dbEntityEntry.Entity.UpdatedOn = DateTime.Now;
            }

            var updatedEntities = this.ChangeTracker.Entries<IBaseEntity>().Where(x => x.State == System.Data.Entity.EntityState.Modified);
            foreach (var updatedEntity in updatedEntities)
            {
                updatedEntity.Entity.UpdatedBy = _userContext.UserName;
                updatedEntity.Entity.UpdatedOn = DateTime.Now;
            }
        }
    }
}
