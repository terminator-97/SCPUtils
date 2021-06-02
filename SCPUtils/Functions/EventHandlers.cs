using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Linq;
using Round = Exiled.API.Features.Round;

namespace SCPUtils
{
    public class EventHandlers
    {
        private readonly ScpUtils pluginInstance;

        public DateTime lastTeslaEvent;

        public static bool TemporarilyDisabledWarns;

        public int ChaosRespawnCount { get; set; }

        public int MtfRespawnCount { get; set; }

        public DateTime LastChaosRespawn { get; set; }

        public DateTime LastMtfRespawn { get; set; }

        public EventHandlers(ScpUtils pluginInstance)
        {
            this.pluginInstance = pluginInstance;
        }

        internal void OnPlayerDeath(DyingEventArgs ev)
        {
            if ((ev.Target.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && ev.Target.Team == Team.TUT)) && Round.IsStarted && pluginInstance.Config.EnableSCPSuicideAutoWarn && !TemporarilyDisabledWarns)
            {
                if ((DateTime.Now - lastTeslaEvent).Seconds >= pluginInstance.Config.Scp079TeslaEventWait)
                {
                    if (ev.HitInformation.GetDamageType() == DamageTypes.Tesla || (ev.HitInformation.GetDamageType() == DamageTypes.Wall && ev.HitInformation.Amount >= 50000) || (ev.HitInformation.GetDamageType() == DamageTypes.Grenade && ev.Killer == ev.Target))
                    {
                        pluginInstance.Functions.LogWarn(ev.Target, ev.HitInformation.GetDamageName());
                        pluginInstance.Functions.OnQuitOrSuicide(ev.Target);
                    }
                    else if ((ev.HitInformation.GetDamageType() == DamageTypes.Wall && ev.HitInformation.Amount == -1f) && ev.Killer == ev.Target && pluginInstance.Config.QuitEqualsSuicide)
                    {
                        pluginInstance.Functions.LogWarn(ev.Target, "Disconnect");
                        pluginInstance.Functions.OnQuitOrSuicide(ev.Target);
                    }
                }
            }

            if (pluginInstance.Config.NotifyLastPlayerAlive)
            {
                System.Collections.Generic.List<Exiled.API.Features.Player> team = Exiled.API.Features.Player.Get(ev.Target.Team).ToList();
                if (team.Count - 1 == 1)
                {
                    if (team[0] == ev.Target)
                    {
                        team[1].ShowHint(pluginInstance.Config.LastPlayerAliveNotificationText, pluginInstance.Config.LastPlayerAliveMessageDuration);
                    }
                    else
                    {
                        team[0].ShowHint(pluginInstance.Config.LastPlayerAliveNotificationText, pluginInstance.Config.LastPlayerAliveMessageDuration);
                    }
                }
            }

            if(pluginInstance.Config.DeathMessageModuleEnabled && ev.Target.IsScp || ev.Target.Role== RoleType.Tutorial && pluginInstance.Config.AreTutorialsSCP)
            {  
                if (ev.Target.Nickname != ev.Killer.Nickname)
                {
                    Map.Broadcast(pluginInstance.Config.ScpDeathMessageDuration, pluginInstance.Config.ScpDeathMessage.Replace("%playername%", ev.Target.Nickname).Replace("%scpname%", ev.Target.Role.ToString()).Replace("%killername%", ev.Killer.Nickname).Replace("%reason%", ev.HitInformation.GetDamageName()), Broadcast.BroadcastFlags.Normal);           
                }
                if (ev.Target.Nickname == ev.Killer.Nickname)
                {
                    Map.Broadcast(pluginInstance.Config.ScpDeathMessageDuration, pluginInstance.Config.ScpSuicideMessage.Replace("%playername%", ev.Target.Nickname).Replace("%scpname%", ev.Target.Role.ToString()).Replace("%reason%", ev.HitInformation.GetDamageName()), Broadcast.BroadcastFlags.Normal);
                }
            }
        }

        internal void OnRoundEnded(RoundEndedEventArgs _)
        {
            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List)
            {
                pluginInstance.Functions.SaveData(player);
            }
            TemporarilyDisabledWarns = true;
        }

        internal void OnTeamRespawn(RespawningTeamEventArgs ev)
        {

            if (ev.NextKnownTeam.ToString() == "ChaosInsurgency")
            {
                ChaosRespawnCount++;
                LastChaosRespawn = DateTime.Now;
            }

            else if (ev.NextKnownTeam.ToString() == "NineTailedFox")
            {
                MtfRespawnCount++;
                LastMtfRespawn = DateTime.Now;
            }

        }

        internal void OnPlayerDestroy(DestroyingEventArgs ev)
        {
            pluginInstance.Functions.SaveData(ev.Player);
        }

        internal void On096AddTarget(AddingTargetEventArgs ev)
        {
            if (pluginInstance.Config.Scp096TargetEnabled)
            {
                ev.Target.ShowHint(pluginInstance.Config.Scp096TargetText, pluginInstance.Config.Scp096TargetMessageDuration);
            }
        }

        internal void OnWaitingForPlayers()
        {
            TemporarilyDisabledWarns = false;
            ChaosRespawnCount = 0;
            MtfRespawnCount = 0;
        }

        internal void On079TeslaEvent(InteractingTeslaEventArgs _)
        {
            lastTeslaEvent = DateTime.Now;
        }

        internal void OnPlayerHurt(HurtingEventArgs ev)
        {
            if (pluginInstance.Config.CuffedImmunityPlayers?.ContainsKey(ev.Target.Team) == true)
            {
                ev.IsAllowed = !(pluginInstance.Functions.IsTeamImmune(ev.Target, ev.Attacker) && pluginInstance.Functions.CuffedCheck(ev.Target) && pluginInstance.Functions.CheckSafeZones(ev.Target));
            }
        }


        internal void OnPlayerVerify(VerifiedEventArgs ev)
        {

            if (!Database.LiteDatabase.GetCollection<Player>().Exists(player => player.Id == DatabasePlayer.GetRawUserId(ev.Player)))
            {
                pluginInstance.DatabasePlayerData.AddPlayer(ev.Player);
            }

            Player databasePlayer = ev.Player.GetDatabasePlayer();
            if (Database.PlayerData.ContainsKey(ev.Player))
            {
                return;
            }

            Database.PlayerData.Add(ev.Player, databasePlayer);
            databasePlayer.LastSeen = DateTime.Now;
            databasePlayer.Name = ev.Player.Nickname;
            if (databasePlayer.FirstJoin == DateTime.MinValue)
            {
                databasePlayer.FirstJoin = DateTime.Now;
            }

            if (pluginInstance.Config.WelcomeEnabled)
            {
                ev.Player.Broadcast(pluginInstance.Config.WelcomeMessageDuration, pluginInstance.Config.WelcomeMessage.Replace("%player%", ev.Player.Nickname), Broadcast.BroadcastFlags.Normal);
            }

            if (pluginInstance.Functions.CheckAsnPlayer(ev.Player))
            {
                ev.Player.Kick($"Auto-Kick: {pluginInstance.Config.AsnKickMessage}", "SCPUtils");
            }
            else
            {
                pluginInstance.Functions.PostLoadPlayer(ev.Player);
            }
        }

        internal void OnPlayerSpawn(SpawningEventArgs ev)
        {
            if (ev.Player.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && ev.Player.Team == Team.TUT))
            {
                ev.Player.GetDatabasePlayer().TotalScpGamesPlayed++;
            }
        }

        internal void OnPlayerLeave(LeftEventArgs ev)
        {
            pluginInstance.Functions.SaveData(ev.Player);
        }

        internal void OnDecontaminate(DecontaminatingEventArgs ev)
        {
            if (pluginInstance.Config.DecontaminationMessageEnabled)
            {
                Map.Broadcast(pluginInstance.Config.DecontaminationMessageDuration, pluginInstance.Config.DecontaminationMessage, Broadcast.BroadcastFlags.Normal);
            }
        }

    }

}
