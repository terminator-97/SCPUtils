//using Exiled.API.Interfaces;
using PlayerStatsSystem;
using PluginAPI.Core;
using System.Collections.Generic;
using System.ComponentModel;
/*using DamageTypes = Exiled.API.Enums.DamageType;
using Log = Exiled.API.Features.Log;*/
using ZoneType = MapGeneration.FacilityZone;
using DamageTypes = PlayerStatsSystem.DeathTranslations;

namespace SCPUtils
{
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
                    "scputils.changecolor"
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
    }
}
