using CommandSystem;
using System;
using PluginAPI.Core;

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
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.broadcast"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"<color=yellow>Usage: {Command} <player> <hint(h)/broadcast(bc)> <id> <duration (optional, if empty will be used the default one set for this broadcast)></color>";
                return false;
            }
            else
            {
                var databaseBroadcast = GetBroadcast.FindBroadcast(arguments.Array[3]);

                if (databaseBroadcast == null)
                {
                    response = "Invalid broadcast ID!";
                    return false;
                }

                PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(arguments.Array[1].ToString());
                if (player == null)
                {
                    response = "Invalid player!";
                    return false;
                }

                int duration = databaseBroadcast.Seconds;
                if (arguments.Count == 4)
                {
                    if (int.TryParse(arguments.Array[4].ToString(), out duration)) { }
                    else
                    {
                        response = "Broadcast duration must be an integer";
                        return false;
                    }
                }

                switch (arguments.Array[2].ToString())
                {
                    case "broadcast":
                    case "bc":
                        player.SendBroadcast(databaseBroadcast.Text, (ushort)duration, global::Broadcast.BroadcastFlags.Normal, false);
                        response = "Success!";
                        break;
                    case "hint":
                    case "h":
                        player.ReceiveHint(databaseBroadcast.Text, duration);
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
