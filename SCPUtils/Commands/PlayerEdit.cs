using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PlayerEdit : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.PlayereditCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.PlayereditAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.PlayereditDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.playeredit"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            else if (arguments.Count < 5)
            {
                response = $"<color=red>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} <Total SCPGames played> <Total suicides/quits as SCP> <Total kicks> <Total bans></color>";
                return false;
            }
            else
            {
                string target = arguments.Array[1].ToString();

                Player databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                    return false;
                }


                if (int.TryParse(arguments.Array[2].ToString(), out int totalSCPGames) && int.TryParse(arguments.Array[3].ToString(), out int totalSCPSuicides) && int.TryParse(arguments.Array[4].ToString(), out int totalSCPKicks) && int.TryParse(arguments.Array[5].ToString(), out int totalSCPBans))
                {

                    databasePlayer.TotalScpGamesPlayed = totalSCPGames;
                    databasePlayer.ScpSuicideCount = totalSCPSuicides;
                    databasePlayer.TotalScpSuicideKicks = totalSCPKicks;
                    databasePlayer.TotalScpSuicideBans = totalSCPBans;
                    databasePlayer.SaveData();
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

