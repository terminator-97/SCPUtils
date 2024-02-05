using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ShowBadge : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.ShowbadgeCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.ShowbadgeAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.ShowbadgeDescription;

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
                player.BadgeHidden = false;
                player.GetDatabasePlayer().HideBadge = false;
                response = ScpUtils.StaticInstance.Translation.Success;
                return true;
            }
        }
    }
}
