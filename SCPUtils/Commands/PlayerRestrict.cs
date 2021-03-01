using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class PlayerRestrict : ICommand
    {

        public string Command { get; } = "scputils_player_restrict";

        public string[] Aliases { get; } = new[] { "restrict", "susp" };

        public string Description { get; } = "This command restrict a player from using change name and set color commands!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            string reason;
            if (!sender.CheckPermission("scputils.moderatecommands"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"Usage: {Command} <player name / id> <Minutes, 0 = permanent> <Reason>";
                return false;
            }
            target = arguments.Array[1].ToString();

            var player = Exiled.API.Features.Player.Get(target);
            var databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            else if (databasePlayer.IsRestricted())
            {
                response = "Player is already suspended!";
                return false;
            }


            else if (int.TryParse(arguments.Array[2], out int minutes))
            {
                reason = string.Join(" ", arguments.Array, 3, arguments.Array.Length - 3);
                databasePlayer.Restricted.Add(DateTime.Now.AddMinutes(minutes), reason);
                if (minutes == 0) databasePlayer.Restricted.Add(DateTime.Now.AddDays(20000), reason);
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = $"Player suspended!";

            }
            else response = "Duration must be integer!";

            return true;
        }
    }
}
