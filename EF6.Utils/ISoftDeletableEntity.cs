using System;

namespace EF6.Utils
{
    public interface ISoftDeletableEntity
    {
        DateTime? DeletedOn { get; set; }
    }
}
