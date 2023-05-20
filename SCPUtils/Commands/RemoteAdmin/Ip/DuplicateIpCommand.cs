namespace SCPUtils.Commands.RemoteAdmin.Ip
{
    using CommandSystem;
    using System;
    using System.Text;

    public class DuplicateIpCommand : ICommand
    {
        public string Command { get; } = "duplicate";
        public string[] Aliases { get; } = new[]
        {
            "dupe", "alias"
        };
        public string Description { get; } = "Check if player has another account on same IP.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils ip duplicate"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils ip duplicate"]}");
                return false;
            }
            else if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", ScpUtils.StaticInstance.commandTranslation.Player);
                return false;
            }
            else
            {
                string target = arguments.Array[3];
                Player databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                    return false;
                }
                var databaseIp = GetIp.GetIpAddress(databasePlayer.Ip);
                if (databaseIp == null)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.IpInvalid;
                    return false;
                }

                StringBuilder duplicateIp = new StringBuilder(ScpUtils.StaticInstance.commandTranslation.IpResponse.Replace("%ip%", $"{databasePlayer.Ip}").Replace("%player%", $"{databasePlayer.Name} - {databasePlayer.Id}@{databasePlayer.Authentication}")).AppendLine();
                foreach (var userId in databaseIp.UserIds)
                {
                    var databasePlayer2 = DatabasePlayer.GetDatabasePlayer(userId);

                    if (databasePlayer2 != null)
                    {

                        duplicateIp.AppendLine();
                        duplicateIp.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.IpPlayer.Replace("%player%", $"{databasePlayer2.Name} ({databasePlayer2.Id}@{databasePlayer2.Authentication}")}");
                        duplicateIp.AppendLine($"\n{ScpUtils.StaticInstance.commandTranslation.IpFirstJoin.Replace("%firstJoin%", $"{databasePlayer2.FirstJoin}")}");
                        duplicateIp.AppendLine($"\n{ScpUtils.StaticInstance.commandTranslation.IpLastJoin.Replace("%lastJoin%", $"{databasePlayer2.LastSeen}")}");
                        duplicateIp.AppendLine($"\n{ScpUtils.StaticInstance.commandTranslation.IpRestricted.Replace("%restricted%", $"{databasePlayer2.IsRestricted()}")}");
                        duplicateIp.AppendLine($"\n{ScpUtils.StaticInstance.commandTranslation.IpBanned.Replace("%banned%", $"{databasePlayer2.IsBanned()}")}");
                        duplicateIp.AppendLine($"\n{ScpUtils.StaticInstance.commandTranslation.IpMuted.Replace("%muted%", $"{VoiceChat.VoiceChatMutes.QueryLocalMute(userId)}")}");
                        duplicateIp.AppendLine($"\n{ScpUtils.StaticInstance.commandTranslation.IpTotalGames.Replace("%games%", $"{databasePlayer2.TotalScpGamesPlayed}")}");
                        duplicateIp.AppendLine($"\n{ScpUtils.StaticInstance.commandTranslation.IpTotalSuicide.Replace("%suicide%", $"{databasePlayer2.ScpSuicideCount}")}");
                        duplicateIp.AppendLine($"\n{ScpUtils.StaticInstance.commandTranslation.IpRoundBans.Replace("%rounds%", $"{databasePlayer2.RoundBanLeft}")}");
                        duplicateIp.AppendLine();
                    }
                }
                response = duplicateIp.ToString();
                return true;
            }
        }
    }
}
