using Microsoft.JSInterop;

namespace QShirt.Public.Client.InteropServices;

/// <summary>
/// User interface blocker
/// </summary>
public class Blocker
{
    #region Fields

    private readonly IJSRuntime jsRuntime;

    private List<string> afterRenderSelectors = new List<string>();

    #endregion Fields

    #region Constructor

    public Blocker(IJSRuntime jsRuntime) => this.jsRuntime = jsRuntime;

    #endregion Constructor

    #region Methods

    /// <summary>
    /// Performs page blocking
    /// </summary>
    public async Task BlockPage() => await jsRuntime.InvokeVoidAsync("blockPage");

    /// <summary>
    /// Performs page unblocking
    /// </summary>
    public async Task UnblockPage() => await jsRuntime.InvokeVoidAsync("unblockPage");

    /// <summary>
    /// Performs element blocking by selector
    /// </summary>
    /// <param name="selector">selector</param>
    public async Task Block(string selector)
    {
        if (!afterRenderSelectors.Contains(selector))
            afterRenderSelectors.Add(selector);
        await jsRuntime.InvokeVoidAsync("block", selector);
    }

    /// <summary>
    /// Performs element unblocking by selector
    /// </summary>
    /// <param name="selector">selector</param>
    public async Task Unblock(string selector)
    {
        afterRenderSelectors.Remove(selector);
        await jsRuntime.InvokeVoidAsync("unblock", selector);
    }

    /// <summary>
    /// Blocks selectors that were deferred to "after render"
    /// </summary>
    /// <remarks>
    /// HACK: selector blocking doesn't work if called before render
    /// To solve this problem, after render we call this method which blocks all needed selectors
    /// </remarks>
    public async Task BlockSelectorsAfterRender()
    {
        foreach (string selector in afterRenderSelectors)
            await jsRuntime.InvokeVoidAsync("block", selector);
    }

    #endregion Methods
}
