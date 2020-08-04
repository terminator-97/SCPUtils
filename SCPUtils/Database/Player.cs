using System;
using System.Collections.Generic;
using Log = Exiled.API.Features.Log;

namespace SCPUtils
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Authentication { get; set; }
        public int ScpSuicideCount { get; set; }
        public int TotalScpGamesPlayed { get; set; }
        public int TotalScpSuicideKicks { get; set; }
        public int TotalScpSuicideBans { get; set; }
        public DateTime FirstJoin { get; set; }
        public DateTime LastSeen { get; set; }
        public string ColorPreference { get; set; }
        public string CustomNickName { get; set; }
        public bool HideBadge { get; set; }
        public string BadgeName { get; set; }
        public DateTime BadgeExpire { get; set; }
        public Dictionary<string, int> PlayTimeRecords { get; set; } = new Dictionary<string, int>();

        public bool ASNWhitelisted { get; set; }

        public float SuicidePercentage => (float)ScpSuicideCount == 0 ? 0 : ((float)ScpSuicideCount / (float)TotalScpGamesPlayed) * 100;



        public void SetCurrentDayPlayTime()
        {
            if (!PlayTimeRecords.ContainsKey(DateTime.Now.Date.ToShortDateString())) PlayTimeRecords.Add(DateTime.Now.Date.ToShortDateString(), 0);
            PlayTimeRecords[DateTime.Now.Date.ToShortDateString()] += (int)(DateTime.Now - LastSeen).TotalSeconds;
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
        }

        public void ResetPreferences()
        {
            ColorPreference = "";
            CustomNickName = "";
            HideBadge = false;
        }

    }
}
