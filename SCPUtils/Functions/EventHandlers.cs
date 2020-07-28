using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using Log = Exiled.API.Features.Log;

namespace SCPUtils
{
    public class EventHandlers
    {
        private readonly ScpUtils pluginInstance;

        public DateTime lastTeslaEvent;

        public EventHandlers(ScpUtils pluginInstance) => this.pluginInstance = pluginInstance;

        internal void OnRoundStart()
        {
            ScpUtils.IsStarted = true;
            if (pluginInstance.Config.EnableRoundRestartCheck) pluginInstance.Functions.StartFixer();
        }

        internal void OnRoundEnd(RoundEndedEventArgs ev)
        {
            ScpUtils.IsStarted = false;
            Timing.KillCoroutines(pluginInstance.Functions.DT);           
            foreach (var player in Exiled.API.Features.Player.List)
            {
                pluginInstance.Functions.SaveData(player);
            }          
         
        }

        internal void OnRoundRestart()
        {            
                foreach (var player in Exiled.API.Features.Player.List)
                {
                    pluginInstance.Functions.SaveData(player);
                }
                
            ScpUtils.IsStarted = false;
            Timing.KillCoroutines(pluginInstance.Functions.DT);
        }

        internal void OnPlayerDeath(DyingEventArgs ev)
        {
            if ((ev.Target.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && ev.Target.Team == Team.TUT)) && ScpUtils.IsStarted && pluginInstance.Config.EnableSCPSuicideAutoWarn)
            {
                if ((DateTime.Now - lastTeslaEvent).Seconds >= pluginInstance.Config.Scp079TeslaEventWait)
                {
                    if (ev.HitInformation.GetDamageType() == DamageTypes.Tesla || ev.HitInformation.GetDamageType() == DamageTypes.Wall) pluginInstance.Functions.OnQuitOrSuicide(ev.Target);
                }
            }

        }

        internal void On079TeslaEvent(InteractingTeslaEventArgs ev)
        {
            lastTeslaEvent = DateTime.Now;
        }



        internal void OnPlayerJoin(JoinedEventArgs ev)
        {
            if (!Database.LiteDatabase.GetCollection<Player>().Exists(player => player.Id == DatabasePlayer.GetRawUserId(ev.Player)))
            {
                Log.Info(ev.Player.Nickname + " is not present on DB!");
                pluginInstance.DatabasePlayerData.AddPlayer(ev.Player);
            }

            var databasePlayer = ev.Player.GetDatabasePlayer();
            if (Database.PlayerData.ContainsKey(ev.Player)) return;
            Database.PlayerData.Add(ev.Player, databasePlayer);
            databasePlayer.LastSeen = DateTime.Now;
            databasePlayer.Name = ev.Player.Nickname;
            if (databasePlayer.FirstJoin == DateTime.MinValue) databasePlayer.FirstJoin = DateTime.Now;
            if (pluginInstance.Config.WelcomeEnabled) ev.Player.Broadcast(pluginInstance.Config.WelcomeMessageDuration, pluginInstance.Config.WelcomeMessage, Broadcast.BroadcastFlags.Normal);
            if (!string.IsNullOrEmpty(databasePlayer.CustomNickName) && databasePlayer.CustomNickName != "None") ev.Player.Nickname = databasePlayer.CustomNickName;
            if (pluginInstance.Config.ASNBlacklist.Contains(ev.Player.ReferenceHub.characterClassManager.Asn) && !databasePlayer.ASNWhitelisted) ev.Player.Kick($"Auto-Kick: {pluginInstance.Config.AsnKickMessage}", "SCPUtils");
            else pluginInstance.Functions.PostLoadPlayer(ev.Player);
        }



        internal void OnPlayerSpawn(SpawningEventArgs ev)
        {
            if (ev.Player.Team == Team.SCP) ev.Player.GetDatabasePlayer().TotalScpGamesPlayed++;
        }

        internal void OnPlayerLeave(LeftEventArgs ev)
        {
            pluginInstance.Functions.SaveData(ev.Player);
        }

        internal void OnDecontaminate(DecontaminatingEventArgs ev)
        {
            if (pluginInstance.Config.DecontaminationMessageEnabled) Map.Broadcast(pluginInstance.Config.DecontaminationMessageDuration, pluginInstance.Config.DecontaminationMessage, Broadcast.BroadcastFlags.Normal);
        }



    }

}
