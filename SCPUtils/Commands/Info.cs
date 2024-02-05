using CommandSystem;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Info : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.InfoCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.InfoAliases;
        public string Description { get; } = ScpUtils.StaticInstance.Translation.InfoDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            response = $"<color=blue>Plugin Info: </color>\n" +
                            "<color=blue>SCPUtils [MongoDB Edition] is a public plugin created by Terminator_97, you can download this plugin at: github.com/terminator-97/SCPUtils on ScpUtils-MongoDb branch</color>\n" +
                            $"<color=blue>This server is running SCPUtils version {ScpUtils.StaticInstance.Version}</color>" +
                            $"{ScpUtils.StaticInstance.Translation.TranslationCredits}";
            return true;
        }
    }
}