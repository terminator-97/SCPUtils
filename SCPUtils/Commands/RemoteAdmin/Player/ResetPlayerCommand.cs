namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using System;

    public class ResetPlayerCommand : ICommand
    {
        public string Command { get; } = "reset";
        public string[] Aliases { get; } = new[]
        {
            "r"
        };
        public string Description { get; } = "Reset player data.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils player reset"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils player reset"]}");
                return false;
            }
            else if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player} {ScpUtils.StaticInstance.commandTranslation.PlayerReset}");
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

                switch (arguments.Array[4].ToString().ToLower())
                {
                    case "preference":
                    case "pr":
                    case "p":
                        databasePlayer.ResetPreferences();
                        databasePlayer.SaveData();
                        break;
                    case "all":
                        databasePlayer.Reset();
                        databasePlayer.SaveData();
                        break;
                }
                response = ScpUtils.StaticInstance.commandTranslation.ResetResponse.Replace("%player%", arguments.Array[3]);
                return true;
            }
        }
    }
}