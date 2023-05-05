using CommandSystem;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PlayerResetReset : ICommand
    {

        public string Command { get; } = "scputils_player_reset_preferences";

        public string[] Aliases { get; } = new[] { "prp", "su_prp", "su_playerpreferences", "su_player_preferences", "su_player_reset_preferences", "scpu_prp", "scpu_playerpreferences", "scpu_player_preferences", "scpu_player_reset_preferences" };

        public string Description { get; } = "Reset player preferences (Nickname, badges etc)!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.playerresetpreferences"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }
            else if (arguments.Count < 1)
            {
                response = $"<color=red>Usage: {Command} <player name/id></color>";
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

                databasePlayer.ResetPreferences();
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = "Player preferences have been resetted!";

                return true;
            }
        }
    }
}
