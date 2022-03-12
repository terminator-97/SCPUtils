using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class ShowWarns : ICommand
    {

        public string Command { get; } = "scputils_player_warnings";

        public string[] Aliases { get; } = new[] { "warns", "swarns", "su_warns", "scpu_warns" };

        public string Description { get; } = "Show all SCPUtils warnings of a specific player!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            if (!sender.CheckPermission("scputils.showwarns"))
            {
                target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).ToString().Split(new string[] { " " }, StringSplitOptions.None)[2];
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

            if (databasePlayer.SuicideDate.Count == 0)
            {
                response = "Player has no warnings";
                return true;
            }
            ScpUtils.StaticInstance.Functions.FixBanTime(databasePlayer);
            int currentindex = 0;
            StringBuilder message = new StringBuilder($"[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]").AppendLine().AppendLine();
            foreach (DateTime a in databasePlayer.SuicideDate)
            {
                message.AppendLine($"ID: {currentindex}");
                message.AppendLine($"Date: {databasePlayer.SuicideDate[currentindex]}");
                message.AppendLine($"Death type: {databasePlayer.SuicideType[currentindex]}");
                message.AppendLine($"Class: {databasePlayer.SuicideScp[currentindex]}");
                message.AppendLine($"Punishment: {databasePlayer.SuicidePunishment[currentindex]}");
                message.AppendLine($"Staffer: {databasePlayer.LogStaffer[currentindex]}");
                if (databasePlayer.SuicidePunishment[currentindex] == "Ban")
                {
                    if (currentindex <= databasePlayer.Expire.Count()) message.AppendLine($"Expire: {databasePlayer.Expire[currentindex]}");
                    else message.AppendLine($"Expire: Unknown");
                }
                if (databasePlayer.SuicidePunishment[currentindex] == "Round-Ban")
                {
                    if (currentindex <= databasePlayer.RoundsBan.Count()) message.AppendLine($"Round(s) ban: {databasePlayer.RoundsBan[currentindex]}");
                    else message.AppendLine($"Round(s) ban: Unknown");
                }
                message.AppendLine($"User Notified: {databasePlayer.UserNotified[currentindex]}");
                message.AppendLine();
                currentindex++;
            }

            response = $"{message}";

            return true;
        }
    }
}
