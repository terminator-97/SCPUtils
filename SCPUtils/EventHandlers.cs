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

            if (!Database.LiteDatabase.GetCollection<Player>().Exists(player => player.Id == DatabasePlayer.GetRawUserId(ev.Player)))
            {
                Log.Info(ev.Player.GetNickname() + " is not present on DB!");
                Database.AddPlayer(ev.Player);
            }

            var databasePlayer = Database.LiteDatabase.GetCollection<Player>().FindOne(player => player.Id == DatabasePlayer.GetRawUserId(ev.Player));
            Database.PlayerData.Add(ev.Player, databasePlayer);
            Database.PlayerData[ev.Player].Name = ev.Player.GetNickname();

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


        public void OnPlayerLeave(PlayerLeaveEvent ev)
        {
            if (ev.Player.GetNickname() != "Dedicated Server" && ev.Player != null && Database.PlayerData.ContainsKey(ev.Player))
            {
                if (ev.Player.GetTeam() == Team.SCP && pluginInstance.quitEqualsSuicide && Utils.IsStarted)
                {
                    if (pluginInstance.enableSCPSuicideAutoWarn && pluginInstance.quitEqualsSuicide) functionsInstance.OnQuitOrSuicide(ev.Player);
                }
                Database.LiteDatabase.GetCollection<Player>().Update(Database.PlayerData[ev.Player]);
                Database.PlayerData.Remove(ev.Player);


            }
        }

        internal void OnPlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.GetTeam() == Team.SCP) Database.PlayerData[ev.Player].TotalScpGamesPlayed++;
        }
    }

}
