using CommandSystem;
using System;
using PluginAPI.Core;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class RevokeBadge : ICommand
    {
        public string Command { get; } = "scputils_revoke_badge";

        public string[] Aliases { get; } = new[] { "rb", "su_rb", "su_remove_badge", "su_revoke_b", "scpu_rb", "scpu_remove_badge", "scpu_revoke_b" };

        public string Description { get; } = "Remove a temporarily badge that has been given to a player!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            string target;
            if (!sender.CheckPermission("scputils.handlebadges"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }

            else if (arguments.Count < 1)
            {
                response = $"Usage: {Command} <player name/id>";
                return false;
            }

            else
            {
                target = arguments.Array[1].ToString();
            }

            PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(target);
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            databasePlayer.BadgeExpire = DateTime.MinValue;
            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
            if (player != null)
            {
                player.ReferenceHub.characterClassManager.UserCode_CmdRequestShowTag(false);
            }

            response = "Badge revoked!";
            return true;

        }
    }
}
