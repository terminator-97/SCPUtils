using EXILED;
using MEC;
using RemoteAdmin;
using EXILED.Extensions;
using System.Collections.Generic;

namespace SCPUtils
{
    public class Functions
    {
        public CoroutineHandle DT;
        public int i = 0;
        private readonly Utils pluginInstance;
        private readonly Commands commandsInstance;
               

        public Functions(Utils pluginInstance, Commands commandsInstance)
        {
            this.commandsInstance = commandsInstance;
            this.pluginInstance = pluginInstance;
            Log.Info("TEST OK");
        }

        public void Test()
        {
            Log.Info(pluginInstance.autoRestartTime.ToString());
        }

        public void StartFixer()
        {
            DT = Timing.RunCoroutine(SCPFixer(5), Segment.FixedUpdate);
        }


        public IEnumerator<float> SCPFixer(int WT)
        {
            yield return Timing.WaitForSeconds(WT);

            while (SCPUtils.Utils.IsStarted)
            {
                yield return Timing.WaitForSeconds(WT);

                if (PlayerManager.players.Count <= 1)
                {
                    if (RoundSummary.RoundLock == false)
                    {

                        QueryProcessor.Localplayer.GetComponent<Broadcast>().RpcAddElement(string.Format(pluginInstance.autoRestartMessage, pluginInstance.autoRestartTime), pluginInstance.autoRestartTime, false);
                        yield return Timing.WaitForSeconds(pluginInstance.autoRestartTime);

                        if (RoundSummary.RoundLock == false)
                        {
                            QueryProcessor.Localplayer.GetComponent<PlayerStats>().Roundrestart();
                            Log.Info("<SCPUtils> Round restarted due to lack of players");
                            SCPUtils.Utils.IsStarted = false;
                            Timing.KillCoroutines(DT);
                        }
                        else Log.Warn("Auto-Restart aborted!");
                    }
                }
                if (SCPUtils.Utils.IsStarted == false) { Timing.KillCoroutines(DT); ServerConsole.AddLog("Killing SCPFix"); }
            }
        }

        public void RoundStartPrepare()
        {
            foreach (var player in PlayerManager.players)
            {
                player.GetComponent<ServerRoles>().CmdSetOverwatchStatus(0);
            }
        }

        public void AutoBanPlayer(ReferenceHub player)
        {
            int duration;
            pluginInstance.PlayerData[player].TotalScpSuicideBans++;
            if (pluginInstance.multiplyBanDurationEachBan == true) duration = pluginInstance.PlayerData[player].TotalScpSuicideBans * pluginInstance.autoBanDuration;
            else duration = pluginInstance.autoBanDuration;
            player.BanPlayer(duration, $"Auto-Ban: {string.Format(pluginInstance.autoBanMessage, duration)}", "SCPUtils");
        }

        public void AutoKickPlayer(ReferenceHub player)
        {
            pluginInstance.PlayerData[player].TotalScpSuicideKicks++;
            player.KickPlayer($"Auto-Kick: {pluginInstance.suicideKickMessage}", "SCPUtils");
        }

        public void AutoWarnPlayer(ReferenceHub player)
        {
            pluginInstance.PlayerData[player].ScpSuicideCount++;
            player.ClearBroadcasts();
            player.Broadcast(pluginInstance.autoWarnMessageDuration, pluginInstance.suicideWarnMessage);
        }

        public void OnQuitOrSuicide(ReferenceHub player)
        {
            var suicidePercentage = pluginInstance.PlayerData[player].SuicidePercentage;
            AutoWarnPlayer(player);
            if (pluginInstance.autoKickOnSCPSuicide && suicidePercentage >= pluginInstance.autoKickThreshold && suicidePercentage < pluginInstance.autoBanThreshold && pluginInstance.PlayerData[player].ScpSuicideCount > pluginInstance.scpSuicideTollerance) AutoKickPlayer(player);
            else if (pluginInstance.enableSCPSuicideAutoBan && suicidePercentage >= pluginInstance.autoBanThreshold && pluginInstance.PlayerData[player].ScpSuicideCount > pluginInstance.scpSuicideTollerance) AutoBanPlayer(player);
        }





    }
}




