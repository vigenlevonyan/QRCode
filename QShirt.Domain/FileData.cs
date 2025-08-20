namespace QShirt.Domain;

public class FileData<Guid> : EntityBase
{
    #region Constructors

    protected FileData()
    {
    }

    public FileData(byte[] data)
    {
        Data = data;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Data
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    /// File ID
    /// </summary>
    public Guid FileId { get; set; }

    #endregion Properties
}

/// <summary>
/// File
/// </summary>
public class FileData : FileData<Guid>
{
    protected FileData()
    {
    }

    public FileData(byte[] data) : base(data)
    {
        Id = Guid.NewGuid();
    }
}
