using MongoDB.Driver;
using System.Linq;

namespace SCPUtils
{
    public static class GetBroadcast
    {
        public static BroadcastDb FindBroadcast(string id) => (BroadcastDb)Database.MongoDatabase.GetCollection<BroadcastDb>("broadcasts").Find(queryBc => queryBc.Id == id);
    }
}
