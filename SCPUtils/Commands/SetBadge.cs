using System;
using CommandSystem;
using Log = Exiled.API.Features.Log;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class SetBadge : ICommand
    {

        public string Command { get; } = "scputils_set_badge";

        public string[] Aliases { get; } = new[] { "setb", "issue_badge" };

        public string Description { get; } = "You can change everyone admin or only your name based on the permissions you have";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            string badge;
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.handlebadges") && !((CommandSender)sender).FullPermissions)
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
            var player = Exiled.API.Features.Player.Get(target);
            var databasePlayer = target.GetDatabasePlayer();

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
                var group = ServerStatic.GetPermissionsHandler()._groups[badge];

                if (group.KickPower > ((CommandSender)sender).KickPower && !((CommandSender)sender).FullPermissions)
                {
                    response = $"You need a higher administration level to use this command: The group you are trying to set has more kick power than yours. (Your kick power: {((CommandSender)sender).KickPower}, Required: {group.KickPower})";
                    return false;
                }

                if (player != null)
                {
                    if (string.IsNullOrEmpty(databasePlayer.PreviousBadge) && player.Group != null) databasePlayer.PreviousBadge = player.GroupName;
                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId)) ServerStatic.PermissionsHandler._members.Remove(player.UserId);
                    player.ReferenceHub.serverRoles.SetGroup(group, false, true, true);
                    ServerStatic.PermissionsHandler._members.Add(player.UserId, badge);
                }

                databasePlayer.BadgeName = badge;
                databasePlayer.BadgeExpire = DateTime.Now.AddMinutes(duration);


                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = $"Successfully set {group.BadgeText} badge! Duration: {duration} minute(s)!";


            }
            else response = "Arg3 must be integer!";

            return true;
        }
    }
}
