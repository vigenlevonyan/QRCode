using Microsoft.AspNetCore.Mvc;
using QShirt.WebApi.Application.Queries;

namespace QShirt.WebApi.Controllers;

[Route("/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class ContentController : ControllerBase
{
    #region Fields

    private readonly GetCustomerContentQuery getCustomerContentQuery;

    #endregion Fields

    #region Constructor
    public ContentController(
        GetCustomerContentQuery getCustomerContentQuery
        )
    {
        this.getCustomerContentQuery = getCustomerContentQuery;
    }

    #endregion Constructor

    #region Methods

    [HttpGet("get")]
    public async Task<IActionResult> GetContent(Guid id)
    {
        var content = await getCustomerContentQuery.Execute(id);

        if (!string.IsNullOrWhiteSpace(content.Url))
            return Redirect(content.Url);

        if (content.File != null)
        {
            if (content.File.Extension == ".jpeg" || content.File.Extension == ".jpg")
                return File(content.File.Data, "image/jpeg");
            else
                return File(content.File.Data, "image/png");
        }

        return Ok();

    }

    #endregion Methods
}
