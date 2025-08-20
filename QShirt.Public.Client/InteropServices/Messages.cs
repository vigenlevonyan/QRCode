using Microsoft.JSInterop;

namespace QShirt.Public.Client.InteropServices;

/// <summary>
/// Displays messages through toast and swal
/// </summary>
public class Messages
{
    #region Fields

    private readonly IJSRuntime jsRuntime;

    #endregion Fields

    #region Constructor

    public Messages(IJSRuntime jsRuntime) => this.jsRuntime = jsRuntime;

    #endregion Constructor

    #region Methods

    /// <summary>
    /// Displays error message
    /// </summary>
    /// <param name="message">message text</param>
    public async ValueTask ShowError(string message) => await jsRuntime.InvokeAsync<object>("Messages.showError", message);

    /// <summary>
    /// Displays success message
    /// </summary>
    /// <param name="message">message text</param>
    public async ValueTask ShowSuccess(string message) => await jsRuntime.InvokeAsync<object>("Messages.showSuccess", message);

    /// <summary>
    /// Displays success dialog
    /// </summary>
    /// <param name="message">message text</param>
    public async ValueTask ShowSuccessModal(string message) => await jsRuntime.InvokeAsync<object>("Messages.showSuccessModal", message);

    /// <summary>
    /// Displays warning
    /// </summary>
    /// <param name="message">message text</param>
    public async ValueTask ShowWarning(string message) => await jsRuntime.InvokeAsync<object>("Messages.showWarning", message);

    /// <summary>
    /// Displays information message
    /// </summary>
    /// <param name="message">message text</param>
    public async ValueTask ShowInfo(string message) => await jsRuntime.InvokeAsync<object>("Messages.showInfo", message);

    /// <summary>
    /// Displays confirmation
    /// </summary>
    /// <param name="message">message text</param>
    public ValueTask<bool> ShowConfirm(string message) => jsRuntime.InvokeAsync<bool>("Messages.showConfirm", message);

    #endregion Methods
}
