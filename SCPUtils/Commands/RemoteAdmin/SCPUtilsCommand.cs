namespace SCPUtils.Commands.RemoteAdmin
{
    using CommandSystem;
    using System;

    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SCPUtilsCommand : ParentCommand, IUsageProvider
    {
        public SCPUtilsCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "scputils";
        public override string[] Aliases { get; } = new[]
        {
            "scpu", "su"
        };
        public override string Description { get; } = "The most famous plugin that offers many additions to the servers.";

        public string[] Usage { get; } = new[]
        {
            "command"
        };

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new ASN.ASN());
            RegisterCommand(new Badge.Badge());
            RegisterCommand(new PlayTime.PlayTime());
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
