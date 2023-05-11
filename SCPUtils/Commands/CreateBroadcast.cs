using CommandSystem;
using Exiled.Permissions.Extensions;
using MongoDB.Driver;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class CreateBroadcast : ICommand
    {
        public string Command { get; } = "scputils_broadcast_create";

        public string[] Aliases { get; } = new[] { "cbc", "su_cbc", "scpu_cbc" };

        public string Description { get; } = "Allows to create custom broadcasts";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.broadcastcreate"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"<color=yellow>Usage: {Command} <id> <duration> <text>";
                return false;
            }
            else
            {
                if (int.TryParse(arguments.Array[2].ToString(), out int duration))
                {
                    if (Database.LiteDatabase.GetCollection<BroadcastDb>().Exists(broadcast => broadcast.Id == arguments.Array[1].ToString()))
                    {
                        response = "Id already exist!";
                        return false;
                    }
                    else
                    {
                        var broadcast = string.Join(" ", arguments.Array, 3, arguments.Array.Length - 3);
                        BroadcastDb = new Broadcast
                        {
                            Aliases 

                        }
                        ScpUtils.StaticInstance.DatabasePlayerData.AddBroadcast(arguments.Array[1].ToString(), sender.LogName, duration, broadcast.ToString());
                        Database.MongoDatabase.GetCollection<BroadcastDb>("broadcasts").InsertOne(arguments.Array[1].ToString(), sender.LogName, duration, broadcast.ToString());
                        response = "Success!";
                        return true;
                    }
                }
                else
                {
                    response = "Broadcast duration must be an integer";
                    return false;
                }
            }


        }
    }
}
