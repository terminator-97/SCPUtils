using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class AsnUnWhitelist : ICommand
    {
        public string Command { get; } = "scputils_unwhitelist_asn";

        public string[] Aliases { get; } = new[] { "asnuw" };

        public string Description { get; } = "Un-Whitelist a player to disallow him access the server even if ASN is blacklisted!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scputils.whitelist"))
            {
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }
            else if (arguments.Count < 1)
            {
                response = $"<color=yellow>Usage: {Command} <player name/id></color>";
                return false;
            }

            else
            {
                string target = arguments.Array[1];
                Player databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                    return false;
                }

                databasePlayer.ASNWhitelisted = false;
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = "Player has been removed from whitelist!";
                return true;
            }
        }
    }
}
