using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace QShirt.Persistence
{
    /// <summary>
    /// Changes saver
    /// </summary>
    /// <remarks>
    /// Saves all changes made in the db context to the database
    /// </remarks>
    public class ChangesSaver<TDbContext> : IChangesSaver where TDbContext : DbContext
    {
        #region Constructor

        public ChangesSaver(TDbContext context)
        {
            this.context = context;
        }

        #endregion Constructor

        #region Fields

        private readonly TDbContext context;

        #endregion Fields

        /// <summary>
        /// Saves all changes made
        /// </summary>
        public void SaveChanges()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Saves all changes made asynchronously
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}


