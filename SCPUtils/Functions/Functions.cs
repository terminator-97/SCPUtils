using MEC;
using RemoteAdmin;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace SCPUtils
{
    public class Functions
    {
        public CoroutineHandle DT;
        public int i = 0;
        private readonly ScpUtils pluginInstance;

        public Functions(ScpUtils pluginInstance) => this.pluginInstance = pluginInstance;

        public void StartFixer()
        {
            DT = Timing.RunCoroutine(SCPFixer(5));
        }

        public IEnumerator<float> SCPFixer(int WT)
        {
            yield return Timing.WaitForSeconds(WT);

            while (ScpUtils.IsStarted)
            {
                yield return Timing.WaitForSeconds(WT);

                if (PlayerManager.players.Count <= 1)
                {
                    if (RoundSummary.RoundLock == false)
                    {
                        Map.Broadcast(pluginInstance.Config.AutoRestartTime, string.Format(pluginInstance.Config.AutoRestartMessage, pluginInstance.Config.AutoRestartTime), Broadcast.BroadcastFlags.Normal);

                        yield return Timing.WaitForSeconds(pluginInstance.Config.AutoRestartTime);

                        if (RoundSummary.RoundLock == false)
                        {
                            QueryProcessor.Localplayer.GetComponent<PlayerStats>().Roundrestart();
                            Log.Info("<SCPUtils> Round restarted due to lack of players");
                            ScpUtils.IsStarted = false;
                            yield break;
                        }
                        else Log.Warn("Auto-Restart aborted!");
                    }
                }
                if (ScpUtils.IsStarted == false) { ServerConsole.AddLog("Killing SCPFix"); yield break; }
            }
        }

        public void AutoBanPlayer(Exiled.API.Features.Player player)
        {
            int duration;
            player.GetDatabasePlayer().TotalScpSuicideBans++;
            if (pluginInstance.Config.MultiplyBanDurationEachBan == true) duration = player.GetDatabasePlayer().TotalScpSuicideBans * pluginInstance.Config.AutoBanDuration;
            else duration = pluginInstance.Config.AutoBanDuration;
            Map.Broadcast(12, $"<color=blue><SCPUtils> {player.Nickname} has been banned from the server for exceeding Quits / Suicides limit. Duration: {duration} mitutes</color>", Broadcast.BroadcastFlags.AdminChat);
            player.Ban(duration, $"Auto-Ban: {string.Format(pluginInstance.Config.AutoBanMessage, duration)}", "SCPUtils");
        }

        public void AutoKickPlayer(Exiled.API.Features.Player player)
        {
            Map.Broadcast(12, $"<color=blue><SCPUtils> {player.Nickname} has been kicked from the server for exceeding Quits / Suicides limit</color>", Broadcast.BroadcastFlags.AdminChat);
            player.GetDatabasePlayer().TotalScpSuicideKicks++;
            player.Kick($"Auto-Kick: {pluginInstance.Config.SuicideKickMessage}", "SCPUtils");
        }

        public void AutoWarnPlayer(Exiled.API.Features.Player player)
        {
            player.GetDatabasePlayer().ScpSuicideCount++;
            player.ClearBroadcasts();
            player.Broadcast(pluginInstance.Config.AutoWarnMessageDuration, pluginInstance.Config.SuicideWarnMessage, Broadcast.BroadcastFlags.Normal);

        }

        public void OnQuitOrSuicide(Exiled.API.Features.Player player)
        {
            var suicidePercentage = player.GetDatabasePlayer().SuicidePercentage;
            AutoWarnPlayer(player);
            if (pluginInstance.Config.EnableSCPSuicideAutoBan && suicidePercentage >= pluginInstance.Config.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Config.ScpSuicideTollerance) AutoBanPlayer(player);
            else if (pluginInstance.Config.AutoKickOnSCPSuicide && suicidePercentage >= pluginInstance.Config.AutoKickThreshold && suicidePercentage < pluginInstance.Config.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Config.ScpSuicideTollerance) AutoKickPlayer(player);
        }

        public void PostLoadPlayer(Exiled.API.Features.Player player)
        {
            var databasePlayer = player.GetDatabasePlayer();


            if (!string.IsNullOrEmpty(databasePlayer.BadgeName))
            {
                Timing.CallDelayed(1f, () =>
                {
                    if (databasePlayer.BadgeExpire >= DateTime.Now)
                    {
                        player.ReferenceHub.serverRoles.Group = ServerStatic.GetPermissionsHandler()._groups[databasePlayer.BadgeName];
                    }
                    else
                    {
                        databasePlayer.BadgeName = "";
                        databasePlayer.ResetPreferences();
                    }
                });
            }

            if (!string.IsNullOrEmpty(databasePlayer.ColorPreference) && databasePlayer.ColorPreference != "None")
            {
                Timing.CallDelayed(1.15f, () =>
                {
                    player.RankColor = databasePlayer.ColorPreference;
                });
            }

            if (databasePlayer.HideBadge == true)
            {
                Timing.CallDelayed(1.25f, () =>
                {
                    player.BadgeHidden = true;
                });
            }


        }

        public bool CheckNickname(string name)
        {
            foreach (var nickname in pluginInstance.Config.BannedNickNames)
            {
                name = Regex.Replace(name, "[^a-zA-Z0-9]", "").ToLower();
                string pattern = Regex.Replace(nickname.ToLower(), "[^a-zA-Z0-9]", "");
                if (Regex.Match(name, pattern).Success) return true;
            }
            return false;
        }

    }
}