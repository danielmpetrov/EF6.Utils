using System;

namespace EF6.Utils.Demo.Data
{
    public class Comment : ITimestampedEntity, ISoftDeletableEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
