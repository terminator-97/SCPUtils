namespace SCPUtils.Commands.RemoteAdmin.Announce
{
    using CommandSystem;
    using MongoDB.Driver;
    using System;
    using System.Linq;
    using System.Text;

    public class ListAnnoucementCommand : ICommand
    {
        public string Command { get; } = "list";
        public string[] Aliases { get; } = new[] { "l" };
        public string Description { get; } = "List of all registred broadcast";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils announce list"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils announce list"]}");
                return false;
            }
            StringBuilder broadcastList = new StringBuilder(ScpUtils.StaticInstance.commandTranslation.List);
            broadcastList.AppendLine();
            foreach (BroadcastDb databaseBroadcast in Database.MongoDatabase.GetCollection<BroadcastDb>("broadcasts").AsQueryable().ToList())
            {
                broadcastList.AppendLine(ScpUtils.StaticInstance.commandTranslation.ListId.Replace("%id%", databaseBroadcast.Id));
                broadcastList.AppendLine(ScpUtils.StaticInstance.commandTranslation.ListAuthor.Replace("%author%", databaseBroadcast.CreatedBy));
                broadcastList.AppendLine(ScpUtils.StaticInstance.commandTranslation.ListMessage.Replace("%text%", databaseBroadcast.Text));
                broadcastList.AppendLine(ScpUtils.StaticInstance.commandTranslation.ListSeconds.Replace("%duration%", databaseBroadcast.Seconds.ToString()));
                broadcastList.AppendLine(ScpUtils.StaticInstance.commandTranslation.ListSeparator);
            }
            response = $"{broadcastList}";
            return true;
        }
    }
}
