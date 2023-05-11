namespace SCPUtils
{
    using Exiled.API.Features;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;


    public static class Database
    {
        public static IMongoClient MongoClient { get; private set; }

        public static IMongoDatabase MongoDatabase { get; private set; }

        public static void OpenDatabase()
        {
            try
            {
                var connectionString = string.IsNullOrEmpty(ScpUtils.StaticInstance.Config.DatabasePassword)
                 ? $"mongodb://{ScpUtils.StaticInstance.Config.DatabaseIp}:{ScpUtils.StaticInstance.Config.DatabasePort}"
                 : $"mongodb://{ScpUtils.StaticInstance.Config.DatabaseUser}:{ScpUtils.StaticInstance.Config.DatabasePassword}@{ScpUtils.StaticInstance.Config.DatabaseIp}:{ScpUtils.StaticInstance.Config.DatabasePort}/?authMechanism={ScpUtils.StaticInstance.Config.DatabaseAuthType}";

                MongoClient = new MongoClient(connectionString);
                MongoDatabase = MongoClient.GetDatabase(ScpUtils.StaticInstance.Config.DatabaseUser);

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
                Log.Error(string.Format("Failed to connect Database! ", e));
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
                Log.Error(string.Format("Failed to disconnect Database! ", e));
            }
        }
    }
}