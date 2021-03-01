using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PlayerResetReset : ICommand
    {

        public string Command { get; } = "scputils_player_reset_preferences";

        public string[] Aliases { get; } = new[] { "prp" };

        public string Description { get; } = "Reset player preferences (Nickname, badges etc)!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!sender.CheckPermission("scputils.playerresetpreferences"))
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

                databasePlayer.ResetPreferences();
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = "Player preferences have been resetted!";

                return true;
            }
        }
    }
}