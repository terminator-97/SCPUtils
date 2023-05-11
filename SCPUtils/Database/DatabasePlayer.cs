using Exiled.API.Extensions;
using MongoDB.Driver;
using System.Linq;
using ExiledPlayer = Exiled.API.Features.Player;

namespace SCPUtils
{
    public static class DatabasePlayer
    {
        public static IMongoClient MongoClient { get; private set; }

        public static IMongoDatabase MongoDatabase { get; private set; }


        public static string GetAuthentication(this Exiled.API.Features.Player player)
        {
            return player.UserId.Split('@')[1];
        }

        public static string GetRawUserId(this Exiled.API.Features.Player player)
        {
            return player.UserId.GetRawUserId();
        }

    

        public static Player GetDatabasePlayer(this string player)
        {
             return ExiledPlayer.Get(player)?.GetDatabasePlayer() ?? MongoDatabase.GetCollection<Player>("players")
                .Find(x => x.Id == player.GetRawUserId() || x.Name.Contains(player)).FirstOrDefault(); 
         
        }

        public static Player GetDatabasePlayer(this ExiledPlayer player)
        {
            if (player == null) return null;
            return MongoDatabase.GetCollection<Player>("players").Find(x => x.Id == x.Id.GetRawUserId()).FirstOrDefault();
        }   

    }
}
