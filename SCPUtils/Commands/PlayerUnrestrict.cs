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
        public string Command { get; } = ScpUtils.StaticInstance.Translation.UnrestrictCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.UnrestrictAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.UnrestrictDescription;

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
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }

            else if (arguments.Count < 1)
            {
                response = $"{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer}";
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
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }

            else if (!databasePlayer.IsRestricted())
            {
                response = ScpUtils.StaticInstance.Translation.InvalidData;
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

            response = ScpUtils.StaticInstance.Translation.Success;
            return true;

        }
    }
}
