using CommandSystem;

using System;
using System.Text;


namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Alias : ICommand
    {
        public string Command { get; } = "scputils_alias";

        public string[] Aliases { get; } = new[] { "alias", "su_alias", "scpu_alias", "alt" };

        public string Description { get; } = "Check player aliases with some basic info, for complete info use dupeip";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.alias"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }
            if (arguments.Count != 1)
            {
                response = $"<color=yellow>Usage: {Command} <player name/id></color>";
                return false;
            }
            string targetName = arguments.Array[1];
            Player databasePlayer = targetName.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }
            var databaseIp = GetIp.GetIpAddress(databasePlayer.Ip);
            if (databaseIp == null)
            {
                response = "<color=yellow>Invalid IP!</color>";
                return false;
            }

            StringBuilder message = new StringBuilder($"<color=green>[Accounts associated with the following account ({databasePlayer.Name} {databasePlayer.Id}@{databasePlayer.Authentication})]</color>").AppendLine();
            foreach (var userId in databaseIp.UserIds)
            {

                message.AppendLine();
                message.Append(
                        $"<color=yellow>User-ID: {userId}</color>\n<color=yellow>Muted: {VoiceChat.VoiceChatMutes.QueryLocalMute(userId)}</color>")
                    .AppendLine();
            }
            response = message.ToString();
            return true;
        }
    }
}