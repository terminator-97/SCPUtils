using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using static SCPUtils.Database;
using ExiledPlayer = Exiled.API.Features.Player;

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


            var onlinePlayer = ExiledPlayer.Get(player);
            if (onlinePlayer == null)
            {
                return MongoDatabase.GetCollection<Player>("players").Find(x => x.Id == player.GetRawUserId() || x.Name.ToLower() == player.ToLower()).FirstOrDefault();
            }
            else
            {
                if (Database.PlayerData.TryGetValue(onlinePlayer, out Player databasePlayer))
                {
                    return databasePlayer;
                }
                else
                {
                    return MongoDatabase.GetCollection<Player>("players").Find(x => x.Id == onlinePlayer.RawUserId).FirstOrDefault();
                }
            }
        }

        public static Player GetDatabasePlayer(this ExiledPlayer player)
        {
            if (player == null) return null;
            if (Database.PlayerData.TryGetValue(player, out Player databasePlayer))
            {
                return databasePlayer;
            }
            else
            {
                return MongoDatabase.GetCollection<Player>("players").Find(x => x.Id == player.RawUserId).FirstOrDefault();
            }
        }

        public static void AddPlayer(this ExiledPlayer player)
        {
            var newPlayer = new Player
            {
                Id = DatabasePlayer.GetRawUserId(player),
                Name = player.Nickname,
                Ip = "None",
                Authentication = DatabasePlayer.GetAuthentication(player),
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
                OwPlayTimeRecords = new Dictionary<string, int>(),
                ASNWhitelisted = false,
                Restricted = new Dictionary<DateTime, string>(),
                KeepPreferences = false,
                IgnoreDNT = false,
                // PlaytimeSessionsLog = null,
                Expire = new List<DateTime>(),
                MultiAccountWhiteList = false,
                NicknameCooldown = DateTime.Now,
                OverwatchActive = false,
                CustomBadgeName = ""
            };
            MongoDatabase.GetCollection<Player>("players").InsertOne(newPlayer);
        }

    }
}
