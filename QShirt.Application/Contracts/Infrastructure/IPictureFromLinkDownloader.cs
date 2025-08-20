using System.Threading.Tasks;

namespace QShirt.Application.Contracts.Infrastructure
{
    /// <summary>
    /// Allows finding and downloading an image from a link
    /// </summary>
    public interface IPictureFromLinkDownloader
    {
        /// <summary>
        /// Gets file from link
        /// </summary>
        /// <param name="url">image link</param>
        Task<byte[]> GetFileData(string url);

        /// <summary>
        /// Checks if byte stream is a valid image?
        /// </summary>
        /// <param name="bytes">file byte stream</param>
        bool IsValidImage(byte[] bytes);
    }
}
