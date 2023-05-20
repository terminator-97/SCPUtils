namespace SCPUtils.Commands.RemoteAdmin.PlayTime
{
    using CommandSystem;
    using System;

    public class PlayTimeCommand : ParentCommand
    {
        public PlayTimeCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "playtime";
        public override string[] Aliases { get; } = new[] { "pt", "play" };
        public override string Description { get; } = "Playtime base command.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new StaffPlayTimeCommand());
           // RegisterCommand(new PlayTimeBadgeCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils playtime"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils playtime"]}");
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
