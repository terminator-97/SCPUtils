namespace SCPUtils.Commands.Console
{
    using CommandSystem;
    using System;
    using System.Text;

    public class StaffListCommand : ICommand
    {
        public string Command { get; } = "staff";
        public string[] Aliases { get; } = new[] { "s" };
        public string Description { get; } = "Show list of online staffer";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            StringBuilder staffList = new StringBuilder(ScpUtils.StaticInstance.commandTranslation.StaffOnline.Replace("%online%", CountStaffMembers().ToString()));
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                if (player.ReferenceHub.serverRoles.RemoteAdmin && string.IsNullOrEmpty(player.ReferenceHub.serverRoles.HiddenBadge))
                {
                    staffList.AppendLine();
                    staffList.Append(ScpUtils.StaticInstance.commandTranslation.StaffList.Replace("%id%", player.PlayerId.ToString()).Replace("%staff%", $"{player.Nickname}").Replace("%role%", $"<color={player.RoleColor}>{player.ReferenceHub.serverRoles.MyText}</color>"));

                    if (player.IsOverwatchEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.Overwatch);

                    //if (FpcNoclip.IsPermitted(player.ReferenceHub))
                    if (player.IsNoclipEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.Noclip);

                    if (player.IsGodModeEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.God);
                }
            }

            if (CountStaffMembers() == 0)
            {
                response = ScpUtils.StaticInstance.commandTranslation.NoStaff;
                return true;
            }

            response = $"{staffList}";
            return true;
        }

        private int CountStaffMembers()
        {
            int value = 0;
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                if (player.RemoteAdminAccess) value++;
            }
            return value;
        }
    }
}
