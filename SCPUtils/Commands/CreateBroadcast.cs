using CommandSystem;
using Exiled.Permissions.Extensions;
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
                    if (GetBroadcast.FindBroadcast(arguments.Array[1].ToString()) != null)
                    {
                        response = "Id already exist!";
                        return false;
                    }
                    else
                    {
                        var broadcast = string.Join(" ", arguments.Array, 3, arguments.Array.Length - 3);
                        GetBroadcast.AddBroadcast(arguments.Array[1].ToString(), duration, broadcast.ToString(), sender.LogName);
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
