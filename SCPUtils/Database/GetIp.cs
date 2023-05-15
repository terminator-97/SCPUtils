namespace SCPUtils
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using static SCPUtils.Database;
    using NWPlayer = PluginAPI.Core.Player;

    public static class GetIp
    {
        public static DatabaseIp GetIpAddress(string ip)
        {

            if (ip == null) return null;

            return MongoDatabase.GetCollection<DatabaseIp>("ipaddresses").Find(x => x.Id == ip).FirstOrDefault();
        }



        public static void AddIp(this NWPlayer player)
        {
            var newIp = new DatabaseIp
            {
                Id = player.IpAddress,
                UserIds = new List<string>() { player.UserId }
            };

            MongoDatabase.GetCollection<DatabaseIp>("ipaddresses").InsertOne(newIp);
        }

    }
}
