using CommandSystem;
using PluginAPI.Core;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class HideBadge : ICommand
    {
        public string Command { get; } = "scputils_hide_badge";

        public string[] Aliases { get; } = new[] { "hb", "su_hb", "su_hbadge", "su_hideb", "scpu_hb", "scpu_hbadge", "scpu_hideb" };

        public string Description { get; } = "Hides your badge permanently until you execute scputils_show_badge or their aliases.";

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
                player.ReferenceHub.characterClassManager.UserCode_CmdRequestHideTag();
                player.GetDatabasePlayer().HideBadge = true;
                response = "<color=green>Your badge has been hidden!</color>";
                return true;
            }
        }
    }
}
