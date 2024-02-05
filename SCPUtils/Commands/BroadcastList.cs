using CommandSystem;
using Exiled.Permissions.Extensions;
using MongoDB.Driver;
using System;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class BroadcastList : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.BroadcastlistCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.BroadcastlistAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.BroadcastlistDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.broadcastlist"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            StringBuilder broadcastList = new StringBuilder("[Broadcast List]");
            broadcastList.AppendLine();
            foreach (BroadcastDb databaseBroadcast in Database.MongoDatabase.GetCollection<BroadcastDb>("broadcasts").AsQueryable().ToList())
            {
                broadcastList.AppendLine($"ID: {databaseBroadcast.Id}");
                broadcastList.AppendLine($"Created by: {databaseBroadcast.CreatedBy}");
                broadcastList.AppendLine($"Default duration: {databaseBroadcast.Seconds}");
                broadcastList.AppendLine($"Text: {databaseBroadcast.Text}");
                broadcastList.AppendLine("---------");
            }
            response = $"{broadcastList}";
            return true;
        }
    }
}
