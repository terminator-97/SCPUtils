using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCPUtils
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Authentication { get; set; }
        public string Ip { get; set; }
        public int ScpSuicideCount { get; set; }
        public int TotalScpGamesPlayed { get; set; }
        public int TotalScpSuicideKicks { get; set; }
        public int TotalScpSuicideBans { get; set; }
        public DateTime FirstJoin { get; set; }
        public DateTime LastSeen { get; set; }
        public string ColorPreference { get; set; }
        public string CustomNickName { get; set; }
        public bool HideBadge { get; set; }
        public bool isMuted { get; set; }
        public string BadgeName { get; set; }
        public DateTime BadgeExpire { get; set; }
        public string PreviousBadge { get; set; }
        public Dictionary<string, int> PlayTimeRecords { get; set; } = new Dictionary<string, int>();
        public bool ASNWhitelisted { get; set; }
        public Dictionary<DateTime, string> Restricted { get; set; } = new Dictionary<DateTime, string>();
        public bool KeepPreferences { get; set; }
        public float SuicidePercentage => (float)ScpSuicideCount == 0 ? 0 : (ScpSuicideCount / (float)TotalScpGamesPlayed) * 100;
        public bool IgnoreDNT { get; set; }
        public Dictionary<DateTime, DateTime> PlaytimeSessions { get; set; } = new Dictionary<DateTime, DateTime>();

        //Suicide logs
        public List<DateTime> SuicideDate { get; set; } = new List<DateTime>();
        public List<string> SuicideType { get; set; } = new List<string>();
        public List<string> SuicideScp { get; set; } = new List<string>();
        public List<string> SuicidePunishment { get; set; } = new List<string>();
        public List<string> LogStaffer { get; set; } = new List<string>();
        public List<bool> UserNotified { get; set; } = new List<bool>();


        public void SetCurrentDayPlayTime()
        {
            if (!PlayTimeRecords.ContainsKey(DateTime.Now.Date.ToShortDateString()))
            {
                PlayTimeRecords.Add(DateTime.Now.Date.ToShortDateString(), 0);
            }

            PlayTimeRecords[DateTime.Now.Date.ToShortDateString()] += (int)(DateTime.Now - LastSeen).TotalSeconds;

            if (!DateTime.TryParse(LastSeen.ToString(), out DateTime o)) return;
            PlaytimeSessions.Add(LastSeen, DateTime.Now);

            try
            {
                foreach(var a in PlaytimeSessions)
                {
                    if (!DateTime.TryParse(a.Key.ToString(), out DateTime output)) PlaytimeSessions = null;
                    if (!DateTime.TryParse(a.Value.ToString(), out DateTime output2)) PlaytimeSessions = null;
                }
            }
            catch(Exception e)
            {
                PlaytimeSessions = null;
                Log.Error("[SCPUTILS] Error: "+e);
            }
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
            PlaytimeSessions.Clear();
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
                if (playerban.Id != Id +"@"+Authentication)
                    return false;
                if (BanHandler.CheckExpiration(playerban, BanHandler.BanType.UserId))
                    return true;
                return false;
            }
            return false;
        }
    }
}
