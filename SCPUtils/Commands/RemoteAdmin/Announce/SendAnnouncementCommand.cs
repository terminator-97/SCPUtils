namespace SCPUtils.Commands.RemoteAdmin.Announce
{
    using CommandSystem;
    using PluginAPI.Core;
    using System;

    public class SendAnnoucementCommand : ICommand
    {
        public string Command { get; } = "send";

        public string[] Aliases { get; } = new[] { "s" };

        public string Description { get; } = "Allows to send custom broadcastes";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils announce send"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils announce send"]}");
                return false;
            }

            else if (arguments.Count < 2)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.AnnounceType} {ScpUtils.StaticInstance.commandTranslation.Announce} {ScpUtils.StaticInstance.commandTranslation.Seconds}");
                return false;
            }
            else
            {
                var databaseBroadcast = GetBroadcast.FindBroadcast(arguments.Array[4]);

                if (databaseBroadcast == null)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.IdNotExist;
                    return false;
                }
                int duration = databaseBroadcast.Seconds;
                if (arguments.Count == 3)
                {
                    if (int.TryParse(arguments.Array[5].ToString(), out duration)) { }
                    else
                    {
                        response = ScpUtils.StaticInstance.commandTranslation.Integer.Replace("%argument%", arguments.Array[5]);
                        return false;
                    }
                }

                switch (arguments.Array[3].ToString())
                {
                    case "Broadcast":
                    case "broadcast":
                    case "Bc":
                    case "bc":
                    case "B":
                    case "b":
                        foreach (Player player in Player.GetPlayers())
                        {
                            player.SendBroadcast(databaseBroadcast.Text, (ushort)duration);
                        }
                        response = ScpUtils.StaticInstance.commandTranslation.Sending.Replace("%type%", ScpUtils.StaticInstance.commandTranslation.Broadcast);
                        break;
                    case "Hint":
                    case "H":
                    case "hint":
                    case "h":
                        foreach (Player player in Player.GetPlayers())
                        {
                            player.ReceiveHint(databaseBroadcast.Text, duration);
                        }
                        response = ScpUtils.StaticInstance.commandTranslation.Sending.Replace("%type%", ScpUtils.StaticInstance.commandTranslation.Hint);
                        break;
                    case "Console":
                    case "console":
                    case "C":
                    case "c":
                        foreach (Player player in Player.GetPlayers())
                        {
                            player.SendConsoleMessage(databaseBroadcast.Text);
                        }
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
