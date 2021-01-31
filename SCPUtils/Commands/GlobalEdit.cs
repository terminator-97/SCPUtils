using System;
using CommandSystem;
using UnityEngine;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class GlobalEdit : ICommand
    {

        public string Command { get; } = "scputils_global_edit";

        public string[] Aliases { get; } = new[] { "gedit" };

        public string Description { get; } = "Remove specified amount of scp games / warns / kick / bans from each player present in DB!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.globaledit") && !((CommandSender)sender).FullPermissions)
            {
                response = "<color=red>You need a higher administration level to use this command!</color>";
                return false;
            }
            else
            {
                if (arguments.Count < 4)
                {
                    response = $"<color=yellow>Usage: {Command} <SCPGames to remove> <Suicides to remove> <Kicks to remove> <Bans to remove></color>";
                    return false;
                }
            }

            if (int.TryParse(arguments.Array[1].ToString(), out int scpGamesToRemove) && int.TryParse(arguments.Array[2].ToString(), out int suicidesToRemove) && int.TryParse(arguments.Array[3].ToString(), out int kicksToRemove) && int.TryParse(arguments.Array[4].ToString(), out int bansToRemove))
            {
                foreach (var databasePlayer in Database.LiteDatabase.GetCollection<Player>().Find(x => x.ScpSuicideCount >= 1))
                {
                    if (databasePlayer.TotalScpGamesPlayed >= scpGamesToRemove) databasePlayer.TotalScpGamesPlayed -= scpGamesToRemove;
                    else databasePlayer.TotalScpGamesPlayed = 0;
                    if (databasePlayer.ScpSuicideCount >= suicidesToRemove) databasePlayer.ScpSuicideCount -= suicidesToRemove;
                    else databasePlayer.ScpSuicideCount = 0;
                    if (databasePlayer.TotalScpSuicideKicks >= kicksToRemove) databasePlayer.TotalScpSuicideKicks -= kicksToRemove;
                    else databasePlayer.TotalScpSuicideKicks = 0;
                    if (databasePlayer.TotalScpSuicideBans >= bansToRemove) databasePlayer.TotalScpSuicideBans -= bansToRemove;
                    else databasePlayer.TotalScpSuicideBans = 0;
                    Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                }
            }

            else
            {
                response = $"One or more arguments are not an integer, Command usage example: {Command} 4 2 1 3";
                return false;
            }

            response = $"Success, the following edits have been made globally: Removed: {scpGamesToRemove} SCP Game(s), {suicidesToRemove} Suicide(s), {kicksToRemove} Kick(s), {bansToRemove} Ban(s) to each player!";
            return true;
        }
    }
}
