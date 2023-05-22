namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using System;
    using MongoDB.Driver;

    public class DeletePlayerCommand : ICommand
    {
        public string Command { get; } = "delete";
        public string[] Aliases { get; } = new[]
        {
            "remove", "cancel", "c", "d"
        };
        public string Description { get; } = "Delete a player (and all the player data) from the database. THIS ACTION IS IRREVERSIBLE";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils player delete"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils player delete"]}");
                return false;
            }
            else if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player}");
                return false;
            }
            else
            {
                string target = arguments.Array[3].ToString();

                SCPUtils.Player databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                    return false;
                }

                databasePlayer.Reset();
                Database.MongoDatabase.GetCollection<SCPUtils.Player>("players").DeleteOne(player => player.Id == databasePlayer.Id);
                response = ScpUtils.StaticInstance.commandTranslation.DeleteResponse.Replace("%player%", arguments.Array[3]);

                return true;
            }
        }
    }
}
