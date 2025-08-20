using System.ComponentModel.DataAnnotations;

namespace QShirt.Domain;

public abstract class EntityBase : IEntityBase<Guid>
{
    /// <summary>
    /// Entity ID
    /// </summary>
    [Display(Name = "Id")]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

}
