using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class HideBadge : ICommand
    {
        public string Command { get; } = "scputils_hide_badge";

        public string[] Aliases { get; } = new[] { "hb", "su_hb", "su_hidebadge", "scpu_hb", "scpu_hidebadge" };

        public string Description { get; } = "Hides your badge permanently until you execute scputils_show_badge or sb";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
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
                response = "<color=green>Your badge has been hidden!</color>";

                return true;
            }
        }
    }
}
