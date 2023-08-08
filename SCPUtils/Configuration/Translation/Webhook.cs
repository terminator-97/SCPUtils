namespace SCPUtils.Config
{
    using System.ComponentModel;

    public class Webhook
    {
        [Description("Discord webhook url for mute evasion reports")]
        public string Url { get; private set; } = "None";

        [Description("Discord webhook configs")]
        public string Nickname { get; private set; } = "SCPUTILS Evasion Report System";
        public string EmbedTitle { get; private set; } = "Mute evasion report!";
        public string EmbedContent { get; private set; } = "SteamID64 of muted user: $muted\n\nPlayer info\n\n\nUsername: $username\nSteamID64: $steamid\nPlayerID: $playerid";
        public int EmbedColor { get; private set; } = 25565;
    }
}