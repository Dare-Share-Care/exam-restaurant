using Ardalis.Specification;

namespace Restaurant.Web.Interface.Repositories;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
}