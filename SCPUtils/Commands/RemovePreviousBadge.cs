using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class RemovePreviousBadge : ICommand
    {

        public string Command { get; } = "scputils_remove_previous_badge";

        public string[] Aliases { get; } = new[] { "rpb" };

        public string Description { get; } = "Removes previous badge!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!sender.CheckPermission("scputils.handlebadges"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            else if (arguments.Count < 1)
            {
                response = $"<color=red>Usage: {Command} <player name/id></color>";
                return false;
            }
            else
            {
                var target = arguments.Array[1].ToString();

                var databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                    return false;
                }

                databasePlayer.PreviousBadge = "";
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = $"{target}'s previous badge removed!";

                return true;
            }
        }
    }
}

