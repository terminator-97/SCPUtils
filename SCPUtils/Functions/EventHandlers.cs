using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Permissions.Extensions;
using EXILED;
using MEC;
using System;


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
        }

        internal void OnRoundRestart()
        {
            ScpUtils.IsStarted = false;
            Timing.KillCoroutines(pluginInstance.Functions.DT);
        }


        internal void OnPlayerDeath(DiedEventArgs ev)
        {
            if ((ev.Target.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && ev.Target.Team == Team.TUT)) && ScpUtils.IsStarted && pluginInstance.Config.EnableSCPSuicideAutoWarn)
            {
                if ((DateTime.Now - lastTeslaEvent).Seconds >= pluginInstance.Config.Scp079TeslaEventWait)
                {
                    if (ev.HitInformations.GetDamageType() == DamageTypes.Tesla || ev.HitInformations.GetDamageType() == DamageTypes.Wall) pluginInstance.Functions.OnQuitOrSuicide(ev.Target);
                }
            }
        }



        internal void OnPlayerJoin(JoinedEventArgs ev)
        {
            if (!Database.LiteDatabase.GetCollection<Player>().Exists(player => player.Id == DatabasePlayer.GetRawUserId(ev.Player)))
            {
                Log.Info(ev.Player.Nickname + " is not present on DB!");
                Database.AddPlayer(ev.Player);
            }

            var databasePlayer = ev.Player.GetDatabasePlayer();
            if (Database.PlayerData.ContainsKey(ev.Player)) return;
            Database.PlayerData.Add(ev.Player, databasePlayer);
            databasePlayer.LastSeen = DateTime.Now;
            databasePlayer.Name = ev.Player.Nickname;
            if (databasePlayer.FirstJoin == DateTime.MinValue) databasePlayer.FirstJoin = DateTime.Now;
            if (pluginInstance.Config.WelcomeEnabled) ev.Player.Broadcast(pluginInstance.Config.WelcomeMessageDuration, pluginInstance.Config.WelcomeMessage, Broadcast.BroadcastFlags.Normal);
            pluginInstance.Functions.PostLoadPlayer(ev.Player);

            Timing.CallDelayed(3f, () =>
            {
                if (pluginInstance.Config.AutoKickBannedNames && pluginInstance.Functions.CheckNickname(ev.Player.Nickname) && !ev.Player.CheckPermission("scputils.bypassnickrestriction")) ev.Player.Kick("Auto-Kick: " + pluginInstance.Config.AutoKickBannedNameMessage, "SCPUtils");
            });
        }

        internal void OnPlayerSpawn(SpawningEventArgs ev)
        {
            if (ev.Player.Team == Team.SCP) ev.Player.GetDatabasePlayer().TotalScpGamesPlayed++;
        }

        internal void OnPlayerLeave(LeftEventArgs ev)
        {
            if (ev.Player.Nickname != "Dedicated Server" && ev.Player != null && Database.PlayerData.ContainsKey(ev.Player))
            {
                if ((ev.Player.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && ev.Player.Team == Team.TUT)) && pluginInstance.Config.QuitEqualsSuicide && ScpUtils.IsStarted)
                {
                    if (pluginInstance.Config.EnableSCPSuicideAutoWarn && pluginInstance.Config.QuitEqualsSuicide) pluginInstance.Functions.OnQuitOrSuicide(ev.Player);
                }
                ev.Player.GetDatabasePlayer().SetCurrentDayPlayTime();
                Database.LiteDatabase.GetCollection<Player>().Update(Database.PlayerData[ev.Player]);
                Database.PlayerData.Remove(ev.Player);
            }
        }

        internal void OnTeslaEvent(TriggeringTeslaEventArgs ev)
        {
            lastTeslaEvent = DateTime.Now;
        }

        internal void OnDecontaminate(DecontaminatingEventArgs ev)
        {
            if (pluginInstance.Config.DecontaminationMessageEnabled) Map.Broadcast(pluginInstance.Config.DecontaminationMessageDuration, pluginInstance.Config.DecontaminationMessage, Broadcast.BroadcastFlags.Normal);
        }




    }

}
