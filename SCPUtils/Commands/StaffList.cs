using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class StaffList : ICommand
    {
        public string Command { get; } = "scputils_staff_list";

        public string[] Aliases { get; } = new[] { "sl", "stafflist" };

        public string Description { get; } = "Show staff list";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scputils.stafflist"))
            {
                response = "You need a higher administration level to use this command!";
                return false;
            }
            StringBuilder message = new StringBuilder($"Online Staffers ({CountStaffMembers()})");

            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List)
            {
                if (player.ReferenceHub.serverRoles.RaEverywhere || player.ReferenceHub.serverRoles.Staff)
                {
                    message.AppendLine();
                    message.Append($"(SCP:SL Staff) {player.Nickname} ({player.UserId}) [{player.GlobalBadge}] [{player.Role}]");
                    if (player.IsOverwatchEnabled)
                    {
                        message.Append(" [OVERWATCH]");
                    }

                    if (player.NoClipEnabled)
                    {
                        message.Append(" [NOCLIP]");
                    }

                    if (player.IsGodModeEnabled)
                    {
                        message.Append(" [GODMODE]");
                    }
                }
                else if (player.ReferenceHub.serverRoles.RemoteAdmin)
                {
                    message.AppendLine();
                    message.Append($"{player.Nickname} ({player.UserId}) [{player.Group.BadgeText}] [{player.Role}]");
                    if (player.IsOverwatchEnabled)
                    {
                        message.Append(" [OVERWATCH]");
                    }

                    if (player.NoClipEnabled)
                    {
                        message.Append(" [NOCLIP]");
                    }

                    if (player.IsGodModeEnabled)
                    {
                        message.Append(" [GODMODE]");
                    }

                    if (player.IsStaffBypassEnabled)
                    {
                        message.Append(" [BYPASS MODE]");
                    }
                }
            }
            if (CountStaffMembers() == 0)
            {
                response = "No staff online!";
                return true;
            }
            response = $"{message}";
            return true;
        }

        private static int CountStaffMembers()
        {
            int value = 0;
            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List)
            {
                if (player.ReferenceHub.serverRoles.RaEverywhere || player.ReferenceHub.serverRoles.Staff || player.RemoteAdminAccess)
                {
                    value++;
                }
            }
            return value;
        }
    }
}