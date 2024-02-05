using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Help : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.HelpCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.HelpAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.HelpDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string text = "";
            text = ScpUtils.StaticInstance.Translation.HelpContent;
            if (sender.CheckPermission("scputils.help"))
            {
                text += ScpUtils.StaticInstance.Translation.HelpContentAdmin;
            }

            response = text;
            return true;
        }
    }
}
