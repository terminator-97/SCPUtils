using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class ShowWarn : ICommand
    {

        public string Command { get; } = "scputils_player_last_warning";

        public string[] Aliases { get; } = new[] { "lwarn", "su_lwarn", "su_last_warn", "su_lastw", "scpu_lwarn", "scpu_last_warn", "scpu_lastw" };

        public string Description { get; } = "Show last SCPUtils warning of a specific player!";

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
            StringBuilder message = new StringBuilder($"[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]").AppendLine().AppendLine();

            message.AppendLine($"ID: {databasePlayer.SuicideDate.Count - 1}");
            message.AppendLine($"Date: {databasePlayer.SuicideDate[databasePlayer.SuicideDate.Count - 1]}");
            message.AppendLine($"Death type: {databasePlayer.SuicideType[databasePlayer.SuicideType.Count - 1]}");
            message.AppendLine($"Class: {databasePlayer.SuicideScp[databasePlayer.SuicideScp.Count - 1]}");
            message.AppendLine($"Punishment: {databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count - 1]}");
            message.AppendLine($"Staffer: {databasePlayer.LogStaffer[databasePlayer.LogStaffer.Count - 1]}");
            if(databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count - 1] == "Ban") message.AppendLine($"Expire: {databasePlayer.Expire[databasePlayer.Expire.Count - 1]}");
            message.AppendLine($"Expire: {databasePlayer.Expire[databasePlayer.Expire.Count - 1]}");
            message.AppendLine($"User Notified: {databasePlayer.UserNotified[databasePlayer.UserNotified.Count - 1]}");
            message.AppendLine();


            response = $"{message}";

            return true;
        }
    }
}
