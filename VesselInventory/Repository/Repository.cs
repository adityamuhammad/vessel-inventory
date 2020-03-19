using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> FindAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);

        T Save(T entity);

    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly VesselInventoryContext _context;
        public Repository(VesselInventoryContext context)
        {
            _context = context;
        }
        
        public IEnumerable<T> FindAll()
        {
            return _context.Set<T>().ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public T Save(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);

        }

        public T Update(int id, T entity)
        {
            if (entity == null)
                return null;

            T current = GetById(id);
            if (current != null)
            {
                _context.Entry(current).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
            return entity;
        }

        public int Delete(int id)
        {
            T current = GetById(id);
            if (current == null)
                return 0;
            _context.Set<T>().Remove(current);
            _context.SaveChanges();
            return 1;

        }
    }

}
