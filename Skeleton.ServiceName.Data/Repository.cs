using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ServiceNameContext _context;

        public Repository(ServiceNameContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> All => _context.Set<TEntity>();

        public void Update(params TEntity[] obj)
        {
            _context.Set<TEntity>().UpdateRange(obj);
            _context.SaveChanges();
        }

        public void Delete(params TEntity[] obj)
        {
            _context.Set<TEntity>().RemoveRange(obj);
            _context.SaveChanges();
        }

        public TEntity Find(Guid key)
        {
            return _context.Find<TEntity>(key);
        }

        public void Insert(params TEntity[] obj)
        {
            _context.Set<TEntity>().AddRange(obj);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(params TEntity[] obj)
        {
            _context.Set<TEntity>().UpdateRange(obj);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(params TEntity[] obj)
        {
            _context.Set<TEntity>().RemoveRange(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> FindAsync(Guid key)
        {
            return await _context.FindAsync<TEntity>(key);
        }

        public async Task InsertAsync(params TEntity[] obj)
        {
            _context.Set<TEntity>().AddRange(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> FindNoTrackingAsync(Guid key)
        {
            var entity = await _context.Set<TEntity>().FindAsync(key);
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
    }
}
