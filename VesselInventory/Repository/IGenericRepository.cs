using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VesselInventory.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetWhere(Expression<Func<T, bool>> expression);
        T Save(T entity);
        T GetById(int id);
        T Update(int id, T entity);
        void Delete(int id);
    }


}
