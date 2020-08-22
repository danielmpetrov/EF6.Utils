using System;

namespace EF6.Utils
{
    public interface ITimestampedEntity
    {
        DateTime CreatedOn { get; set; }

        DateTime UpdatedOn { get; set; }
    }
}
