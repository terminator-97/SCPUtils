using System;
using CommandSystem;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class DisableSuicideWarn : ICommand
    {
        public string Command { get; } = "scputils_disable_suicide_warns";
        public string[] Aliases { get; } = new[] { "dsw", "disable_suicide_warns" };

        public string Description { get; } = "Temporarily disable SCP suicide/quit detector for the rest of this round!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.warnmanagement") && !((CommandSender)sender).FullPermissions)
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            else if (SCPUtils.EventHandlers.TemporarilyDisabledWarns)
            {
                response = "Warns are already disabled";
                return false;
            }
            else if (!ScpUtils.StaticInstance.Config.EnableSCPSuicideAutoWarn)
            {
                response = "Suicides / Quits warns are already disabled by server config!";
                return false;
            }
            EventHandlers.TemporarilyDisabledWarns = true;
            response = "Suicides and Quits warns have been disabled for this round!";
            return true;
        }
    }
}