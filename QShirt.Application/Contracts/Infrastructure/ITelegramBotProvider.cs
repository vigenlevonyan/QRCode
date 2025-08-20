using System.Threading.Tasks;

namespace QShirt.Application.Contracts.Infrastructure
{
    public interface ITelegramBotProvider
    {
        ///// <summary>
        ///// Send message
        ///// </summary>
        Task<string> SendTextMessage(string message);

        /// <summary>
        /// Send document
        /// </summary>
        Task SendDocument(string filePath, string fileName);
    }
}
