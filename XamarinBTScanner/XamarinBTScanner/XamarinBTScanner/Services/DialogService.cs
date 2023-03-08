using System.Threading.Tasks;
using Acr.UserDialogs;
using XamarinBTScanner.Contracts.Services;

namespace XamarinBTScanner.Services;

public class DialogService : IDialogService
{
    #region Methods

    /// <summary>
    /// Show alert.
    /// </summary>
    /// <param name="message">Alert message.</param>
    /// <returns>Awaitable.</returns>
    public Task ShowAlertAsync(string message)
    {
        return InternalShowAlertAsync(message,
            "Turtle Scanner",
            "Ok");
    }

    /// <summary>
    /// Show alert.
    /// </summary>
    /// <param name="message">Alert message.</param>
    /// <param name="title">Title.</param>
    /// <returns>Awaitable.</returns>
    public Task ShowAlertAsync(string message, string title)
    {
        return InternalShowAlertAsync(message,
            title,
            "Ok");
    }

    /// <summary>
    /// Show alert.
    /// </summary>
    /// <param name="message">Alert message.</param>
    /// <param name="title">Title.</param>
    /// <param name="buttonText">Button text.</param>
    /// <returns>Awaitable.</returns>
    public Task ShowAlertAsync(string message, string title, string buttonText)
    {
        return InternalShowAlertAsync(message,
            title,
            buttonText);
    }

    /// <summary>
    /// Show alert.
    /// </summary>
    /// <param name="message">Alert message.</param>
    /// <param name="title">Title.</param>
    /// <param name="buttonText">Button text.</param>
    /// <returns>Awaitable.</returns>
    private static async Task InternalShowAlertAsync(string message, string title, string buttonText)
    {
        await UserDialogs.Instance.AlertAsync(message,
            title,
            buttonText);
    }

    #endregion
}