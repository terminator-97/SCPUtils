using CommandSystem;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class RemovePreviousBadge : ICommand
    {

        public string Command { get; } = "scputils_remove_previous_badge";

        public string[] Aliases { get; } = new[] { "rpb", "su_rpb", "su_remove_pb", "scpu_rpb", "scpu_remove_pb" };

        public string Description { get; } = "Remove previous badge!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.handlebadges"))
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

                databasePlayer.PreviousBadge = "";
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = $"{target}'s previous badge removed!";

                return true;
            }
        }
    }
}

