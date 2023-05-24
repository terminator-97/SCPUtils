namespace SCPUtils.Commands.Console
{
    using CommandSystem;
    using System;
    using System.Text;

    public class PlayerListCommand : ICommand
    {
        public string Command { get; } = "player";
        public string[] Aliases { get; } = new[] { "p", "online", "o" };
        public string Description { get; } = "Show online player list";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            StringBuilder playerList = new StringBuilder(ScpUtils.StaticInstance.commandTranslation.PlayersOnline.Replace("%count%", PluginAPI.Core.Player.Count.ToString()));
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                playerList.AppendLine();
                playerList.Append(ScpUtils.StaticInstance.commandTranslation.PlayerList.Replace("%id%", player.PlayerId.ToString()).Replace("%player%", $"{player.DisplayNickname}"));

                if (player.ReferenceHub.serverRoles.MyText != null) playerList.Append($" [<color={player.RoleColor}>{player.ReferenceHub.serverRoles.MyText}</color>]");

                if (player.IsGodModeEnabled)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.God);

                if (player.IsBypassEnabled)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.Bypass);

                if (player.IsIntercomMuted)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.Intercom);

                if (player.IsMuted)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.VoiceChat);

                if (player.RemoteAdminAccess && string.IsNullOrEmpty(player.ReferenceHub.serverRoles.HiddenBadge))
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.Ra);
            }

            if (PluginAPI.Core.Player.Count == 0)
            {
                response = ScpUtils.StaticInstance.commandTranslation.NoPlayers;
                return true;
            }

            response = playerList.ToString();
            return true;
        }
    }
}
