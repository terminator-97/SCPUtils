using CommandSystem;
using Exiled.Permissions.Extensions;
using SCPUtils.Events;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class SetBadge : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.SetbadgeCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.SetbadgeAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.SetbadgeDescription;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }
            string target;
            string badge;
            if (!sender.CheckPermission("scputils.handlebadges"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} {ScpUtils.StaticInstance.Translation.ArgUsergroup} {ScpUtils.StaticInstance.Translation.ArgTimespan}";
                return false;
            }

            target = arguments.Array[1].ToString();
            badge = arguments.Array[2].ToString();
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(target);
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }

            else if (!ServerStatic.GetPermissionsHandler().GetAllGroups().ContainsKey(badge))
            {
                response = ScpUtils.StaticInstance.Translation.InvalidUsergroup;
                return false;
            }

            else if (TimeSpan.TryParse(arguments.Array[3], out TimeSpan duration))
            {
                UserGroup group = ServerStatic.GetPermissionsHandler()._groups[badge];

                if (group.KickPower > ((CommandSender)sender).KickPower && !((CommandSender)sender).FullPermissions)
                {
                    response = $"{ScpUtils.StaticInstance.Translation.NoPermissions}: {ScpUtils.StaticInstance.Translation.SetbadgeKickpower} {((CommandSender)sender).KickPower}, {ScpUtils.StaticInstance.Translation.SetbadgeRequired} {group.KickPower})";
                    return false;
                }

                if (player != null)
                {
                    BadgeSetEvent args = new BadgeSetEvent();
                    args.Player = player;
                    args.NewBadgeName = badge;


                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                    {
                        ServerStatic.PermissionsHandler._members.Remove(player.UserId);
                    }

                    player.ReferenceHub.serverRoles.SetGroup(group, false, true);
                    ServerStatic.PermissionsHandler._members.Add(player.UserId, badge);

                    ScpUtils.StaticInstance.Events.OnBadgeSet(args);
                }

                databasePlayer.BadgeName = badge;
                databasePlayer.BadgeExpire = DateTime.Now.Add(duration);


                databasePlayer.SaveData();
                var message = ScpUtils.StaticInstance.Translation.SetbadgeSuccess.Replace("%group%", group.BadgeText).Replace("%duration%", duration.ToString());
                response = message;


            }
            else
            {                
                response = ScpUtils.StaticInstance.Translation.InvalidTimespan;
            }

            return true;
        }
    }
}
