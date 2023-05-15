namespace SCPUtils
{
    using PluginAPI.Core;
    using System.Collections.Generic;
    using System.ComponentModel;
    using ZoneType = MapGeneration.FacilityZone;

    public class Configs
    {
        public bool IsEnabled { get; set; } = true;

        public bool IsDebugEnabled { get; set; } = false;

        [Description("Should SCPs be warned for quitting or suicide?")]
        public bool EnableSCPSuicideAutoWarn { get; private set; } = true;

        [Description("Should round autorestart if there is only one player left?")]
        public bool EnableRoundRestartCheck { get; private set; } = true;

        [Description("Are quits as SCP considered a warnable offence?")]
        public bool QuitEqualsSuicide { get; private set; } = true;

        [Description("Should SCPs be kicked for quitting or suicide after a certain threshold?")]
        public bool AutoKickOnSCPSuicide { get; private set; } = true;

        [Description("Should SCPs be banned for quitting or suicide after a certain threshold?")]
        public bool EnableSCPSuicideAutoBan { get; private set; } = false;

        [Description("Should SCPs be banned from playing again SCP (except 049-2) for X round after a certain threshold? If this is true EnableSCPSuicideAutoBan must be turned false")]
        public bool EnableSCPSuicideSoftBan { get; private set; } = true;

        [Description("Should ban duration multiply after each ban for quit / suicide as SCP?")]
        public bool MultiplyBanDurationEachBan { get; private set; } = true;

        [Description("Should player preferences be reset after badge expire?")]
        public bool ResetPreferencedOnBadgeExpire { get; private set; } = true;

        [Description("Should a player with a banned game be autokicked?")]
        public bool AutoKickBannedNames { get; private set; } = true;

        [Description("Should Tutorials be considered as SCP? If true they will get warned for suicides / quits if enabled")]
        public bool AreTutorialsSCP { get; private set; } = false;

        [Description("Broadcast in admin chat auto kick and bans for quitting or suicide as SCP")]
        public bool BroadcastSanctions { get; private set; } = true;

        [Description("Broadcast in admin chat auto warns for quitting or suicide as SCP")]
        public bool BroadcastWarns { get; private set; } = false;

        [Description("Should the nickname gets resetted if the user when joins doesn't have the permission to execute the command? You can bypass it using scputils_preference_persist command")]
        public bool KeepNameWithoutPermission { get; private set; } = false;

        [Description("Should the color gets resetted if the user when joins doesn't have the permission to execute the command? You can bypass it using scputils_preference_persist command")]
        public bool KeepColorWithoutPermission { get; private set; } = false;

        [Description("Should the badge visibility gets resetted if the user when joins doesn't have the permission to execute the command? You can bypass it using scputils_preference_persist command")]
        public bool KeepBadgeVisibilityWithoutPermission { get; private set; } = false;

        [Description("Is 096's notify target feature enabled?")]
        public bool Scp096TargetNotifyEnabled { get; private set; } = true;

        [Description("Notify last player alive?")]
        public bool NotifyLastPlayerAlive { get; private set; } = true;

        [Description("Ignore DNT requests?")]
        public bool IgnoreDntRequests { get; private set; } = false;

        [Description("Enable auto restart module?")]
        public bool EnableAutoRestart { get; private set; } = false;

        [Description("Automute player if evasion is detected?")]
        public bool AutoMute { get; private set; } = false;

        [Description("Should a broadcast appear in case of multiaccount?")]
        public bool MultiAccountBroadcast { get; private set; } = false;

        [Description("Should only the owner of the handcuff (and the allies of the handcuffed players) be able to unhandcull?")]
        public bool HandCuffOwnership { get; private set; } = false;

        [Description("Allow SCP swap?")]
        public bool AllowSCPSwap { get; private set; } = true;
        [Description("Allow SCP swap only if both SCPs are at full health? You must enable also allow_scp_swap option.")]
        public bool AllowSCPSwapOnlyFullHealth { get; private set; } = true;

        [Description("Report command abuse in console?")]
        public bool CommandAbuseReport { get; private set; } = true;

        [Description("Show death message for SCP-049-2?")]
        public bool ShowDeathMessage0492 { get; private set; } = false;

        [Description("Autowarn message for suiciding as SCP")]
        public Extensions.Broadcast SuicideWarnMessage { get; private set; } = new Extensions.Broadcast("<color=red>WARN:\nAs per server rules SCP's suicide is an offence, doing it too much will result in a ban!</color>", 30, true, Broadcast.BroadcastFlags.Normal);

        [Description("Welcome message (if enabled)")]
        public Extensions.Broadcast WelcomeMessage { get; private set; } = new Extensions.Broadcast("<color=green>Welcome to the server %player%!</color>", 12, true, Broadcast.BroadcastFlags.Normal);

        [Description("Decontamination message (if enabled)")]
        public Extensions.Broadcast DecontaminationMessage { get; private set; } = new Extensions.Broadcast("<color=yellow>Decontamination has started</color>", 12, false, Broadcast.BroadcastFlags.Normal);

        [Description("Suicide auto-kick reason (if enabled)")]
        public string SuicideKickMessage { get; private set; } = "Suicide as SCP";

        [Description("Auto-kick message for using a restricted nickname")]
        public string AutoKickBannedNameMessage { get; private set; } = "You're using a restricted nickname or too similar to a restricted one, please change it";

        [Description("Suicide auto-ban reason (if enabled)")]
        public string AutoBanMessage { get; private set; } = "Exceeded SCP suicide limit";

        [Description("Message if player is not authorized to use this command")]
        public string UnauthorizedNickNameChange { get; private set; } = "<color=red>Permission denied.</color>";

        [Description("Message if player is not authorized to use this command")]
        public string UnauthorizedColorChange { get; private set; } = "<color=red>Permission denied.</color>";

        [Description("Message if player is not authorized to use this command")]
        public string UnauthorizedBadgeChangeVisibility { get; private set; } = "<color=red>Permission denied.</color>";

        [Description("Message if player is not authorized to use this command")]
        public string NicknameCooldownMessage { get; private set; } = "<color=red>This command is in user cooldown, try again in few minutes.</color>";

        [Description("Message if player try to change his nickname to a restricted one")]
        public string InvalidNicknameText { get; private set; } = "This nickname has been restricted by server owner, please use another nickname!";

        [Description("Discord webhook url for mute evasion reports")]
        public string WebhookUrl { get; private set; } = "None";

        [Description("Discord webhook bot nickname")]
        public string WebhookNickname { get; private set; } = "The Frontman";

        [Description("Which broadcast should be shown when a SCP die?")]
        public Extensions.Broadcast ScpDeathMessage { get; private set; } = new Extensions.Broadcast("<color=blue>SCP %playername% (%scpname%) was killed by %killername%. Cause of death: %reason%</color>", 12, true, Broadcast.BroadcastFlags.Normal);

        [Description("Which broadcast should be shown when a SCP die?")]
        public Extensions.Broadcast ScpSuicideMessage { get; private set; } = new Extensions.Broadcast("<color=blue>SCP %playername% (%scpname%) has killed by themselves. Cause of death: %reason%</color>", 12, true, Broadcast.BroadcastFlags.Normal);

        [Description("Text shown if a player doesn't own the handcuffed player")]
        public Extensions.Broadcast UnhandCuffDenied { get; private set; } = new Extensions.Broadcast("<color=blue>You do not have the ownership of this player therefore you can't un-handcuff him!</color>", 8, true, Broadcast.BroadcastFlags.Normal);

        [Description("Auto-restart time if there is only one player in server (if enabled)")]
        public ushort AutoRestartTime { get; private set; } = 15;

        [Description("SCP-096 target message duration (if enabled)")]
        public ushort Scp096TargetMessageDuration { get; private set; } = 12;

        [Description("Last player alive message duration (if enabled)")]
        public ushort LastPlayerAliveMessageDuration { get; private set; } = 12;

        [Description("Default duration for broadcast sent via scputils_broadcast command")]
        public ushort BroadcastDuration { get; private set; } = 12;

        [Description("Default duration for hints sent via scputils_broadcast command")]
        public ushort HintsDuration { get; private set; } = 12;

        [Description("Which is the minimun number of suicides before the player may not receive any kick o ban ignoring the SCP suicides / quit percentage? (if enabled)")]
        public int ScpSuicideTollerance { get; private set; } = 5;

        [Description("SCP Suicide / Quit base ban duration (if enabled)")]
        public int AutoBanDuration { get; private set; } = 15;

        [Description("SCP Suicide / Quit base number of auto-ban rounds (if enbaled)")]
        public int AutoBanRoundsCount { get; private set; } = 2;

        [Description("Which is the max length of a nickname using change name command?")]
        public int NicknameMaxLength { get; private set; } = 32;

        [Description("If 079 trigger tesla for how many seconds player shouldn't get warned for suicide? (2 is enough for most of servers)")]
        public int Scp079TeslaEventWait { get; private set; } = 2;

        [Description("For bpt command, min seconds of gameplay to be considered the day as complete")]
        public int BptMinSeconds { get; private set; } = 1200;

        [Description("Change nickname cooldown in seconds")]
        public int ChangeNicknameCooldown { get; private set; } = 120;

        [Description("Min players required on server for PT to be counted")]
        public int MinPlayersPtCount { get; private set; } = 1;

        [Description("Max allowed time in seconds from start of round to send a SCP Swap request")]
        public int MaxAllowedTimeScpSwapRequest { get; private set; } = 60;

        [Description("Max allowed time in seconds from start of round to accept a scp swap request")]
        public int MaxAllowedTimeScpSwapRequestAccept { get; private set; } = 75;

        [Description("Auto-warn tesla immunity time after respawn")]
        public int WarnImmunityTeslaRespawn { get; private set; } = 2;

        [Description("Command cooldown in seconds")]
        public double CommandCooldownSeconds { get; private set; } = 5;

        [Description("List of session variables for custom SCPs, putting the id there will deny the swap")]
        public List<string> DeniedSwapCustomInfo { get; private set; } = new List<string>() { "<color=#960018>SCP-20743</color>", "SCP-20743", "SCP-20743-1", "<color=#960018>SCP-20743-1</color>" };

        [Description("Which quit / suicide percentage as SCP a player require before getting banned? (You can add tollerence in settings)")]
        public float AutoBanThreshold { get; private set; } = 30.5f;

        [Description("Which quit / suicide percentage as SCP a player require before getting kicked? (You can add tollerence in settings)")]
        public float AutoKickThreshold { get; private set; } = 15.5f;

        [Description("Which colors are restricted on .scputils_change_color command? Use command colors in game console to see them")]
        public List<string> RestrictedRoleColors { get; private set; } = new List<string>() { "Color1", "Color2" };

        [Description("Which nicknames are restricted on .scputils_change_nickname command?")]
        public List<string> BannedNickNames { get; private set; } = new List<string>() { "@everyone", "@here", "Admin" };

        [Description("Which ASNs should be blacklisted? Players to connect from blacklisted ASN should be whitelisted via scputils_whitelist_asn command (50889 is geforce now ASN)")]
        public List<string> ASNBlacklist { get; private set; } = new List<string>() { "50889" };

        [Description("Which ASNs should be ignored in multi-account detector? (50889 is geforce now ASN)")]
        public List<string> ASNWhiteslistMultiAccount { get; private set; } = new List<string>() { "50889" };

        [Description("Which message non-whitelisted players should get while connecting from blacklisted ASN?")]
        public string AsnKickMessage { get; private set; } = "The ASN you are connecting from is blacklisted from this server, please contact server staff to request to being whitelisted";

        [Description("Which message should be shown to who become SCP-096 target?")]
        public string Scp096TargetNotifyText { get; private set; } = "<color=red>Attention:</color>\n<color=purple>You became a target of SCP-096!</color>";

        [Description("Which message should be shown to last player alive of a team?")]
        public string LastPlayerAliveNotificationText { get; private set; } = "<color=red>Attention:</color>\n<color=purple>You are the last player alive of your team!</color>";

        [Description("Which message should be shown for offline warns when a player rejoin?")]
        public Extensions.Broadcast OfflineWarnNotification { get; private set; } = new Extensions.Broadcast("<color=red>Post-Warning notification:</color>\n<color=yellow>You've been recently warned for your recent quit as SCP in game, continuing this behaviour may cause a ban!</color>", 30, true, Broadcast.BroadcastFlags.Normal);

        [Description("Which message should be shown to a player when he gets banned from playing SCP?")]
        public Extensions.Broadcast RoundBanNotification { get; private set; } = new Extensions.Broadcast("<color=red>You have been banned:</color>\n<color=yellow><size=27>You have been banned from playing SCP. You are excluded from playing SCP %roundnumber% rounds due your past offences!</size></color>", 30, true, Broadcast.BroadcastFlags.Normal);

        [Description("Which message should be shown to a player when he spawns as SCP while being banned and replaced with another player?")]
        public Extensions.Broadcast RoundBanSpawnNotification { get; private set; } = new Extensions.Broadcast("<color=red>You're SCP banned:</color>\n<color=yellow><size=27>You have been removed as SCP because you're currently SCP-Banned! You must be replaced other %roundnumber% time(s) before you will be able to play SCP again!</size></color>", 30, true, Broadcast.BroadcastFlags.Normal);

        [Description("SCP swap request broadcast")]
        public Extensions.Broadcast SwapRequestBroadcast { get; private set; } = new Extensions.Broadcast("<color=blue>%player% (%scp%) wants to swap their role with you, to accept open the console with ò key and type .accept otherwise type .deny, you have %seconds% seconds left to accept this request.</color>", 20, true, Broadcast.BroadcastFlags.Normal);

        [Description("SCP swap request canceled broadcast")]
        public Extensions.Broadcast SwapRequestCanceledBroadcast { get; private set; } = new Extensions.Broadcast("<color=blue>The swap request has been canceled</color>", 12, true, Broadcast.BroadcastFlags.Normal);

        [Description("SCP swap request denied broadcast")]
        public Extensions.Broadcast SwapRequestDeniedBroadcast { get; private set; } = new Extensions.Broadcast("<color=blue>The swap request has been denied</color>", 12, true, Broadcast.BroadcastFlags.Normal);

        [Description("SCP swap request informative broadcast")]
        public Extensions.Broadcast SwapRequestInfoBroadcast { get; private set; } = new Extensions.Broadcast("<color=blue>You are an SCP, for %seconds% seconds you can exchange your role with other SCP player using swap command on ò, you can use .scplist to see who is scp</color>", 15, true, Broadcast.BroadcastFlags.Normal);

        [Description("Which time of the day the server should perform autorestart task?")]
        public string AutoRestartTimeTask { get; private set; } = "1:35:0";

        [Description("Which text should be shown outside discord embed?")]
        public string ExtraText { get; private set; } = "@everyone";

        [Description("Command cooldown text")]
        public string CooldownMessage { get; private set; } = "<color=red>Command execution failed! You are under cooldown or command banned, wait 5 seconds and try again, if the error persist you might have been banned from using commands, to see the reason and duration open the console after joining the server, this abusive action has been reported to the staff for futher punishments</color>";

        [Description("From which groups plugin should ignore DNT flag?")]
        public List<string> DntIgnoreList { get; private set; } = new List<string>() { "testusergroup1", "testusergroup2" };

        [Description("Allowed classes to see MTF and Next respawn info")]
        public List<PlayerRoles.Team> AllowedMtfInfoTeam { get; private set; } = new List<PlayerRoles.Team>() { PlayerRoles.Team.FoundationForces, PlayerRoles.Team.Scientists, PlayerRoles.Team.Dead };

        [Description("Allowed classes to see Chaos info and Next respawn info")]
        public List<PlayerRoles.Team> AllowedChaosInfoTeam { get; private set; } = new List<PlayerRoles.Team>() { PlayerRoles.Team.ClassD, PlayerRoles.Team.ChaosInsurgency, PlayerRoles.Team.Dead };

        /*

          public Dictionary<Team, ImmunityPlayers> ImmunityPlayers { get; private set; } = new Dictionary<Team, ImmunityPlayers>()
          {
              //Class-D example dictionary config
              {
                Team.CDP, //key
                new ImmunityPlayers
                {                  
                   Attacker = new List<Team>() { Team.RSC, Team.MTF }, //Attacker List
                   ShouldBeCuffed = true, //Cuffed
                   ImmunityZones = new List<ZoneType>() { ZoneType.Entrance, ZoneType.Surface } //Zone List                  
                }
              },

              //Rip example dictionary config
              {
                Team.RIP, //key
                new ImmunityPlayers
                {
                    Attacker = new List<Team>() { Team.TUT }, //Attacker List
                    ShouldBeCuffed = false, //Cuffed
                    ImmunityZones = new List<ZoneType>() { ZoneType.Entrance, ZoneType.Surface, ZoneType.LightContainment, ZoneType.HeavyContainment, ZoneType.Unspecified } //Zone list
                }
              }
          };

          */

        [Description("You have to add the team you want to protect from the target as key and enemy teams on the list as value, on github documentation you can see all the teams.")]

        public Dictionary<PlayerRoles.Team, List<PlayerRoles.Team>> CuffedImmunityPlayers { get; private set; } = new Dictionary<PlayerRoles.Team, List<PlayerRoles.Team>>()
        {
            {
                PlayerRoles.Team.ClassD,
                new List<PlayerRoles.Team>
                {
                    PlayerRoles.Team.Scientists, PlayerRoles.Team.FoundationForces
                }
            },
            {
                PlayerRoles.Team.Scientists,
                new List<PlayerRoles.Team>
                {
                    PlayerRoles.Team.ClassD, PlayerRoles.Team.ChaosInsurgency
                }
            },
        };

        [Description("Indicates if the protected teams should be cuffed to get the protection, if you don't add a team it will get protection regardless")]
        public List<PlayerRoles.Team> CuffedProtectedTeams { get; private set; } = new List<PlayerRoles.Team>() { PlayerRoles.Team.ClassD, PlayerRoles.Team.Scientists };

        [Description("Indicates in which zones the protected team is protected, Zone list: Surface, Entrance, HeavyContainment, LightContainment, Unspecified")]
        public Dictionary<PlayerRoles.Team, List<ZoneType>> CuffedSafeZones { get; private set; } = new Dictionary<PlayerRoles.Team, List<ZoneType>>()
        {
            {
                PlayerRoles.Team.ClassD,
                new List<ZoneType>
                {
                   ZoneType.Entrance, ZoneType.Surface, ZoneType.HeavyContainment, ZoneType.LightContainment, ZoneType.Other, ZoneType.Other
                }
            },
            {
                PlayerRoles.Team.Scientists,
                new List<ZoneType>
                {
                    ZoneType.Entrance, ZoneType.Surface, ZoneType.HeavyContainment, ZoneType.LightContainment, ZoneType.Other, ZoneType.Other
                }
            },
        };

        //[Description("Translations for damage types")]
        /*public Dictionary<string, string> DamageTypesTranslations { get; private set; } = new Dictionary<string, string>() { { DamageTypes.AK.ToString().ToUpper(), DamageTypes.AK.ToString().ToUpper() }, { DamageTypes.Asphyxiation.ToString().ToUpper(), DamageTypes.Asphyxiation.ToString().ToUpper() }, { DamageTypes.Bleeding.ToString().ToUpper(), DamageTypes.Bleeding.ToString().ToUpper() },
        { DeathTranslations.Scp207.Id.ToString().ToUpper(), DamageTypes.Com15.ToString().ToUpper() }, { DamageTypes.Com18.ToString().ToUpper(), DamageTypes.Com18.ToString().ToUpper() }, { DamageTypes.Crossvec.ToString().ToUpper(), DamageTypes.Crossvec.ToString().ToUpper() }, { DamageTypes.Crushed.ToString().ToUpper(), DamageTypes.Crushed.ToString().ToUpper() }, { DamageTypes.Custom.ToString().ToUpper(), DamageTypes.Custom.ToString().ToUpper() },
        { DamageTypes.Decontamination.ToString().ToUpper(), DamageTypes.Decontamination.ToString().ToUpper() }, { DamageTypes.E11Sr.ToString().ToUpper(), DamageTypes.E11Sr.ToString().ToUpper() }, { DamageTypes.Explosion.ToString().ToUpper(), DamageTypes.Explosion.ToString().ToUpper() }, { DamageTypes.Falldown.ToString().ToUpper(), DamageTypes.Falldown.ToString().ToUpper() }, { DamageTypes.FemurBreaker.ToString().ToUpper(), DamageTypes.FemurBreaker.ToString().ToUpper() },
        { DamageTypes.Firearm.ToString().ToUpper(), DamageTypes.Firearm.ToString().ToUpper() }, { DamageTypes.FriendlyFireDetector.ToString().ToUpper(), DamageTypes.FriendlyFireDetector.ToString().ToUpper() }, { DamageTypes.Fsp9.ToString().ToUpper(), DamageTypes.Fsp9.ToString().ToUpper() }, { DamageTypes.Hypothermia.ToString().ToUpper(), DamageTypes.Hypothermia.ToString().ToUpper() }, { DamageTypes.Logicer.ToString().ToUpper(), DamageTypes.Logicer.ToString().ToUpper() },
        { DamageTypes.MicroHid.ToString().ToUpper(), DamageTypes.MicroHid.ToString().ToUpper() }, { DamageTypes.ParticleDisruptor.ToString().ToUpper(), DamageTypes.ParticleDisruptor.ToString().ToUpper() }, { DamageTypes.PocketDimension.ToString().ToUpper(), DamageTypes.PocketDimension.ToString().ToUpper() }, { DamageTypes.Poison.ToString().ToUpper(), DamageTypes.Poison.ToString().ToUpper() }, { DamageTypes.Recontainment.ToString().ToUpper(), DamageTypes.Recontainment.ToString().ToUpper() },
        { DamageTypes.Scp.ToString().ToUpper(), DamageTypes.Scp.ToString().ToUpper() }, { DamageTypes.Scp018.ToString().ToUpper(), DamageTypes.Scp018.ToString().ToUpper() }, { DamageTypes.Scp049.ToString().ToUpper(), DamageTypes.Scp049.ToString().ToUpper() }, { DamageTypes.Scp0492.ToString().ToUpper(), DamageTypes.Scp0492.ToString().ToUpper() }, { DamageTypes.Scp096.ToString().ToUpper(), DamageTypes.Scp096.ToString().ToUpper() }, { DamageTypes.Scp106.ToString().ToUpper(), DamageTypes.Scp106.ToString().ToUpper() },
        { DamageTypes.Scp173.ToString().ToUpper(), DamageTypes.Scp173.ToString().ToUpper() }, { DamageTypes.Scp207.ToString().ToUpper(), DamageTypes.Scp207.ToString().ToUpper() }, { DamageTypes.Scp939.ToString().ToUpper(), DamageTypes.Scp939.ToString().ToUpper() }, { DamageTypes.SeveredHands.ToString().ToUpper(), DamageTypes.SeveredHands.ToString().ToUpper() },
        { ItemType.GunShotgun.ToString().ToUpper(), DamageTypes.Shotgun.ToString().ToUpper() }, { DamageTypes.Tesla.ToString().ToUpper(), DamageTypes.Tesla.ToString().ToUpper() }, { DamageTypes.Unknown.ToString().ToUpper(), DamageTypes.Unknown.ToString().ToUpper() }, { DamageTypes.Warhead.ToString().ToUpper(), DamageTypes.Warhead.ToString().ToUpper() }};
        */

        [Description("Broadcast to send to all online staff when player enter with more than 1 account")]
        public Extensions.Broadcast AlertStaffBroadcastMultiAccount { get; private set; } = new Extensions.Broadcast("<size=40><color=red>Alert</color></size>\n<size=35>Player <color=yellow>{player}</color> has entered with <color=yellow>{accountNumber}</color> accounts</size>\n<size=30>Check console pressing <color=yellow>ò</color></size>", 10);

        [Description("Broadcast to send to all online staff when player change IP")]
        public Extensions.Broadcast AlertStaffBroadcastChangeIP { get; private set; } = new Extensions.Broadcast("<size=40><color=red>Alert</color></size>\n<size=35>Player <color=yellow>{player}</color> has changed IP. <color=yellow>{oldIP}</color> to <color=yellow>{newIP}</color></size>\n<size=35>Check console pressing <color=yellow>ò</color></size>", 10);

        public Extensions.Broadcast AutoBanPlayerMessage { get; private set; } = new Extensions.Broadcast("<color=#4169E1>[SCPUTILS: ADMINISTRATION BROADCAST]</color>\n\n%player.Nickname% (%player.Role%) has been <color=#FF0000>BANNED</color> from the server for exceeding Quits / Suicides (as SCP) limit.\nDuration: %duration% mitutes", 12, true, Broadcast.BroadcastFlags.AdminChat);
        public void ConfigValidator()
        {
            if (MaxAllowedTimeScpSwapRequest > MaxAllowedTimeScpSwapRequestAccept)
            {
                Log.Warning("MaxAllowedTimeScpSwapRequest is higher than MaxAllowedTimeScpSwapRequestAccept, players might not be able to accept some requests doublecheck config!");
            }

            if (ScpSuicideTollerance < 0)
            {
                Log.Warning("Invalid config scputils_scp_suicide_tollerance, loading dafault one!");
                ScpSuicideTollerance = 5;
            }

            if (AutoKickThreshold >= AutoBanThreshold)
            {
                Log.Warning("Invalid config scputils_auto_kick_threshold OR scputils_auto_ban_threshold, loading dafault one!");
                AutoBanThreshold = 30.5f;
            }

            if (AutoRestartTime < 0)
            {
                Log.Warning("Invalid config scputils_auto_restart_time, loading dafault one!");
                AutoRestartTime = 15;
            }

            if (Scp079TeslaEventWait < 0)
            {
                Log.Warning("Invalid config scputils_scp_079_tesla_event_wait, loading dafault one!");
                Scp079TeslaEventWait = 2;
            }

            if (IgnoreDntRequests)
            {
                Log.Warning("You have set in server configs to ignore Do Not Track requests but that's a violation on Verified Server Rules (if your server is verified) and could cause punishement such as delist [Rule 8.11]");
            }

            if (EnableSCPSuicideSoftBan)
            {
                EnableSCPSuicideAutoBan = false;
            }
        }
    }
}