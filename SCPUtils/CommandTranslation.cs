namespace SCPUtils.Commands
{
    using System.ComponentModel;

    public class CommandTranslation
    {
        public string SenderError { get; set; } = "<color=red>You need \"<b>%permission%</b>\" to use this command!</color>";
        public string PlayerDatabaseError { get; set; } = "Player not found on Database!";
        public string Integer { get; set; } = "This argument (%argument%) is not an integer!";

        [Description("Announce command")]
        public string IdExist { get; set; } = "Id already exist!";
        public string AnnounceSuccess { get; set; } = "Success!";
        public string IdNotExist { get; set; } = "Id does not exist!";
        public string Sending { get; set; } = "Sending %type% to all players...";
        public string InvalidType { get; set; } = "Invalid type! Check it.";

        [Description("%player% arguments are: Nickname [SteamID64]\n# ASN command")]
        public string AsnWhitelist { get; set; } = "%player% has been added to Whitelist.";
        public string AsnWhitelistError { get; set; } = "%player% is already in Whitelist.";
        public string AsnUnwhitelist { get; set; } = "%player% has been removed to Whitelist.";
        public string AsnUnwhitelistError { get; set; } = "%player% is not in Whitelist.";
        [Description("Badge command")]
        public string BadgeSet { get; set; } = "Successfully set <color=%badgeColor%>%badgeName%<color> to %player%.\n\tDuration: %time%";
        public string BadgeRevoke { get; set; } = "Badge revoked!";
        public string BadgeNameError { get; set; } = "Invalid role name, check it on 'groups' in console.";
        public string BadgeDurationInvalid { get; set; } = "Argument \"[Time in day:minutes:seconds]\" is invalid! Check it.";
        public string BadgeKickPower { get; set; } = "You need a higher administration level to use this command: the group you are trying to set has more kick power than yours.\n\tYour 'Kick Power': %kickPower%\n\tRequired: %groupKickPower%)";

        [Description("PlayTime command")]
        public string DaysInteger { get; set; } = "<color=red>You have to specify a number higher than 0 or is not an integer</color>";
        public string DaysMaximus { get; set; } = "<color=red>You can specify a range of max 120 days!</color>";
        public string NoPlayerBadge { get; set; } = "No players found on specified badge!";
        public string PlaytimeNoActivity { get; set; } = "Playtime: [No activity]";
        public string Playtime { get; set; } = "Playtime: [%playtime%]";
        public string TotalPlaytime { get; set; } = "Total playtime: [%playtime%]";
        public string SpecifiedPlaytime { get; set; } = "Specified Period PlayTime: [%totalPlaytime%]";
        public string DaysJoined { get; set; } = "Days joined: [%daysJoined%/%totalDays%]";
        public string BadgePlaytime { get; set; } = "Checking playtime of badge %badge%...";

        [Description("Definitions")]
        public string Player { get; set; } = "[PlayerID, PlayerNickname or PlayerSteamID64]";
        public string Server { get; set; } = "[SERVER]";
        public string Plugin { get; set; } = "[SCPUtils]";
        public string Staff { get; set; } = "[STAFF]";
        public string Badge { get; set; } = "[Badge Name]";
        public string Announce { get; set; } = "[AnnounceID (for scputils broadcast create), AnnounceName]";
        public string Time { get; set; } = "[Time in day:minutes:seconds]";
        public string Days { get; set; } = "[Days]";
        public string Seconds { get; set; } = "[Seconds]";
        public string AnnounceMessage { get; set; } = "[Announcement Message]";
        public string AnnounceType { get; set; } = "[Broadcast/Bc, Hint/H or Console/C]";
        public string Broadcast { get; set; } = "[BROADCAST]";
        public string Hint { get; set; } = "[HINT]";
        public string Console { get; set; } = "[CONSOLE MESSAGE]";

        [Description("ListOnlineCommand and ListStaffCommand")]
        public string Hp { get; set; } = "[HEALTH]";
        public string Ahp { get; set; } = "[ARTIFICIAL HEALTH]";
        public string Overwatch { get; set; } = "[OVERWATCH ENABLED]";
        public string Noclip { get; set; } = "[NOCLIP ENABLED]";
        public string God { get; set; } = "[GODMODE ENABLED]";
        public string Bypass { get; set; } = "[INTERCOM AND BYPASS ENABLED]";
        public string Intercom { get; set; } = "[INTERCOM MUTED]";
        public string VoiceChat { get; set; } = "[VOICE CHAT MUTED]";
        public string Dnt { get; set; } = "[DO NOT TRACK ENABLED]";
        public string Ra { get; set; } = "[REMOTED ADMIN AUTHENTICATED]";

        [Description("%role% is scpsl staff role")]
        public string SlStaff { get; set; } = "[SCP:SL STAFF: %role%]";
        public string NoPlayers { get; set; } = "No players online!";
        public string NoStaff { get; set; } = "No staffers online!";

        [Description("\"%arguments%\" will get usage this definition ↑")]
        public string UsageError { get; set; } = $"<color=yellow>Usage: %command% %arguments%</color>";
    }
}