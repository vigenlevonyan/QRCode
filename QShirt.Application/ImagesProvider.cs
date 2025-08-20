using Microsoft.EntityFrameworkCore;
using Nc.Images.Abstractions;
using QShirt.Domain;
using QShirt.Persistence;
using System;
using System.Threading.Tasks;

namespace QShirt.Application;

public class ImagesProvider
{
    #region Fields

    private readonly IRepository<File> fileRepository;
    private readonly ILocalImageCacheManager localImageCacheManager;

    #endregion Fields

    #region Constructor

    public ImagesProvider(IRepository<File> fileRepository
        , ILocalImageCacheManager localImageCacheManager
        )
    {
        this.fileRepository = fileRepository;
        this.localImageCacheManager = localImageCacheManager;
    }

    #endregion Constructor

    /// <summary>
    /// Returns cached image
    /// </summary>
    public async Task<(byte[] Data, string FileName, string Extension)> GetCachedImage(Guid fileId, int? width, int? height)
    {
        File file = await fileRepository
            .FirstOrDefaultAsync(t => t.Id == fileId);
        if (file == null)
            throw new Exception("File not found");

        byte[] fileData = localImageCacheManager.GetCachedImage(fileId, $"{file.FileName}{file.Extension}", width, height);
        return (fileData, file.FileName, file.Extension);
    }
}