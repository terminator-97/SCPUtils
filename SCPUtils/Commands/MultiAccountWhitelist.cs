using CommandSystem;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class MultiAccountWhitelist : ICommand
    {
        public string Command { get; } = "scputils_multiaccount_whitelist";

        public string[] Aliases { get; } = new[] { "mawl" };

        public string Description { get; } = "Whitelist/Unwhitelist a player to make him being ignored/detected by multiaccount system!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.whitelistma"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }
            else if (arguments.Count < 1)
            {
                response = $"<color=yellow>Usage: {Command} <player name/id></color>";
                return false;
            }

            else
            {
                string target = arguments.Array[1];
                Player databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                    return false;
                }

                databasePlayer.MultiAccountWhiteList = !databasePlayer.MultiAccountWhiteList;
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = "Success!";
                return true;
            }
        }
    }
}
