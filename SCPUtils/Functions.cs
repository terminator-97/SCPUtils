using EXILED;
using MEC;
using RemoteAdmin;
using EXILED.Extensions;
using System.Collections.Generic;
using System;


namespace SCPUtils
{
    public class Functions
    {
        public CoroutineHandle DT;
        public int i = 0;
        private readonly SCPUtils pluginInstance;

        public Functions(SCPUtils pluginInstance) => this.pluginInstance = pluginInstance;

        public void StartFixer()
        {
            DT = Timing.RunCoroutine(SCPFixer(5));
        }

        public IEnumerator<float> SCPFixer(int WT)
        {
            yield return Timing.WaitForSeconds(WT);

            while (SCPUtils.IsStarted)
            {
                yield return Timing.WaitForSeconds(WT);

                if (PlayerManager.players.Count <= 1)
                {
                    if (RoundSummary.RoundLock == false)
                    {
                        Map.Broadcast(string.Format(pluginInstance.autoRestartMessage, pluginInstance.autoRestartTime), pluginInstance.autoRestartTime, false);

                        yield return Timing.WaitForSeconds(pluginInstance.autoRestartTime);

                        if (RoundSummary.RoundLock == false)
                        {
                            QueryProcessor.Localplayer.GetComponent<PlayerStats>().Roundrestart();
                            Log.Info("<SCPUtils> Round restarted due to lack of players");
                            SCPUtils.IsStarted = false;
                            yield break;
                        }
                        else Log.Warn("Auto-Restart aborted!");
                    }
                }
                if (SCPUtils.IsStarted == false) { ServerConsole.AddLog("Killing SCPFix"); yield break; }
            }
        }

        public void AutoBanPlayer(ReferenceHub player)
        {
            int duration;
            player.GetDatabasePlayer().TotalScpSuicideBans++;
            if (pluginInstance.multiplyBanDurationEachBan == true) duration = player.GetDatabasePlayer().TotalScpSuicideBans * pluginInstance.autoBanDuration;
            else duration = pluginInstance.autoBanDuration;
            player.BanPlayer(duration, $"Auto-Ban: {string.Format(pluginInstance.autoBanMessage, duration)}", "SCPUtils");
        }

        public void AutoKickPlayer(ReferenceHub player)
        {
            player.GetDatabasePlayer().TotalScpSuicideKicks++;
            player.KickPlayer($"Auto-Kick: {pluginInstance.suicideKickMessage}", "SCPUtils");
        }

        public void AutoWarnPlayer(ReferenceHub player)
        {
            player.GetDatabasePlayer().ScpSuicideCount++;
            player.ClearBroadcasts();
            player.Broadcast(pluginInstance.autoWarnMessageDuration, pluginInstance.suicideWarnMessage, false);

        }

        public void OnQuitOrSuicide(ReferenceHub player)
        {
            var suicidePercentage = player.GetDatabasePlayer().SuicidePercentage;
            AutoWarnPlayer(player);
            if (pluginInstance.enableSCPSuicideAutoBan && suicidePercentage >= pluginInstance.autoBanThreshold && player.GetDatabasePlayer().ScpSuicideCount > pluginInstance.scpSuicideTollerance) AutoBanPlayer(player);
            else if (pluginInstance.autoKickOnSCPSuicide && suicidePercentage >= pluginInstance.autoKickThreshold && suicidePercentage < pluginInstance.autoBanThreshold && player.GetDatabasePlayer().ScpSuicideCount > pluginInstance.scpSuicideTollerance) AutoKickPlayer(player);
        }

        public void PostLoadPlayer(ReferenceHub player)
        {
            var databasePlayer = player.GetDatabasePlayer();


            if (!string.IsNullOrEmpty(databasePlayer.BadgeName))
            {
                Timing.CallDelayed(1f, () =>
                {
                    if (databasePlayer.BadgeExpire >= DateTime.Now)
                    {
                        EXILED.Extensions.Player.SetRank(player, ServerStatic.GetPermissionsHandler()._groups[databasePlayer.BadgeName]);
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
                Timing.CallDelayed(1.25f, () =>
                {
                    player.SetRankColor(databasePlayer.ColorPreference);
                });
            }

            if (databasePlayer.HideBadge == true)
            {
                Timing.CallDelayed(1.5f, () =>
                {
                    player.characterClassManager.CallCmdRequestHideTag();
                });
            }
        }

    }
}




