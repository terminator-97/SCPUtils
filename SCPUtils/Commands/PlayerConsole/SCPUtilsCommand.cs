namespace SCPUtils.Commands.Console
{
    using CommandSystem;
    using System;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class SCPUtilsCommand : ParentCommand
    {
        public SCPUtilsCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "scputils";
        public override string[] Aliases { get; } = new[]
        {
            "scpu", "su"
        };
        public override string Description { get; } = "The most famous plugin that offers many additions to the servers.";
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new UserPlayTimeCommand());
            RegisterCommand(new BadgeCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
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