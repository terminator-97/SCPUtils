using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PlayerDelete : ICommand
    {

        public string Command { get; } = "scputils_player_delete";

        public string[] Aliases { get; } = new[] { "pdelete" };

        public string Description { get; } = "Delete a player (and all the player data) from the database, action is irreversible!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!sender.CheckPermission("scputils.playerdelete"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            else if (arguments.Count < 1)
            {
                response = $"<color=red>Usage: {Command} <player name/id> (You will delete the player from the database)</color>";
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

                databasePlayer.Reset();
                Database.LiteDatabase.GetCollection<Player>().Delete(databasePlayer.Id);
                response = $"{target} has been deleted from the database!";

                return true;
            }
        }
    }
}

