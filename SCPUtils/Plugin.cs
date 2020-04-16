using EXILED;
using MEC;
using System;
using System.Collections.Generic;
using Harmony;

namespace SCPUtils
{

    public class SCPUtils : Plugin
    {
        public static bool IsStarted { get; set; }
        public static string pluginVersion = "1.7.1";
        public override string getName { get; } = "SCPUtils";

        public EventHandlers EventHandlers { get; private set; }
        public Commands Commands { get; private set; }
        public Functions Functions { get; private set; }
        public Player Player { get; private set; }
        public ConsoleCommands PlayerConsoleCommands { get; private set; }
        internal ExiledVersion ExiledVersion { get; private set; } = new ExiledVersion() { Major = 1, Minor = 9, Patch = 10 };
        public int PatchesCounter { get; private set; }
        public HarmonyInstance HarmonyInstance { get; private set; }

        //Configs
        public bool isEnabled;
        public bool enableSCPSuicideAutoWarn;
        public bool enableRoundRestartCheck;
        public bool quitEqualsSuicide;
        public bool welcomeEnabled;
        public bool decontaminationMessageEnabled;
        public bool autoKickOnSCPSuicide;
        public bool enableSCPSuicideAutoBan;
        public bool multiplyBanDurationEachBan;
        public bool saveColorChoice;
        public bool resetPreferencedOnBadgeExpire;
        public string autoRestartMessage;
        public string suicideWarnMessage;
        public string welcomeMessage;
        public string decontaminationMessage;
        public string suicideKickMessage;
        public string autoBanMessage;
        public string unauthorizedNickNameChange;
        public string unauthorizedColorChange;
        public string unauthorizedBadgeChangeVisibility;
        public uint welcomeMessageDuration;
        public uint decontaminationMessageDuration;
        public uint autoRestartTime;
        public uint autoWarnMessageDuration;
        public int scpSuicideTollerance;
        public int autoBanDuration;
        public int SCP079TeslaEventWait;
        public float autoBanThreshold;
        public float autoKickThreshold;
        public List<string> restrictedRoleColors = new List<string>();  
    

        public override void OnEnable()
        {
            LoadConfig();

            if (!isEnabled) return;
            Commands = new Commands();
            Functions = new Functions(this);
            EventHandlers = new EventHandlers(this);
            PlayerConsoleCommands = new ConsoleCommands(this);
            LoadEvents();
            LoadCommands();
            Database.CreateDatabase();
            Database.OpenDatabase();
            Log.Info($"{getName} {pluginVersion} Loaded!");
            HarmonyInstance = HarmonyInstance.Create($"com.terminator97.scputils.{PatchesCounter++}");
            HarmonyInstance.PatchAll();
        }

        public override void OnDisable()
        {
            Events.RoundStartEvent -= EventHandlers.OnRoundStart;
            Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
            Events.PlayerDeathEvent -= EventHandlers.OnPlayerDeath;
            Events.RoundRestartEvent -= EventHandlers.OnRoundRestart;
            Events.PlayerJoinEvent -= EventHandlers.OnPlayerJoin;
            Events.DecontaminationEvent -= EventHandlers.OnDecontaminate;
            Events.RemoteAdminCommandEvent -= Commands.OnRaCommand;
            Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
            Events.PlayerSpawnEvent -= EventHandlers.OnPlayerSpawn;
            Events.Scp079TriggerTeslaEvent -= EventHandlers.On079Tesla;
            Timing.KillCoroutines(Functions.DT);
            EventHandlers = null;
            Commands = null;
            Functions = null;
            Database.LiteDatabase.Dispose();
            HarmonyInstance.UnpatchAll();
        }

        public override void OnReload()
        {
        }

        public void LoadEvents()
        {
            Events.RoundStartEvent += EventHandlers.OnRoundStart;
            Events.RoundEndEvent += EventHandlers.OnRoundEnd;
            Events.PlayerDeathEvent += EventHandlers.OnPlayerDeath;
            Events.RoundRestartEvent += EventHandlers.OnRoundRestart;
            Events.PlayerJoinEvent += EventHandlers.OnPlayerJoin;
            Events.DecontaminationEvent += EventHandlers.OnDecontaminate;
            Events.Scp079TriggerTeslaEvent += EventHandlers.On079Tesla;
            Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
            Events.PlayerSpawnEvent += EventHandlers.OnPlayerSpawn;
        }

        public void LoadCommands()
        {
            Events.RemoteAdminCommandEvent += Commands.OnRaCommand;
            Events.ConsoleCommandEvent += PlayerConsoleCommands.OnConsoleCommand;
        }

        internal void LoadConfig()
        {
            isEnabled = Config.GetBool("scputils_enabled", true);
            enableRoundRestartCheck = Config.GetBool("scputils_enable_round_restart_check", true);
            enableSCPSuicideAutoWarn = Config.GetBool("scputils_enable_scp_suicide_autowarn", true);
            autoKickOnSCPSuicide = Config.GetBool("scputils_auto_kick_scp_suicide", true);
            quitEqualsSuicide = Config.GetBool("scputils_quit_equals_suicide", true);
            welcomeEnabled = Config.GetBool("scputils_welcome_enabled", true);
            decontaminationMessageEnabled = Config.GetBool("scputils_decontamination_message_enabled", false);
            enableSCPSuicideAutoBan = Config.GetBool("scputils_enable_scp_suicide_auto_ban", true);
            multiplyBanDurationEachBan = Config.GetBool("scputils_double_ban_duration_each_ban", true);
            resetPreferencedOnBadgeExpire = Config.GetBool("scputils_reset_preferences_on_badge_expire", true);      
            welcomeMessage = Config.GetString("scputils_welcome_message", "Welcome to the server!");
            decontaminationMessage = Config.GetString("scputils_decontamination_message", "Decontamination has started!");
            autoRestartMessage = Config.GetString("scputils_auto_restart_message", "<color=red>Round Restart:</color>\n<color=yellow>Restarting round in {0} seconds due lack of players</color>");
            suicideWarnMessage = Config.GetString("scputils_suicide_warn_message", "<color=red>WARN:\nAs per server rules SCP's suicide is an offence, doing it will result in a ban!</color>");
            suicideKickMessage = Config.GetString("scputils_suicide_kick_message", "Suicide as SCP");
            autoBanMessage = Config.GetString("scputils_auto_ban_message", "Exceeded SCP suicide limit Duration: {0} minutes");
            unauthorizedNickNameChange = Config.GetString("scputils_unauthorized_nickname_change", "You can't do that!");
            unauthorizedColorChange = Config.GetString("scputils_unauthorized_color_change", "You can't do that!");
            unauthorizedBadgeChangeVisibility = Config.GetString("scputils_unauthorized_badge_change_visibility", "You need a higher administration level to use this command!");
            welcomeMessageDuration = Config.GetUInt("scputils_welcome_duration", 12);
            decontaminationMessageDuration = Config.GetUInt("scputils_decontamination_message_duration", 10);
            autoRestartTime = Config.GetUInt("scputils_auto_restart_time", 15);
            autoWarnMessageDuration = Config.GetUInt("scputils_autowarn_message_duration", 30);
            autoBanDuration = Config.GetInt("scputils_auto_ban_duration", 15);
            scpSuicideTollerance = Config.GetInt("scputils_auto_ban_tollerance", 5);
            SCP079TeslaEventWait = Config.GetInt("scputils_scp_079_tesla_event_wait", 2);
            autoBanThreshold = Config.GetFloat("scputils_auto_ban_threshold", 30.5f);
            autoKickThreshold = Config.GetFloat("scputils_auto_kick_threshold", 15.5f);
            restrictedRoleColors = Config.GetStringList("scputils_restricted_role_colors");
     
            ConfigValidator();
        }

        public void ConfigValidator()
        {
            if (scpSuicideTollerance < 0)
            {
                Log.Warn("Invalid config scputils_scp_suicide_tollerance, loading dafault one!");
                scpSuicideTollerance = 5;

            }
            if (autoKickThreshold >= autoBanThreshold)
            {
                Log.Warn("Invalid config scputils_auto_kick_threshold OR scputils_auto_ban_threshold, loading dafault one!");
                autoBanThreshold = 30.5f;

            }
            if (autoRestartTime < 0)
            {
                Log.Warn("Invalid config scputils_auto_restart_time, loading dafault one!");
                autoRestartTime = 15;
            }
            if (SCP079TeslaEventWait < 0)
            {
                Log.Warn("Invalid config scputils_scp_079_tesla_event_wait, loading dafault one!");
                SCP079TeslaEventWait = 2;
            }
            if (!isEnabled)
            {
                Log.Warn("You disabled the plugin in server configs!");
            }
            if (Version.Parse($"{EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}") < Version.Parse($"{ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}"))
            {
                Log.Warn($"You are running the plugin in an outdated EXILED version, you may try to use the plugin but it's advisable to update your EXILED version (Required version: {ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}), plugin developer won't offer support for incompatible EXILED versions!");
            }
        }
    }
}