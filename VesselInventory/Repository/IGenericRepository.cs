﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VesselInventory.Models;

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

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public virtual IEnumerable<T> GetAll()
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Set<T>().ToList();
            }
        }

        public virtual IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Set<T>().Where(predicate).ToList();
            }
        }

        public virtual T Save(T entity)
        {
            using (var context = new VesselInventoryContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
                return entity;
            }
        }

        public virtual T GetById(int id)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Set<T>().Find(id);
            }

        }

        public virtual T Update(int id, T entity)
        {
            using (var context = new VesselInventoryContext())
            {
                if (entity == null)
                    return null;

                T current = context.Set<T>().Find(id);
                if (current != null)
                {
                    context.Entry(current).CurrentValues.SetValues(entity);
                    context.SaveChanges();
                }
                return entity;
            }

        }

        public virtual void Delete(int id)
        {
            using (var context = new VesselInventoryContext())
            {
                T current = context.Set<T>().Find(id);
                if (current == null)
                    return;
                context.Set<T>().Remove(current);
                context.SaveChanges();
            }
        }
    }

}