using System;
using CommandSystem;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    class Help : ICommand
    {
        public string Command => "scputils_help";

        public string[] Aliases { get; } = new string[] { };

        public string Description => "Show plugin info";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string text = "";
            text = $"<color=#1BBC9B>User commands:</color> \n" +
               "<color=#1BBC9B>.scputils_info, .scputils_help, .scputils_change_nickname, .scputils_change_color, .scputils_show_badge, .scputils_hide_badge, .scputils_my_info</color>";
            if (CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.help") && !((CommandSender)sender).FullPermissions) text += "\n<color=#FFD700>Administration commands (Remote Admin): </color>\n" +
                    "<color=#FFD700>scputils_help, scputils_player_info, scputils_player_list, scputils_player_reset_preferences, scputils_player_reset, scputils_set_color, scputils_set_name, scputils_set_badge,  scputils_revoke_badge, scputils_play_time, scputils_whitelist_asn, scputils_unwhitelist_asn</color>";
            response = text;
            return true;
        }
    }
}