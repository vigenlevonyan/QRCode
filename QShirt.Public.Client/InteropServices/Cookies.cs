using Microsoft.JSInterop;

namespace QShirt.Public.Client.InteropServices;

public class Cookies
{
    #region Fields

    private readonly IJSRuntime jsRuntime;

    #endregion Fields

    #region Constructor

    public Cookies(IJSRuntime jsRuntime) => this.jsRuntime = jsRuntime;

    #endregion Constructor

    #region Methods

    public async ValueTask SetAsync(string name, string value, int days) => await jsRuntime.InvokeVoidAsync("WriteCookie", name, value, days);

    public async ValueTask RemoveAsync(string name) => await jsRuntime.InvokeVoidAsync("RemoveCookie", name);


    #endregion Methods
}
