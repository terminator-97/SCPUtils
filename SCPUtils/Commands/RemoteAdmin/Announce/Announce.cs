namespace SCPUtils.Commands.RemoteAdmin.Announce
{
    using CommandSystem;
    using System;

    public class Announce : ParentCommand
    {
        public Announce() => LoadGeneratedCommands();

        public override string Command { get; } = "announce";
        public override string[] Aliases { get; } = new[]
        {
            "a"
        };
        public override string Description { get; } = "Announce base command.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new CreateAnnouncementCommand());
            //RegisterCommand(new DeleteBroadcastCommand());
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
