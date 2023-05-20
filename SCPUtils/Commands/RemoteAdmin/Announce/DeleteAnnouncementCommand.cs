namespace SCPUtils.Commands.RemoteAdmin.Announce
{
    using CommandSystem;
    using MongoDB.Driver;
    using System;

    public class DeleteAnnouncementCommand : ICommand
    {
        public string Command { get; } = "delete";
        public string[] Aliases { get; } = new[] { "d", "remove", "r" };
        public string Description { get; } = "Allows to delete custom announcement";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils broadcast delete"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils broadcast delete"]}");
                return false;
            }

            else if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Broadcast}");
                return false;
            }
            else
            {
                var databaseBroadcast = GetBroadcast.FindBroadcast(arguments.Array[3]);
                if (databaseBroadcast != null)
                {
                    Database.MongoDatabase.GetCollection<BroadcastDb>("broadcasts").DeleteOne(broadcast => broadcast.Id == databaseBroadcast.Id);
                    response = ScpUtils.StaticInstance.commandTranslation.AnnounceSuccess;
                    return true;
                }
                else
                {
                    response = ScpUtils.StaticInstance.commandTranslation.IdNotExist;
                    return false;
                }
            }
        }
    }
}
