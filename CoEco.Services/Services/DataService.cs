using CoEco.Data;
using CoEco.Services.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CoEco.Services.Services
{
    public class DataService<T> : IDataService<T> where T : class
    {
        private readonly CoEcoEntities _context;
        private IDbSet<T> _entities;
        private string _userId;

        public DataService(CoEcoEntities context, string userId)
        {
            _context = context;
            _userId = userId;
        }
        public DataService()
        {

        }
        public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
            _context.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> items)
        {
            _context.Set<T>().RemoveRange(items);
            _context.SaveChanges();
        }





        public void Edit(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task EditAsync(T item)
        {
            await _context.SaveChangesAsync();
        }
        public void EditAll(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }


        public void Detach(T item)
        {
            _context.Entry(item).State = EntityState.Detached;

        }


        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            //throw new NotImplementedException();
            return Entities.Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return Entities;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public T GetById(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Insert(T item)
        {
            _context.Set<T>().Add(item);
            _context.SaveChanges();
        }

        public async Task<T> InsertAsync(T item)
        {
            _context.Set<T>().Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public void InsertAll(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _context.Set<T>().Add(item);
            }
            _context.SaveChanges();

        }



        protected virtual IDbSet<T> Entities
        {
            get { return _entities ?? (_entities = _context.Set<T>()); }
        }
    }
}
