using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Text;


namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Alias : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.AliasCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.AliasAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.AliasDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.alias"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            if (arguments.Count != 1)
            {
                response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer}</color>";
                return false;
            }
            string targetName = arguments.Array[1];
            Player databasePlayer = targetName.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }
            var databaseIp = GetIp.GetIpAddress(databasePlayer.Ip);
            if (databaseIp == null)
            {
                response = ScpUtils.StaticInstance.Translation.InvalidIp;
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