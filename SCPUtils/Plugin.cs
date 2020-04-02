using EXILED;
using EXILED.Extensions;
using LiteDB;
using MEC;
using System;
using System.Collections.Generic;
using System.IO;


namespace SCPUtils
{
    public static class Database
    {       
        public static string databaseName = "SCPUtils";
        public static LiteDatabase LiteDatabase { get; private set; }
        public static string DatabaseDirectory => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), databaseName);
        public static string DatabaseFullPath => Path.Combine(DatabaseDirectory, $"{databaseName}.db");

        public static Dictionary<ReferenceHub, Player> PlayerData = new Dictionary<ReferenceHub, Player>();
        public static void CreateDatabase()
        {
            if (Directory.Exists(DatabaseDirectory)) return;

            try
            {
                Directory.CreateDirectory(DatabaseDirectory);
                Log.Warn("Database not found, Creating new DB");
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot create new DB!\n{ex.ToString()}");
            }
        }

        public static void OpenDatabase()
        {
            try
            {
                LiteDatabase = new LiteDatabase(DatabaseFullPath);

                LiteDatabase.GetCollection<Player>().EnsureIndex(x => x.Name);

                Log.Info("DB Loaded!");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to open DB!\n{ex.ToString()}");
            }
        }

        public static void AddPlayer(ReferenceHub player)
        {
            try
            {
                if (LiteDatabase.GetCollection<Player>().Exists(x => x.Id == DatabasePlayer.GetRawUserId(player))) return;

                LiteDatabase.GetCollection<Player>().Insert(new Player()
                {
                    Id = DatabasePlayer.GetRawUserId(player),
                    Name = player.GetNickname(),
                    Authentication = DatabasePlayer.GetAuthentication(player),
                    ScpSuicideCount = 0,
                    TotalScpGamesPlayed = 0,
                    TotalScpSuicideKicks = 0,
                    TotalScpSuicideBans = 0
                });
                Log.Info("Trying to add ID: " + player.GetUserId().Split('@')[0] + " Discriminator: " + player.GetUserId().Split('@')[1] + " to Database");
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot add new user to Database: {player.GetNickname()} ({player.GetUserId().Split('@')[0]})!\n{ex.ToString()}");
            }
        }


    }
    public class Utils : Plugin
    {     
        public static bool IsStarted { get; set; }
        public static string pluginVersion = "1.2.4";
        public override string getName { get; } = "SCPUtils";

        public EventHandlers EventHandlers { get; private set; }
        public Commands Commands { get; private set; }
        public Functions Functions { get; private set; }
        public Player Player { get; private set; }

        internal ExiledVersion ExiledVersion { get; private set; } = new ExiledVersion() { Major = 1, Minor = 9, Patch = 10 };

        //Configs
        public bool enableSCPSuicideAutoWarn;
        public bool enableRoundRestartCheck;
        public bool quitEqualsSuicide;
        public bool welcomeEnabled;
        public bool decontaminationMessageEnabled;
        public bool autoKickOnSCPSuicide;
        public bool removeOverwatchRoundStart;
        public bool enableSCPSuicideAutoBan;
        public bool multiplyBanDurationEachBan;
        public string autoRestartMessage;
        public string suicideWarnMessage;
        public string welcomeMessage;
        public string decontaminationMessage;
        public string suicideKickMessage;
        public string autoBanMessage;
        public uint welcomeMessageDuration;
        public uint decontaminationMessageDuration;
        public uint autoRestartTime;
        public uint autoWarnMessageDuration;
        public int scpSuicideTollerance;
        public int autoBanDuration;
        public int SCP079TeslaEventWait;
        public float autoBanThreshold;
        public float autoKickThreshold;

        public override void OnEnable()
        {
            Commands = new Commands();
            Functions = new Functions(this, Commands);
            EventHandlers = new EventHandlers(Functions, this);
            LoadEvents();
            LoadConfig();
            LoadCommands();
            Database.CreateDatabase();
            Database.OpenDatabase();
            Log.Info($"{getName} {pluginVersion} Loaded!");
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
            Timing.KillCoroutines(Functions.DT);
            EventHandlers = null;
            Commands = null;
            Functions = null;
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
        }

        internal void LoadConfig()
        {
            enableRoundRestartCheck = Config.GetBool("scputils_enable_round_restart_check", true);
            enableSCPSuicideAutoWarn = Config.GetBool("scputils_enable_scp_suicide_autowarn", true);
            autoKickOnSCPSuicide = Config.GetBool("scputils_auto_kick_scp_suicide", true);
            quitEqualsSuicide = Config.GetBool("scputils_quit_equals_suicide", true);
            welcomeEnabled = Config.GetBool("scputils_welcome_enabled", true);
            decontaminationMessageEnabled = Config.GetBool("scputils_decontamination_message_enabled", false);
            enableSCPSuicideAutoBan = Config.GetBool("scputils_enable_scp_suicide_auto_ban", true);
            removeOverwatchRoundStart = Config.GetBool("scputils_remove_overwatch_round_start", false);
            multiplyBanDurationEachBan = Config.GetBool("scputils_double_ban_duration_each_ban", true);
            welcomeMessage = Config.GetString("scputils_welcome_message", "Welcome to the server!");
            decontaminationMessage = Config.GetString("scputils_decontamination_message", "Decontamination has started!");
            autoRestartMessage = Config.GetString("scputils_auto_restart_message", "<color=red>Round Restart:</color>\n<color=yellow>Restarting round in {0} seconds due lack of players</color>");
            suicideWarnMessage = Config.GetString("scputils_suicide_warn_message", "<color=red>WARN:\nAs per server rules SCP's suicide is an offence, doing it will result in a ban!</color>");
            suicideKickMessage = Config.GetString("scputils_suicide_kick_message", "Suicide as SCP");
            autoBanMessage = Config.GetString("scputils_auto_ban_message", "Exceeded SCP suicide limit Duration: {0} minutes");
            welcomeMessageDuration = Config.GetUInt("scputils_welcome_duration", 12);
            decontaminationMessageDuration = Config.GetUInt("scputils_decontamination_message_duration", 10);
            autoRestartTime = Config.GetUInt("scputils_auto_restart_time", 15);
            autoWarnMessageDuration = Config.GetUInt("scputils_autowarn_message_duration", 30);
            autoBanDuration = Config.GetInt("scputils_auto_ban_duration", 15);
            scpSuicideTollerance = Config.GetInt("scputils_auto_ban_tollerance", 5);
            SCP079TeslaEventWait = Config.GetInt("scputils_scp_079_tesla_event_wait", 2);
            autoBanThreshold = Config.GetFloat("scputils_auto_ban_threshold", 30.5f);
            autoKickThreshold = Config.GetFloat("scputils_auto_kick_threshold", 15.5f);
            ConfigValidator();
        }

        public void ConfigValidator()
        {
            if (scpSuicideTollerance < 0) Log.Warn("Invalid config scputils_scp_suicide_tollerance!");
            if (autoKickThreshold >= autoBanThreshold) Log.Warn("Invalid config scputils_auto_kick_threshold OR scputils_auto_ban_threshold!");
            if (autoRestartTime < 0) Log.Warn("Invalid config scputils_auto_restart_time!");
            if (SCP079TeslaEventWait < 0) Log.Warn("Invalid config scputils_scp_079_tesla_event_wait!");
            if (Version.Parse($"{EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}") < Version.Parse($"{ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}")) Log.Warn($"You are running the plugin in an outdated EXILED version, you may try to use the plugin but it's advisable to update your EXILED version (Required version: {ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}), plugin developer won't offer support for incompatible EXILED versions!");
        }
    }
}