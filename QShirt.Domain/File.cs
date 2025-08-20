namespace QShirt.Domain;

public class File<Guid> : EntityBase
{
    #region Constructors

    protected File()
    {
    }

    public File(string fileNameWithExtension, byte[] data)
    {
        FileName = Path.GetFileNameWithoutExtension(fileNameWithExtension);
        Extension = Path.GetExtension(fileNameWithExtension);
        Size = data.LongLength;

        Data = new FileData(data);
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// File name (without extension)
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Extension
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// Size in bytes
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// File data
    /// </summary>
    public FileData Data { get; set; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Returns full file name (with extension)
    /// </summary>
    public string GetFullName() => $"{FileName}{Extension}";

    #endregion Methods
}

/// <summary>
/// File
/// </summary>
public class File : File<Guid>
{
    protected File()
    {
    }

    public File(string fileNameWithExtension, byte[] data) : base(fileNameWithExtension, data)
    {
        Id = Guid.NewGuid();
    }
}
