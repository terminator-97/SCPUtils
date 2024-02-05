using CommandSystem;
using Exiled.Permissions.Extensions;
using MongoDB.Driver;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class DeleteBroadcast : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.DeletebroadcastCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.DeletebroadcastAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.DeletebroadcastDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.broadcastdelete"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }

            else if (arguments.Count < 1)
            {
                response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgId}";
                return false;
            }
            else
            {
                var databaseBroadcast = GetBroadcast.FindBroadcast(arguments.Array[1]);

                if (databaseBroadcast != null)
                {
                    Database.MongoDatabase.GetCollection<BroadcastDb>("broadcasts").DeleteOne(broadcast => broadcast.Id == databaseBroadcast.Id);
                    response = ScpUtils.StaticInstance.Translation.Success;
                    return true;
                }
                else
                {
                    response = ScpUtils.StaticInstance.Translation.InvalidId;
                    return false;
                }
            }
        }
    }
}
