using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QShirt.Domain;

namespace QShirt.Persistence.EntityConfigs.Customers;

/// <summary>
/// Customer Content Config
/// </summary>    
internal class CustomerContentConfig : EntityTypeConfigurationBase<CustomerContent>
{
    public override void ConfigureMore(EntityTypeBuilder<CustomerContent> builder)
    {
        
    }
}
