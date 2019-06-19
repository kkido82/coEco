using CoEco.Core.Context;
using CoEco.Front.Auth.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Data
{
    public class AuthDbContext : DbContext
    {
        private readonly IUserContext _userContext;
        public const string DefaultUserName = "Anonymous";
        public AuthDbContext() : base("Auth") { }
        public AuthDbContext(IUserContext userContext) : base("Auth")
        {
            this._userContext = userContext;
        }

        public DbSet<LoginResultLog> LoginResults { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Auth");
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetUpdateDetails();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            SetUpdateDetails();
            return base.SaveChangesAsync();
        }


        private void SetUpdateDetails()
        {
            var addedEntites = this.ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Added);
            var userName = _userContext?.UserName ?? DefaultUserName;
            foreach (var dbEntityEntry in addedEntites)
            {
                dbEntityEntry.Entity.CreatedBy = userName;
                dbEntityEntry.Entity.UpdatedBy = userName;
                dbEntityEntry.Entity.CreatedOn = DateTime.Now;
                dbEntityEntry.Entity.UpdatedOn = DateTime.Now;

            }

            var updatedEntities = this.ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Modified);
            foreach (var updatedEntity in updatedEntities)
            {
                updatedEntity.Entity.UpdatedBy = userName;
                updatedEntity.Entity.CreatedBy = updatedEntity.Entity.CreatedBy ?? DefaultUserName;
                updatedEntity.Entity.UpdatedOn = DateTime.Now;
                updatedEntity.Entity.CreatedOn = updatedEntity.Entity.CreatedOn > DateTime.MinValue
                    ? updatedEntity.Entity.CreatedOn
                    : DateTime.Now;
            }
        }
    }
}
