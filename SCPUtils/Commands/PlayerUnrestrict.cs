using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PlayerUnrestrict : ICommand
    {
        public string Command { get; } = "scputils_player_unrestrict";

        public string[] Aliases { get; } = new[] { "unrestrict", "unsusp", "su_playerunrestrict", "su_player_unr", "scpu_playerunrestrict", "scpu_player_unr" };

        public string Description { get; } = "This command removes the restriction to use scputils_change_nickname or scputils_change_color from a player!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            if (!sender.CheckPermission("scputils.moderatecommands"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }

            else if (arguments.Count < 1)
            {
                response = $"Usage: {Command} <player name/id>";
                return false;
            }


            else
            {
                target = arguments.Array[1].ToString();
            }

            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(target);
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            else if (!databasePlayer.IsRestricted())
            {
                response = "Player is not suspended!";
                return false;
            }

            databasePlayer.Restricted.Remove(databasePlayer.Restricted.Keys.Last());
            databasePlayer.SaveData();

            if (target != null)
            {
                if (ScpUtils.StaticInstance.EventHandlers.LastCommand.ContainsKey(player))
                {
                    ScpUtils.StaticInstance.EventHandlers.LastCommand.Remove(player);
                }
            }

            response = "Player unsuspended!";
            return true;

        }
    }
}
