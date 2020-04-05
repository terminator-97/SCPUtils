using EXILED;
using EXILED.Extensions;
using LiteDB;
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
                    TotalScpSuicideBans = 0,
                    FirstJoin = DateTime.Now
                    
                });
                Log.Info("Trying to add ID: " + player.GetUserId().Split('@')[0] + " Discriminator: " + player.GetUserId().Split('@')[1] + " to Database");
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot add new user to Database: {player.GetNickname()} ({player.GetUserId().Split('@')[0]})!\n{ex.ToString()}");
            }
        }


    }
}
