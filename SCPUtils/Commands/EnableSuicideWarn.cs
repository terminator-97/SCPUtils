using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class EnableSuicideWarn : ICommand
    {
        public string Command { get; } = "scputils_enable_suicide_warns";

        public string[] Aliases { get; } = new[] { "esw", "enable_suicide_warns", "su_esw", "scpu_esw" };

        public string Description { get; } = "Enables again SCP suicide/quit detector";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.warnmanagement"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            else if (!SCPUtils.EventHandlers.TemporarilyDisabledWarns)
            {
                response = "Warns are already enabled";
                return false;
            }
            else if (!ScpUtils.StaticInstance.Config.EnableSCPSuicideAutoWarn)
            {
                response = "Suicide / Quit warns are disabled by server config, contact server owner if this is an error!";
                return false;
            }
            EventHandlers.TemporarilyDisabledWarns = false;
            response = "Suicide and Quit warns have been re-enabled!";
            return true;
        }
    }
}
