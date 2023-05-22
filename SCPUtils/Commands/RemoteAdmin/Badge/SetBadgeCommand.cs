namespace SCPUtils.Commands.RemoteAdmin.Badge
{
    using CommandSystem;
    using SCPUtils.Events;
    using System;

    public class SetBadgeCommand : ICommand
    {
        public string Command { get; } = "set";
        public string[] Aliases { get; } = new[]
        {
            "s"
        };
        public string Description { get; } = "With this command you can set temporary badge, by their name for example: scpu_setb 2 owner 60";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }
            string target;
            string badge;
            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils badge set"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils badge set"]}");
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player} {ScpUtils.StaticInstance.commandTranslation.Badge} {ScpUtils.StaticInstance.commandTranslation.Time}");
                return false;
            }

            target = arguments.Array[3].ToString();
            badge = arguments.Array[4].ToString();
            PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(target);
            SCPUtils.Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                return false;
            }

            else if (!ServerStatic.GetPermissionsHandler().GetAllGroups().ContainsKey(badge))
            {
                response = ScpUtils.StaticInstance.commandTranslation.BadgeNameError;
                return false;
            }

            else if (TimeSpan.TryParse(arguments.Array[5], out var duration))
            {
                UserGroup group = ServerStatic.GetPermissionsHandler()._groups[badge];

                if (group.KickPower > ((CommandSender)sender).KickPower && !((CommandSender)sender).FullPermissions)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.BadgeKickPower.Replace("%kickPower%", ((CommandSender)sender).KickPower.ToString()).Replace("groupKickPower", group.KickPower.ToString());
                    return false;
                }

                if (player != null)
                {
                    BadgeSetEvent args = new BadgeSetEvent
                    {
                        Player = player,
                        NewBadgeName = badge
                    };

                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                    {
                        ServerStatic.PermissionsHandler._members.Remove(player.UserId);
                    }

                    player.ReferenceHub.serverRoles.SetGroup(group, false, true, true);
                    ServerStatic.PermissionsHandler._members.Add(player.UserId, badge);

                    ScpUtils.StaticInstance.Events.OnBadgeSet(args);
                }

                databasePlayer.BadgeName = badge;
                databasePlayer.BadgeExpire = DateTime.Now.Add(duration);
                databasePlayer.SaveData();

                response = ScpUtils.StaticInstance.commandTranslation.BadgeSet.Replace("%player%", $"{databasePlayer.Name}@{databasePlayer.Authentication}").Replace("%badgeName%", $"{group.BadgeText}").Replace("%badgeColor%", $"{group.BadgeColor}").Replace("%time%", $"{duration}");
            }
            else
            {
                response = ScpUtils.StaticInstance.commandTranslation.BadgeDurationInvalid;
            }

            return true;
        }
    }
}