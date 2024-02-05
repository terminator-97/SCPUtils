using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class PlayerRestrict : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.RestrictCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.RestrictAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.RestrictDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            string reason;
            if (!sender.CheckPermission("scputils.moderatecommands"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} {ScpUtils.StaticInstance.Translation.ArgMinutes}(0 = permanent) <Reason>";
                return false;
            }
            target = arguments.Array[1].ToString();

            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(target);
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
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

                databasePlayer.SaveData();
                response = $"{ScpUtils.StaticInstance.Translation.Success}";

            }
            else
            {
                response = ScpUtils.StaticInstance.Translation.Success;
            }

            return true;
        }
    }
}
