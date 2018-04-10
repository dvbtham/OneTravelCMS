﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OneTravelApi.DataLayer
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        private readonly OneTravelDbContext _dbContext;
        private bool _disposed;

        public Repository(OneTravelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Query()
        {
            return _dbContext.Set<T>();
        }

        public Task<T> GetAsync(int id)
        {
            return _dbContext.Set<T>().FindAsync(id);
        }
        public Task<T> GetAsync(long id)
        {
            return _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(match);
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return entity;
        }

        public async Task UpdateAsync(T changes)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            await _dbContext.SaveChangesAsync();
        }

        public void Update(T changes)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            _dbContext.SaveChanges();
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);

                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task DeleteRangeAsync(IList<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);

            await _dbContext.SaveChangesAsync();

        }

        public async Task<T> DeleteAsync(T entity)
        {
            if (entity == null) return null;

            _dbContext.Set<T>().Remove(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public T Delete(int id)
        {
            var entity = _dbContext.Set<T>().Find(id);

            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
            }

            return entity;
        }

        public T Delete(T entity)
        {
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
            }

            return entity;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task<T> DeleteAsync(long id)
        {
            var entity = await GetAsync(id);

            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);

                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();

                    _disposed = true;
                }
            }
        }
    }
}
