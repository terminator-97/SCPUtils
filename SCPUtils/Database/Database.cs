namespace SCPUtils
{
    using MongoDB.Driver;
    using PluginAPI.Core;
    using System;
    using System.Collections.Generic;
    using static ScpUtils;

    public static class Database
    {
        public static IMongoClient MongoClient { get; private set; }

        public static IMongoDatabase MongoDatabase { get; private set; }

        public static Dictionary<PluginAPI.Core.Player, Player> PlayerData = new Dictionary<PluginAPI.Core.Player, Player>();

        public static void OpenDatabase()
        {
            try
            {
                var connectionString = string.IsNullOrEmpty(StaticInstance.databaseConfig.DatabasePassword)
                  ? $"mongodb://{StaticInstance.databaseConfig.DatabaseIp}:{StaticInstance.databaseConfig.DatabasePort}"
                  : $"mongodb://{StaticInstance.databaseConfig.DatabaseUser}:{StaticInstance.databaseConfig.DatabasePassword}@{StaticInstance.databaseConfig.DatabaseIp}:{StaticInstance.databaseConfig.DatabasePort}/?authMechanism={StaticInstance.databaseConfig.DatabaseAuthType}";

                MongoClient = new MongoClient(connectionString);
                MongoDatabase = MongoClient.GetDatabase(StaticInstance.databaseConfig.DatabaseName.ToLower());

                var player = new List<CreateIndexModel<Player>>()
                {
                    new CreateIndexModel<Player>(Builders<Player>.IndexKeys.Ascending(x => x.Id)),
                    new CreateIndexModel<Player>(Builders<Player>.IndexKeys.Ascending(x => x.Name)),
                    new CreateIndexModel<Player>(Builders<Player>.IndexKeys.Ascending(x => x.Ip)),
                };

                var ips = new List<CreateIndexModel<DatabaseIp>>
                {
                    new CreateIndexModel<DatabaseIp>(Builders<DatabaseIp>.IndexKeys.Ascending(x => x.Id)),
                };

                var broadcast = new List<CreateIndexModel<BroadcastDb>>
                {
                    new CreateIndexModel<BroadcastDb>(Builders<BroadcastDb>.IndexKeys.Ascending(x => x.Id)),
                };


                MongoDatabase.GetCollection<Player>("players").Indexes.CreateMany(player);
                MongoDatabase.GetCollection<DatabaseIp>("ipaddresses").Indexes.CreateMany(ips);
                MongoDatabase.GetCollection<BroadcastDb>("broadcasts").Indexes.CreateMany(broadcast);

                Log.Info("You have been connected!");
            }
            catch (Exception e)
            {
                Log.Error($"Failed to connect Database! Error: {e}");
            }
        }

        public static void Close()
        {
            try
            {
                MongoClient = null;
                MongoDatabase = null;

                Log.Info("You have been disconnected!");
            }
            catch (Exception e)
            {
                Log.Error($"Failed to disconnect Database! Error: {e}");
            }
        }
    }
}