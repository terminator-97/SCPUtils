using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class Broadcast : ICommand
    {
        public string Command { get; } = "scputils_broadcast";

        public string[] Aliases { get; } = new[] { "sbc", "gbc" };

        public string Description { get; } = "Allows to send custom broadcastes";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string broadcast;
            if (!sender.CheckPermission("scputils.broadcast"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }

            else if (arguments.Count < 2)
            {
                response = $"<color=yellow>Usage: {Command} <hint/broadcast> <text></color>";
                return false;
            }
            else
            {
                broadcast = string.Join(" ", arguments.Array, 2, arguments.Array.Length - 2);

                switch (arguments.Array[1].ToString())
                {
                    case "broadcast":
                    case "bc":
                        Map.Broadcast(ScpUtils.StaticInstance.Config.BroadcastDuration, broadcast);
                        response = "Sending broadcast to all the players!";
                        break;
                    case "hint":
                    case "h":
                        Map.ShowHint(broadcast, ScpUtils.StaticInstance.Config.BroadcastDuration);
                        response = "Sending hint to all the players!";
                        break;
                    default:
                        response = "Invalid argument, you should use broadcast/bc or hint/h.";
                        break;
                }
            }

            return true;
        }
    }
}