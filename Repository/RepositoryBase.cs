using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext _repositoryContext; // access to our DbContext (tables)

    public RepositoryBase(RepositoryContext repositoryContext) => _repositoryContext = repositoryContext;

    // Returns a set of Entity
    public IQueryable<T> FindAll(bool trackChanges)
        => !trackChanges ? _repositoryContext.Set<T>().AsNoTracking() : _repositoryContext.Set<T>();

    // Additional Where clause
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        => !trackChanges
            ? _repositoryContext.Set<T>().Where(expression).AsNoTracking()
            : _repositoryContext.Set<T>().Where(expression);

    // Adds input entity
    public void Create(T entity)
        => _repositoryContext.Set<T>().Add(entity);

    // Updates input entity
    public void Update(T entity)
        => _repositoryContext.Set<T>().Update(entity);

    // Removes input entity
    public void Delete(T entity)
        => _repositoryContext.Set<T>().Remove(entity);
}