using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Data
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> All { get; }
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
        Task SaveChanges();
    }
}
