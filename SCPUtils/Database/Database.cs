
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using PluginAPI.Core;

namespace SCPUtils
{
    public class Database
    {
        private readonly ScpUtils pluginInstance;

        public Database(ScpUtils pluginInstance)
        {
            this.pluginInstance = pluginInstance;
        }

        public static LiteDatabase LiteDatabase { get; private set; }
        public string DatabaseDirectory => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), pluginInstance.configs.DatabaseFolder), pluginInstance.configs.DatabaseName);
        public string DatabaseFullPath => Path.Combine(DatabaseDirectory, $"{pluginInstance.configs.DatabaseName}.db");
        public static Dictionary<PluginAPI.Core.Player, Player> PlayerData = new Dictionary<PluginAPI.Core.Player, Player>();
        public static Dictionary<string, Broadcast> Broadcast = new Dictionary<string, Broadcast>();
        public static Dictionary<string, PlaySession> PlaySession = new Dictionary<string, PlaySession>();

        public void CreateDatabase()
        {
            if (Directory.Exists(DatabaseDirectory))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(DatabaseDirectory);
                Log.Warning("Database not found, Creating new DB");
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot create new DB!\n{ex.ToString()}");
            }
        }

        public void OpenDatabase()
        {
            try
            {
                LiteDatabase = new LiteDatabase(DatabaseFullPath);
                LiteDatabase.GetCollection<Player>().EnsureIndex(x => x.Id);
                LiteDatabase.GetCollection<Player>().EnsureIndex(x => x.Name);
                LiteDatabase.GetCollection<BroadcastDb>().EnsureIndex(x => x.Id);
                LiteDatabase.GetCollection<DatabaseIp>().EnsureIndex(x => x.Id);
                LiteDatabase.GetCollection<PlaySession>().EnsureIndex(x => x.Id);
                Log.Info("DB Loaded!");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to open DB!\nPlease make sure that there is only 1 server open on same database, check that there are no ghost proccess, if the error still occurrs check LITEDB version and if there are the proper permissions. Bellow you can see the error. \n \n {ex.ToString()}");
            }
        }
        public void NewSession(PluginAPI.Core.Player player)
        {
            try
            {
                if (LiteDatabase.GetCollection<PlaySession>().Exists(x => x.Id == DatabasePlayer.GetRawUserId(player))) return;

                LiteDatabase.GetCollection<PlaySession>().Insert(new PlaySession()
                {
                    Id = DatabasePlayer.GetRawUserId(player),
                    Authentication = DatabasePlayer.GetAuthentication(player),
                    PlaytimeSessionsLog = null
                });


            }
            catch (Exception ex)
            {
                Log.Error($"Error occurred in PlayTimeSession!\n{ex}");
            }
        }
        public void AddBroadcast(string id, string nickname, int seconds, string text)
        {
            try
            {
                if (LiteDatabase.GetCollection<BroadcastDb>().Exists(x => x.Id == id))
                {
                    return;
                }


                LiteDatabase.GetCollection<BroadcastDb>().Insert(new BroadcastDb()
                {
                    Id = id,
                    CreatedBy = nickname,
                    Seconds = seconds,
                    Text = text
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot create the broadcast!\n{ex.ToString()}");
            }
        }

        public void AddIp(string ip, string uid)
        {
            try
            {
                if (LiteDatabase.GetCollection<DatabaseIp>().Exists(x => x.Id == ip))
                {
                    return;
                }


                LiteDatabase.GetCollection<DatabaseIp>().Insert(new DatabaseIp()
                {
                    Id = ip,
                    UserIds = new List<string>() { uid }
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot add new IP!\n{ex.ToString()}");
            }
        }


        public void AddPlayer(PluginAPI.Core.Player player)
        {
            try
            {
                if (LiteDatabase.GetCollection<Player>().Exists(x => x.Id == DatabasePlayer.GetRawUserId(player)))
                {
                    return;
                }

                LiteDatabase.GetCollection<Player>().Insert(new Player()
                {
                    Id = DatabasePlayer.GetRawUserId(player),
                    Name = player.Nickname,
                    Ip = "None",
                    Authentication = DatabasePlayer.GetAuthentication(player),
                    ScpSuicideCount = 0,
                    TotalScpGamesPlayed = 0,
                    TotalScpSuicideKicks = 0,
                    TotalScpSuicideBans = 0,
                    RoundBanLeft = 0,
                    FirstJoin = DateTime.Now,
                    LastSeen = DateTime.Now,
                    ColorPreference = "",
                    CustomNickName = "",
                    BadgeName = "",
                    BadgeExpire = DateTime.MinValue,
                    PreviousBadge = "",
                    HideBadge = false,
                    PlayTimeRecords = null,
                    ASNWhitelisted = false,
                    Restricted = null,
                    KeepPreferences = false,
                    IgnoreDNT = false,
                    // PlaytimeSessionsLog = null,
                    Expire = null,
                    MultiAccountWhiteList = false,
                    NicknameCooldown = DateTime.Now,
                    OverwatchActive = false,
                    LastRespawn = DateTime.MinValue
                });

            }
            catch (Exception ex)
            {
                Log.Error($"Cannot add new user to Database: {player.Nickname} ({player.UserId.Split('@')[0]})!\n{ex.ToString()}");
            }
        }

    }
}
