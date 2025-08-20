using QShirt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QShirt.WebApi.Application.Queries.Models;

public class ContentModel
{
    public bool IsMain { get; set; }

    public string? Url { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public File? File { get; set; }
}

public class File
{
    public string FileName { get; set; }

    public string Extension { get; set; }

    public long Size { get; set; }
    public byte[] Data { get; set; }
}