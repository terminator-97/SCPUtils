using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class HideBadge : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.HidebadgeCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.HidebadgeAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.HidebadgeDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.badgevisibility"))
            {
                response = $"{ScpUtils.StaticInstance.Config.UnauthorizedBadgeChangeVisibility} ";
                return false;
            }
            else if (((CommandSender)sender).Nickname.Equals("SERVER CONSOLE"))
            {
                response = "This command cannot be executed from console!";
                return false;
            }
            else
            {
                Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId);
                player.BadgeHidden = true;
                player.GetDatabasePlayer().HideBadge = true;
                response = ScpUtils.StaticInstance.Translation.Success;
                return true;
            }
        }
    }
}
