﻿namespace SCPUtils.Commands.RemoteAdmin.Badge
{
    using CommandSystem;
    using System;

    public class RevokeBadgeCommand : ICommand
    {
        public string Command { get; } = "revoke";
        public string[] Aliases { get; } = new[] { "r", "remove" };
        public string Description { get; } = "Remove a temporarily badge that has been given to a player!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            string target;
            if (!sender.CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }

            else if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", ScpUtils.StaticInstance.commandTranslation.Player);
                return false;
            }

            else
            {
                target = arguments.Array[3].ToString();
            }

            PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(target);
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                return false;
            }

            databasePlayer.BadgeName = string.Empty;
            databasePlayer.BadgeExpire = DateTime.MinValue;
            databasePlayer.SaveData();

            player?.ReferenceHub.characterClassManager.UserCode_CmdRequestShowTag__Boolean(true);

            response = ScpUtils.StaticInstance.commandTranslation.BadgeRevoke;
            return true;
        }
    }
}