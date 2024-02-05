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

        public string Command { get; } = ScpUtils.StaticInstance.Translation.ShowwarnCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.ShowwarnAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.ShowwarnDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            if (!sender.CheckPermission("scputils.showwarns"))
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
            if (databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count - 1] == "Ban") message.AppendLine($"Expire: {databasePlayer.Expire[databasePlayer.Expire.Count - 1]}");
            if (databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count - 1] == "Round-Ban") message.AppendLine($"Round(s) ban: {databasePlayer.RoundsBan[databasePlayer.RoundsBan.Count - 1]}");
            message.AppendLine($"Expire: {databasePlayer.Expire[databasePlayer.Expire.Count - 1]}");
            message.AppendLine($"User Notified: {databasePlayer.UserNotified[databasePlayer.UserNotified.Count - 1]}");
            message.AppendLine();


            response = $"{message}";

            return true;
        }
    }
}
