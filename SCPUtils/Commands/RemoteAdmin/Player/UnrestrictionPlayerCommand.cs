namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using System;
    using System.Linq;

    public class UnrestrictionPlayerCommand : ICommand
    {
        public string Command { get; } = "unresction";

        public string[] Aliases { get; } = new[] { "un", "ur" };

        public string Description { get; } = "This command removes the restriction to use scputils_change_nickname or scputils_change_color from a player!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            string target = arguments.Array[1];
            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils player unrestriction"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils player unrestriction"]}");
                return false;
            }

            else if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player}");
                return false;
            }

            PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(target);
            SCPUtils.Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                return false;
            }

            else if (!databasePlayer.IsRestricted())
            {
                response = ScpUtils.StaticInstance.commandTranslation.AlreadyUnrestrict.Replace("%player%", arguments.Array[3]);
                return false;
            }

            databasePlayer.Restricted.Remove(databasePlayer.Restricted.Keys.Last());
            databasePlayer.SaveData();

            if (target != null)
            {
                if (ScpUtils.StaticInstance.EventHandlers.LastCommand.ContainsKey(player))
                {
                    ScpUtils.StaticInstance.EventHandlers.LastCommand.Remove(player);
                }
            }

            response = ScpUtils.StaticInstance.commandTranslation.UnrestrictionResponse.Replace("%player%", arguments.Array[3]);
            return true;

        }
    }
}
