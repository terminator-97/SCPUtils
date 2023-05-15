namespace SCPUtils.Commands.RemoteAdmin.ASN
{
    using CommandSystem;
    using System;

    public class ASN : ParentCommand
    {
        public ASN() => LoadGeneratedCommands();

        public override string Command { get; } = "asn";
        public override string[] Aliases { get; }
        public override string Description { get; } = "ASN base command.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new AsnWhitelistCommand());
            RegisterCommand(new AsnUnWhitelistCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(PlayerPermissions.KickingAndShortTermBanning))
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
