using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class DisableSuicideWarn : ICommand
    {
        public string Command { get; } = "scputils_disable_suicide_warns";
        public string[] Aliases { get; } = new[] { "dsw", "disable_suicide_warns", "su_dsw", "scpu_dsw" };

        public string Description { get; } = "Temporarily disable SCP suicide/quit detector for the duration of the round";

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
            response = "Suicide and Quit warns have been disabled for this round!";
            return true;
        }
    }
}
