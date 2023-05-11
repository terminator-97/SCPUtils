using CommandSystem;
using Exiled.Permissions.Extensions;
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
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.whitelistma"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
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
                databasePlayer.SaveData();
                response = "Success!";
                return true;
            }
        }
    }
}
