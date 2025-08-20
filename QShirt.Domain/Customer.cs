namespace QShirt.Domain;

public class Customer : EntityBase
{
    /// <summary>
    /// Unique external Id
    /// </summary>
    public Guid IdentifyId { get; set; }

    /// <summary>
    /// Customer contents
    /// </summary>
    public ICollection<CustomerContent> Contents { get; set; }

    /// <summary>
    /// Auth token
    /// </summary>
    public Guid AuthTokenId { get; set; }
}
