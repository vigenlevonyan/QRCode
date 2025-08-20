namespace QShirt.Domain;

public class CustomerContent : EntityBase
{
    public bool IsMain { get; set; }

    public string? Url { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public File? File { get; set; }

    #region Initiator

    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; }

    #endregion Initiator

}
