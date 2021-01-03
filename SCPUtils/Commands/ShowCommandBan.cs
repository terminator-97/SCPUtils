using System;
using System.Linq;
using CommandSystem;
using Log = Exiled.API.Features.Log;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class ShowCommandBans : ICommand
    {

        public string Command { get; } = "scputils_show_command_bans";

        public string[] Aliases { get; } = new[] { "scb" };

        public string Description { get; } = "You can see detailed informations about playtime";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.moderatecommands") && !((CommandSender)sender).FullPermissions)
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
                else target = arguments.Array[1].ToString();
            }
            var databasePlayer = target.GetDatabasePlayer();

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


            if (databasePlayer.IsRestricted()) message += $"*** Active Restrictions: ***\n\n" +
                               $"Reason: [ {databasePlayer.Restricted.Values.Last()} ]\n" +
                               $"Expire: [ {databasePlayer.Restricted.Keys.Last()} ]\n";

            if (databasePlayer.Restricted.Count >= 1)
            {
                message += $"\n*** Restrictions History: ***\n\n";
                foreach (var a in databasePlayer.Restricted)
                {
                    message += $"Reason: [ {a.Value} ]\n";
                    message += $"Expire: [ {a.Key} ]\n\n";
                }
            }
            else message += "No restrictions!";

            response = $"{message}";

            return true;
        }
    }
}
