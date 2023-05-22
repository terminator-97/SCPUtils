namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using System;

    public class PlayerCommand : ParentCommand
    {
        public PlayerCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "player";
        public override string[] Aliases { get; } = new[]
        {
            "p", "pl"
        };
        public override string Description { get; } = "Player base command.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new BroadcastPlayerCommand());
            RegisterCommand(new DeletePlayerCommand());
            RegisterCommand(new DntPlayerCommand());
            RegisterCommand(new EditPlayerCommand());
            RegisterCommand(new InfoPlayerCommand());
            RegisterCommand(new ListPlayerCommand());
            RegisterCommand(new ResetPlayerCommand());
            RegisterCommand(new RestrictionPlayerCommand());
            RegisterCommand(new UnrestrictionPlayerCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = ScpUtils.StaticInstance.commandTranslation.ParentCommands;
            foreach (ICommand command in AllCommands)
            {
                response = string.Concat(new string[]
                {
                    response,
                    "\n\n",
                    ScpUtils.StaticInstance.commandTranslation.CommandName+command.Command,
                    "\n",
                    ScpUtils.StaticInstance.commandTranslation.CommandDescription+command.Description,
                });
                if (command.Aliases != null && command.Aliases.Length != 0)
                {
                    response = response + "\n" + ScpUtils.StaticInstance.commandTranslation.CommandAliases + string.Join(", ", command.Aliases);
                }
            }
            return false;
        }
    }
}