using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace SCPUtils
{
    public class Player
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Authentication { get; set; }
        public string Ip { get; set; }
        public int ScpSuicideCount { get; set; }
        public int TotalScpGamesPlayed { get; set; }
        public int TotalScpSuicideKicks { get; set; }
        public int TotalScpSuicideBans { get; set; }
        public int RoundBanLeft { get; set; }
        public DateTime FirstJoin { get; set; }
        public DateTime LastSeen { get; set; }
        public string ColorPreference { get; set; }
        public string CustomNickName { get; set; }
        public bool HideBadge { get; set; }
        public bool MultiAccountWhiteList { get; set; }
        public string BadgeName { get; set; }
        public DateTime BadgeExpire { get; set; }
        public DateTime NicknameCooldown { get; set; }
        public string PreviousBadge { get; set; }
        public string CustomBadgeName { get; set; }
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<string, int> PlayTimeRecords { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> OwPlayTimeRecords { get; set; } = new Dictionary<string, int>();
        public bool ASNWhitelisted { get; set; }
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<DateTime, string> Restricted { get; set; } = new Dictionary<DateTime, string>();
        public bool KeepPreferences { get; set; }
        public bool StreamerMode { get; set; }
        [BsonIgnore]
        public float SuicidePercentage => (float)ScpSuicideCount == 0 ? 0 : (ScpSuicideCount / (float)TotalScpGamesPlayed) * 100;
        [BsonIgnore]
        public DateTime OwTime { get; set; }
        public bool IgnoreDNT { get; set; }
        public bool OverwatchActive { get; set; }
        //Suicide logs
        public List<DateTime> SuicideDate { get; set; } = new List<DateTime>();
        public List<string> SuicideType { get; set; } = new List<string>();
        public List<string> SuicideScp { get; set; } = new List<string>();
        public List<string> SuicidePunishment { get; set; } = new List<string>();
        public List<string> LogStaffer { get; set; } = new List<string>();
        public List<bool> UserNotified { get; set; } = new List<bool>();
        public List<DateTime> Expire { get; set; } = new List<DateTime>();
        public List<int> RoundsBan { get; set; } = new List<int>();




        public void SetCurrentDayPlayTime()
        {
            if (PlayTimeRecords == null)
            {
                PlayTimeRecords.Add(DateTime.Now.Date.ToShortDateString(), 0);
            }
            if (!PlayTimeRecords.ContainsKey(DateTime.Now.Date.ToShortDateString()))
            {
                PlayTimeRecords.Add(DateTime.Now.Date.ToShortDateString(), 0);
            }

            PlayTimeRecords[DateTime.Now.Date.ToShortDateString()] += (int)(DateTime.Now - LastSeen).TotalSeconds;

            LastSeen = DateTime.Now;

        }

        public void SetCurrentDayOwPlayTime()
        {
            if (OwTime == DateTime.MinValue) return;
            if (OwPlayTimeRecords == null)
            {
                OwPlayTimeRecords.Add(DateTime.Now.Date.ToShortDateString(), 0);
            }
            if (!OwPlayTimeRecords.ContainsKey(DateTime.Now.Date.ToShortDateString()))
            {
                OwPlayTimeRecords.Add(DateTime.Now.Date.ToShortDateString(), 0);
            }

            OwPlayTimeRecords[DateTime.Now.Date.ToShortDateString()] += (int)(DateTime.Now - OwTime).TotalSeconds;

            OwTime = DateTime.MinValue;
        }

        public void Reset()
        {
            ScpSuicideCount = 0;
            TotalScpSuicideKicks = 0;
            TotalScpSuicideBans = 0;
            TotalScpGamesPlayed = 0;
            ColorPreference = "";
            CustomNickName = "";
            HideBadge = false;
            BadgeName = "";
            PreviousBadge = "";
            KeepPreferences = false;
            PlayTimeRecords.Clear();
            // PlaytimeSessionsLog.Clear();
        }

        public void ResetPreferences()
        {
            ColorPreference = "";
            CustomNickName = "";
            HideBadge = false;
            KeepPreferences = false;
        }

        public bool IsRestricted()
        {
            foreach (KeyValuePair<DateTime, string> a in Restricted)
            {
                if (a.Key >= DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsBanned()
        {
            var bans = BanHandler.GetBans(BanHandler.BanType.UserId);
            foreach (var playerban in bans)
            {
                if (playerban.Id != Id + "@" + Authentication)
                    return false;
                if (BanHandler.CheckExpiration(playerban, BanHandler.BanType.UserId))
                    return true;
                return false;
            }
            return false;
        }

        public ReplaceOneResult SaveData()
        {

            return Database.MongoDatabase.GetCollection<Player>("players").ReplaceOne(x => x.Id == Id, this, new ReplaceOptions() { IsUpsert = true });
        }



    }
}
