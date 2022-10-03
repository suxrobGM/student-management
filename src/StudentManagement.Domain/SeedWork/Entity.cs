namespace StudentManagement.Domain;

public abstract class Entity : IAggregateRoot
{
    public ulong Id { get; set; }
}