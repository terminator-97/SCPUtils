using EXILED;
using EXILED.Extensions;
using MEC;
using RemoteAdmin;
using System;



namespace SCPUtils
{
    public class EventHandlers
    {
        private readonly Utils pluginInstance;
        private readonly Functions functionsInstance;

        public DateTime lastTeslaEvent;



        public EventHandlers(Functions functionsInstance, Utils pluginInstance)
        {
            this.functionsInstance = functionsInstance;
            this.pluginInstance = pluginInstance;

            Log.Info("TEST OK");
        }



        public void OnRoundStart()
        {
            Utils.IsStarted = true;
            if (pluginInstance.enableRoundRestartCheck) functionsInstance.StartFixer();
        }

        public void OnRoundEnd()
        {
            SCPUtils.Utils.IsStarted = false;
            Timing.KillCoroutines(functionsInstance.DT);


        }

        public void OnRoundRestart()
        {
            SCPUtils.Utils.IsStarted = false;
            Timing.KillCoroutines(functionsInstance.DT);
        }



        public void OnPlayerJoin(PlayerJoinEvent ev)
        {

            if (!pluginInstance.Database.GetCollection<Player>().Exists(player => player.Id == DatabasePlayer.GetRawUserId(ev.Player)))
            {
                Log.Info(ev.Player.GetNickname() + " is not present on DB!");
                pluginInstance.AddPlayer(ev.Player);
            }

            var databasePlayer = pluginInstance.Database.GetCollection<Player>().FindOne(player => player.Id == DatabasePlayer.GetRawUserId(ev.Player));
            pluginInstance.PlayerData.Add(ev.Player, databasePlayer);

            if (pluginInstance.welcomeEnabled) ev.Player.Broadcast(pluginInstance.welcomeMessageDuration, pluginInstance.welcomeMessage);

        }

        public void OnDecontaminate(ref DecontaminationEvent ev)
        {
            if (pluginInstance.decontaminationMessageEnabled) QueryProcessor.Localplayer.GetComponent<Broadcast>().RpcAddElement(pluginInstance.decontaminationMessage, pluginInstance.decontaminationMessageDuration, false);
        }

        public void OnPlayerDeath(ref PlayerDeathEvent ev)
        {
            if (ev.Player.GetTeam() == Team.SCP && Utils.IsStarted && pluginInstance.enableSCPSuicideAutoWarn)
            {

                if ((DateTime.Now - lastTeslaEvent).Seconds >= pluginInstance.SCP079TeslaEventWait)
                {
                    if (ev.Info.GetDamageType() == DamageTypes.Tesla || ev.Info.GetDamageType() == DamageTypes.Wall) functionsInstance.OnQuitOrSuicide(ev.Player);

                }
            }
        }

        public void On079Tesla(ref Scp079TriggerTeslaEvent ev)
        {
            lastTeslaEvent = DateTime.Now;
        }

        public void WaitForPlayers()
        {
            Log.Info("DON FRANCO TI UCCIDE!");
            Log.Info(pluginInstance.welcomeEnabled.ToString());
            functionsInstance.Test();
        }

        public void OnPlayerLeave(PlayerLeaveEvent ev)
        {
            if (ev.Player.GetNickname() != "Dedicated Server")
            {
                if (ev.Player.GetTeam() == Team.SCP && pluginInstance.quitEqualsSuicide && Utils.IsStarted)
                {
                    if (pluginInstance.enableSCPSuicideAutoWarn && pluginInstance.quitEqualsSuicide) functionsInstance.OnQuitOrSuicide(ev.Player);
                }
                pluginInstance.Database.GetCollection<Player>().Update(pluginInstance.PlayerData[ev.Player]);
                pluginInstance.PlayerData.Remove(ev.Player);


            }
        }

        internal void OnPlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.GetTeam() == Team.SCP) pluginInstance.PlayerData[ev.Player].TotalScpGamesPlayed++;
        }
    }

}
