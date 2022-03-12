using System.Collections.Generic;

namespace SCPUtils
{
    public class DatabaseIp
    {
        public string Id { get; set; }
        public List<string> UserIds { get; set; } = new List<string>();
    }
}