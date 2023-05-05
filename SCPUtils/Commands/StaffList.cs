using CommandSystem;
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
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.stafflist"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }
            StringBuilder message = new StringBuilder($"Online Staffers ({CountStaffMembers()})");

            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                if (player.IsNorthwoodStaff || player.IsGlobalModerator)
                {
                    message.AppendLine();
                    message.Append($"(SCP:SL Staff) {player.Nickname} ({player.UserId}) [{player.ReferenceHub.serverRoles.GlobalBadge}] [{player.Role}]");
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
                }
                else if (player.ReferenceHub.serverRoles.RemoteAdmin)
                {
                    message.AppendLine();
                    message.Append($"{player.Nickname} ({player.UserId}) [{player.ReferenceHub.serverRoles.Group.BadgeText}] [{player.Role}]");
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
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
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
