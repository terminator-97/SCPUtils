namespace SCPUtils.Commands.RemoteAdmin.Announce
{
    using CommandSystem;
    using System;

    public class CreateAnnouncementCommand : ICommand
    {
        public string Command { get; } = "create";
        public string[] Aliases { get; } = new[] { "c", "add", "a" };
        public string Description { get; } = "Allows to create custom announcement";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils announce create"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils announce create"]}");
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Broadcast} {ScpUtils.StaticInstance.commandTranslation.Seconds} {ScpUtils.StaticInstance.commandTranslation.AnnounceMessage}");
                return false;
            }
            else
            {
                if (int.TryParse(arguments.Array[4].ToString(), out int duration))
                {
                    if (GetBroadcast.FindBroadcast(arguments.Array[3].ToString()) != null)
                    {
                        response = ScpUtils.StaticInstance.commandTranslation.IdExist;
                        return false;
                    }
                    else
                    {
                        var broadcast = string.Join(" ", arguments.Array, 5, arguments.Array.Length - 5);
                        GetBroadcast.AddBroadcast(arguments.Array[3].ToString(), duration, broadcast.ToString(), sender.LogName);
                        response = ScpUtils.StaticInstance.commandTranslation.AnnounceSuccess;
                        return true;
                    }
                }
                else
                {
                    response = ScpUtils.StaticInstance.commandTranslation.DaysInteger;
                    return false;
                }
            }
        }
    }
}
