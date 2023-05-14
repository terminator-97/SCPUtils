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

        public string Command { get; } = "scputils_player_info";

        public string[] Aliases { get; } = new[] { "upi", "scputils_my_info", "su_pi", "su_player_info", "su_playerinfo", "scpu_pi", "scpu_player_info", "scpu_playerinfo" };

        public string Description { get; } = "Show player info, in case you are not admin you can see only your info";

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
                    response = $"<color=yellow>Usage: {Command} <player name/id></color>";
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
                response = $"<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }



            string text = $"<color=green>\n[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n" +
            $"Total SCP Suicides/Quits: [ {databasePlayer.ScpSuicideCount} ]\n" +
            $"Total SCP Suicides/Quits Kicks: [ {databasePlayer.TotalScpSuicideKicks} ]\n" +
            $"Total SCP Suicides/Quits Bans: [ {databasePlayer.TotalScpSuicideBans} ]\n" +
            $"Total Games played as SCP: [ {databasePlayer.TotalScpGamesPlayed} ]\n" +
            $"Total Suicides/Quits Percentage: [ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]\n" +
            $"First Join: [ {databasePlayer.FirstJoin} ]\n" +
            $"Last Seen: [ {databasePlayer.LastSeen} ]\n" +
            $"Custom Color: [ {databasePlayer.ColorPreference} ]\n" +
            $"Custom Name: [ {databasePlayer.CustomNickName} ]\n" +
            $"Temporarily Badge: [ {databasePlayer.BadgeName} ]\n" +
            $"Badge Expire: [ {databasePlayer.BadgeExpire} ]\n" +
            //  $"Previous Badge: [ {databasePlayer.PreviousBadge} ]\n" +
            $"Hide Badge: [ {databasePlayer.HideBadge} ]\n" +
            $"Asn Whitelisted: [ {databasePlayer.ASNWhitelisted} ]\n" +
            $"Keep Flag: [ {databasePlayer.KeepPreferences} ]\n" +
            $"Ignore DNT: [ {databasePlayer.IgnoreDNT} ]\n" +
            $"MultiAccount Whitelist: [ {databasePlayer.MultiAccountWhiteList} ]\n" +
            $"Total Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString() } ]\n" +
            $"Nickname cooldown: [ { databasePlayer.NicknameCooldown } ]\n" +
            $"Overwatch active: [ { databasePlayer.OverwatchActive } ]</color>";

            if (databasePlayer.IsRestricted())
            {
                text += $"\n<color=red>User account is currently restricted</color>\nReason: [ {databasePlayer.Restricted.Values.Last()} ]\nExpire: [ {databasePlayer.Restricted.Keys.Last()} ]";
            }

            if (databasePlayer.RoundBanLeft >= 1)
            {
                text += $"\n<color=red>User account is currently SCP-Banned:</color>\nRound(s) left: [ {databasePlayer.RoundBanLeft} ]";
            }
            response = text;

            return true;
        }
    }
}
