using Microsoft.EntityFrameworkCore;
using QShirt.Domain;
using QShirt.Persistence;
using QShirt.WebApi.Application.Queries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QShirt.WebApi.Application.Queries;

public class GetCustomerContentQuery
{
    #region Fields

    private readonly IRepository<Domain.File> filesRepository;
    private readonly IRepository<Customer> customersRepository;

    #endregion Fields

    #region Constructor
    public GetCustomerContentQuery(
        IRepository<Domain.File> filesRepository,
        IRepository<Customer> customersRepository)
    {
        this.filesRepository = filesRepository;
        this.customersRepository = customersRepository;
    }

    #endregion Constructor

    #region Methods

    public async Task<ContentModel> Execute(Guid identifyId)
    {

        var content = await customersRepository
            .Where(c => c.IdentifyId == identifyId)
            .SelectMany(c => c.Contents)
            .Where(c => c.IsMain)
            .Include(c => c.File)
            .FirstOrDefaultAsync();

        return new ContentModel
        {
            Description = content.Description,
            IsMain = content.IsMain,
            Title = content.Title,
            Url = content.Url,
            File = content.File != null ? new Models.File
            {
                FileName = content.File.FileName,
                Extension = content.File.Extension,
                Size = content.File.Size,
                Data = content.File.Data.Data
            } : null
        };
    }

    #endregion Methods
}
