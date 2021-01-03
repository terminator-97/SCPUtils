using System;
using System.Linq;
using System.Text;
using CommandSystem;
using Log = Exiled.API.Features.Log;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    class PlayTime : ICommand
    {

        public string Command { get; } = "scputils_play_time";

        public string[] Aliases { get; } = new[] { "pt" };

        public string Description { get; } = "You can see detailed informations about playtime";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            int range;
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.playerinfo") && !((CommandSender)sender).FullPermissions)
            {
                target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).ToString().Split(new string[] { " " }, StringSplitOptions.None)[2];

                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <days range> </color>";
                    return false;
                }

                else int.TryParse(arguments.Array[1], out range);

                if (range > 120)
                {
                    response = "<color=red>You can specify a range of max 120 days!</color>";
                    return false;
                }
            }
            else
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>Usage: {Command} <player name/id> <days range> </color>";
                    return false;
                }
                else target = arguments.Array[1].ToString();

                int.TryParse(arguments.Array[2], out range);

            }
            var databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }


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
            StringBuilder message = new StringBuilder($"[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})");
            message.AppendLine();
            message.Append($"Total Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString() } ]");


            for (int i = 0; i <= range; i++)
            {
                message.AppendLine();
                DateTime.TryParse((DateTime.Now.Date.AddDays(-i)).ToString(), out DateTime date);
                if (databasePlayer.PlayTimeRecords.ContainsKey(date.Date.ToShortDateString())) message.Append($"{date.Date.ToShortDateString()} Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()]).ToString() } ]");
                else message.Append($"{date.Date.ToShortDateString()} Playtime: [ No activity ]");
            }

            response = $"{message}";

            return true;
        }
    }
}
