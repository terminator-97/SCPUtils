namespace SCPUtils
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Permissions
    {

        [Description("A list with permissions for SCPUtils")]
        public Dictionary<string, List<string>> PermissionsList { get; set; } = new Dictionary<string, List<string>>() {
            {
                "founder",
                new List<string>
                {
                    "scputils.*"
                }
            },
            {
                "staff",
                new List<string> 
                {
                    "scputils.asn.*"
                }
            },
            {
                "user",
                new List<string>
                { 
                    "scputils.example", "scputils.example2"
                }
            }
        };

        [Description("A list of all permissions, changing it will also force you to have to change the permissions in the previous configuration.")]
        public List<string> PermissionsName { get; set; } = new List<string>()
        {
            "scputils.asn",
            "scputils.asn.whitelist",
            "scputils.asn.unwhitelist",
            "scputils.badge",
            "scputils.badge.set",
            "scputils.badge.remove",
            "scputils.badge.playtime",
            //"scputils."
        };
    }
}
