using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class PlayerInfo : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.PlayerinfoCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.PlayerinfoAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.PlayerinfoDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            if (!sender.CheckPermission("scputils.playerinfo"))
            {
                target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).UserId;
            }
            else
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer}</color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                }
            }
            Player databasePlayer = target.GetDatabasePlayer();
            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }

            string text = $"<color=green>\n[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoTotalSuicides} [ {databasePlayer.ScpSuicideCount} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoKicks} [ {databasePlayer.TotalScpSuicideKicks} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoBans} [ {databasePlayer.TotalScpSuicideBans} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoGamesPlayed} [ {databasePlayer.TotalScpGamesPlayed} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoPercentage} [ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoFirstjoin} [ {databasePlayer.FirstJoin} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoLastseen} [ {databasePlayer.LastSeen} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoCustomcolor} [ {databasePlayer.ColorPreference} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoCustomname} [ {databasePlayer.CustomNickName} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoTempbadge} [ {databasePlayer.BadgeName} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoCustombadge} [ {databasePlayer.CustomBadgeName} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoBadgexpire} [ {databasePlayer.BadgeExpire} ]\n" +
            //  $"Previous Badge: [ {databasePlayer.PreviousBadge} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoHidebadge} [ {databasePlayer.HideBadge} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoAsnwhitelist} [ {databasePlayer.ASNWhitelisted} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoKeepflag} [ {databasePlayer.KeepPreferences} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoDnt} [ {databasePlayer.IgnoreDNT} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoWhitelist} [ {databasePlayer.MultiAccountWhiteList} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoPlaytime} [ {new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString()} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoOverwatchtime} [ {new TimeSpan(0, 0, databasePlayer.OwPlayTimeRecords.Values.Sum()).ToString()} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoCooldown} [ {databasePlayer.NicknameCooldown} ]\n" +
            $"{ScpUtils.StaticInstance.Translation.PlayerinfoOverwatch} [ {databasePlayer.OverwatchActive} ]</color>";

            if (databasePlayer.IsRestricted())
            {
                text += $"\n<color=red>{ScpUtils.StaticInstance.Translation.PlayerinfoRestricted}</color>\nReason: [ {databasePlayer.Restricted.Values.Last()} ]\n{ScpUtils.StaticInstance.Translation.PlayerinfoExpire} [ {databasePlayer.Restricted.Keys.Last()} ]";
            }

            if (databasePlayer.RoundBanLeft >= 1)
            {
                text += $"\n<color=red>{ScpUtils.StaticInstance.Translation.PlayerinfoScpbanned}:</color>\n{ScpUtils.StaticInstance.Translation.PlayerinfoRoundsleft} [ {databasePlayer.RoundBanLeft} ]";
            }
            response = text;

            return true;
        }
    }
}
