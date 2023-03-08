using System.Threading.Tasks;

namespace XamarinBTScanner.Contracts.Services;

public interface IDialogService
{
    #region Methods

    /// <summary>
    /// Show alert.
    /// </summary>
    /// <param name="message">Alert message.</param>
    /// <returns>Awaitable.</returns>
    Task ShowAlertAsync(string message);

    /// <summary>
    /// Show alert.
    /// </summary>
    /// <param name="message">Alert message.</param>
    /// <param name="title">Title.</param>
    /// <returns>Awaitable.</returns>
    Task ShowAlertAsync(string message, string title);

    /// <summary>
    /// Show alert.
    /// </summary>
    /// <param name="message">Alert message.</param>
    /// <param name="title">Title.</param>
    /// <param name="buttonText">Button text.</param>
    /// <returns>Awaitable.</returns>
    Task ShowAlertAsync(string message, string title, string buttonText);

    #endregion
}