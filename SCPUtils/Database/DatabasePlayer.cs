

namespace SCPUtils
{
    public static class DatabasePlayer
    {
        public static string GetAuthentication(this Exiled.API.Features.Player player)
        {
            return player.UserId.Split('@')[1];
        }

        public static string GetRawUserId(this Exiled.API.Features.Player player)
        {
            return player.UserId.GetRawUserId();
        }

        public static string GetRawUserId(this string player)
        {
            return player.Split('@')[0];
        }

        public static Player GetDatabasePlayer(this string player)
        {
            return Exiled.API.Features.Player.Get(player)?.GetDatabasePlayer() ??
                Database.LiteDatabase.GetCollection<Player>().FindOne(queryPlayer => queryPlayer.Id == player.GetRawUserId() || queryPlayer.Name == player);
        }

        public static Player GetDatabasePlayer(this Exiled.API.Features.Player player)
        {
            if (player == null)
            {
                return null;
            }
            else if (Database.PlayerData.TryGetValue(player, out Player databasePlayer))
            {
                return databasePlayer;
            }
            else
            {
                return Database.LiteDatabase.GetCollection<Player>().FindOne(queryPlayer => queryPlayer.Id == player.GetRawUserId());
            }
        }

    }
}
