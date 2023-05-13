using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class PlayerList : ICommand
    {

        public string Command { get; } = "scputils_player_list";

        public string[] Aliases { get; } = new[] { "pli", "su_pl", "su_playerlist", "scpu_pl", "scpu_playerlist" };

        public string Description { get; } = "Show player list in scputils database with some basic informations, don't use values like 0 otherwise the list may get huge";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.playerlist"))
            {
                response = "<color=red>You need a higher administration level to use this command!</color>";
                return false;
            }
            else
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <Minimun SCP quit percentage></color>";
                    return false;
                }
            }
            StringBuilder playerListString = new StringBuilder("[Quits/Suicides Percentage]");
            playerListString.AppendLine();
            if (int.TryParse(arguments.Array[1].ToString(), out int minpercentage))
            {
                foreach (Player databasePlayer in Database.LiteDatabase.GetCollection<Player>().Find(x => x.SuicidePercentage >= minpercentage))
                {
                    playerListString.AppendLine();
                    playerListString.Append($"{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication}) -[ {Math.Round((float)databasePlayer.ScpSuicideCount == 0 ? 0 : (databasePlayer.ScpSuicideCount / (float)databasePlayer.TotalScpGamesPlayed) * 100, 2)}% ]");
                }
                response = $"{playerListString}";
            }

            else
            {
                response = "Arg1 is not an integer, Comand usage example: scputils_player_list 50";
                return false;
            }


            return true;
        }
    }
}
