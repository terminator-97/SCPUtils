namespace SCPUtils
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Permissions
    {
        [Description("A list of all commands and our permissions.\n# Don't change command name.\n# All permissions are:\n # KickingAndShortTermBanning\n # BanningUpToDay\n # LongTermBanning\n # ForceclassSelf\n # ForceclassToSpectator\n # ForceclassWithoutRestrictions\n # GivingItems\n # WarheadEvents\n # RespawnEvents\n # RoundEvents\n # SetGroup\n # GameplayData\n # Overwatch\n # FacilityManagement\n # PlayersManagement\n # PermissionsManagement\n # ServerConsoleCommands\n # ViewHiddenBadges\n # ServerConfigs\n # Broadcasting\n # PlayerSensitiveDataAccess\n # Noclip\n # AFKImmunity\n # AdminChat\n # ViewHiddenGlobalBadges\n # Announcer\n # Effects\n # FriendlyFireDetectorImmunity\n # FriendlyFireDetectorTempDisable\n# All permissions will be wrote in readme.md")]
        public Dictionary<string, PlayerPermissions> PermissionsList { get; set; } = new Dictionary<string, PlayerPermissions>()
        {
            {
                "scputils announce", PlayerPermissions.Broadcasting
            },
            {
                "scputils announce create", PlayerPermissions.Broadcasting
            },
            {
                "scputils announce delete", PlayerPermissions.Broadcasting
            },
            {
                "scputils announce send", PlayerPermissions.Broadcasting
            },
            {
                "scputils asn", PlayerPermissions.KickingAndShortTermBanning
            },
            {
                "scputils asn whitelist", PlayerPermissions.KickingAndShortTermBanning
            },
            {
                "scputils asn unwhitelist", PlayerPermissions.KickingAndShortTermBanning
            },
            {
                "scputils badge", PlayerPermissions.PlayersManagement
            },
            {
                "scputils badge set", PlayerPermissions.PlayersManagement
            },
            {
                "scputils badge revoke", PlayerPermissions.PlayersManagement
            },
            {
                "scputils badge playtime", PlayerPermissions.SetGroup
            },
            {
                "scputils playtime", PlayerPermissions.GameplayData
            },
            {
                "scputils playtime user", PlayerPermissions.GameplayData
            },
            {
                "scputils playtime badge", PlayerPermissions.SetGroup
            },
        };
    }
}