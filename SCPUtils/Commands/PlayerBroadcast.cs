using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class PlayerBroadcast : ICommand
    {
        public string Command { get; } = "scputils_player_broadcast";

        public string[] Aliases { get; } = new[] { "spbc", "su_pbc", "su_player_bc", "su_p_bc", "su_p_broadcast", "scpu_pbc", "scpu_player_bc", "scpu_p_bc", "scpu_p_broadcast" };

        public string Description { get; } = "Allows to send custom broadcaste";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string broadcast;
            if (!sender.CheckPermission("scputils.broadcast"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"<color=yellow>Usage: {Command} <player> <hint/broadcast> <text></color>";
                return false;
            }
            else
            {
                broadcast = string.Join(" ", arguments.Array, 3, arguments.Array.Length - 3);
                Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(arguments.Array[1].ToString());
                if (player == null)
                {
                    response = "Invalid player!";
                    return false;
                }
                switch (arguments.Array[2].ToString())
                {
                    case "broadcast":
                    case "bc":
                        player.Broadcast(ScpUtils.StaticInstance.Config.BroadcastDuration, broadcast);
                        response = "Success!";
                        break;
                    case "hint":
                    case "h":
                        player.ShowHint(broadcast, ScpUtils.StaticInstance.Config.BroadcastDuration);
                        response = "Success!";
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
