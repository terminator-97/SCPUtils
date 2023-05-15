﻿namespace SCPUtils.Commands.RemoteAdmin.Badge
{
    using CommandSystem;
    using System;

    public class Badge : ParentCommand
    {
        public Badge() => LoadGeneratedCommands();

        public override string Command { get; } = "badge";
        public override string[] Aliases { get; } = new[] { "b", "group" };
        public override string Description { get; } = "Badge base command.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new SetBadgeCommand());
            RegisterCommand(new RevokeBadgeCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }

            response = "Please specify a valid subcommand:";
            foreach (ICommand command in AllCommands)
            {
                response = string.Concat(new string[]
                {
                    response,
                    "\n",
                    command.Command,
                    " - ",
                    command.Description
                });
            }
            return false;
        }
    }
}