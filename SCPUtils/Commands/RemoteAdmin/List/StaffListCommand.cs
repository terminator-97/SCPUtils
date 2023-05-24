namespace SCPUtils.Commands.RemoteAdmin.List
{
    using CommandSystem;
    using PlayerRoles.FirstPersonControl;
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

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils list staff"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils list staff"]}");
                return false;
            }

            StringBuilder staffList = new StringBuilder(ScpUtils.StaticInstance.commandTranslation.StaffOnline.Replace("%online%", CountStaffMembers().ToString()));
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                if (player.IsNorthwoodStaff || player.IsGlobalModerator)
                {
                    staffList.AppendLine();
                    staffList.Append(ScpUtils.StaticInstance.commandTranslation.SlStaff.Replace("%staff%", $"{player.Nickname} ({player.UserId})").Replace("%role%", $"<color={player.RoleColor}>{player.RoleName}</color> ({player.ReferenceHub.serverRoles.GlobalBadgeType})").Replace("%class%", player.Role.ToString()));
                    
                    if (player.IsOverwatchEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.Overwatch);

                    if (player.IsNoclipEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.Noclip);

                    if (player.IsGodModeEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.God);
                }
                else if (player.ReferenceHub.serverRoles.RemoteAdmin)
                {
                    staffList.AppendLine();
                    staffList.Append(ScpUtils.StaticInstance.commandTranslation.StaffList.Replace("%id%", player.PlayerId.ToString()).Replace("%staff%", $"{player.Nickname} ({player.UserId})").Replace("%role%", $"<color={player.RoleColor}>{player.ReferenceHub.serverRoles.MyText}</color>").Replace("%class%", player.Role.ToString()));

                    if (player.IsOverwatchEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.Overwatch);
                    
                    //if (FpcNoclip.IsPermitted(player.ReferenceHub))
                    if (player.IsNoclipEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.Noclip);
                    
                    if (player.IsGodModeEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.God);
                    
                    if (player.IsBypassEnabled)
                        staffList.Append(ScpUtils.StaticInstance.commandTranslation.Bypass);
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
                if (player.ReferenceHub.serverRoles.RaEverywhere || player.ReferenceHub.serverRoles.Staff || player.RemoteAdminAccess)
                {
                    value++;
                }
            }
            return value;
        }
    }
}
