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

        public float SuicidePercentage => (float)ScpSuicideCount == 0 ? 0 : ((float)ScpSuicideCount / (float)TotalScpGamesPlayed) * 100;

        public void Reset()
        {
            ScpSuicideCount = 0;
            TotalScpSuicideKicks = 0;
            TotalScpSuicideBans = 0;
            TotalScpGamesPlayed = 0;
        }
    }
}
