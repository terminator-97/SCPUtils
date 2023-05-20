namespace SCPUtils
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class CommandConfiguration
    {
        [Description("PlayerLeave configs.\n# left_message_enabled:\n# left_message:")]
        public bool LeftMessageEnabled { get; private set; } = true;
        public string LeftMessage { get; private set; } = "%player% left!";

        [Description("Set the maximum playtime show.")]
        public int MaxPlaytime { get; private set; } = 120;

        public List<string> BadgeVisibility { get; private set; } = new List<string>()
        {
            "SERVER OWNER"
        };

        [Description("Message if player is not authorized to use this command")]
        public string UnauthorizedNicknameChange { get; private set; } = "<color=red>Permission denied.</color>";

        [Description("Message if player is not authorized to use this command")]
        public string UnauthorizedColorChange { get; private set; } = "<color=red>Permission denied.</color>";

        [Description("Message if player is not authorized to use this command")]
        public string UnauthorizedBadgeChangeVisibility { get; private set; } = "<color=red>Permission denied.</color>";

        [Description("Message if player is not authorized to use this command")]
        public string NicknameCooldownMessage { get; private set; } = "<color=red>This command is in user cooldown, try again in few minutes.</color>";

        [Description("Message if player try to change his nickname to a restricted one")]
        public string InvalidNicknameText { get; private set; } = "This nickname has been restricted by server owner, please use another nickname!";

        [Description("Which colors are restricted on .scputils_change_color command? Use command colors in game console to see them")]
        public List<string> RestrictedRoleColors { get; private set; } = new List<string>()
        {
            "Color1",
            "Color2" 
        };

        [Description("Which nicknames are restricted on .scputils_change_nickname command?")]
        public List<string> BannedNickNames { get; private set; } = new List<string>() 
        {
            "@everyone",
            "@here",
            "Admin"
        };

        [Description("Which ASNs should be blacklisted? Players to connect from blacklisted ASN should be whitelisted via scputils_whitelist_asn command (50889 is geforce now ASN)")]
        public List<string> ASNBlacklist { get; private set; } = new List<string>()
        {
            "50889"
        };

        [Description("Which ASNs should be ignored in multi-account detector? (50889 is geforce now ASN)")]
        public List<string> ASNWhiteslistMultiAccount { get; private set; } = new List<string>() { "50889" };

        [Description("Which message non-whitelisted players should get while connecting from blacklisted ASN?")]
        public string AsnKickMessage { get; private set; } = "The ASN you are connecting from is blacklisted from this server, please contact server staff to request to being whitelisted";

        [Description("Command cooldown text")]
        public string CooldownMessage { get; private set; } = "<color=red>Command execution failed! You are under cooldown or command banned, wait 5 seconds and try again, if the error persist you might have been banned from using commands, to see the reason and duration open the console after joining the server, this abusive action has been reported to the staff for futher punishments</color>";

        [Description("From which groups plugin should ignore DNT flag?")]
        public List<string> DntIgnoreList { get; private set; } = new List<string>() 
        {
            "testusergroup1",
            "testusergroup2"
        };

        [Description("Allowed classes to see MTF and Next respawn info")]
        public List<PlayerRoles.Team> AllowedMtfInfoTeam { get; private set; } = new List<PlayerRoles.Team>() 
        {
            PlayerRoles.Team.FoundationForces,
            PlayerRoles.Team.Scientists,
            PlayerRoles.Team.Dead
        };

        [Description("Allowed classes to see Chaos info and Next respawn info")]
        public List<PlayerRoles.Team> AllowedChaosInfoTeam { get; private set; } = new List<PlayerRoles.Team>() 
        {
            PlayerRoles.Team.ClassD,
            PlayerRoles.Team.ChaosInsurgency,
            PlayerRoles.Team.Dead
        };
    }
}
