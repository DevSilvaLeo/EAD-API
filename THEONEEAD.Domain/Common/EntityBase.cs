namespace THEONEEAD.Domain.Common;

public abstract class EntityBase
{
    public Guid Id { get; protected set; }

    protected EntityBase() => Id = Guid.NewGuid();

    protected EntityBase(Guid id) => Id = id;

    public override bool Equals(object? obj) =>
        obj is EntityBase other && Id.Equals(other.Id);

    public override int GetHashCode() => Id.GetHashCode();
}
