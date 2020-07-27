using System;
using CommandSystem;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class SeetBadge : ICommand
    {

        public string Command { get; } = "scputils_set_badge";

        public string[] Aliases { get; } = new string[] { "setb", "issue_badge" };

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
                databasePlayer.BadgeName = badge;
                databasePlayer.BadgeExpire = DateTime.Now.AddMinutes(duration);
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                if (player != null) player.ReferenceHub.serverRoles.Group = ServerStatic.GetPermissionsHandler()._groups[badge];
                response = "Badge set!";

            }
            else response = "Arg3 must be integer!";

            return true;
        }
    }
}
