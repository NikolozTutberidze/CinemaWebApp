using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cinema.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal CinemaContext _context;
        internal DbSet<TEntity> dbSet;

        public Repository(CinemaContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> All => dbSet;

        public TEntity Get<Tkey>(Tkey id) where Tkey : struct
        {
            return dbSet.Find(new object[] { id })!;
        }

        public async Task<TEntity> GetAsync<Tkey>(Tkey id, CancellationToken cancellationToken) where Tkey : struct
        {
            return (await dbSet.FindAsync(new object[] { id }, cancellationToken))!;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbSet.ToListAsync(cancellationToken);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
