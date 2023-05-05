using CommandSystem;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class DeleteBroadcast : ICommand
    {
        public string Command { get; } = "scputils_broadcast_delete";

        public string[] Aliases { get; } = new[] { "dbc", "su_dbc", "scpu_dbc" };

        public string Description { get; } = "Allows to delete custom broadcastes";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.broadcastdelete"))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }

            else if (arguments.Count < 1)
            {
                response = $"<color=yellow>Usage: {Command} <id>";
                return false;
            }
            else
            {

                if (Database.LiteDatabase.GetCollection<BroadcastDb>().Exists(broadcast => broadcast.Id == arguments.Array[1].ToString()))
                {
                    Database.LiteDatabase.GetCollection<BroadcastDb>().Delete(arguments.Array[1].ToString());
                    response = "Success!";
                    return true;
                }
                else
                {
                    response = "Id does not exist!";
                    return false;
                }
            }
        }
    }
}
