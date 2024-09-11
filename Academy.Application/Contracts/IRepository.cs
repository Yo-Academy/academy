using Ardalis.Specification;

namespace Academy.Application.Persistence.Repository;

/// <summary>
/// The regular read/write repository for an aggregate root.
/// </summary>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
}

/// <summary>
/// The read-only repository for an aggregate root.
/// </summary>
public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
}

/// <summary>
/// A special (read/write) repository for an aggregate root,
/// that also adds EntityCreated, EntityUpdated or EntityDeleted
/// events to the DomainEvents of the entities before adding,
/// updating or deleting them.
/// </summary>
public interface IRepositoryWithEvents<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
}


/// <summary>
/// The regular read/write repository for an aggregate root.
/// </summary>
public interface IWriteRepository<T> : IRepositoryBase<T>
    where T : class
{
}

/// <summary>
/// The read-only repository for an aggregate root.
/// </summary>
public interface IReadOnlyRepository<T> : IReadRepositoryBase<T>
    where T : class
{
}