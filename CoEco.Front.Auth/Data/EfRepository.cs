using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Data
{
    public class EfRepository<T> : IRepository<T>
           where T : class
    {
        private readonly DbContext db;
        private readonly DbSet<T> dbSet;

        public EfRepository(DbContext db)
        {
            this.db = db;
            dbSet = db.Set<T>();
        }
        public IQueryable<T> All => dbSet;

        public async Task<T> Insert(T entity)
        {
            dbSet.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            await db.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Delete(T entity)
        {
            dbSet.Remove(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }
    }
}
