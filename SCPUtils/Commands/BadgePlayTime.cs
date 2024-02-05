using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class BadgePlayTime : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.BadgeplaytimeCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.BadgeplaytimeAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.BadgeplaytimeDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string badge;
            int range;
            int count = 0;
            int playtime;
            int completedays;
            // TimeSpan playtime;

            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }


            if (!sender.CheckPermission("scputils.playtime") && !((CommandSender)sender).FullPermissions)
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            if (arguments.Count < 2)
            {
                response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgUsergroup} {ScpUtils.StaticInstance.Translation.ArgDays} </color>";
                return false;
            }
            else
            {
                badge = arguments.Array[1].ToString();
            }

            int.TryParse(arguments.Array[2], out range);

            if (!ServerStatic.GetPermissionsHandler().GetAllGroups().ContainsKey(badge))
            {
                response = ScpUtils.StaticInstance.Translation.InvalidUsergroup;
                return false;
            }

            StringBuilder message = new StringBuilder($"{ScpUtils.StaticInstance.Translation.BadgeplaytimeChecking} {badge}").AppendLine();
         
                
            foreach (var player in ServerStatic.RolesConfig.GetStringDictionary("Members"))
            {
                playtime = 0;
                completedays = 0;
                int days = range + 1;
                var owpt = 0;
                var owdays = 0;                
                if (player.Value.ToString() == badge)
                {
                    var databasePlayer = player.Key.GetDatabasePlayer();
                    if (databasePlayer == null)
                    {
                        response = $"{ScpUtils.StaticInstance.Translation.BadgeplaytimeNullplayer} {player.Key}";
                        return false;
                    }

                    for (int i = 0; i <= range; i++)
                    {
                        databasePlayer.PlayTimeRecords.Count();
                        DateTime.TryParse((DateTime.Now.Date.AddDays(-i)).ToString(), out DateTime date);
                        if (databasePlayer.PlayTimeRecords.ContainsKey(date.Date.ToShortDateString()))
                        {

                            if (databasePlayer.OwPlayTimeRecords.ContainsKey(date.Date.ToShortDateString()))
                            {
                                owpt += databasePlayer.OwPlayTimeRecords[date.Date.ToShortDateString()];
                                owdays++;
                            }

                            playtime += databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()];
                            if (databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()] >= ScpUtils.StaticInstance.Config.BptMinSeconds) completedays++;
                        }
                    }
                    if (playtime == 0)
                    {

                        message.AppendLine($"[{databasePlayer.Name} - {databasePlayer.Id}@{databasePlayer.Authentication}] - {ScpUtils.StaticInstance.Translation.PlaytimeNoactivity}");
                    }

                    else
                    {
                        message.AppendLine($"[{databasePlayer.Name} - {databasePlayer.Id}@{databasePlayer.Authentication}] - {ScpUtils.StaticInstance.Translation.BadgeplaytimePlaytime} [ {new TimeSpan(0, 0, playtime).ToString()} ] - {ScpUtils.StaticInstance.Translation.BadgeplaytimeDaysjoined} [ {completedays}/{days} ] - {ScpUtils.StaticInstance.Translation.BadgeplaytimeOverwatchtime} [ {new TimeSpan(0, 0, owpt).ToString()} ] - {ScpUtils.StaticInstance.Translation.BadgeplaytimeOverwatchdays} [ {owdays}/{days} ]");
                    }
                    count++;

                }
            }

            if (count == 0)
            {
                response = ScpUtils.StaticInstance.Translation.InvalidPlayer;
                return false;
            }


            response = $"{message}";

            return true;
        }
    }
}
