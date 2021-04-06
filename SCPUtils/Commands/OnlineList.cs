using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    class OnlineList : ICommand
    {
        public string Command { get; } = "scputils_online_list";

        public string[] Aliases { get; } = new[] { "ol", "onlinelist" };

        public string Description { get; } = "Show online player list";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scputils.onlinelist.basic"))
            {
                response = "You need a higher administration level to use this command!";
                return false;
            }
            StringBuilder message = new StringBuilder($"Online Players ({Exiled.API.Features.Player.Dictionary.Count})");

            foreach (var player in Exiled.API.Features.Player.List)
            {
                message.AppendLine();
                message.Append($"({player.Id}) {player.Nickname}");
                if (sender.CheckPermission("scputils.onlinelist.userid")) message.Append($" ({player.UserId})");
                if (sender.CheckPermission("scputils.onlinelist.badge") && player.Group?.BadgeText != null) message.Append($" [{player.Group.BadgeText}]");
                if (sender.CheckPermission("scputils.onlinelist.role")) message.Append($" [{player.Role}]");
                if (sender.CheckPermission("scputils.onlinelist.health")) message.Append($" [HP {player.Health} / {player.MaxHealth}]");
                if (sender.CheckPermission("scputils.onlinelist.flags"))
                {
                    if (player.IsOverwatchEnabled) message.Append(" [OVERWATCH]");
                    if (player.NoClipEnabled) message.Append(" [NOCLIP]");
                    if (player.IsGodModeEnabled) message.Append(" [GODMODE]");
                    if (player.IsStaffBypassEnabled) message.Append(" [BYPASS MODE]");
                    if (player.IsIntercomMuted) message.Append(" [INTERCOM MUTED]");
                    if (player.IsMuted) message.Append(" [SERVER MUTED]");
                    if (player.DoNotTrack) message.Append(" [DO NOT TRACK]");
                    if (player.RemoteAdminAccess) message.Append(" [RA]");
                }
            }
            if (Exiled.API.Features.Player.Dictionary.Count == 0)
            {
                response = "No players online!";
                return true;
            }
            response = message.ToString();
            return true;
        }

    }
}