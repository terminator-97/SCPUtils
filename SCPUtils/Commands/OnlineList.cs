using CommandSystem;
using System;
using System.Text;
using PluginAPI.Core;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class OnlineList : ICommand
    {
        public string Command { get; } = "scputils_online_list";

        public string[] Aliases { get; } = new[] { "ol", "onlinelist", "su_ol", "su_onlinelist", "scpu_ol", "scpu_onlinelist" };

        public string Description { get; } = "Show online player list";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.onlinelist.basic"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }
            StringBuilder message = new StringBuilder($"Online Players ({PluginAPI.Core.Player.Count})");

            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                message.AppendLine();
                message.Append($"({player.PlayerId}) {player.Nickname}");

                if (sender.CheckPermission("scputils.onlinelist.userid"))
                {
                    message.Append($" ({player.UserId})");
                }

                if (sender.CheckPermission("scputils.onlinelist.badge") && player.ReferenceHub.serverRoles.Group.BadgeText != null)
                {
                    message.Append($" [{player.ReferenceHub.serverRoles.Group.BadgeText}]");
                }

                if (sender.CheckPermission("scputils.onlinelist.role"))
                {
                    message.Append($" [{player.Role}]");
                }

                if (sender.CheckPermission("scputils.onlinelist.health"))
                {
                    message.Append($" [HP {player.Health} / {player.MaxHealth}]");
                    if (player.ArtificialHealth >= 1) message.Append($" [AHP {player.ArtificialHealth}]");
                }

                if (sender.CheckPermission("scputils.onlinelist.flags"))
                {
                    if (player.IsOverwatchEnabled)
                    {
                        message.Append(" [OVERWATCH]");
                    }                            
                        
                    if (player.IsNoclipEnabled)
                    {
                        message.Append(" [NOCLIP]");
                    }                  

                    if (player.IsGodModeEnabled)
                    {
                        message.Append(" [GODMODE]");
                    }

                    if (player.IsBypassEnabled)
                    {
                        message.Append(" [BYPASS MODE]");
                    }

                    if (player.IsIntercomMuted)
                    {
                        message.Append(" [INTERCOM MUTED]");
                    }

                    if (player.IsMuted)
                    {
                        message.Append(" [SERVER MUTED]");
                    }

                    if (player.DoNotTrack)
                    {
                        message.Append(" [DO NOT TRACK]");
                    }

                    if (player.RemoteAdminAccess)
                    {
                        message.Append(" [RA]");
                    }
                }
            }
            if (PluginAPI.Core.Player.Count == 0)
            {
                response = "No players online!";
                return true;
            }
            response = message.ToString();
            return true;
        }

    }
}
