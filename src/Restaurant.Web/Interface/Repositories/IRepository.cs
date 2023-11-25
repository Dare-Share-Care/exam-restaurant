using Ardalis.Specification;

namespace Restaurant.Web.Interface.Repositories;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    
}