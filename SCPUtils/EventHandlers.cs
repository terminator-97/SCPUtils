using EXILED;
using EXILED.Extensions;
using MEC;
using RemoteAdmin;
using System;


namespace SCPUtils
{
    public class EventHandlers
    {
        private readonly SCPUtils pluginInstance;

        public DateTime lastTeslaEvent;

        public EventHandlers(SCPUtils pluginInstance) => this.pluginInstance = pluginInstance;

        public void OnRoundStart()
        {
            SCPUtils.IsStarted = true;
            if (pluginInstance.enableRoundRestartCheck) pluginInstance.Functions.StartFixer();
        }

        public void OnRoundEnd()
        {
            SCPUtils.IsStarted = false;
            Timing.KillCoroutines(pluginInstance.Functions.DT);

        }

        public void OnRoundRestart()
        {
            SCPUtils.IsStarted = false;
            Timing.KillCoroutines(pluginInstance.Functions.DT);
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
            if (Database.PlayerData[ev.Player].FirstJoin == DateTime.MinValue) Database.PlayerData[ev.Player].FirstJoin = DateTime.Now;
            if (pluginInstance.welcomeEnabled) ev.Player.Broadcast(pluginInstance.welcomeMessageDuration, pluginInstance.welcomeMessage, false);

        }

        public void OnDecontaminate(ref DecontaminationEvent ev)
        {
            if (pluginInstance.decontaminationMessageEnabled) Map.Broadcast(pluginInstance.decontaminationMessage, pluginInstance.decontaminationMessageDuration, false);
        }

        public void OnPlayerDeath(ref PlayerDeathEvent ev)
        {
            if (ev.Player.GetTeam() == Team.SCP && SCPUtils.IsStarted && pluginInstance.enableSCPSuicideAutoWarn)
            {
                if ((DateTime.Now - lastTeslaEvent).Seconds >= pluginInstance.SCP079TeslaEventWait)
                {
                    if (ev.Info.GetDamageType() == DamageTypes.Tesla || ev.Info.GetDamageType() == DamageTypes.Wall) pluginInstance.Functions.OnQuitOrSuicide(ev.Player);
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
                if (ev.Player.GetTeam() == Team.SCP && pluginInstance.quitEqualsSuicide && SCPUtils.IsStarted)
                {
                    if (pluginInstance.enableSCPSuicideAutoWarn && pluginInstance.quitEqualsSuicide) pluginInstance.Functions.OnQuitOrSuicide(ev.Player);
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
