using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QShirt.Domain;

namespace QShirt.Persistence.EntityConfigs.Customers;

/// <summary>
/// Customer config
/// </summary>    
internal class CustomerConfig : EntityTypeConfigurationBase<Customer>
{
    public override void ConfigureMore(EntityTypeBuilder<Customer> builder)
    {
        builder.HasMany(i => i.Contents)
           .WithOne(i => i.Customer)
           .HasForeignKey(i => i.CustomerId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
