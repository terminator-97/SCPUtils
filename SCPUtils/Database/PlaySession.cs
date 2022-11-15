using System;
using System.Collections.Generic;

namespace SCPUtils
{
    public class PlaySession
    {
        public string Id { get; set; }
        public string Authentication { get; set; }

        public Dictionary<DateTime, DateTime> PlaytimeSessionsLog { get; set; } = new Dictionary<DateTime, DateTime>();
    }
}
