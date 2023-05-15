namespace SCPUtils.Commands.RemoteAdmin.PlayTime
{
    using CommandSystem;
    using System;

    public class PlayTime : ParentCommand
    {
        public PlayTime() => LoadGeneratedCommands();

        public override string Command { get; } = "playtime";
        public override string[] Aliases { get; } = new[] { "pt", "play" };
        public override string Description { get; } = "Playtime base command.";

        public override void LoadGeneratedCommands()
        {
            //RegisterCommand(new UserPlayTimeCommand());
            RegisterCommand(new StaffPlayTimeCommand());
            //RegisterCommand(new BadgePlayTimeCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(PlayerPermissions.GameplayData))
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
