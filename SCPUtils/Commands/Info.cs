using CommandSystem;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Info : ICommand
    {
        public string Command { get; } = "scputils_info";

        public string[] Aliases { get; } = new string[] { "su_info", "su_i", "scpu_info", "scpu_i" };
        public string Description { get; } = "Show plugin info";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            response = $"<color=blue>Plugin Info: </color>\n" +
                            "<color=blue>SCPUtils is a public plugin created by Terminator_97#0507, you can download this plugin at: github.com/terminator-97/SCPUtils </color>\n" +
                            $"<color=blue>This server is running SCPUtils version 6.0.0 [NwApi]</color>";
            return true;
        }
    }
}