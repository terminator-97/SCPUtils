using Exiled.API.Features;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;

namespace SCPUtils
{
    public class Database
    {
        private readonly ScpUtils pluginInstance;

        public Database(ScpUtils pluginInstance) => this.pluginInstance = pluginInstance;


        public static LiteDatabase LiteDatabase { get; private set; }
        public string DatabaseDirectory => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), pluginInstance.Config.DatabaseFolder), pluginInstance.Config.DatabaseName);
        public string DatabaseFullPath => Path.Combine(DatabaseDirectory, $"{pluginInstance.Config.DatabaseName}.db");
        public static Dictionary<Exiled.API.Features.Player, Player> PlayerData = new Dictionary<Exiled.API.Features.Player, Player>();

        public void CreateDatabase()
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

        public void OpenDatabase()
        {
            try
            {
                LiteDatabase = new LiteDatabase(DatabaseFullPath);
                LiteDatabase.GetCollection<Player>().EnsureIndex(x => x.Id);
                LiteDatabase.GetCollection<Player>().EnsureIndex(x => x.Name);
                Log.Info("DB Loaded!");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to open DB!\n{ex.ToString()}");
            }
        }

        public void AddPlayer(Exiled.API.Features.Player player)
        {
            try
            {
                if (LiteDatabase.GetCollection<Player>().Exists(x => x.Id == DatabasePlayer.GetRawUserId(player))) return;

                LiteDatabase.GetCollection<Player>().Insert(new Player()
                {
                    Id = DatabasePlayer.GetRawUserId(player),
                    Name = player.Nickname,
                    Authentication = DatabasePlayer.GetAuthentication(player),
                    ScpSuicideCount = 0,
                    TotalScpGamesPlayed = 0,
                    TotalScpSuicideKicks = 0,
                    TotalScpSuicideBans = 0,
                    FirstJoin = DateTime.Now,
                    LastSeen = DateTime.Now,
                    ColorPreference = "",
                    CustomNickName = "",
                    BadgeName = "",
                    BadgeExpire = DateTime.MinValue,
                    HideBadge = false,
                    PlayTimeRecords = null,
                    ASNWhitelisted = false
                });
                Log.Info("Trying to add ID: " + player.UserId.Split('@')[0] + " Discriminator: " + player.UserId.Split('@')[1] + " to Database");
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot add new user to Database: {player.Nickname} ({player.UserId.Split('@')[0]})!\n{ex.ToString()}");
            }
        }

    }
}
