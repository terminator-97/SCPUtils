using System;
using CommandSystem;


namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class AsnWhitelist : ICommand
    {


        public string Command { get; } = "scputils_whitelist_asn";

        public string[] Aliases { get; } = new[] { "asnw" };

        public string Description { get; } = "Whitelist a player to make him access the server even if ASN is blacklisted!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.whitelist") || !((CommandSender)sender).FullPermissions)
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
                var target = arguments.Array[1];
                var databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                    return false;
                }

                databasePlayer.ASNWhitelisted = true;
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = "Player has been whitelisted!";
                return true;
            }
        }
    }
}
