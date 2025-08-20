using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QShirt.Domain;

namespace QShirt.Persistence.EntityConfigs;

internal abstract class EntityTypeConfigurationBase<T> : IEntityTypeConfiguration<T> where T : EntityBase
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        ConfigureMore(builder);
    }

    public abstract void ConfigureMore(EntityTypeBuilder<T> builder);
}
