namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using System;
    using System.Text;

    public class EditPlayerCommand : ICommand
    {
        public string Command { get; } = "edit";

        public string[] Aliases { get; } = new[] { "e" };

        public string Description { get; } = "Edits the specified player data!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils player edit"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils player edit"]}");
                return false;
            }
            else if (arguments.Count < 5)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player} {ScpUtils.StaticInstance.commandTranslation.TotalGames} {ScpUtils.StaticInstance.commandTranslation.TotalQuitSuicide} {ScpUtils.StaticInstance.commandTranslation.TotalKicks} {ScpUtils.StaticInstance.commandTranslation.TotalBans}");
                return false;
            }
            else
            {
                string target = arguments.Array[3].ToString();

                SCPUtils.Player databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                    return false;
                }

                if (int.TryParse(arguments.Array[4].ToString(), out int totalSCPGames) && int.TryParse(arguments.Array[5].ToString(), out int totalSCPSuicides) && int.TryParse(arguments.Array[6].ToString(), out int totalSCPKicks) && int.TryParse(arguments.Array[7].ToString(), out int totalSCPBans))
                {
                    databasePlayer.TotalScpGamesPlayed = totalSCPGames;
                    databasePlayer.ScpSuicideCount = totalSCPSuicides;
                    databasePlayer.TotalScpSuicideKicks = totalSCPKicks;
                    databasePlayer.TotalScpSuicideBans = totalSCPBans;
                    databasePlayer.SaveData();
                }

                else
                {
                    response = ScpUtils.StaticInstance.commandTranslation.Integer.Replace("(%argument%)", "");
                    return false;
                }

                response = ScpUtils.StaticInstance.commandTranslation.EditResponse.Replace("%player%", arguments.Array[3]).Replace("%totalGames%", ScpUtils.StaticInstance.commandTranslation.TotalGames).Replace("%newTotalGames%", $"{totalSCPGames}").Replace("%totalQuit%", ScpUtils.StaticInstance.commandTranslation.TotalQuitSuicide).Replace("%newTotalQuit%", $"{totalSCPSuicides}").Replace("%totalKicks%", ScpUtils.StaticInstance.commandTranslation.TotalKicks).Replace("%newTotalKicks%", $"{totalSCPKicks}").Replace("%totalBans%", ScpUtils.StaticInstance.commandTranslation.TotalBans).Replace("%newTotalKicks%", $"{totalSCPKicks}");
                return true;
            }
        }
    }
}
