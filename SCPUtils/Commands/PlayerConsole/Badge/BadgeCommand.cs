namespace SCPUtils.Commands.Console
{
    using CommandSystem;
    using PluginAPI.Core;
    using System;

    public class BadgeCommand : ParentCommand
    {
        public BadgeCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "badge";
        public override string[] Aliases { get; } = new[]
        {
            "b", "group", "g"
        };
        public override string Description { get; } = "Change your badge visibility.";
        public override void LoadGeneratedCommands()
        {
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            /*response = "Please specify a valid subcommand:";
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
            return false;*/

            Player user = Player.Get(((CommandSender)sender).SenderId);

            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }
            if (!ScpUtils.StaticInstance.configs.BadgeVisibility.Contains(user.ReferenceHub.serverRoles.Group.BadgeText))
            {
                response = ScpUtils.StaticInstance.configs.UnauthorizedBadgeChangeVisibility;
                return false;
            }
            else if (((CommandSender)sender).Nickname.Equals("SERVER CONSOLE"))
            {
                response = "This command cannot be executed from console!";
                return false;
            }

            switch (arguments.At(0))
            {
                case "hide":
                    Player player1 = Player.Get(((CommandSender)sender).SenderId);
                    player1.ReferenceHub.characterClassManager.UserCode_CmdRequestHideTag();
                    player1.GetDatabasePlayer().HideBadge = true;
                    response = "<color=green>Your badge has been shown!</color>";
                    return true;
                case "show":
                    Player player2 = Player.Get(((CommandSender)sender).SenderId);
                    player2.ReferenceHub.characterClassManager.UserCode_CmdRequestShowTag__Boolean(false);
                    player2.GetDatabasePlayer().HideBadge = false;
                    response = "<color=green>Your badge has been hidden!</color>";
                    return true;
            }
            response = "";
            return false;
        }
    }
}