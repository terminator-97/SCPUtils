using System;
using System.Linq;
using CommandSystem;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class PlayTime : ICommand
    {

        public string Command { get; } = "scputils_play_time";

        public string[] Aliases { get; } = new[] { "pt" };

        public string Description { get; } = "You can see detailed informations about playtime";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.playtime") && !((CommandSender)sender).FullPermissions)
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            else
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>Usage: {Command} <player name/id> <days range> </color>";
                    return false;
                }
                else target = arguments.Array[1].ToString();
            }
            var databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }


            int.TryParse(arguments.Array[2], out int range);

            if (range < 0)
            {
                response = "<color=red>You have to specify a positive number!</color>";
                return false;
            }

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }
            string message = $"\n[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n" +
  $"Total Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString() } ]\n";


            for (int i = 0; i <= range; i++)
            {
                DateTime.TryParse((DateTime.Now.Date.AddDays(-i)).ToString(), out DateTime date);
                if (databasePlayer.PlayTimeRecords.ContainsKey(date.Date.ToShortDateString())) message += $"{date.Date.ToShortDateString()} Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()]).ToString() } ]\n";
                else message += $"{date.Date.ToShortDateString()} Playtime: [ No activity ]\n";
            }

            response = $"{message}";

            return true;
        }
    }
}
