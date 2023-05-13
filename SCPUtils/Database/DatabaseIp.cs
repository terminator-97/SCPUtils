using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;

namespace SCPUtils
{
    public class DatabaseIp
    {
        [BsonId]
        public string Id { get; set; }
        public List<string> UserIds { get; set; } = new List<string>();

        public ReplaceOneResult SaveIp()
        {
            return Database.MongoDatabase.GetCollection<DatabaseIp>("ipaddresses").ReplaceOne(x => x.Id == Id, this, new ReplaceOptions() { IsUpsert = true });
        }



    }
}