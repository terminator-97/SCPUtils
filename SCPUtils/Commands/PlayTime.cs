using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class PlayTime : ICommand
    {

        public string Command { get; } = "scputils_play_time";

        public string[] Aliases { get; } = new[] { "pt", "su_playtime", "su_pt", "scpu_playtime", "scpu_pt" };

        public string Description { get; } = "You can see detailed informations about playtime";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            int range;
            int playtime;
            if (!sender.CheckPermission("scputils.ownplaytime") && !sender.CheckPermission("scputils.playtime") && !((CommandSender)sender).FullPermissions)
            {
                response = "<color=red>You need a higher administration level to use this command!</color>";
                return false;
            }

            if (!sender.CheckPermission("scputils.playtime"))
            {
                target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).UserId;

                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <days range> </color>";
                    return false;
                }

                else
                {
                    if (!int.TryParse(arguments.Array[1], out range))
                    {
                        response = "<color=red>Number is not an integer</color>";
                        return false;
                    }

                }

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
                else
                {
                    target = arguments.Array[1].ToString();
                }

                if (!int.TryParse(arguments.Array[2], out range))
                {
                    response = "<color=red>Number is not an integer</color>";
                    return false;
                }

            }
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }


            if (range <= 0)
            {
                response = "<color=red>You have to specify a number higher than 0!</color>";
                return false;
            }

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }
            StringBuilder message = new StringBuilder($"[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]");
            message.AppendLine();
            message.Append($"Total Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString() } ] - Total Overwatch time: [ { new TimeSpan(0, 0, databasePlayer.OwPlayTimeRecords.Values.Sum()).ToString() } ]");



            playtime = 0;
            var owpt = string.Empty;
            for (int i = 0; i <= range; i++)
            {
                databasePlayer.PlayTimeRecords.Count();
                message.AppendLine();
                DateTime.TryParse((DateTime.Now.Date.AddDays(-i)).ToString(), out DateTime date);
                if (databasePlayer.PlayTimeRecords.ContainsKey(date.Date.ToShortDateString()))
                {
                    if (databasePlayer.OwPlayTimeRecords.ContainsKey(date.Date.ToShortDateString()))
                    {
                        owpt = new TimeSpan(0, 0, databasePlayer.OwPlayTimeRecords[date.Date.ToShortDateString()]).ToString();
                    }
                    else owpt = "00:00:00";
                    message.Append($"{date.Date.ToShortDateString()} Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()]).ToString() } ] - Overwatch time: [ { owpt } ]");
                   
                    playtime += databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()];
                }
                else
                {
                    message.Append($"{date.Date.ToShortDateString()} Playtime: [ No activity ]");
                }
            }
            message.AppendLine($"\nSpecified Period PlayTime: [ { new TimeSpan(0, 0, playtime).ToString() } ]");
            response = $"{message}";

            return true;
        }
    }
}
