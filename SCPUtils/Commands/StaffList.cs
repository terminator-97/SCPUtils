using CommandSystem;
using Exiled.API.Features.Roles;
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

        public string[] Aliases { get; } = new[] { "sl", "stafflist", "su_sl", "su_staffl", "su_staff_l", "su_slist", "scpu_sl", "scpu_staffl", "scpu_staff_l", "scpu_slist" };

        public string Description { get; } = "Show list of online staffer.";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.stafflist"))
            {
                response = "You need a higher administration level to use this command!";
                return false;
            }
            StringBuilder message = new StringBuilder($"Online Staffers ({CountStaffMembers()})");

            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List)
            {
                if (player.IsNorthwoodStaff || player.IsGlobalModerator)
                {
                    message.AppendLine();
                    message.Append($"(SCP:SL Staff) {player.Nickname} ({player.UserId}) [{player.GlobalBadge}] [{player.Role.Type}]");
                    if (player.IsOverwatchEnabled)
                    {
                        message.Append(" [OVERWATCH]");
                    }

                    if (player.Role.Is(out FpcRole role))
                    {
                        if (role.IsNoclipEnabled)
                        {
                            message.Append(" [NOCLIP]");
                        }
                    }
                    else
                    {
                        message.Append(" [NOT-FPCROLE]");
                    }

                    if (player.IsGodModeEnabled)
                    {
                        message.Append(" [GODMODE]");
                    }
                }
                else if (player.ReferenceHub.serverRoles.RemoteAdmin)
                {
                    message.AppendLine();
                    message.Append($"{player.Nickname} ({player.UserId}) [{player.Group.BadgeText}] [{player.Role.Type}]");
                    if (player.IsOverwatchEnabled)
                    {
                        message.Append(" [OVERWATCH]");
                    }

                    if (player.Role.Is(out Exiled.API.Features.Roles.FpcRole role))
                    {
                        if (role.IsNoclipEnabled)
                        {
                            message.Append(" [NOCLIP]");
                        }
                    }
                    else
                    {
                        message.Append(" [NOT-FPCROLE]");
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
                if (player.IsGlobalModerator || player.IsNorthwoodStaff || player.RemoteAdminAccess)
                {
                    value++;
                }
            }
            return value;
        }
    }
}
