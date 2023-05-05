using CommandSystem;
using System;
using PluginAPI.Core;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class Broadcast : ICommand
    {
        public string Command { get; } = "scputils_broadcast";

        public string[] Aliases { get; } = new[] { "sbc", "su_bc", "scpu_bc" };

        public string Description { get; } = "Allows to send custom broadcastes";

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

            else if (arguments.Count < 2)
            {
                response = $"<color=yellow>Usage: {Command} <hint(h)/broadcast(bc)> <id> <duration (optional, if empty will be used the default one set for this broadcast)></color>";
                return false;
            }
            else
            {
                var databaseBroadcast = GetBroadcast.FindBroadcast(arguments.Array[2]);

                if (databaseBroadcast == null)
                {
                    response = "Invalid broadcast ID!";
                    return false;
                }
                int duration = databaseBroadcast.Seconds;
                if (arguments.Count == 3)
                {
                    if (int.TryParse(arguments.Array[3].ToString(), out duration)) { }
                    else
                    {
                        response = "Broadcast duration must be an integer";
                        return false;
                    }
                }

                switch (arguments.Array[1].ToString())
                {
                    case "broadcast":
                    case "bc":                     
                        PluginAPI.Core.Server.SendBroadcast(databaseBroadcast.Text, (ushort)duration);
                        response = "Sending broadcast to all the players!";
                        break;
                    case "hint":
                    case "h":                       
                        foreach(var player in PluginAPI.Core.Player.GetPlayers())
                        {
                            player.ReceiveHint(databaseBroadcast.Text, duration);
;                        }
                       
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
