﻿namespace SCPUtils.Commands.RemoteAdmin.List
{
    using CommandSystem;
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
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils list player"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils list player"]}");
                return false;
            }

            StringBuilder playerList = new StringBuilder(ScpUtils.StaticInstance.commandTranslation.PlayersOnline.Replace("%count%", PluginAPI.Core.Player.Count.ToString()));
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                playerList.AppendLine();
                playerList.Append(ScpUtils.StaticInstance.commandTranslation.PlayerList.Replace("%id%", player.PlayerId.ToString()).Replace("%player%", $"{player.DisplayNickname} [{player.UserId}] [{player.Role}]"));

                if (player.ReferenceHub.serverRoles.MyText != null) playerList.Append($" [<color={player.RoleColor}>{player.ReferenceHub.serverRoles.MyText}</color>]");

                if (player.IsAlive)
                {
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.Hp.Replace("%hp%", $"{player.Health}"));
                    
                    if (player.ReferenceHub.playerStats.StatModules[1].CurValue >= 1) 
                        playerList.Append(ScpUtils.StaticInstance.commandTranslation.Ahp.Replace("%ahp%", $"{player.ReferenceHub.playerStats.StatModules[1].NormalizedValue}"));

                    //if (FpcNoclip.IsPermitted(player.ReferenceHub))
                    if (player.IsNoclipEnabled)
                        playerList.Append(ScpUtils.StaticInstance.commandTranslation.Noclip);

                    if (player.IsUsingVoiceChat)
                        playerList.Append(ScpUtils.StaticInstance.commandTranslation.IsSpeaking);
                }

                if (player.IsOverwatchEnabled)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.Overwatch);

                if (player.IsGodModeEnabled)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.God);

                if (player.IsBypassEnabled)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.Bypass);

                if (player.IsIntercomMuted)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.Intercom);

                if (player.IsMuted)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.VoiceChat);
                
                if (VoiceChatMutes.GetFlags(player.ReferenceHub) == VcMuteFlags.GlobalRegular)
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.GlobalVoiceChat);

                if (player.DoNotTrack) 
                    playerList.Append(ScpUtils.StaticInstance.commandTranslation.Dnt);

                if (player.RemoteAdminAccess)
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
