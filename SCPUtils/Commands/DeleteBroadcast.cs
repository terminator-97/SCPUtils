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
        public string Command { get; } = "scputils_broadcast_delete";

        public string[] Aliases { get; } = new[] { "dbc", "su_dbc", "scpu_dbc" };

        public string Description { get; } = "Allows to delete custom broadcastes";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.broadcastdelete"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }

            else if (arguments.Count < 1)
            {
                response = $"<color=yellow>Usage: {Command} <id>";
                return false;
            }
            else
            {
                var databaseBroadcast = GetBroadcast.FindBroadcast(arguments.Array[1]);

                if (databaseBroadcast != null)
                {
                    Database.MongoDatabase.GetCollection<BroadcastDb>("broadcasts").DeleteOne(broadcast => broadcast.Id == databaseBroadcast.Id);
                    response = "Success!";
                    return true;
                }
                else
                {
                    response = "Id does not exist!";
                    return false;
                }
            }
        }
    }
}
