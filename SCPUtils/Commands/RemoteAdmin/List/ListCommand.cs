namespace SCPUtils.Commands.RemoteAdmin.List
{
    using CommandSystem;
    using System;

    public class ListCommand : ParentCommand
    {
        public ListCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "list";
        public override string[] Aliases { get; } = new[]
        {
            "l"
        };
        public override string Description { get; } = "List base command.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new StaffListCommand());
            RegisterCommand(new PlayerListCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils list"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils list"]}");
                return false;
            }

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
