namespace QShirt.Domain;

public interface IEntityBase<T>
{
    public T Id { get; set; }
}