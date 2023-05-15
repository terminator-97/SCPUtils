namespace SCPUtils
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static SCPUtils.Database;
    using NWPlayer = PluginAPI.Core.Player;

    public static class DatabasePlayer
    {


        public static string GetAuthentication(this NWPlayer player)
        {
            return player.UserId.Split('@')[1];
        }


        public static string GetRawUserId(this NWPlayer player)
        {
            return player.UserId.GetRawUserId();
        }

        public static string GetRawUserId(this string player)
        {
            return player.Split('@')[0];
        }

        public static Player GetDatabasePlayer(this string player)
        {


            var onlinePlayer = NWPlayer.Get(player);
            if (onlinePlayer == null)
            {
                return MongoDatabase.GetCollection<Player>("players").Find(x => x.Id == player.GetRawUserId() || x.Name.ToLower() == player.ToLower()).FirstOrDefault();
            }
            else
            {
                if (PlayerData.TryGetValue(onlinePlayer, out Player databasePlayer))
                {
                    return databasePlayer;
                }
                else
                {
                    return MongoDatabase.GetCollection<Player>("players").Find(x => x.Id == onlinePlayer.GetRawUserId()).FirstOrDefault();
                }
            }
        }

        public static Player GetDatabasePlayer(this NWPlayer player)
        {
            if (player == null) return null;
            if (Database.PlayerData.TryGetValue(player, out Player databasePlayer))
            {
                return databasePlayer;
            }
            else
            {
                return MongoDatabase.GetCollection<Player>("players").Find(x => x.Id == player.GetRawUserId()).FirstOrDefault();
            }
        }

        public static void AddPlayer(this NWPlayer player)
        {
            var newPlayer = new Player
            {
                Id = GetRawUserId(player),
                Name = player.Nickname,
                Ip = "None",
                Authentication = GetAuthentication(player),
                ScpSuicideCount = 0,
                TotalScpGamesPlayed = 0,
                TotalScpSuicideKicks = 0,
                TotalScpSuicideBans = 0,
                RoundBanLeft = 0,
                FirstJoin = DateTime.Now,
                LastSeen = DateTime.Now,
                ColorPreference = "",
                CustomNickName = "",
                BadgeName = "",
                BadgeExpire = DateTime.MinValue,
                PreviousBadge = "",
                HideBadge = false,
                PlayTimeRecords = new Dictionary<string, int>(),
                ASNWhitelisted = false,
                Restricted = new Dictionary<DateTime, string>(),
                KeepPreferences = false,
                IgnoreDNT = false,
                // PlaytimeSessionsLog = null,
                Expire = new List<DateTime>(),
                MultiAccountWhiteList = false,
                NicknameCooldown = DateTime.Now,
                OverwatchActive = false
            };
            MongoDatabase.GetCollection<Player>("players").InsertOne(newPlayer);
        }

    }
}
