namespace QShirt.Application.Contracts.Infrastructure.MessageFormers
{
    /// <summary>
    /// Email message text formatter for customer order download
    /// </summary>
    public interface IOrderForDownloadEmailMessageFormer
    {
        /// <summary>
        /// Email message text formatter for customer order download
        /// </summary>
        string Form(string downloadKey, string congratulation, string customerName);
    }
}
