namespace SCPUtils.Commands.RemoteAdmin.Badge
{
    using CommandSystem;
    using System;

    public class BadgeCommand : ParentCommand
    {
        public BadgeCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "badge";
        public override string[] Aliases { get; } = new[]
        {
            "b", "group"
        };
        public override string Description { get; } = "Badge base command.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new SetBadgeCommand());
            RegisterCommand(new RevokeBadgeCommand());
            //RegisterCommand(new PlayTimeBadgeCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils badge"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils badge"]}");
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
