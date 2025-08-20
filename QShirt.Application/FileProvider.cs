using Microsoft.EntityFrameworkCore;
using QShirt.Domain;
using QShirt.Persistence;
using System;
using System.Threading.Tasks;

namespace QShirt.Application;

public class FileProvider
{
    #region Fields

    private readonly IRepository<File> fileRepository;

    #endregion Fields

    #region Constructor

    public FileProvider(IRepository<File> fileRepository)
    {
        this.fileRepository = fileRepository;
    }

    #endregion Constructor

    public async Task<(byte[] Data, string FileName, string Extension)> GetFileById(Guid fileId)
    {
        File file = await fileRepository
            .Include(f => f.Data)
            .FirstOrDefaultAsync(t => t.Id == fileId);
        if (file == null)
            throw new Exception("Missed file");

        return (file.Data.Data, file.FileName, file.Extension);
    }
}