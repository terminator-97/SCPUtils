using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    class PlayerInfo : ICommand
    {

        public string Command { get; } = "scputils_player_info";

        public string[] Aliases { get; } = new[] { "upi", "scputils_my_info" };

        public string Description { get; } = "Show player info, in case you are not admin you can see only your info";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            if (!sender.CheckPermission("scputils.playerinfo")) target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).ToString().Split(new string[] { " " }, StringSplitOptions.None)[2];
            else
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <player name/id></color>";
                    return false;
                }
                else target = arguments.Array[1].ToString();
            }
            var databasePlayer = target.GetDatabasePlayer();

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
            $"Previous Badge: [ {databasePlayer.PreviousBadge} ]\n" +
            $"Hide Badge: [ {databasePlayer.HideBadge} ]\n" +
            $"Asn Whitelisted: [ {databasePlayer.ASNWhitelisted} ]\n" +
            $"Keep Flag: [ {databasePlayer.KeepPreferences} ]\n" +
            $"Total Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString() } ]</color>";

            if (databasePlayer.IsRestricted()) text += $"\n<color=red>User account is currently restricted</color>\nReason: [ {databasePlayer.Restricted.Values.Last()} ]\nExpire: [ {databasePlayer.Restricted.Keys.Last()} ]";

            response = text;

            return true;
        }
    }
}
