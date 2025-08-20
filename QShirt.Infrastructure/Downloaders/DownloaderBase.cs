using System.Net;
using System.Threading.Tasks;

namespace QShirt.Infrastructure.Downloaders
{
    /// <summary>
    /// Allows finding and downloading a file from a link
    /// </summary>
    public class DownloaderBase
    {
        /// <summary>
        /// Gets file from link
        /// </summary>
        /// <param name="url">image link</param>
        public async Task<byte[]> GetFileData(string url)
        {
            byte[] data;
            using (var client = new WebClient())
                data = Task.Run(() => client.DownloadDataTaskAsync(url)).GetAwaiter().GetResult();

            return data;
        }

    }
}
