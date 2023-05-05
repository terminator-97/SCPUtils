using CommandSystem;
using System;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class BroadcastList : ICommand
    {
        public string Command { get; } = "scputils_broadcast_list";

        public string[] Aliases { get; } = new[] { "sbcl", "bcl", "su_bcl", "scpu_bcl" };

        public string Description { get; } = "List of all registred broadcast";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.broadcastlist"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }
            StringBuilder broadcastList = new StringBuilder("[Broadcast List]");
            broadcastList.AppendLine();
            foreach (BroadcastDb databaseBroadcast in Database.LiteDatabase.GetCollection<BroadcastDb>().FindAll())
            {
                broadcastList.AppendLine($"ID: {databaseBroadcast.Id}");
                broadcastList.AppendLine($"Created by: {databaseBroadcast.CreatedBy}");
                broadcastList.AppendLine($"Default duration: {databaseBroadcast.Seconds}");
                broadcastList.AppendLine($"Text: {databaseBroadcast.Text}");
                broadcastList.AppendLine("---------");
            }
            response = $"{broadcastList}";
            return true;
        }
    }
}
