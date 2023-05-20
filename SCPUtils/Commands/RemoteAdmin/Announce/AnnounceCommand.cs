namespace SCPUtils.Commands.RemoteAdmin.Announce
{
    using CommandSystem;
    using System;

    public class AnnounceCommand : ParentCommand
    {
        public AnnounceCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "announce";
        public override string[] Aliases { get; } = new[]
        {
            "a", "broadcast", "bc"
        };
        public override string Description { get; } = "Announce base command.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new CreateAnnouncementCommand());
            RegisterCommand(new DeleteAnnouncementCommand());
            RegisterCommand(new SendAnnoucementCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils announce"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils announce"]}");
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
                    response = response + "\n"+ ScpUtils.StaticInstance.commandTranslation.CommandAliases + string.Join(", ", command.Aliases);
                }
            }
            return false;
        }
    }
}
