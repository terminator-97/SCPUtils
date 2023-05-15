namespace SCPUtils
{
    using MongoDB.Driver;
    using System.Linq;
    using static SCPUtils.Database;

    public static class GetBroadcast
    {

        public static BroadcastDb FindBroadcast(string id)
        {

            if (id == null) return null;

            return MongoDatabase.GetCollection<BroadcastDb>("broadcasts").Find(x => x.Id.ToLower() == id.ToLower()).FirstOrDefault();
        }

        public static void AddBroadcast(string id, int duration, string text, string creator)
        {
            var bc = new BroadcastDb
            {
                Id = id,
                CreatedBy = creator,
                Seconds = duration,
                Text = text,
            };

            MongoDatabase.GetCollection<BroadcastDb>("broadcasts").InsertOne(bc);
        }
    }
}
