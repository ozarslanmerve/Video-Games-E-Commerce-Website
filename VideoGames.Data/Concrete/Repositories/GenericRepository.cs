using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Data.Abstract;
using VideoGames.Data.Concrete.Contexts;

namespace VideoGames.Data.Concrete.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly VideoGamesDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(VideoGamesDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includes != null)
            {
                query = includes.Aggregate(
                    query, (current, include) => include(current));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate, params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includes != null)
            {
                query = includes.Aggregate(
                    query,
                    (current, include) => include(current));
            }
            return query.FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
