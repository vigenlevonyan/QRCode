using Microsoft.EntityFrameworkCore;
using QShirt.Persistence.EntityConfigs.Customers;

namespace QShirt.Persistence;

public class QShirtContext : DbContext
{
    #region Constructors

    public QShirtContext(DbContextOptions<QShirtContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    #endregion Constructors

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerConfig).Assembly);
    }
}
