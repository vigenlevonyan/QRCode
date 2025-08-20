
using System.Threading.Tasks;

namespace QShirt.Persistence
{
    /// <summary>
    /// Changes saver
    /// </summary>
    public interface IChangesSaver
    {
        /// <summary>
        /// Saves all changes made
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves all changes made asynchronously
        /// </summary>
        Task SaveChangesAsync();
    }
}