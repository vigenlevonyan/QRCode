using System;

namespace QShirt.Persistence
{
    public interface IEntityBase
    {
        public Guid Id { get; set; }
    }
}