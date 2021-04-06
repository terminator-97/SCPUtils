using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PlayerEdit : ICommand
    {

        public string Command { get; } = "scputils_player_edit";

        public string[] Aliases { get; } = new[] { "pedit" };

        public string Description { get; } = "Edits the specified player data!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!sender.CheckPermission("scputils.playeredit"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            else if (arguments.Count < 5)
            {
                response = $"<color=red>Usage: {Command} <player name/id> <Total SCPGames played> <Total suicides/quits as SCP> <Total kicks> <Total bans></color>";
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


                if (int.TryParse(arguments.Array[2].ToString(), out int totalSCPGames) && int.TryParse(arguments.Array[3].ToString(), out int totalSCPSuicides) && int.TryParse(arguments.Array[4].ToString(), out int totalSCPKicks) && int.TryParse(arguments.Array[5].ToString(), out int totalSCPBans))
                {

                    databasePlayer.TotalScpGamesPlayed = totalSCPGames;
                    databasePlayer.ScpSuicideCount = totalSCPSuicides;
                    databasePlayer.TotalScpSuicideKicks = totalSCPKicks;
                    databasePlayer.TotalScpSuicideBans = totalSCPBans;
                    Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                }

                else
                {
                    response = $"One or more arguments are not an integer, Command usage example: {Command} 76561198347253445@steam 25 3 0 1";
                    return false;
                }

                response = $"Success, {target} has been edited as follows: Total SCP Games Played: {totalSCPGames}, Total Suicides/Quits: {totalSCPSuicides}, Total Kicks: {totalSCPKicks}, Total Bans: {totalSCPBans}";
                return true;
            }
        }
    }
}

