using CommandSystem;
using System;
using System.Text;
using PluginAPI.Core;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class ScpList : ICommand
    {
        public string Command { get; } = "scputils_scp_list";

        public string[] Aliases { get; } = new[] { "scpl", "scplist", "su_scpl", "su_scplist", "scpu_scplist" };

        public string Description { get; } = "Show scp list";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (((CommandSender)sender).Nickname.Equals("SERVER CONSOLE"))
            {
                response = "This command cannot be executed from console!";
                return false;
            }


            PluginAPI.Core.Player csender = PluginAPI.Core.Player.Get(((CommandSender)sender).SenderId);
            if (!csender.IsSCP || !ScpUtils.StaticInstance.configs.AllowSCPSwap)
            {
                response = "You are not SCP or Swap module is disabled by the server admin!";
                return false;
            }

            StringBuilder message = new StringBuilder($"Online Players ({PluginAPI.Core.Player.Count})").AppendLine();
            message.Append($"[SCPs LIST]");
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                if (player.IsSCP)
                {
                    message.AppendLine();
                    message.Append($"({player.PlayerId}) {player.Nickname} {player.Role}");
                }
            }
            response = message.ToString();
            return true;
        }

    }
}

