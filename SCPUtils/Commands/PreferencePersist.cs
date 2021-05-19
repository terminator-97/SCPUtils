using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class PreferencePersist : ICommand
    {

        public string Command { get; } = "scputils_preference_persist";

        public string[] Aliases { get; } = new[] { "pp", "su_pp", "su_preference_p", "scpu_pp", "scpu_preference_p" };

        public string Description { get; } = "Use this command to keep player badge and color even if he doesn't have access to that permission!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            if (!sender.CheckPermission("scputils.keep"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            else
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <player name/id></color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                }
            }
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = $"<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            if (databasePlayer.KeepPreferences == false)
            {
                databasePlayer.KeepPreferences = true;
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = "Success, keep mode has been enabled!";
            }
            else
            {
                databasePlayer.KeepPreferences = false;
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = "Success, keep mode has been disabled!";
            }


            return true;
        }
    }
}
