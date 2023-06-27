using CommandSystem;
using Exiled.Permissions.Extensions;
using MongoDB.Driver;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PlayerDelete : ICommand
    {

        public string Command { get; } = "scputils_player_delete";

        public string[] Aliases { get; } = new[] { "pdelete", "su_pdelete", "su_playerdelete", "scpu_pdelete", "scpu_playerdelete" };

        public string Description { get; } = "Delete a player (and all the player data) from the database, action is irreversible!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

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
                string target = arguments.Array[1].ToString();

                Player databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                    return false;
                }

                databasePlayer.Reset();
                Database.MongoDatabase.GetCollection<Player>("players").DeleteOne(broadcast => broadcast.Id == databasePlayer.Id);
                response = $"{target} has been deleted from the database!";

                return true;
            }
        }
    }
}

