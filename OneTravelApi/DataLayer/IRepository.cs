﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneTravelApi.DataLayer
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> Query();

        Task DeleteRangeAsync(IList<T> entities);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        Task<T> GetAsync(int id);
        Task<T> GetAsync(long id);

        Task<T> AddAsync(T entity);

        T Add(T entity);

        Task UpdateAsync(T changes);
        void Update(T changes);

        Task<T> DeleteAsync(int id);
        Task<T> DeleteAsync(long id);
        Task<T> DeleteAsync(T entity);

        T Delete(T entity);

        T Delete(int id);

        void SaveChanges();
    }
}
