namespace StudentManagement.Domain;

/// <summary>
/// Aggregate root
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Primary key
    /// </summary>
    ulong Id { get; set; }
}