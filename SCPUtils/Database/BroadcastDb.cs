using MongoDB.Bson.Serialization.Attributes;

namespace SCPUtils
{
    public class BroadcastDb
    {
        
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public int Seconds { get; set; }
        public string Text { get; set; }
    }
}
