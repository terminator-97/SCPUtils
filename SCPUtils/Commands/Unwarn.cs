using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class Unwarn : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Config.UnwarnCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Config.UnwarnCommandAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.UnwarnDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            if (!sender.CheckPermission("scputils.unwarn"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            else
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} {ScpUtils.StaticInstance.Translation.ArgId}</color>";
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
            bool success = int.TryParse(arguments.Array[2], out int id);
            if (!success)
            {
                response = ScpUtils.StaticInstance.Translation.InvalidArgInt;
                return false;
            }

            if ((databasePlayer.SuicideScp.Count - 1) < id || id < 0)
            {
                response = ScpUtils.StaticInstance.Translation.InvalidData;
                return true;
            }
            string message = ScpUtils.StaticInstance.Translation.Success;
            switch (databasePlayer.SuicidePunishment[id])
            {
                case "Warn":
                    databasePlayer.ScpSuicideCount--;
                    databasePlayer.SuicidePunishment[id] = "REMOVED";
                    databasePlayer.LogStaffer[id] = sender.LogName;
                    databasePlayer.UserNotified[id] = true;
                    databasePlayer.SaveData();
                    break;
                case "Kick":
                    databasePlayer.ScpSuicideCount--;
                    databasePlayer.TotalScpSuicideKicks--;
                    databasePlayer.SuicidePunishment[id] = "REMOVED";
                    databasePlayer.LogStaffer[id] = sender.LogName;
                    databasePlayer.UserNotified[id] = true;
                    databasePlayer.SaveData();
                    break;
                case "Ban":
                    databasePlayer.ScpSuicideCount--;
                    databasePlayer.TotalScpSuicideBans--;
                    databasePlayer.SuicidePunishment[id] = "REMOVED";
                    databasePlayer.LogStaffer[id] = sender.LogName;
                    databasePlayer.UserNotified[id] = true;
                    databasePlayer.SaveData();
                    if (DateTime.Now < databasePlayer.Expire[id])
                    {
                        BanHandler.RemoveBan($"{databasePlayer.Id}@{databasePlayer.Authentication}", BanHandler.BanType.UserId);
                        BanHandler.RemoveBan(databasePlayer.Ip, BanHandler.BanType.IP);
                    }
                    break;
                case "Round-Ban":
                    databasePlayer.ScpSuicideCount--;
                    databasePlayer.TotalScpSuicideBans--;
                    databasePlayer.SuicidePunishment[id] = "REMOVED";
                    databasePlayer.LogStaffer[id] = sender.LogName;
                    databasePlayer.UserNotified[id] = true;

                    databasePlayer.RoundBanLeft -= databasePlayer.RoundsBan[id];
                    if (databasePlayer.RoundBanLeft < 0) databasePlayer.RoundBanLeft = 0;


                    databasePlayer.SaveData();

                    break;
                case "REMOVED":
                    message = ScpUtils.StaticInstance.Translation.Success;
                    break;
                default:
                    break;
            }


            response = $"{message}";

            return true;
        }
    }
}
