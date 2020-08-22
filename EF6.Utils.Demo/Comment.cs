﻿using System;

namespace EF6.Utils.Demo
{
    public class Comment : ITimestampedEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
