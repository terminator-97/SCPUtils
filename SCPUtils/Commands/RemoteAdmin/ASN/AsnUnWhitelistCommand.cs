namespace SCPUtils.Commands.RemoteAdmin.ASN
{
    using CommandSystem;
    using System;

    public class AsnUnWhitelistCommand : ICommand
    {
        public string Command { get; } = "whitelist";
        public string[] Aliases { get; } = new[]
        {
            "add", "wl", "w"
        };
        public string Description { get; } = "Whitelist a player, ie GFN member, to enter in this server.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils asn unwhitelist"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils asn unwhitelist"]}");
                return false;
            }
            else if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", ScpUtils.StaticInstance.commandTranslation.Player);
                return false;
            }
            else
            {
                string target = arguments.Array[3];
                Player databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                    return false;
                }
                else if (databasePlayer.ASNWhitelisted is false)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.AsnUnwhitelistError.Replace("%player%", $"{databasePlayer.Name} {databasePlayer.Id}@{databasePlayer.Authentication}");
                    return false;
                }

                databasePlayer.ASNWhitelisted = false;
                databasePlayer.SaveData();
                response = ScpUtils.StaticInstance.commandTranslation.AsnUnwhitelist.Replace("%player%", $"{databasePlayer.Name} {databasePlayer.Id}@{databasePlayer.Authentication}");
                return true;
            }
        }
    }
}
