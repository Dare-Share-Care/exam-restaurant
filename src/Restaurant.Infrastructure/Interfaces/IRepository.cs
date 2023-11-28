using Ardalis.Specification;

namespace Restaurant.Infrastructure.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}