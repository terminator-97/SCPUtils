namespace SCPUtils
{
    using MEC;
    using PluginAPI.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FunctionEnums = Enums.FunctionsType;
    using MessageType = Enums.FunctionsMessage;

    public class Function : EventArgs
    {
        public CoroutineHandle RS;

        public int i = 0;

        private readonly ScpUtils pluginInstance;

        public Function(ScpUtils pluginInstance) => this.pluginInstance = pluginInstance;

        public void CoroutineRestart()
        {
            TimeSpan timeParts = TimeSpan.Parse(pluginInstance.Configs.AutoRestartTimeTask);

            double timeCalc;
            timeCalc = (timeParts - DateTime.Now.TimeOfDay).TotalSeconds;

            if (timeCalc <= 0)
                timeCalc += 86400;

            RS = Timing.RunCoroutine(Restarter((float)timeCalc), Segment.FixedUpdate);
        }

        private IEnumerator<float> Restarter(float second)
        {
            yield return
                Timing.WaitForSeconds(second);

            /*Log.Info(pluginInstance.Configs.FunctionMessage[FunctionEnums.Restarter]);

            if (pluginInstance.Configs.FunctionBroadcasts[FunctionEnums.Restarter].Show != false)
            {
                foreach (PluginAPI.Core.Player players in PluginAPI.Core.Player.GetPlayers())
                {
                    var broadcast = pluginInstance.Configs.FunctionBroadcasts[FunctionEnums.Restarter];
                    players.SendBroadcast(broadcast.Content, broadcast.Duration, broadcast.Type, broadcast.ClearAll);
                }
            }*/

            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())

            switch (pluginInstance.functions.FunctionMessageType[FunctionEnums.Restarter])
            {
                case MessageType.PlayerConsole: player.SendConsoleMessage(pluginInstance.functions.FunctionMessage[FunctionEnums.Restarter]); break;
                case MessageType.ServerConsole: Log.Info(pluginInstance.functions.FunctionMessage[FunctionEnums.Restarter]); break;
                case MessageType.AdminBroadcast: player.SendBroadcast(pluginInstance.functions.FunctionMessage[FunctionEnums.Restarter], 15, Broadcast.BroadcastFlags.AdminChat); break;
                case MessageType.ServerBroadcast: player.SendBroadcast(pluginInstance.functions.FunctionMessage[FunctionEnums.Restarter], 15, Broadcast.BroadcastFlags.Normal); break;
                case MessageType.AdminHint: player.SendAdminHint("SCPUtils", pluginInstance.functions.FunctionMessage[FunctionEnums.Restarter], 15); break;
                case MessageType.ServerHint: player.ReceiveHint(pluginInstance.functions.FunctionMessage[FunctionEnums.Restarter], 15); break;
            }

            if (PluginAPI.Core.Player.Count > 0)
                Server.RunCommand("/rnr");
            else
                Server.Restart();
        }

        public Dictionary<string, DateTime> LastWarn { get; private set; } = new();

        /*public void AutoRoundBanPlayer(PluginAPI.Core.Player player)
        {
            int rounds;

            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideBans++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Round-Ban";

            if (pluginInstance.Configs.MultiplyBanDurationEachBan == true)
                rounds = databasePlayer.TotalScpSuicideBans * pluginInstance.Configs.AutoBanRoundsCount;
            else
                rounds = pluginInstance.Configs.AutoBanDuration;

            if (pluginInstance.Configs.FunctionType[FunctionEnums.Sanctions] && databasePlayer.RoundBanLeft == 0)
                player.SendSanctionMessage(pluginInstance.Configs.FunctionMessage[FunctionEnums.ScpBanned].Replace("$playerNickname$", player.Nickname).Replace("$playerRole$", player.RoleName).Replace("$rounds$", rounds.ToString()));

            if (pluginInstance.Configs.FunctionType[FunctionEnums.Sanctions] && databasePlayer.RoundBanLeft >= 1)
                player.SendSanctionMessage(pluginInstance.Configs.FunctionMessage[FunctionEnums.IssuesBan].Replace("$playerNickname$", player.Nickname));

            databasePlayer.RoundsBan[databasePlayer.RoundsBan.Count() - 1] = rounds;
            databasePlayer.RoundBanLeft += rounds;

            if (pluginInstance.Translation.RoundBanNotification.Show)
            {
                var message = pluginInstance.Translation.RoundBanNotification.Content;
                message = message.Replace("%roundnumber%", databasePlayer.RoundBanLeft.ToString());

                var broadcast = pluginInstance.GetMotd.Motd;

                player.SendBroadcast(message, broadcast.Duration, broadcast.Type, broadcast.ClearAll);
            }
        }
        */
        public void AutoBanPlayer(PluginAPI.Core.Player player)
        {
            int duration;

            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideBans++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Ban";

            if (pluginInstance.Configs.MultiplyBanDurationEachBan == true)
                duration = databasePlayer.TotalScpSuicideBans * pluginInstance.Configs.AutoBanDuration * 60;
            else
                duration = pluginInstance.Configs.AutoBanDuration * 60;

            if (pluginInstance.Configs.BroadcastSanctions)
                player.SendSanctionMessage(pluginInstance.Translation.AutoBanPlayerMessage.Content.Replace("%player.Nickname%", player.Nickname).Replace("%player.Role%", player.Role.ToString()).Replace("%duration%", (duration / 60).ToString()));

            if (pluginInstance.Configs.MultiplyBanDurationEachBan == true)
                databasePlayer.Expire[databasePlayer.Expire.Count() - 1] = DateTime.Now.AddMinutes(duration / 60 * databasePlayer.TotalScpSuicideBans);
            else
                databasePlayer.Expire[databasePlayer.Expire.Count() - 1] = DateTime.Now.AddMinutes(duration / 60);

            player.Ban($"Auto-Ban: {string.Format(pluginInstance.Translation.AutoBanMessage)}", duration);
        }

        public void AutoKickPlayer(PluginAPI.Core.Player player)
        {
            if (pluginInstance.Configs.BroadcastSanctions)
                player.SendSanctionMessage($"<color=blue><SCPUtils> {player.Nickname} ({player.Role}) has been <color=red>KICKED</color> from the server for exceeding Quits / Suicides (as SCP) limit</color>");

            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideKicks++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Kick";

            player.Kick($"Auto-Kick: {pluginInstance.Translation.SuicideKickMessage}");
        }

        
    }
}
