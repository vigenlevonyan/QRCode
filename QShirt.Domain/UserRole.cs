namespace QShirt.Domain;

/// <summary>
/// User role
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Administrator
    /// </summary>
    [Name("Administrator")]
    Admin = 1,

    /// <summary>
    /// Customer
    /// </summary>
    [Name("Customer")]
    Customer = 2
}