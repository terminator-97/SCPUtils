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

        public string Command { get; } = "scputils_set_badge";

        public string[] Aliases { get; } = new[] { "setb", "issue_badge", "su_setb", "su_setbadge", "scpu_setb", "scpu_setbadge" };

        public string Description { get; } = "With this command you can set temporary badge, by their name for example: scpu_setb 2 owner 60";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            string badge;
            if (!sender.CheckPermission("scputils.handlebadges"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"Usage: {Command} <player name / id> <Badge Name> <Minutes>";
                return false;
            }

            target = arguments.Array[1].ToString();
            badge = arguments.Array[2].ToString();
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(target);
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            else if (!ServerStatic.GetPermissionsHandler().GetAllGroups().ContainsKey(badge))
            {
                response = "Invalid role name!";
                return false;
            }

            else if (int.TryParse(arguments.Array[3], out int duration))
            {
                UserGroup group = ServerStatic.GetPermissionsHandler()._groups[badge];

                if (group.KickPower > ((CommandSender)sender).KickPower && !((CommandSender)sender).FullPermissions)
                {
                    response = $"You need a higher administration level to use this command: The group you are trying to set has more kick power than yours. (Your kick power: {((CommandSender)sender).KickPower}, Required: {group.KickPower})";
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

                    player.ReferenceHub.serverRoles.SetGroup(group, false, true, true);
                    ServerStatic.PermissionsHandler._members.Add(player.UserId, badge);

                    ScpUtils.StaticInstance.Events.OnBadgeSet(args);
                }

                databasePlayer.BadgeName = badge;
                databasePlayer.BadgeExpire = DateTime.Now.AddMinutes(duration);


                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = $"Successfully set {group.BadgeText} badge! Duration: {duration} minute(s)!";


            }
            else
            {
                response = "Arg3 must be integer!";
            }

            return true;
        }
    }
}
