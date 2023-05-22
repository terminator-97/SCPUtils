namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using MongoDB.Driver;
    using System;
    using System.Text;

    public class ListPlayerCommand : ICommand
    {
        public string Command { get; } = "list";
        public string[] Aliases { get; } = new[]
        {
            "l"
        };
        public string Description { get; } = "Show player list in scputils database with some basic informations, don't use values like 0 otherwise the list may get huge.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils player list"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils player list"]}");
                return false;
            }
            else
            {
                if (arguments.Count != 1)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", ScpUtils.StaticInstance.commandTranslation.MinimQuitSuicide);
                    return false;
                }
            }
            StringBuilder playerListString = new StringBuilder(ScpUtils.StaticInstance.commandTranslation.ListQuitSuicide.Replace("%percentage%", arguments.Array[3]));
            playerListString.AppendLine();
            if (int.TryParse(arguments.Array[3].ToString(), out int minpercentage))
            {
                foreach (SCPUtils.Player databasePlayer in Database.MongoDatabase.GetCollection<SCPUtils.Player>("players").AsQueryable().ToList().FindAll(x => x.SuicidePercentage >= minpercentage))
                {
                    playerListString.AppendLine();
                    playerListString.Append($"{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication}) -[ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]");
                }
                response = $"{playerListString}";
                return true;
            }
            else
            {
                response = ScpUtils.StaticInstance.commandTranslation.Integer.Replace("%argument%", arguments.Array[3]);
                return false;
            }
        }
    }
}
