using System.Linq.Expressions;

namespace Cinema.Domain.Abstracts.RepositoryAbstracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> All { get; }

        TEntity Get<Tkey>(Tkey id) where Tkey : struct;
        Task<TEntity> GetAsync<Tkey>(Tkey id, CancellationToken cancellationToken) where Tkey : struct;

        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
    }
}
