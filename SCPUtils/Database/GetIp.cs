using MongoDB.Driver;

namespace SCPUtils
{
    public static class GetIp
    {
        public static DatabaseIp GetIpAddress(string ip) => (DatabaseIp)Database.MongoDatabase.GetCollection<DatabaseIp>("ipaddresses").Find(queryIp => queryIp.Id == ip);     

    }
}
