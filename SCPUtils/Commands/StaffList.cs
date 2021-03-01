using System;
using System.Text;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class StaffList : ICommand
    {
        public string Command { get; } = "scputils_staff_list";

        public string[] Aliases { get; } = new[] { "sl", "stafflist" };

        public string Description { get; } = "Show staff list";

        public static int Count { get; private set; }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scputils.stafflist"))
            {
                response = "You need a higher administration level to use this command!";
                return false;
            }
            StringBuilder message = new StringBuilder($"Online Staffers({CountStaffMembers()}");           
            
            foreach (var player in Exiled.API.Features.Player.List)
            {
                if (player.ReferenceHub.serverRoles.RaEverywhere || player.ReferenceHub.serverRoles.Staff)
                {
                    message.AppendLine();
                    message.Append($"(SCP:SL Staff) {player.Nickname} ({player.UserId}) [{player.GlobalBadge}] [{player.Role}]");
                    if (player.IsOverwatchEnabled) message.Append(" [OVERWATCH]");
                    if (player.NoClipEnabled) message.Append(" [NOCLIP]");
                    if (player.IsGodModeEnabled) message.Append(" [GODMODE]");
                }
                else if (player.ReferenceHub.serverRoles.RemoteAdmin)
                {
                    message.AppendLine();
                    message.Append($"{player.Nickname} ({player.UserId}) [{player.Group.BadgeText}] [{player.Role}]");
                    if (player.IsOverwatchEnabled) message.Append(" [OVERWATCH]");
                    if (player.NoClipEnabled) message.Append(" [NOCLIP]");
                    if (player.IsGodModeEnabled) message.Append(" [GODMODE]");
                }
            }
            if (CountStaffMembers()==0)
            {
                response = "No staff online!";
                return true;
            }
            response = $"{message}";
            return true;
        }

        private static int CountStaffMembers()
        {        
            foreach (var player in Exiled.API.Features.Player.List)
            {
                if (player.ReferenceHub.serverRoles.RaEverywhere || player.ReferenceHub.serverRoles.Staff || player.RemoteAdminAccess) Count++;
            }
            return Count;
        }
    }
}