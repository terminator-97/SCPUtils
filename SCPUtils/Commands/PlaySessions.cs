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
    internal class PlaySessions : ICommand
    {

        public string Command { get; } = "scputils_play_sessions";

        public string[] Aliases { get; } = new[] { "ps", "su_playsessions", "su_ps", "scpu_playsession", "scpu_ps" };

        public string Description { get; } = "You can see detailed informations about playsessions";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            int range;
            if (!sender.CheckPermission("scputils.playsessions"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }

            if (arguments.Count < 2)
                {
                    response = $"<color=yellow>Usage: {Command} <player name/id> <number of previous sessions to print(0 prints the last one)> </color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                }

                int.TryParse(arguments.Array[2], out range);

            
            Player databasePlayer = target.GetDatabasePlayer();

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
            StringBuilder message = new StringBuilder($"[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]");
            message.AppendLine();
           

            for (int i = 0; i <= range; i++)
            {
                if(databasePlayer.PlaytimeSessionsLog.Count()-i-1>=0)
                {
                    var session = databasePlayer.PlaytimeSessionsLog.ElementAt(databasePlayer.PlaytimeSessionsLog.Count()-i-1);
                    message.AppendLine($"Session Start: [ {session.Key} ] - Session End: [ { session.Value} ] - Duration: [ {(session.Value-session.Key).ToString(@"hh\:mm\:ss")} ]");
                }               
                else
                {
                    message.Append($"Limit reached! No more data about this player.");
                    response = $"{message}";
                    return true;
                }
            }

            response = $"{message}";

            return true;
        }
    }
}
