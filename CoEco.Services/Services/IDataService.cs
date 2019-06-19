using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CoEco.Services.Services
{
    public interface IDataService<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T GetById(string id);
        T GetById(int id);
        void Insert(T item);
        Task<T> InsertAsync(T item);
        void Edit(T item);
        Task EditAsync(T item);
        void EditAll(IEnumerable<T> items);
        void Delete(T item);
        void DeleteRange(IEnumerable<T> items);
        void InsertAll(IEnumerable<T> items);
        void Detach(T item);
    }
}
