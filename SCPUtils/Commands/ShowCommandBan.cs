using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class ShowCommandBans : ICommand
    {

        public string Command { get; } = "scputils_show_command_bans";

        public string[] Aliases { get; } = new[] { "scb", "su_show_cb", "su_scb", "scpu_show_cb", "scpu_scb" };

        public string Description { get; } = "You can see detailed informations about restriction";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            if (!sender.CheckPermission("scputils.moderatecommands"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            else
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <player name/id></color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                }
            }
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }
            string message = $"\n[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n" +
  $"Total Command restrictions: [ { databasePlayer.Restricted.Count } ]\n";


            if (databasePlayer.IsRestricted())
            {
                message += $"*** Active Restrictions: ***\n\n" +
                               $"Reason: [ {databasePlayer.Restricted.Values.Last()} ]\n" +
                               $"Expire: [ {databasePlayer.Restricted.Keys.Last()} ]\n";
            }

            if (databasePlayer.Restricted.Count >= 1)
            {
                message += $"\n*** Restrictions History: ***\n\n";
                foreach (System.Collections.Generic.KeyValuePair<DateTime, string> a in databasePlayer.Restricted)
                {
                    message += $"Reason: [ {a.Value} ]\n";
                    message += $"Expire: [ {a.Key} ]\n\n";
                }
            }
            else
            {
                message += "No restrictions!";
            }

            response = $"{message}";

            return true;
        }
    }
}
