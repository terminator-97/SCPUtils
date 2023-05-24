namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using System;

    public class DntPlayerCommand : ICommand
    {
        public string Command { get; } = "dnt";
        public string[] Aliases { get; }
        public string Description { get; } = "Use this command to forcefully refuse dnt requests.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            string target = arguments.Array[1];
            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils player dnt"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils player dnt"]}");
                return false;
            }
            else if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player}");
                return false;
            }
            SCPUtils.Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                return false;
            }

            if (databasePlayer.IgnoreDNT is false)
            {
                databasePlayer.IgnoreDNT = true;
                databasePlayer.SaveData();
                response = ScpUtils.StaticInstance.commandTranslation.DntResponseTrue;
                return true;
            }
            else
            {
                databasePlayer.IgnoreDNT = false;
                databasePlayer.SaveData();
                response = ScpUtils.StaticInstance.commandTranslation.DntResponseFalse;
                return true;
            }
        }
    }
}
