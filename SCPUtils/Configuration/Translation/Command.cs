namespace SCPUtils.Config
{
    using System.ComponentModel;

    public class Command
    {
        public string SenderError { get; set; } = "<color=red>You need \"<b>%permission%</b>\" to use this command!</color>";
        public string PlayerDatabaseError { get; set; } = "Player not found on Database!";
        public string Integer { get; set; } = "This argument (%argument%) is not an integer!";

        [Description("Parent command")]
        public string ParentCommands { get; set; } = "Please specify a valid subcommand:";
        public string CommandName { get; set; } = "Command name: ";
        public string CommandDescription { get; set; } = "Command description: ";
        public string CommandAliases { get; set; } = "Command alias: ";

        [Description("Announce command")]
        public string IdExist { get; set; } = "Id already exist!";
        public string AnnounceSuccess { get; set; } = "Success!";
        public string IdNotExist { get; set; } = "Id does not exist!";
        public string Sending { get; set; } = "Sending %type% to all players...";
        public string InvalidType { get; set; } = "Invalid type! Check it.";
        public string List { get; set; } = "[Broadcast List]";
        public string ListId { get; set; } = "ID: %id%";
        public string ListSeconds { get; set; } = "Duration: %duration%";
        public string ListMessage { get; set; } = "Message: %text%";
        public string ListAuthor { get; set; } = "Author: %author%";
        public string ListSeparator { get; set; } = "-------";

        [Description("ASN command")]
        public string AsnWhitelist { get; set; } = "%player% has been added to Whitelist.";
        public string AsnWhitelistError { get; set; } = "%player% is already in Whitelist.";
        public string AsnUnwhitelist { get; set; } = "%player% has been removed to Whitelist.";
        public string AsnUnwhitelistError { get; set; } = "%player% is not in Whitelist.";

        [Description("Badge command")]
        public string BadgeSet { get; set; } = "Successfully set <color=%badgeColor%>%badgeName%<color> to %player%.\n\tDuration: %time%";
        public string BadgeRevoke { get; set; } = "Badge revoked!";
        public string BadgeNameError { get; set; } = "Invalid role name, check it on 'groups' in console.";
        public string BadgeDurationInvalid { get; set; } = "Argument \"[Time in day:minutes:seconds]\" is invalid! Check it.";
        public string BadgeKickPower { get; set; } = "You need a higher administration level to use this command: the group you are trying to set has more kick power than yours.\n\tYour 'Kick Power': %kickPower%\n\tRequired: %groupKickPower%";

        [Description("Ip command")]
        public string IpResponse { get; set; } = "[Accounts associated with the same IP: %ip% (%player%)]";
        public string IpPlayer { get; set; } = "Player: <color=#FADA5E>%player%</color>";
        public string IpFirstJoin { get; set; } = "First join: <color=#FADA5E>%firstJoin%</color>";
        public string IpLastJoin { get; set; } = "Last join: <color=#FADA5E>%lastJoin%</color>";
        public string IpRestricted { get; set; } = "Restricted: <color=#FADA5E>%restricted%</color>";
        public string IpBanned { get; set; } = "Banned: <color=#FADA5E>%banned%</color>";
        public string IpMuted { get; set; } = "Muted: <color=#FADA5E>%muted%</color>";
        public string IpTotalGames { get; set; } = "Total played as SCP: <color=#FADA5E>%games%</color>";
        public string IpTotalSuicide { get; set; } = "Total suicide: <color=#FADA5E>%suicide%</color>";
        public string IpRoundBans { get; set; } = "nRound(s) ban left: <color=#FADA5E>%rounds%</color>";
        public string IpInvalid { get; set; } = "<color=#FADA5E>Invalid IP!</color>";

        [Description("Player command: Delete")]
        public string DeleteResponse { get; set; } = "%player% has been deleted from the database!";

        [Description("Player command: DoNotTrack")]
        public string DntResponseTrue { get; set; } = "Success, ignore DNT has been enabled!";
        public string DntResponseFalse { get; set; } = "Success, ignore DNT has been disabled!";

        [Description("Player command: Edit")]
        public string EditResponse { get; set; } = "Successfully edit %player%! Modified:\n\t%totalGames%: %newTotalGames%\n\t%totalScpQuit%: %newTotalQuit%\n\t%totalKicks%: %newTotalKicks%\n\t%totalBans%: %newTotalBans%";

        [Description("Player command: Info")]
        public string InfoPlayer { get; set; } = "[%player%]";
        public string InfoPlayerTotalQuit { get; set; } = "Total SCP Suicides/Quits: [ %total% ]";
        public string InfoPlayerQuitKick { get; set; } = "Total SCP Suicides/Quits Kicks: [ %total% ]";
        public string InfoPlayerQuitBans { get; set; } = "Total SCP Suicides/Quits Bans: [ %total% ]";
        public string InfoPlayerGames { get; set; } = "Total Games played as SCP: [ %total% ]";
        public string InfoPlayerPercentage { get; set; } = "Total Suicides/Quits Percentage: [ %percentage% ]";
        public string InfoPlayerFirst { get; set; } = "First Join: [ %firstJoin% ]";
        public string InfoPlayerLast { get; set; } = "Last Seen: [ %lastJoin% ]";
        public string InfoPlayerCustomColor { get; set; } = "Custom Color: [ %color% ]";
        public string InfoPlayerCustomName { get; set; } = "Custom Nickname: [ %name% ]";
        //public string InfoPlayerCustomInfo { get; set; } = "Custom Info: [ %info% ]";
        public string InfoPlayerBadge { get; set; } = "Temporarily Badge: [ %badge% ]";
        public string InfoPlayerExpire { get; set; } = "Badge Expire: [ %expire% ]";
        public string InfoPlayerHide { get; set; } = "Hide Badge: [ %hide% ]";
        public string InfoPlayerWhitelist { get; set; } = "Asn Whitelisted: [ %whitelist% ]";
        public string InfoPlayerFlag { get; set; } = "Keep Flag: [ %flag% ]";
        public string InfoPlayerDnt { get; set; } = "Ignore DNT: [ %dnt% ]";
        public string InfoPlayerMultiAccount { get; set; } = "MultiAccount Whitelist: [ %multiaccount% ]";
        public string InfoPlayerPlaytime { get; set; } = "Total Playtime: [ %playtime% ]";
        public string InfoPlayerCooldown { get; set; } = "Nickname cooldown: [ %cooldown% ]";
        public string InfoPlayerOverwatch { get; set; } = "Overwatch active: [ %overwatch% ]";
        public string InfoPlayerRestrict { get; set; } = "<color=red>User account is currently restricted</color>\n\tReason: %reason%\n\tExpire: %expire%";
        public string InfoPlayerScpBan { get; set; } = "<color=red>User account is currently SCP-Banned:</color>\n\tRound left: %rounds%";

        [Description("Player command: List")]
        public string ListQuitSuicide { get; set; } = "[List of all total Quit/Suicide percetage: %percentage%%]";

        [Description("Player command: Reset")]
        public string ResetResponse { get; set; } = "%player% has been reset!";

        [Description("Player command: Restrict")]
        public string AlreadyRestrict { get; set; } = "%player% is already restricted!";
        public string AlreadyUnrestrict { get; set; } = "%player% is not restricted!";
        public string RestrictionResponse { get; set; } = "%player% suspended!";
        public string UnrestrictionResponse { get; set; } = "%player% unsuspended!";

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
        public string Reason { get; set; } = "[Reason]";
        public string Announce { get; set; } = "[AnnounceID (for scputils broadcast create), AnnounceName]";
        public string Time { get; set; } = "[Time in day:minutes:seconds]";
        public string Days { get; set; } = "[Days]";
        public string Seconds { get; set; } = "[Seconds]";
        public string AnnounceMessage { get; set; } = "[Announcement Message]";
        public string AnnounceType { get; set; } = "[Broadcast/Bc, Hint/H or Console/C]";
        public string Broadcast { get; set; } = "[BROADCAST]";
        public string Hint { get; set; } = "[HINT]";
        public string Console { get; set; } = "[CONSOLE MESSAGE]";
        public string MinimQuitSuicide { get; set; } = "[Minimun Quit or Suicide percetage]";
        public string TotalGames { get; set; } = "[Total SCP games played]";
        public string TotalQuitSuicide { get; set; } = "[Total suicides/quits as SCP]";
        public string TotalKicks { get; set; } = "[Total kicks]";
        public string TotalBans { get; set; } = "[Total bans]";
        public string PlayerReset { get; set; } = "[Preference or All]";

        [Description("ListOnlineCommand and ListStaffCommand")]
        public string Hp { get; set; } = " [HEALTH: %hp%]";
        public string Ahp { get; set; } = " [ARTIFICIAL HEALTH: %ahp%]";
        public string Overwatch { get; set; } = " [OVERWATCH ENABLED]";
        public string Noclip { get; set; } = " [NOCLIP ENABLED]";
        public string God { get; set; } = " [GODMODE ENABLED]";
        public string Bypass { get; set; } = " [INTERCOM AND BYPASS ENABLED]";
        public string Intercom { get; set; } = " [INTERCOM MUTED]";
        public string VoiceChat { get; set; } = " [VOICE CHAT MUTED]";
        public string GlobalVoiceChat { get; set; } = " [GLOBAL VOICE CHAT MUTED]";
        public string Dnt { get; set; } = " [DO-NOT-TRACK ENABLED]";
        public string IsSpeaking { get; set; } = " [IS SPEAKING]";
        public string Ra { get; set; } = " [REMOTED ADMIN AUTHENTICATED]";

        [Description("%role% is badge")]
        public string SlStaff { get; set; } = "NORTHWOOD STAFF: %staff% [%role%] [%class%]";
        public string StaffOnline { get; set; } = "[Online Staffers: %online%]";
        public string StaffList { get; set; } = "[%id%] %staff% [%role%] [%class%]";
        public string PlayersOnline { get; set; } = "[Online players: %count%]";
        public string PlayerList { get; set; } = "[%id%] %player%";
        public string NoPlayers { get; set; } = "No players online!";
        public string NoStaff { get; set; } = "No staffers online!";

        [Description("Sanction type\n# 8.3.4.\n # Every automated kick or ban (except ones issued by native game functions) must\n # be clearly described to the kicked/banned Player as an automated action not\n # related to native game function, unless it clearly does not look like an automated ction.")]
        public string Warn { get; set; } = "[WARNING]";
        public string Kick { get; set; } = "[AUTO-KICK]";
        public string Ban { get; set; } = "[AUTO-BAN]";
        public string SoftBan { get; set; } = "[AUTO-SOFTBAN]";


        [Description("\"%arguments%\" will get usage this definition ↑")]
        public string UsageError { get; set; } = $"<color=yellow>Usage: %command% %arguments%</color>";
    }
}