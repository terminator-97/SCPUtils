using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using Log = Exiled.API.Features.Log;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Unwarn : ICommand
    {

        public string Command { get; } = "scputils_player_unwarn";

        public string[] Aliases { get; } = new[] { "unwarn", "sunwarn", "su_player_unw", "su_punw", "su_puw", "scpu_player_unw", "scpu_punw", "scpu_puw" };

        public string Description { get; } = "Removes a specific warning from a player!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            if (!sender.CheckPermission("scputils.unwarn"))
            {
                target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).ToString().Split(new string[] { " " }, StringSplitOptions.None)[2];
            }
            else
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>Usage: {Command} <player name/id> <WarnID></color>";
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
            bool success = int.TryParse(arguments.Array[2], out int id);
            if (!success)
            {
                response = $"Parameter is not an integer!";
                return false;
            }

            if ((databasePlayer.SuicideScp.Count - 1) < id || id < 0)
            {
                response = $"Warning with index {id} not found!";
                return true;
            }
            string message = "Success!";
            switch (databasePlayer.SuicidePunishment[id])
            {
                case "Warn":
                    databasePlayer.ScpSuicideCount--;
                    databasePlayer.SuicidePunishment[id] = "REMOVED";
                    databasePlayer.LogStaffer[id] = sender.LogName;
                    databasePlayer.UserNotified[id] = true;
                    Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                    break;
                case "Kick":
                    databasePlayer.ScpSuicideCount--;
                    databasePlayer.TotalScpSuicideKicks--;
                    databasePlayer.SuicidePunishment[id] = "REMOVED";
                    databasePlayer.LogStaffer[id] = sender.LogName;
                    databasePlayer.UserNotified[id] = true;
                    Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                    break;
                case "Ban":
                    databasePlayer.ScpSuicideCount--;
                    databasePlayer.TotalScpSuicideBans--;
                    databasePlayer.SuicidePunishment[id] = "REMOVED";
                    databasePlayer.LogStaffer[id] = sender.LogName;
                    databasePlayer.UserNotified[id] = true;
                    Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                    if (DateTime.Now < databasePlayer.Expire[id])
                    {
                        BanHandler.RemoveBan($"{databasePlayer.Id}@{databasePlayer.Authentication}", BanHandler.BanType.UserId);
                        BanHandler.RemoveBan(databasePlayer.Ip, BanHandler.BanType.IP);
                    }               
                    break;
                case "REMOVED":
                    message = "This sanction has already been removed!";
                    break;
                default:
                    break;
            }


            response = $"{message}";

            return true;
        }
    }
}
