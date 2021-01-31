using System;
using CommandSystem;


namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ShowBadge : ICommand
    {

        public string Command { get; } = "scputils_show_badge";

        public string[] Aliases { get; } = new[] { "sb" };

        public string Description { get; } = "Shows your badge permanently until you execute scputils_hide_badge or hb";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.badgevisibility"))
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
                var player = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId);
                player.BadgeHidden = false;
                player.GetDatabasePlayer().HideBadge = false;
                response = "<color=green>Your badge has been shown!</color>";
                return true;
            }
        }
    }
}
