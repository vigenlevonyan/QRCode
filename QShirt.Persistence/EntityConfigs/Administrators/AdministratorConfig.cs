using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QShirt.Domain;

namespace QShirt.Persistence.EntityConfigs.Customers;

/// <summary>
/// Customer config
/// </summary>    
internal class AdministratorConfig : EntityTypeConfigurationBase<Administrator>
{
    public override void ConfigureMore(EntityTypeBuilder<Administrator> builder)
    {
       
    }
}
