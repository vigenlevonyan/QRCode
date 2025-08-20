using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QShirt.Persistence
{
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<QShirtContext>
    {
        public QShirtContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<QShirtContext>();
            string connectionString =
            //"Data Source=37.186.70.82;Initial Catalog=QShirt_test;User Id=sa;Password=#QShirtLevonyan1984!;";
            "Server=(localdb)\\MSSQLLocalDB;Database=QShirt_Dev;Trusted_Connection=True;";
            //"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
            //"Server=Home;Database=QShirt_Dev;Trusted_Connection=True;";

            builder.UseSqlServer(connectionString);

            return new QShirtContext(builder.Options);
        }
    }
}
