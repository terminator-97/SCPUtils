using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PlayerUnrestrict : ICommand
    {
        public string Command { get; } = "scputils_player_unrestrict";

        public string[] Aliases { get; } = new[] { "unrestrict", "unsusp" };

        public string Description { get; } = "Removes a restriction from a player!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            if (!sender.CheckPermission("scputils.moderatecommands"))
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

            else if (!databasePlayer.IsRestricted())
            {
                response = "Player is not suspended!";
                return false;
            }

            databasePlayer.Restricted.Remove(databasePlayer.Restricted.Keys.Last());
            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
            response = "Player unsuspended!";
            return true;

        }
    }
}
