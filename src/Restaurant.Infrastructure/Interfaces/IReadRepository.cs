using Ardalis.Specification;

namespace Restaurant.Infrastructure.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
}