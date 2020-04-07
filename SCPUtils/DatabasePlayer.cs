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
            Player databasePlayer = Database.LiteDatabase.GetCollection<Player>().FindOne(tempPlayer => tempPlayer.Id == player.GetRawUserId() || tempPlayer.Name == player);

            if (databasePlayer != null) return databasePlayer;

            return EXILED.Extensions.Player.GetPlayer(player)?.GetDatabasePlayer();
        }

        public static Player GetDatabasePlayer(this ReferenceHub player)
        {
            return player != null && Database.PlayerData.TryGetValue(player, out Player databasePlayer) ? databasePlayer : null;
        }
    }
}
