using Amazon.Runtime.Internal;
using Exiled.API.Interfaces;
using PluginAPI.Roles;
using System.ComponentModel;

namespace SCPUtils
{
    public class Translations : ITranslation
    {
        [Description("Translation credits:")]

        public string TranslationCredits { get; private set; } = "English translation made by Terminator97";

        [Description("--- Generic translations --- \n \n Insufficient permission:")]

        public string NoPermissions { get; private set; } = "<color=red>You need a higher administration level to use this command!</color>";

        [Description("Player not found on database:")]
        public string NoDbPlayer { get; private set; } = "<color=yellow>Player not found on Database!</color>";

        [Description("Invalid player:")]
        public string InvalidPlayer { get; private set; } = "<color=red>Player not found</color>";

        [Description("Invalid Usergroup:")]
        public string InvalidUsergroup { get; private set; } = "<color=red>Invalid usergroup!</color>";

        [Description("Invalid Usergroup:")]
        public string InvalidId { get; private set; } = "<color=red>Invalid ID!</color>";

        [Description("Data not found:")]
        public string InvalidData { get; private set; } = "<color=red>Data not found!</color>";

        [Description("Data not found:")]
        public string InvalidTimespan { get; private set; } = "<color=red>Invalid, Timespan must be DD:HH:MM:SS or DAYSNUMBER</color>";

        [Description("Already disabled:")]
        public string AlreadyDisabled { get; private set; } = "<color=red>Already disabled!</color>";

        [Description("Already Enabled:")]
        public string AlreadyEnabled { get; private set; } = "<color=red>Already enabled!</color>";

        [Description("Round not started:")]
        public string RoundNotStarted { get; private set; } = "<color=red>Round is not started!</color>";

        [Description("Not Alive:")]
        public string NotAlive { get; private set; } = "<color=red>You are not alive!</color>";

        [Description("Out of time")]
        public string OutofTime { get; private set; } = "<color=yellow>The time window to execute this command is over, try to be faster next time!</color>";

        [Description("Taken damage:")]
        public string Damaged { get; private set; } = "<color=red>You have taken damage therefore this command is not available, next time be more careful and play better.</color>";

        [Description("Target Taken damage:")]
        public string TargetDamaged { get; private set; } = "<color=red>Error: Target has taken damage</color>";

        [Description("Usage:")]
        public string Usage { get; private set; } = "Usage";

        [Description("Player:")]
        public string ArgPlayer { get; private set; } = "<player/id>";

        [Description("Text:")]
        public string ArgText { get; private set; } = "<text>";

        [Description("Id:")]
        public string ArgId { get; private set; } = "<id>";

        [Description("Ip:")]
        public string ArgIp { get; private set; } = "<ip>";

        [Description("Timespan:")]
        public string ArgTimespan { get; private set; } = "<DD:HH:MM:SS or DAYSNUMBER>";

        [Description("Usergroup:")]
        public string ArgUsergroup { get; private set; } = "<user group>";

        [Description("Minutes:")]
        public string ArgMinutes { get; private set; } = "<minutes>";

        [Description("Seconds:")]
        public string ArgSeconds { get; private set; } = "<seconds>";

        [Description("Days:")]
        public string ArgDays { get; private set; } = "<days range>";

        [Description("Broadcast:")]
        public string ArgBroadcast { get; private set; } = "<hint(h)/broadcast(bc)>";

        [Description("Number:")]
        public string ArgNumber { get; private set; } = "<number>";

        [Description("Optional:")]
        public string Optional { get; private set; } = "(optional)";

        [Description("Success:")]
        public string Success { get; private set; } = "<color=green>Success</color>";

        [Description("Invalid argument:")]
        public string InvalidArg { get; private set; } = "<color=red>Invalid argument, check command usage!</color>";

        [Description("Invalid argument int:")]
        public string InvalidArgInt { get; private set; } = "<color=red>Argument must be an integer!</color>";

        [Description("None argument:")]
        public string None { get; private set; } = "none";

        [Description("No players online:")]
        public string NoPlayers { get; private set; } = "No online players";
        [Description("Enabled:")]
        public string Enabled { get; private set; } = "Successfully enabled!";
        [Description("No players online:")]
        public string Disabled { get; private set; } = "Successfully disabled!";
        [Description("Success nextround:")]
        public string SuccessNextRound { get; private set; } = "<color=green>Success, changes will take effect next round!</color>";

        [Description("Invalid IP")]
        public string InvalidIp { get; private set; } = "<color=yellow>Invalid IP!</color>";

        [Description("SCP-RoundBan staff BC")]
        public string ScpRoundBanStaffBc { get; private set; } = "<color=blue><SCPUtils> %nick%(%scp%) has been <color=red>BANNED</color> from playing SCP for exceeding Quits / Suicides (as SCP) limit for %rounds% rounds.</color>";

        [Description("Suicide broadcast while active ban")]
        public string ScpSuicideBroadcastActiveBan { get; private set; } = "<color=blue><SCPUtils>%user% has suicided while having an active ban!</color>";

        [Description("SCP-Ban staff BC")]
        public string ScpBanStaffBc { get; private set; } = "<color=blue><SCPUtils> %nick%(%scp%) has been <color=red>BANNED</color> for exceeding Quits / Suicides (as SCP) limit for %duration% minutes.</color>";

        [Description("SCP-Kick staff BC")]
        public string ScpKickStaffBc { get; private set; } = "<color=blue><SCPUtils> %nick%(%scp%) has been <color=red>KICKED</color> for exceeding Quits / Suicides (as SCP) limit</color>";

        [Description("SCP-Warn staff BC")]
        public string ScpWarnStaffBc { get; private set; } = "<color=blue><SCPUtils> %nick%(%scp%) has been <color=red>WARNED</color> for Quitting or Suiciding as SCP</color>";

        [Description("Global badge error")]
        public string GlobalBadgeConsoleError { get; private set; } = "You have a custom color assigned but you have a global badge, as per VSR rules your custom color won't be usable!";

        [Description("NoUsergroup error")]
        public string NoUsergroupError { get; private set; } = "You have a custom badge assigned but you don't have a base usergroup, your custom group won't be visible until you have one, contact server staff for more info!";

        [Description("NoUsergroup error")]
        public string MultiAccountBc { get; private set; } = "Multi-Account detected on %user% - ID: %userid$ Number of accounts: %count%";

        [Description("NoUsergroup error")]
        public string MuteEvasionBc { get; private set; } = "<color=red><size=25>Mute evasion detected on %user% ID: %useid% Userid of muted user: %userid2%</size></color>";

        [Description("By server configs")]
        public string ByServerConfigs { get; private set; } = "by server configs";
        public string CommandBanString { get; private set; } = "You are banned from using commands until";
        public string CommandBanStringReason { get; private set; } = "for the following reason:";

        [Description("--- Commands name and aliases translations --- \n \n Alias command:")]
        public string AliasCommand { get; private set; } = "scputils_alias";

        public string[] AliasAliases { get; private set; } = new[] { "alias", "su_alias", "scpu_alias", "alt" };

        public string AliasDescription { get; private set; } = "Check player aliases with some basic info, for complete info use dupeip";

        [Description("Unwhitelist command:")]
        public string UnwhitelistCommand { get; private set; } = "scputils_unwhitelist_asn";

        public string[] UnwhitelistAliases { get; private set; } = new[] { "asunw", "su_uwl_asn", "scpu_uwl_asn" };

        public string UnwhitelistDescription { get; private set; } = "Un-Whitelist a player to disallow him access the server if ASN is blacklisted!";

        [Description("AsnWhitelist command:")]
        public string AsnwhitelistCommand { get; private set; } = "scputils_whitelist_asn";

        public string[] AsnwhitelistAliases { get; private set; } = new[] { "asnw", "su_wl_asn", "scpu_wl_asn" };

        public string AsnwhitelistDescription { get; private set; } = "Whitelist a player to make him access the server even if ASN is blacklisted!";
        [Description("Badgeplaytime command:")]
        public string BadgeplaytimeCommand { get; private set; } = "scputils_badge_play_time";

        public string[] BadgeplaytimeAliases { get; private set; } = new[] { "bpt", "tpt", "su_tpt", "scpu_badgeplaytime", "scpu_bpt" };

        public string BadgeplaytimeDescription { get; private set; } = "Shows the playing time of all players who have that badge.";

        public string BadgeplaytimeChecking { get; private set; } = "Checking playtime of badge";

        public string BadgeplaytimeNullplayer { get; private set; } = "Null player detected, this usually happens if you have a player in remoteadmin file while he has not joined the server yet.";

        public string BadgeplaytimeNoActivity { get; private set; } = "Playtime: [ No activity ]";

        public string BadgeplaytimePlaytime { get; private set; } = "Playtime:";

        public string BadgeplaytimeDaysjoined { get; private set; } = "Days joined:";

        public string BadgeplaytimeOverwatchtime { get; private set; } = "Overwatch time:";

        public string BadgeplaytimeOverwatchdays { get; private set; } = "Overwatch days:";

        [Description("Broadcast command:")]
        public string BroadcastCommand { get; private set; } = "scputils_broadcast";

        public string[] BroadcastAliases { get; private set; } = new[] { "sbc", "su_bc", "scpu_bc" };

        public string BroadcastDescription { get; private set; } = "Allows to send custom broadcastes";


        [Description("Broadcastlist command:")]

        public string BroadcastlistCommand { get; private set; } = "scputils_broadcast_list";

        public string[] BroadcastlistAliases { get; private set; } = new[] { "sbcl", "bcl", "su_bcl", "scpu_bcl" };

        public string BroadcastlistDescription { get; private set; } = "List of all registred broadcast";

        [Description("Broadcastcreate command:")]

        public string BroadcastcreateCommand { get; private set; } = "scputils_broadcast_create";

        public string[] BroadcastcreateAliases { get; private set; } = new[] { "cbc", "su_cbc", "scpu_cbc" };

        public string BroadcastcreateDescription { get; private set; } = "Allows to create custom broadcasts";

        [Description("CustomBadge command:")]

        public string CustomBadgeCommand { get; private set; } = "scputils_custom_badge";

        public string[] CustomBadgeAliases { get; private set; } = new[] { "cb", "scputils_cbadge", "su_cb", "su_customb" };

        public string CustomBadgeDescription { get; private set; } = "Set a custom badge name to any player if they already have a badge";

        [Description("Deletebroadcast command:")]
        public string DeletebroadcastCommand { get; private set; } = "scputils_broadcast_delete";

        public string[] DeletebroadcastAliases { get; private set; } = new[] { "dbc", "su_dbc", "scpu_dbc" };

        public string DeletebroadcastDescription { get; private set; } = "Delete a custom broadcast from database";

        [Description("Disablesuicidewarn command:")]

        public string DisablesuicidewarnCommand { get; private set; } = "scputils_disable_suicide_warns";

        public string[] DisablesuicidewarnAliases { get; private set; } = new[] { "dsw", "disable_suicide_warns", "su_dsw", "scpu_dsw" };

        public string DisablesuicidewarnDescription { get; private set; } = "Temporarily disable SCP suicide/quit detector for the duration of the round";

        [Description("Dupeip command:")]

        public string DupeipCommand { get; private set; } = "scputils_dupeip";

        public string[] DupeipAliases { get; private set; } = new[] { "dupeip", "su_dupeip", "scpu_dupeip" };

        public string DupeipDescription { get; private set; } = "Check if player has another account on same IP";

        [Description("Enablesuicidewarn command:")]

        public string EnablesuicidewarnCommand { get; private set; } = "scputils_enable_suicide_warns";

        public string[] EnablesuicidewarnAliases { get; private set; } = new[] { "esw", "enable_suicide_warns", "su_esw", "scpu_esw" };

        public string EnablesuicidewarnDescription { get; private set; } = "Enables again SCP suicide/quit detector";

        [Description("Globaledit command:")]

        public string GlobaleditCommand { get; private set; } = "scputils_global_edit";

        public string[] GlobaleditAliases { get; private set; } = new[] { "gedit", "su_gedit", "su_globaledit", "su_ge", "scpu_gedit", "scpu_globaledit", "scpu_ge" };

        public string GlobaleditDescription { get; private set; } = "Remove specified amount of scp games / warns / kick / bans from each player present in DB!";


        [Description("Help command:")]
        public string HelpCommand { get; private set; } = "scputils_help";

        public string[] HelpAliases { get; private set; } = new string[] { "su_help", "su_h", "scpu_help", "scpu_h" };

        public string HelpDescription { get; private set; } = "Plugin help command";

        public string HelpContent { get; private set; } = "<color=#1BBC9B>User commands:</color> \n <color=#1BBC9B>.scputils_info, .scputils_change_nickname, .scputils_change_color, .scputils_show_badge, .scputils_hide_badge, .scputils_my_info, .scputils_play_time, scputils_round_info, scputils_permissions_view</color>";

        public string HelpContentAdmin { get; private set; } = "n<color=#FFD700>Administration commands (Remote Admin): </color>\n<color=#FFD700>scputils_player_info, scputils_player_list, scputils_player_reset_preferences, scputils_player_reset, scputils_set_color, scputils_set_name, scputils_set_badge, scputils_revoke_badge, scputils_play_time, scputils_whitelist_asn, scputils_unwhitelist_asn, scputils_enable_suicide_warns, scputils_disable_suicide_warns, scputils_global_edit, scputils_player_edit, scputils_player_delete, scputils_player_restrict, scputils_player_unrestrict, scputils_show_command_bans, scputils_preference_persist, scputils_remove_previous_badge, scputils_player_dnt, scputils_round_info, scputils_player_last_warning, scputils_player_warnings, scputils_player_unwarn, scputils_play_sessions, scputils_badge_playtime, scputils_multiaccount_whitelist, scputils_dupeip, scputils_alias, scputils_custom_badge, scputils_searchip</color>";

        [Description("Hidebadge command:")]

        public string HidebadgeCommand { get; private set; } = "scputils_hide_badge";

        public string[] HidebadgeAliases { get; private set; } = new[] { "hb", "su_hb", "su_hbadge", "su_hideb", "scpu_hb", "scpu_hbadge", "scpu_hideb" };

        public string HidebadgeDescription { get; private set; } = "Hides your badge permanently until you execute scputils_show_badge or their aliases.";

        [Description("Info command:")]

        public string InfoCommand { get; private set; } = "scputils_info";

        public string[] InfoAliases { get; private set; } = new string[] { "su_info", "su_i", "scpu_info", "scpu_i" };
        public string InfoDescription { get; private set; } = "Show plugin informations";

        [Description("Multiaccountwhitelist command:")]

        public string MultiaccountwhitelistCommand { get; private set; } = "scputils_multiaccount_whitelist";

        public string[] MultiaccountwhitelistAliases { get; private set; } = new[] { "mawl" };

        public string MultiaccountwhitelistDescription { get; private set; } = "Whitelist/Unwhitelist a player to make him being ignored/detected by multiaccount system!";

        [Description("Onlinelist command:")]

        public string OnlinelistCommand { get; private set; } = "scputils_online_list";

        public string[] OnlinelistAliases { get; private set; } = new[] { "ol", "onlinelist", "su_ol", "su_onlinelist", "scpu_ol", "scpu_onlinelist" };

        public string OnlinelistDescription { get; private set; } = "Show online player list";

        [Description("Playerbroadcast command:")]

        public string PlayerbroadcastCommand { get; private set; } = "scputils_player_broadcast";

        public string[] PlayerbroadcastAliases { get; private set; } = new[] { "spbc", "su_pbc", "su_player_bc", "su_p_bc", "su_p_broadcast", "scpu_pbc", "scpu_player_bc", "scpu_p_bc", "scpu_p_broadcast" };

        public string PlayerbroadcastDescription { get; private set; } = "Allows to send custom broadcast";

        [Description("Playerdelete command:")]

        public string PlayerdeleteCommand { get; private set; } = "scputils_player_delete";

        public string[] PlayerdeleteAliases { get; private set; } = new[] { "pdelete", "su_pdelete", "su_playerdelete", "scpu_pdelete", "scpu_playerdelete" };

        public string PlayerdeleteDescription { get; private set; } = "Delete a player (and all the player data) from the database, action is irreversible!";

        public string PlayerdeleteSuccess { get; private set; } = "%user% has been deleted from the database!";

        [Description("Playerdnt command:")]

        public string PlayerdntCommand { get; private set; } = "scputils_player_dnt";

        public string[] PlayerdntAliases { get; private set; } = new[] { "pdnt", "dnt", "su_pdnt", "su_playerdnt", "scpu_pdnt", "scpu_playerdnt" };

        public string PlayerdntDescription { get; private set; } = "Allows you to ignore a user's DNT requests";

        [Description("Playeredit command:")]
        public string PlayereditCommand { get; private set; } = "scputils_player_edit";

        public string[] PlayereditAliases { get; private set; } = new[] { "pedit", "su_pedit", "su_playeredit", "scpu_pedit", "scpu_playeredit" };

        public string PlayereditDescription { get; private set; } = "Edit some player data in the database";

        [Description("Playerinfo command:")]
        public string PlayerinfoCommand { get; private set; } = "scputils_player_info";

        public string[] PlayerinfoAliases { get; private set; } = new[] { "upi", "scputils_my_info", "su_pi", "su_player_info", "su_playerinfo", "scpu_pi", "scpu_player_info", "scpu_playerinfo" };

        public string PlayerinfoDescription { get; private set; } = "Show player info, in case you are not admin you can see only your info";

        public string PlayerinfoTotalSuicides { get; private set; } = "Total SCP Suicides/Quits:";

        public string PlayerinfoKicks { get; private set; } = "Total SCP Suicides/Quits Kicks:";

        public string PlayerinfoBans { get; private set; } = "Total SCP Suicides/Quits Bans:";

        public string PlayerinfoGamesPlayed { get; private set; } = "Total Games played as SCP:";

        public string PlayerinfoPercentage { get; private set; } = "Total Suicides/Quits Percentage:";

        public string PlayerinfoFirstjoin { get; private set; } = "First Join:";

        public string PlayerinfoLastseen { get; private set; } = "Last Seen:";

        public string PlayerinfoCustomcolor { get; private set; } = "Custom Color:";

        public string PlayerinfoCustomname { get; private set; } = "Custom Name:";

        public string PlayerinfoTempbadge { get; private set; } = "Temporarily Badge:";

        public string PlayerinfoCustombadge { get; private set; } = "Custom Badge:";

        public string PlayerinfoBadgexpire { get; private set; } = "Badge Expire:";

        public string PlayerinfoHidebadge { get; private set; } = "Hide Badge:";

        public string PlayerinfoAsnwhitelist { get; private set; } = "Asn Whitelisted:";

        public string PlayerinfoKeepflag { get; private set; } = "Keep Flag:";

        public string PlayerinfoDnt { get; private set; } = "Ignore DNT:";

        public string PlayerinfoWhitelist { get; private set; } = "MultiAccount Whitelist:";

        public string PlayerinfoPlaytime { get; private set; } = "Total Playtime:";

        public string PlayerinfoOverwatchtime { get; private set; } = "Total Overwatch time:";

        public string PlayerinfoCooldown { get; private set; } = "Nickname cooldown:";

        public string PlayerinfoOverwatch { get; private set; } = "Overwatch active:";

        public string PlayerinfoRestricted { get; private set; } = "<color=red>User account is currently restricted</color>";

        public string PlayerinfoReason { get; private set; } = "Reason:";

        public string PlayerinfoExpire { get; private set; } = "Expire:";

        public string PlayerinfoRoundsleft { get; private set; } = "Round(s) left:";

        public string PlayerinfoScpbanned { get; private set; } = "<color=red>User account is currently SCP-Banned:</color>";




        [Description("Playerlist command:")]

        public string PlayerlistCommand { get; private set; } = "scputils_player_list";

        public string[] PlayerlistAliases { get; private set; } = new[] { "pli", "su_pl", "su_playerlist", "scpu_pl", "scpu_playerlist" };

        public string PlayerlistDescription { get; private set; } = "Show player list in scputils database with some basic informations, don't use values like 0 otherwise the list may get huge";


        [Description("Reset command:")]

        public string ResetCommand { get; private set; } = "scputils_player_reset";

        public string[] ResetAliases { get; private set; } = new[] { "pr", "su_pr", "su_playerreset", "su_playereset", "scpu_pr", "scpu_playerreset", "scpu_playereset" };

        public string ResetDescription { get; private set; } = "Reset player data (Quits,Ban,Kicks,Nickname,Badge etc, everything)!";

        [Description("Resetpreferences command:")]

        public string ResetpreferencesCommand { get; private set; } = "scputils_player_reset_preferences";

        public string[] ResetpreferencesAliases { get; private set; } = new[] { "prp", "su_prp", "su_playerpreferences", "su_player_preferences", "su_player_reset_preferences", "scpu_prp", "scpu_playerpreferences", "scpu_player_preferences", "scpu_player_reset_preferences" };

        public string ResetpreferencesDescription { get; private set; } = "Reset player preferences (Nickname, badges etc)!";

        [Description("Restrict command:")]

        public string RestrictCommand { get; private set; } = "scputils_player_restrict";

        public string[] RestrictAliases { get; private set; } = new[] { "restrict", "susp", "su_playerrestrict", "su_playerestrict", "su_player_r", "scpu_playerestrict", "scpu_playerrestrict", "scpu_player_r" };

        public string RestrictDescription { get; private set; } = "This command restricts a player from using the: scputils_change_nickname and scputils_change_color.";

        [Description("Unrestrict command:")]
        public string UnrestrictCommand { get; private set; } = "scputils_player_unrestrict";

        public string[] UnrestrictAliases { get; private set; } = new[] { "unrestrict", "unsusp", "su_playerunrestrict", "su_player_unr", "scpu_playerunrestrict", "scpu_player_unr" };

        public string UnrestrictDescription { get; private set; } = "This command removes the restriction to use scputils_change_nickname or scputils_change_color from a player!";

        [Description("Playtime command:")]
        public string PlaytimeCommand { get; private set; } = "scputils_play_time";

        public string[] PlaytimeAliases { get; private set; } = new[] { "pt", "su_playtime", "su_pt", "scpu_playtime", "scpu_pt" };

        public string PlaytimeDescription { get; private set; } = "Show detailed informations about playtime";

        public string PlaytimeZero { get; private set; } = "<color=red>You have to specify a number higher than 0!</color>";

        public string PlaytimeMaxRange { get; private set; } = "<color=red>You can specify a range of max 120 days!</color>";

        public string PlaytimeTotal { get; private set; } = "Total Playtime:";

        public string PlaytimeOverwatch { get; private set; } = "Overwatch time:";

        public string PlaytimeNoactivity { get; private set; } = "Playtime: [ No activity ]";

        public string PlaytimeSpecified { get; private set; } = "Specified Period PlayTime:";

        public string PlaytimePlaytime { get; private set; } = "Playtime:";



        [Description("Preferencepersist command:")]


        public string PreferencepersistCommand { get; private set; } = "scputils_preference_persist";

        public string[] PreferencepersistAliases { get; private set; } = new[] { "pp", "su_pp", "su_preference_p", "scpu_pp", "scpu_preference_p" };

        public string PreferencepersistDescription { get; private set; } = "Use this command to keep player nickname and color even if he doesn't have access to that permission!";

        [Description("Reports command:")]

        public string ReportsCommand { get; private set; } = "scputils_reports";

        public string[] ReportsAliases { get; private set; } = new[] { "scpu_reports", "rep", "sreports", "reports" };

        public string ReportsDescription { get; private set; } = "Show useful stats about users inside SCPUtils database.";

        [Description("Respawn command (check main config for more customization):")]

        public string RespawnDescription { get; private set; } = "Allows you to respawn in case you are bugged";

        [Description("RevokeBadge command:")]

        public string RevokeBadgeCommand { get; private set; } = "scputils_revoke_badge";

        public string[] RevokeBadgeAliases { get; private set; } = new[] { "rb", "su_rb", "su_remove_badge", "su_revoke_b", "scpu_rb", "scpu_remove_badge", "scpu_revoke_b" };

        public string RevokeBadgeDescription { get; private set; } = "Remove a temporarily badge that has been given to a player!";

        [Description("Roundinfo command:")]

        public string RoundinfoCommand { get; private set; } = "scputils_round_info";

        public string[] RoundinfoAliases { get; private set; } = new[] { "ri", "roundinfo", "round_info", "su_ri", "su_roundinfo", "su_round_info", "scpu_ri", "scpu_roundinfo", "scpu_round_info" };
        public string RoundinfoDescription { get; private set; } = "Show round info";

        public string Roundinfo { get; private set; } = "Round Info:";

        public string Roundinfort { get; private set; } = "Round Time:";

        public string Roundinfochaostickets { get; private set; } = "Number of Chaos Tickets:";

        public string Roundinfomtftickets { get; private set; } = "Number of MTF Tickets:";

        public string Roundinforespawnteam { get; private set; } = "Next known Respawn Team:";

        public string Roundinforespawntime { get; private set; } = "Time until respawn:";

        public string Roundinfonumberchaosrespawn { get; private set; } = "Number of Chaos Respawn Waves:";
        public string Roundinfonumbermtfrespawn { get; private set; } = "Number of Mtf Respawn Waves:";
        public string Roundinfolastchaoswave { get; private set; } = "Last Chaos wave respawn elapsed time:";

        public string Roundinfolastmtfwave { get; private set; } = "Last MTF wave respawn elapsed time:";


        [Description("Scplist command:")]

        public string ScplistCommand { get; private set; } = "scputils_scp_list";

        public string[] ScplistAliases { get; private set; } = new[] { "scpl", "scplist", "su_scpl", "su_scplist", "scpu_scplist" };

        public string ScplistDescription { get; private set; } = "Show scp list";

        public string ScplistNoScp { get; private set; } = "You are not SCP or Swap module is disabled by the server admin!";

        [Description("Searchip command:")]

        public string SearchipCommand { get; private set; } = "scputils_search_ip";

        public string[] SearchipAliases { get; private set; } = new[] { "searchip", "su_searchip", "scpu_searchip", "sip", "scputils_searchip" };

        public string SearchipDescription { get; private set; } = "Check if there is a player linked with a specific ip address";

        [Description("Setbadge command:")]

        public string SetbadgeCommand { get; private set; } = "scputils_set_badge";

        public string[] SetbadgeAliases { get; private set; } = new[] { "setb", "issue_badge", "su_setb", "su_setbadge", "scpu_setb", "scpu_setbadge" };

        public string SetbadgeDescription { get; private set; } = "Set a temporarily badge to a player";

        public string SetbadgeSuccess { get; private set; } = "Successfully set %group% badge for %duration%";

        public string SetbadgeKickpower { get; private set; } = "The group you are trying to set has more kick power than yours. (Your kick power:";

        public string SetbadgeRequired { get; private set; } = "Required:";

        [Description("Setcolor command:")]

        public string SetcolorCommand { get; private set; } = "scputils_set_color";

        public string[] SetcolorAliases { get; private set; } = new[] { "scl", "scputils_change_color", "su_sc", "su_cc", "su_setc", "sc_scolor", "scpu_sc", "scpu_cc", "scpu_setc", "scpu_scolor" };

        public string SetcolorDescription { get; private set; } = "Change everyone's color or only your one based on the permissions you have";

        public string SetcolorArgColor { get; private set; } = "<color, none, rainbow>";

        public string SetcolorInvalidcolor { get; private set; } = "<color=red> Invalid color, type color in console to see valid SCP colors<color>";

        public string SetcolorRainbowdisabled { get; private set; } = "<color=red>Random/Rainbow roles are disabled by server owner!</color>";

        public string SetcolorGlobalbadge { get; private set; } = "<color=red>This user has a global badge, as VSR rules you cannot change global badge colors, if you have a local badge please set it and try using this command again</color>";
        public string SetcolorRestrictedcolor { get; private set; } = "<color=red>This color has been restricted by server owner, please use another color!</color>";

        public string SetcolorRainbow { get; private set; } = "rainbow";

        [Description("Setname command:")]

        public string SetnameCommand { get; private set; } = "scputils_set_name";

        public string[] SetnameAliases { get; private set; } = new[] { "un", "scputils_change_nickname", "su_setn", "su_sname", "su_cn", "scpu_setn", "scpu_sname", "scpu_cn" };

        public string SetnameDescription { get; private set; } = "Change everyone name or only your name based on the permissions you have";

        public string SetnameArgnickname { get; private set; } = "<Nickname / None>";

        public string SetnameTaken { get; private set; } = "<color=red>This nickname is already used by another player, please choose another name!</color>";

        public string SetnameToolong { get; private set; } = "<color=red>Nickname is too long!</color>";


        [Description("Roundban command:")]

        public string RoundbanCommand { get; private set; } = "scputils_set_round_ban";

        public string[] RoundbanAliases { get; private set; } = new[] { "srb", "roundban", "su_srb", "su_roundban" };

        public string RoundbanDescription { get; private set; } = "Sets the number of round ban to one player";

        [Description("Showbadge command:")]

        public string ShowbadgeCommand { get; private set; } = "scputils_show_badge";

        public string[] ShowbadgeAliases { get; private set; } = new[] { "sb", "su_showb", "su_sbadge", "scpu_showb", "scpu_sbadge" };

        public string ShowbadgeDescription { get; private set; } = "Shows your permanently hidden badge again.";

        [Description("Showcommandbans command:")]
        public string ShowcommandbansCommand { get; private set; } = "scputils_show_command_bans";

        public string[] ShowcommandbansAliases { get; private set; } = new[] { "scb", "su_show_cb", "su_scb", "scpu_show_cb", "scpu_scb" };

        public string ShowcommandbansDescription { get; private set; } = "Show detailed information about a restriction";


        [Description("Showwarn command:")]

        public string ShowwarnCommand { get; private set; } = "scputils_player_last_warning";

        public string[] ShowwarnAliases { get; private set; } = new[] { "lwarn", "su_lwarn", "su_last_warn", "su_lastw", "scpu_lwarn", "scpu_last_warn", "scpu_lastw" };

        public string ShowwarnDescription { get; private set; } = "Show last SCPUtils warning of a specific player!";

        [Description("Showwarns command:")]

        public string ShowwarnsCommand { get; private set; } = "scputils_player_warnings";

        public string[] ShowwarnsAliases { get; private set; } = new[] { "warns", "swarns", "su_warns", "scpu_warns" };

        public string ShowwarnsDescription { get; private set; } = "Show all SCPUtils warnings of a specific player!";

        [Description("Stafflist command:")]
        public string StafflistCommand { get; private set; } = "scputils_staff_list";

        public string[] StafflistAliases { get; private set; } = new[] { "sl", "stafflist", "su_sl", "su_staffl", "su_staff_l", "su_slist", "scpu_sl", "scpu_staffl", "scpu_staff_l", "scpu_slist" };

        public string StafflistDescription { get; private set; } = "Show list of online staffer.";

        [Description("Swaprequest command (check main configs for more customization):")]

        public string SwaprequestDescription { get; private set; } = "Send a SCP swap request to a player";

        public string SwaprequestDisabled { get; private set; } = "<color=yellow>SCP swap module is disabled on this server!</color>";

        public string SwaprequestNotscp { get; private set; } = "<color=yellow>Only SCPs are allowed to use this command!</color>";

        public string SwaprequestUsage { get; private set; } = "<color=yellow>Usage: %command% <player id/nickname or SCP-NUMBER (example: swap SCP-939)></color>";

        public string SwaprequestRoledisabled { get; private set; } = "<color=red>You cannot swap with an unspawned SCP using this SCP</color>";

        public string SwaprequestLimit { get; private set; } = "<color=red>You have reached swaps requests limit for this round!</color>";

        public string SwaprequestNoautoswap { get; private set; } = "<color=red>This SCP is not allowed to auto-swap and no player is currently playing this SCP, please choose another SCP.</color>";

        public string SwaprequestInvalidplayer { get; private set; } = "<color=red>Invalid player nickname/id or invalid SCP name!</color>";

        public string SwaprequestPendingtargeterror { get; private set; } = "<color=red>Target already sent a request or has a pending request</color>";

        public string SwaprequestPendingerror { get; private set; } = "<color=red>You already sent or received a swap request, cancel or deny it before sending a new one</color>";

        public string SwaprequestSelferror { get; private set; } = "<color=red>You can't send swap request to yourself!</color>";

        public string SwaprequestTargetnoscp { get; private set; } = "<color=red>Target is not an SCP!</color>";

        public string SwaprequestSameroleerror { get; private set; } = "<color=red>You have the same role of the other player!</color";

        public string SwaprequestTargetcustomscperror { get; private set; } = "<color=red>Error: Target is using a custom SCP!</color>";

        public string SwaprequestCustomscperror { get; private set; } = "<color=red>Error: You are using a custom SCP</color>";

        public string SwaprequestSuccess { get; private set; } = "<color=green>Request has been sent successfully to %target%, player has %seconds% seconds to accept the request. Use %cancelcommand% to cancel the request</color>";



        [Description("Swaprequestqccept (check main configs for more customization):")]

        public string SwaprequestacceptDescription { get; private set; } = "Accept an SCP swap request";

        public string SwaprequestacceptNoswaprequest { get; private set; } = "<color=red>You haven't swap requests!</color>";


        [Description("Swaprequestcancel command (check main configs for more customization):")]

        public string SwaprequestcancelDescription { get; private set; } = "Cancel a swap request sent by you";

        [Description("Swaprequestdeny command (check main configs for more customization):")]

        public string SwaprequestdenyDescription { get; private set; } = "Deny a swap request";

        [Description("Unwarn command (check main configs for more customization):")]
        public string UnwarnDescription { get; private set; } = "Removes a specific warning from a player!";


        [Description("--- Webhook translation --- \n \n Mute evasion:")]

        public string Report { get; private set; } = "Mute evasion report!";

        public string Description { get; private set; } = "Mute evasion detected! Userid of muted user:";

        public string PlayerInfo { get; private set; } = "Player Info:";

        public string Username { get; private set; } = "Username:";

        public string UserId { get; private set; } = "User-ID:";

        public string TemporaryId { get; private set; } = "Temporary ID:";

    }
}

