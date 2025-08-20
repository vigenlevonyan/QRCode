using QShirt.Application.Contracts.Infrastructure;
using System;
using System.Drawing;
using System.IO;

namespace QShirt.Infrastructure.Downloaders
{
    /// <summary>
    /// Allows finding and downloading an image from a link
    /// </summary>
    public class PictureFromLinkDownloader : DownloaderBase, IPictureFromLinkDownloader
    {
        /// <summary>
        /// Checks if byte stream is a valid image?
        /// </summary>
        /// <param name="bytes">file byte stream</param>
        public bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    Image.FromStream(ms);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}
