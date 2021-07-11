using System;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Dupeip : ICommand
    {
        public string Command { get; } = "scputils_dupeip";

        public string[] Aliases { get; } = new[] { "dupeip", "su_dupeip", "scpu_dupeip" };

        public string Description { get; } = "Check if player has another account on same IP";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scputils.dupeip"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            if (arguments.Count != 1)
            {
                response = $"<color=yellow>Usage: {Command} <player name/id></color>";
                return false;
            }
            string targetName = arguments.Array[1];
            Player databasePlayer = targetName.GetDatabasePlayer();
            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            StringBuilder message = new StringBuilder($"<color=green>[Accounts associated with the same IP ({databasePlayer.Ip} - {databasePlayer.Name} {databasePlayer.Id}@{databasePlayer.Authentication})]</color>").AppendLine();
            foreach (var ips in Database.LiteDatabase.GetCollection<Player>().FindAll().Where(ip => ip.Ip == databasePlayer.Ip).ToList())
            {
                message.AppendLine();
                message.Append(
                        $"Player: <color=yellow>{ips.Name} ({ips.Id}{ips.Authentication})</color>\nFirst Join: <color=yellow>{ips.FirstJoin}</color>\nIsRestricted: <color=yellow>{ips.IsRestricted()}</color>\nIsBanned: <color=yellow>{ips.IsBanned()}</color>\nTotal played as SCP: <color=yellow>{ips.TotalScpGamesPlayed}</color>\nTotal suicide: <color=yellow>{ips.ScpSuicideCount}</color>")
                    .AppendLine();

            }
            response = message.ToString();
            return true;
        }
    }
}