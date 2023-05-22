namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using System;

    public class BroadcastPlayerCommand : ICommand
    {
        public string Command { get; } = "broadcast";
        public string[] Aliases { get; } = new[] { "b" };
        public string Description { get; } = "Allows to send custom broadcast.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils player broadcast"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils player broadcast"]}");
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player} {ScpUtils.StaticInstance.commandTranslation.AnnounceType} {ScpUtils.StaticInstance.commandTranslation.Announce} {ScpUtils.StaticInstance.commandTranslation.Seconds}");
                return false;
            }
            else
            {
                var databaseBroadcast = GetBroadcast.FindBroadcast(arguments.Array[5]);

                if (databaseBroadcast == null)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.IdNotExist;
                    return false;
                }

                PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(arguments.Array[3].ToString());
                if (player == null)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                    return false;
                }

                int duration = databaseBroadcast.Seconds;
                if (arguments.Count == 4)
                {
                    if (int.TryParse(arguments.Array[6].ToString(), out duration)) { }
                    else
                    {
                        response = ScpUtils.StaticInstance.commandTranslation.Integer.Replace("%argument%", arguments.Array[6]);
                        return false;
                    }
                }

                switch (arguments.Array[4].ToString().ToLower())
                {
                    case "broadcast":
                    case "bc":
                    case "b":
                        player.SendBroadcast(databaseBroadcast.Text, (ushort)duration);
                        response = ScpUtils.StaticInstance.commandTranslation.Sending.Replace("%type%", ScpUtils.StaticInstance.commandTranslation.Broadcast);
                        break;
                    case "hint":
                    case "h":
                        player.ReceiveHint(databaseBroadcast.Text, duration);
                        response = ScpUtils.StaticInstance.commandTranslation.Sending.Replace("%type%", ScpUtils.StaticInstance.commandTranslation.Hint);
                        break;
                    case "console":
                    case "c":
                        player.SendConsoleMessage(databaseBroadcast.Text);
                        response = ScpUtils.StaticInstance.commandTranslation.Sending.Replace("%type%", ScpUtils.StaticInstance.commandTranslation.Console);
                        break;
                    default:
                        response = ScpUtils.StaticInstance.commandTranslation.InvalidType;
                        break;
                }
            }

            return true;
        }
    }
}
