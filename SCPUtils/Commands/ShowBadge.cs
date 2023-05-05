using CommandSystem;
using PluginAPI.Core;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ShowBadge : ICommand
    {

        public string Command { get; } = "scputils_show_badge";

        public string[] Aliases { get; } = new[] { "sb", "su_showb", "su_sbadge", "scpu_showb", "scpu_sbadge" };

        public string Description { get; } = "Shows your badge permanently until you execute scputils_hide_badge or their aliases.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.badgevisibility"))
            {
                response = $"{ScpUtils.StaticInstance.configs.UnauthorizedBadgeChangeVisibility} ";
                return false;
            }
            else if (((CommandSender)sender).Nickname.Equals("SERVER CONSOLE"))
            {
                response = "This command cannot be executed from console!";
                return false;
            }
            else
            {
                PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(((CommandSender)sender).SenderId);
                player.ReferenceHub.characterClassManager.UserCode_CmdRequestShowTag(false);
                player.GetDatabasePlayer().HideBadge = false;
                response = "<color=green>Your badge has been shown!</color>";
                return true;
            }
        }
    }
}
