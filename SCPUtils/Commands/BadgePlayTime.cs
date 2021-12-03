using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using System.Text;
using Log = Exiled.API.Features.Log;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class BadgePlayTime : ICommand
    {

        public string Command { get; } = "scputils_badge_play_time";

        public string[] Aliases { get; } = new[] { "bpt", "tpt", "su_tpt", "scpu_badgeplaytime", "scpu_bpt" };

        public string Description { get; } = "Short playtime with a specified range by using badge as input.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string badge;
            int range;
            int count = 0;
            int playtime;
           // TimeSpan playtime;
            if (!sender.CheckPermission("scputils.playtime") && !((CommandSender)sender).FullPermissions)
            {
                response = "<color=red>You need a higher administration level to use this command!</color>";
                return false;
            }
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>Usage: {Command} <badgeName> <days range> </color>";
                    return false;
                }
                else
                {
                    badge = arguments.Array[1].ToString();
                }

                int.TryParse(arguments.Array[2], out range);

            if (!ServerStatic.GetPermissionsHandler().GetAllGroups().ContainsKey(badge))
            {
                response = "Invalid role name!";
                return false;
            }

            StringBuilder message = new StringBuilder($"Checking playtime of badge {badge}").AppendLine();
            foreach (var player in ServerStatic.RolesConfig.GetStringDictionary("Members"))
            {
                playtime = 0;          
                if (player.Value.ToString() == badge)
                {
                    var databasePlayer = player.Key.GetDatabasePlayer();
                    if (databasePlayer == null)
                    {
                        response = $"Null player detected! {player.Key}";
                        return false;
                    }
                        
                        for (int i = 0; i <= range; i++)
                        {
                            databasePlayer.PlayTimeRecords.Count();
                            DateTime.TryParse((DateTime.Now.Date.AddDays(-i)).ToString(), out DateTime date);
                            if (databasePlayer.PlayTimeRecords.ContainsKey(date.Date.ToShortDateString()))
                            {
                                playtime += databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()];
                            }
                        }
                        if (playtime == 0) message.AppendLine($"[{databasePlayer.Name} - {databasePlayer.Id}@{databasePlayer.Authentication}] - Playtime: [ No activity ]");
                        else message.AppendLine($"[{databasePlayer.Name} - {databasePlayer.Id}@{databasePlayer.Authentication}] - Playtime: [ { new TimeSpan(0, 0, playtime).ToString() } ]");
                        count++;                                      
                }
            }

            if(count==0)
            {
                response = "No players found on specified badge!";
                return false;
            }
            

            response = $"{message}";

            return true;
        }
    }
}
