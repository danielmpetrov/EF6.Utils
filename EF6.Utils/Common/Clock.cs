using System;

namespace EF6.Utils.Common
{
    internal class Clock : IClock
    {
        public DateTime Now() => DateTime.Now;
    }
}
