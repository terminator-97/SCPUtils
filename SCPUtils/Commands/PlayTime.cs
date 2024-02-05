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

        public string Command { get; } = ScpUtils.StaticInstance.Translation.PlaytimeCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.PlaytimeAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.PlaytimeDescription;

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
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }

            if (!sender.CheckPermission("scputils.playtime"))
            {
                target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).UserId;

                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgDays}</color>";
                    return false;
                }

                else
                {
                    if (!int.TryParse(arguments.Array[1], out range))
                    {
                        response = ScpUtils.StaticInstance.Translation.InvalidArgInt;
                        return false;
                    }

                }

                if (range > 120)
                {
                    response = ScpUtils.StaticInstance.Translation.PlaytimeMaxRange;
                    return false;
                }
            }
            else
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} {ScpUtils.StaticInstance.Translation.ArgDays}</color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                }

                if (!int.TryParse(arguments.Array[2], out range))
                {
                    response = ScpUtils.StaticInstance.Translation.InvalidArgInt;
                    return false;
                }

            }
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }


            if (range <= 0)
            {
                response = ScpUtils.StaticInstance.Translation.PlaytimeZero;
                return false;
            }

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }
            StringBuilder message = new StringBuilder($"[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]");
            message.AppendLine();
            message.Append($"{ScpUtils.StaticInstance.Translation.PlaytimeTotal} [ {new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString()} ] - {ScpUtils.StaticInstance.Translation.PlaytimeOverwatch} [ {new TimeSpan(0, 0, databasePlayer.OwPlayTimeRecords.Values.Sum()).ToString()} ]");



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
                    message.Append($"{date.Date.ToShortDateString()} {ScpUtils.StaticInstance.Translation.PlaytimePlaytime} [ {new TimeSpan(0, 0, databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()]).ToString()} ] - {ScpUtils.StaticInstance.Translation.PlaytimeOverwatch} [ {owpt} ]");

                    playtime += databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()];
                }
                else
                {
                    message.Append($"{date.Date.ToShortDateString()} {ScpUtils.StaticInstance.Translation.PlaytimeNoactivity}");
                }
            }
            message.AppendLine($"\n{ScpUtils.StaticInstance.Translation.PlaytimeSpecified} [ {new TimeSpan(0, 0, playtime).ToString()} ]");
            response = $"{message}";

            return true;
        }
    }
}
