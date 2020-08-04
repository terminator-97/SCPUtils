using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using Log = Exiled.API.Features.Log;
using Round = Exiled.API.Features.Round;
using System.Collections.Generic;
using System.Linq;

namespace SCPUtils
{
    public class EventHandlers
    {
        private readonly ScpUtils pluginInstance;

        public DateTime lastTeslaEvent;

        public Dictionary<string, Team> roleManager = new Dictionary<string, Team>();

        public static bool TemporarilyDisabledWarns;

        public EventHandlers(ScpUtils pluginInstance) => this.pluginInstance = pluginInstance;

        internal void OnPlayerDeath(DyingEventArgs ev)
        {
            if ((ev.Target.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && ev.Target.Team == Team.TUT)) && Round.IsStarted && pluginInstance.Config.EnableSCPSuicideAutoWarn && !TemporarilyDisabledWarns)
            {
                if ((DateTime.Now - lastTeslaEvent).Seconds >= pluginInstance.Config.Scp079TeslaEventWait)
                {
                    if (ev.HitInformation.GetDamageType() == DamageTypes.Tesla || ev.HitInformation.GetDamageType() == DamageTypes.Wall) pluginInstance.Functions.OnQuitOrSuicide(ev.Target);
                }
            }
            roleManager[ev.Target.UserId] = Team.RIP;

        }

        internal void OnChangeRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == null) return;
            if (!roleManager.ContainsKey(ev.Player.UserId)) roleManager.Add(ev.Player.UserId, ev.Player.Team);
            if (ev.Player.Team == Team.RIP) roleManager[ev.Player.UserId] = Team.RIP;
        }

        internal void OnPlayerHurt(HurtingEventArgs ev)
        {
            if (pluginInstance.Config.CuffedImmunityPlayers?.ContainsKey(ev.Target.Team) == true)
            {
                ev.IsAllowed = !(pluginInstance.Functions.IsTeamImmune(ev.Target, ev.Attacker) && pluginInstance.Functions.CuffedCheck(ev.Target) && pluginInstance.Functions.CheckSafeZones(ev.Target));
            }
        }

        internal void OnWaitingForPlayers() => TemporarilyDisabledWarns = false;


        internal void On079TeslaEvent(InteractingTeslaEventArgs ev) => lastTeslaEvent = DateTime.Now;




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
            if (ev.Player.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && ev.Player.Team == Team.TUT)) ev.Player.GetDatabasePlayer().TotalScpGamesPlayed++;

            if (!roleManager.ContainsKey(ev.Player.UserId)) roleManager.Add(ev.Player.UserId, ev.Player.Team);
            else roleManager[ev.Player.UserId] = ev.Player.Team;
        }

        internal void OnPlayerLeave(LeftEventArgs ev)
        {
            if (!roleManager.ContainsKey(ev.Player.UserId)) return;
            else if ((ev.Player.Team == Team.SCP || roleManager[ev.Player.UserId] == Team.SCP) || (pluginInstance.Config.AreTutorialsSCP && ev.Player.Team == Team.TUT || roleManager[ev.Player.UserId] == Team.TUT) && pluginInstance.Config.QuitEqualsSuicide && Round.IsStarted && !TemporarilyDisabledWarns)
            {
                if (pluginInstance.Config.EnableSCPSuicideAutoWarn && pluginInstance.Config.QuitEqualsSuicide) pluginInstance.Functions.OnQuitOrSuicide(ev.Player);
            }
            pluginInstance.Functions.SaveData(ev.Player);
            roleManager.Remove(ev.Player.UserId);
        }

        internal void OnDecontaminate(DecontaminatingEventArgs ev)
        {
            if (pluginInstance.Config.DecontaminationMessageEnabled) Map.Broadcast(pluginInstance.Config.DecontaminationMessageDuration, pluginInstance.Config.DecontaminationMessage, Broadcast.BroadcastFlags.Normal);
        }

    }

}
