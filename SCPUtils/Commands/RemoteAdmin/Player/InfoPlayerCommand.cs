namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using System;
    using System.Linq;
    using System.Text;

    public class InfoPlayerCommand : ICommand
    {
        public string Command { get; } = "info";
        public string[] Aliases { get; } = new[]
        {
            "i"
        };
        public string Description { get; } = "Show player info.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils player info"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils player info"]}");
                return false;
            }
            else if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player}");
                return false;
            }

            string target = arguments.Array[3];
            SCPUtils.Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                return false;
            }

            StringBuilder info = new StringBuilder(ScpUtils.StaticInstance.commandTranslation.InfoPlayer.Replace("%player%", $"{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})")).AppendLine("\n");

            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerTotalQuit.Replace("%total%", $"{databasePlayer.ScpSuicideCount}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerQuitKick.Replace("%total%", $"{databasePlayer.TotalScpSuicideKicks}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerQuitBans.Replace("%total%", $"{databasePlayer.TotalScpSuicideBans}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerGames.Replace("%total%", $"{databasePlayer.TotalScpGamesPlayed}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerPercentage.Replace("%percentage%", $"{Math.Round(databasePlayer.SuicidePercentage, 2)}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerFirst.Replace("%firstJoin%", $"{databasePlayer.FirstJoin}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerLast.Replace("%lastJoin%", $"{databasePlayer.LastSeen}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerCustomColor.Replace("%color%", $"{databasePlayer.ColorPreference}")}");
            //info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerCustomInfo.Replace("%info%", $"{Player.Get(((CommandSender)sender).SenderId).CustomInfo}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerCustomName.Replace("%name%", $"{databasePlayer.CustomNickName}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerBadge.Replace("%badge%", $"{databasePlayer.BadgeName}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerExpire.Replace("%expire%", $"{databasePlayer.BadgeExpire}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerHide.Replace("%hide%", $"{databasePlayer.HideBadge}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerWhitelist.Replace("%whitelist%", $"{databasePlayer.ASNWhitelisted}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerFlag.Replace("%flag%", $"{databasePlayer.KeepPreferences}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerDnt.Replace("%dnt%", $"{databasePlayer.IgnoreDNT}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerMultiAccount.Replace("%multiaccount%", $"{databasePlayer.MultiAccountWhiteList}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerPlaytime.Replace("%playtime%", $"{new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum())}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerCooldown.Replace("%cooldown%", $"{databasePlayer.NicknameCooldown}")}");
            info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerOverwatch.Replace("%overwatch%", $"{databasePlayer.OverwatchActive}")}");

            if (databasePlayer.IsRestricted()) info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerRestrict.Replace("%reason%", $"{databasePlayer.Restricted.Values.Last()}").Replace("%expire%", $"{databasePlayer.Restricted.Keys.Last()}")}");

            if (databasePlayer.RoundBanLeft != 0) info.AppendLine($"{ScpUtils.StaticInstance.commandTranslation.InfoPlayerScpBan.Replace("%rounds%", $"{databasePlayer.RoundBanLeft}")}");

            response = $"{info}";
            return true;
        }
    }
}
