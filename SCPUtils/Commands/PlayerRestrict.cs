using CommandSystem;
using System;
using PluginAPI.Core;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class PlayerRestrict : ICommand
    {

        public string Command { get; } = "scputils_player_restrict";

        public string[] Aliases { get; } = new[] { "restrict", "susp", "su_playerrestrict", "su_playerestrict", "su_player_r", "scpu_playerestrict", "scpu_playerrestrict", "scpu_player_r" };

        public string Description { get; } = "This command restricts a player from using the: scputils_change_nickname and scputils_change_color.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            string target;
            string reason;
            if (!sender.CheckPermission("scputils.moderatecommands"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"Usage: {Command} <player name / id> <Minutes, 0 = permanent> <Reason>";
                return false;
            }
            target = arguments.Array[1].ToString();

            PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(target);
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            else if (databasePlayer.IsRestricted())
            {
                response = "Player is already suspended!";
                return false;
            }


            else if (int.TryParse(arguments.Array[2], out int minutes))
            {
                reason = string.Join(" ", arguments.Array, 3, arguments.Array.Length - 3);
                databasePlayer.Restricted.Add(DateTime.Now.AddMinutes(minutes), reason);
                if (minutes == 0)
                {
                    databasePlayer.Restricted.Add(DateTime.MaxValue, reason);
                }
                if (target != null)
                {
                    if (!ScpUtils.StaticInstance.EventHandlers.LastCommand.ContainsKey(player))
                    {
                        if (minutes != 0)
                        {
                            ScpUtils.StaticInstance.EventHandlers.LastCommand.Add(player, DateTime.Now.AddMinutes(minutes));
                        }
                        else
                        {
                            ScpUtils.StaticInstance.EventHandlers.LastCommand.Add(player, DateTime.MaxValue);
                        }
                    }
                    else
                    {
                        if (minutes != 0)
                        {
                            ScpUtils.StaticInstance.EventHandlers.LastCommand[player] = DateTime.Now.AddMinutes(minutes);
                        }
                        else
                        {
                            ScpUtils.StaticInstance.EventHandlers.LastCommand[player] = DateTime.MaxValue;
                        }
                    }
                }

                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = $"Player suspended!";

            }
            else
            {
                response = "Duration must be integer!";
            }

            return true;
        }
    }
}
