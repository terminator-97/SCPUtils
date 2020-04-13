using EXILED.Extensions;

namespace SCPUtils
{
    public static class DatabasePlayer
    {
        public static string GetAuthentication(this ReferenceHub player) => player.GetUserId().Split('@')[1];
        public static string GetRawUserId(this ReferenceHub player) => player.GetUserId().GetRawUserId();
        public static string GetRawUserId(this string player) => player.Split('@')[0];

        public static Player GetDatabasePlayer(this string player)
        {
            return EXILED.Extensions.Player.GetPlayer(player)?.GetDatabasePlayer() ??
                Database.LiteDatabase.GetCollection<Player>().FindOne(queryPlayer => queryPlayer.Id == player.GetRawUserId() || queryPlayer.Name == player);
        }

        public static Player GetDatabasePlayer(this ReferenceHub player)
        {
            if (player == null) return null;
            else if (Database.PlayerData.TryGetValue(player, out Player databasePlayer)) return databasePlayer;
            else return Database.LiteDatabase.GetCollection<Player>().FindOne(queryPlayer => queryPlayer.Id == player.GetRawUserId());
        }

    }
}
