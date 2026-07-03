using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Domain.Entities;
using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GramaMaster.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity:BaseEntity
    {
        private readonly GramaMasterDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GramaMasterDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }
        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet
                .Where(x => !x.IsDeleted)
                .AnyAsync(predicate);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual  void Update(TEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public virtual  void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            _dbSet.Update(entity);
        }

        public virtual  IQueryable<TEntity> Query()
        {
            return _dbSet.Where(x => !x.IsDeleted).AsQueryable();
        }
    }
}
