using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class RevokeBadge : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.RevokeBadgeCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.RevokeBadgeAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.RevokeBadgeDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            if (!sender.CheckPermission("scputils.handlebadges"))
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

            databasePlayer.BadgeExpire = DateTime.MinValue;
            databasePlayer.SaveData();
            if (player != null)
            {
                player.BadgeHidden = false;
            }

            response = ScpUtils.StaticInstance.Translation.Success;
            return true;

        }
    }
}
