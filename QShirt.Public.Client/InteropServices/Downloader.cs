using Microsoft.JSInterop;

namespace QShirt.Public.Client.InteropServices;

public class Downloader
{
    #region Fields

    private readonly IJSRuntime jsRuntime;

    #endregion Fields

    #region Constructor

    public Downloader(IJSRuntime jsRuntime) => this.jsRuntime = jsRuntime;

    #endregion Constructor

    #region Methods

    /// <summary>
    /// Returns byte array as downloadable file
    /// </summary>
    /// <param name="bytes">byte array</param>
    /// <param name="filename">file name</param>
    /// <param name="contentType">Content type</param>
    public async Task DownloadFileAsync(byte[] bytes, string filename, string contentType = null)
    {
        await jsRuntime.InvokeVoidAsync("downloadFromByteArray", new
        {
            ByteArray = bytes,
            FileName = filename,
            ContentType = contentType
        });
    }

    #endregion Methods
}
