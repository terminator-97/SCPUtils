using System;
using CommandSystem;


namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class RevokeBadge : ICommand
    {
        public string Command { get; } = "scputils_revoke_badge";

        public string[] Aliases { get; } = new string[] { "rb" };

        public string Description { get; } = "Removes a temporarily badge that has been given to a player!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.handlebadges") && !((CommandSender)sender).FullPermissions)
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }

            else if (arguments.Count < 1)
            {
                response = $"Usage: {Command} <player name/id>";
                return false;
            }

            else target = arguments.Array[1].ToString();

            var player = Exiled.API.Features.Player.Get(target);
            var databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            databasePlayer.BadgeName = "";
            databasePlayer.BadgeExpire = DateTime.MinValue;
            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
            if (player != null) player.BadgeHidden = false;
            response = "Badge revoked!";
            return true;

        }
    }
}
