using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class EnableSuicideWarn : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.EnablesuicidewarnCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.EnablesuicidewarnAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.EnablesuicidewarnDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.warnmanagement"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            else if (!SCPUtils.EventHandlers.TemporarilyDisabledWarns)
            {
                response = $"{ScpUtils.StaticInstance.Translation.AlreadyEnabled}";
                return false;
            }
            else if (!ScpUtils.StaticInstance.Config.EnableSCPSuicideAutoWarn)
            {
                response = $"{ScpUtils.StaticInstance.Translation.Disabled} {ScpUtils.StaticInstance.Translation.ByServerConfigs}";
                return false;
            }
            EventHandlers.TemporarilyDisabledWarns = false;
            response = ScpUtils.StaticInstance.Translation.Success;
            return true;
        }
    }
}
