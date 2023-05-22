namespace SCPUtils.Commands.RemoteAdmin.List
{
    using CommandSystem;
    using PlayerRoles.FirstPersonControl;
    using System;
    using System.Text;
    using VoiceChat;

    public class PlayerListCommand : ICommand
    {
        public string Command { get; } = "player";
        public string[] Aliases { get; } = new[] { "p", "online", "o" };
        public string Description { get; } = "Show online player list";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils list player"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils list player"]}");
                return false;
            }
            StringBuilder message = new StringBuilder(ScpUtils.StaticInstance.commandTranslation.PlayersOnline.Replace("%count%", PluginAPI.Core.Player.Count.ToString()));

            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                message.AppendLine();
                message.Append(ScpUtils.StaticInstance.commandTranslation.PlayerList.Replace("%id%", player.PlayerId.ToString()).Replace("%player%", $"{player.DisplayNickname} [{player.UserId}] [{player.Role}]"));

                if (player.ReferenceHub.serverRoles.MyText != null)
                {
                    message.Append($" [<color={player.RoleColor}>{player.ReferenceHub.serverRoles.MyText}</color>]");
                }

                if (player.IsAlive)
                {
                    message.Append(ScpUtils.StaticInstance.commandTranslation.Hp.Replace("%hp%", $"{player.Health}"));
                    //if (player.ArtificialHealth >= 1) message.Append(ScpUtils.StaticInstance.commandTranslation.Ahp.Replace("%ahp%", $"{player.ArtificialHealth}"));
                }

                if (player.IsOverwatchEnabled)
                {
                    message.Append(ScpUtils.StaticInstance.commandTranslation.Overwatch);
                }

                if (FpcNoclip.IsPermitted(player.ReferenceHub))
                {
                    message.Append(ScpUtils.StaticInstance.commandTranslation.Noclip);
                }

                if (player.IsGodModeEnabled)
                {
                    message.Append(ScpUtils.StaticInstance.commandTranslation.God);
                }

                if (player.IsBypassEnabled)
                {
                    message.Append(ScpUtils.StaticInstance.commandTranslation.Bypass);
                }

                if (player.IsIntercomMuted)
                {
                    message.Append(ScpUtils.StaticInstance.commandTranslation.Intercom);
                }

                if (player.IsMuted)
                {
                    message.Append(ScpUtils.StaticInstance.commandTranslation.VoiceChat);
                }
                
                if (VoiceChatMutes.GetFlags(player.ReferenceHub) == VcMuteFlags.GlobalRegular)
                {    
                    message.Append(ScpUtils.StaticInstance.commandTranslation.GlobalVoiceChat);
                }

                if (player.DoNotTrack)
                {
                    message.Append(ScpUtils.StaticInstance.commandTranslation.Dnt);
                }

                if (player.RemoteAdminAccess)
                {
                    message.Append(ScpUtils.StaticInstance.commandTranslation.Ra);
                }
            }
            if (PluginAPI.Core.Player.Count == 0)
            {
                response = ScpUtils.StaticInstance.commandTranslation.NoPlayers;
                return true;
            }
            response = message.ToString();
            return true;
        }

    }
}
