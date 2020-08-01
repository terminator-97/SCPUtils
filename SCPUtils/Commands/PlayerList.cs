using System;
using CommandSystem;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class PlayerList : ICommand
    {

        public string Command { get; } = "scputils_player_list";

        public string[] Aliases { get; } = new[] { "pl" };

        public string Description { get; } = "Show player list in scputils database with some basic informations, don't use values like 0 otherwise the list may get huge";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.playerlist") && !((CommandSender)sender).FullPermissions)
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
            var playerListString = "[Quits/Suicides Percentage]\n";

            if (int.TryParse(arguments.Array[1].ToString(), out int minpercentage))
            {
                foreach (var databasePlayer in Database.LiteDatabase.GetCollection<Player>().Find(x => x.SuicidePercentage >= minpercentage))
                {
                    playerListString += $"\n{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication}) -[ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]";
                }
                if (playerListString == "[Quits/Suicides as SCP]\n") response = "No results found";
                else response = $"{playerListString}";
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
