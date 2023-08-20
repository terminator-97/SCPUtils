namespace SCPUtils
{
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using static ScpUtils;

    public static class Database
    {
        public static IMongoClient MongoClient { get; private set; }

        public static IMongoDatabase MongoDatabase { get; private set; }

        public static Dictionary<Exiled.API.Features.Player, Player> PlayerData = new Dictionary<Exiled.API.Features.Player, Player>();

        public static void OpenDatabase()
        {
            try
            {


                var connectionString = string.IsNullOrEmpty(StaticInstance.Config.DatabasePassword)
                  ? $"mongodb://{StaticInstance.Config.DatabaseIp}:{StaticInstance.Config.DatabasePort}"
                  : $"mongodb://{StaticInstance.Config.DatabaseUser}:{StaticInstance.Config.DatabasePassword}@{StaticInstance.Config.DatabaseIp}:{StaticInstance.Config.DatabasePort}/?authMechanism={StaticInstance.Config.DatabaseAuthType}";


                MongoClient = new MongoClient(connectionString);
                MongoDatabase = MongoClient.GetDatabase(StaticInstance.Config.DatabaseName.ToSnakeCase());

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
                Log.Error($"Failed to connect Database! \n Please make sure that you correctly installed mongodb server and check if you correctly configured this plugin / mongodb server itself. \n If you have not installed mongodb server download it there: https://www.mongodb.com/try/download/community \n \n Error: {e}");
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