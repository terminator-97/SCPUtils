using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Round = Exiled.API.Features.Round;
using Features = Exiled.API.Features;

namespace SCPUtils
{
    public class EventHandlers
    {
        private readonly ScpUtils pluginInstance;

        public DateTime lastTeslaEvent;

        public static bool TemporarilyDisabledWarns;

        private static Dictionary<string, DateTime> PreauthTime { get; set; } = new Dictionary<string, DateTime>();

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
                List<Features.Player> team = Features.Player.Get(ev.Target.Team).ToList();
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

            if (ev.Target.IsScp || ev.Target.Role == RoleType.Tutorial && pluginInstance.Config.AreTutorialsSCP)
            {
                if (ev.Target.Nickname != ev.Killer.Nickname)
                {
                    if (pluginInstance.Config.ScpDeathMessage.Show)
                    {
                        var message = pluginInstance.Config.ScpDeathMessage.Content;
                        message = message.Replace("%playername%", ev.Target.Nickname).Replace("%scpname%", ev.Target.Role.ToString()).Replace("%killername%", ev.Killer.Nickname).Replace("%reason%", ev.HitInformation.GetDamageName());
                        Map.Broadcast(pluginInstance.Config.ScpDeathMessage.Duration,message,pluginInstance.Config.ScpDeathMessage.Type);
                    }
                }
             
                if (ev.Target.Nickname == ev.Killer.Nickname)
                {
                    if (pluginInstance.Config.ScpSuicideMessage.Show)
                    {
                        var message = pluginInstance.Config.ScpSuicideMessage.Content;
                        message = message.Replace("%playername%", ev.Target.Nickname).Replace("%scpname%", ev.Target.Role.ToString()).Replace("%reason%", ev.HitInformation.GetDamageName());
                        Map.Broadcast(pluginInstance.Config.ScpSuicideMessage.Duration,message,pluginInstance.Config.ScpSuicideMessage.Type);
                    }
                }
            }
        }

        internal void OnRoundRestart()
        {
            foreach (Features.Player player in Features.Player.List)
            {
                pluginInstance.Functions.SaveData(player);
            }
        }


        internal void OnPlayerPreauth(PreAuthenticatingEventArgs ev)
        {
            if (PreauthTime.ContainsKey(ev.UserId))
            {
                PreauthTime.Remove(ev.UserId);
            }

            PreauthTime.Add(ev.UserId, DateTime.Now);
        }


        internal void OnRoundEnded(RoundEndedEventArgs _)
        {
            foreach (Features.Player player in Exiled.API.Features.Player.List)
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
            databasePlayer.LastSeen = PreauthTime[ev.Player.UserId];    
            PreauthTime.Remove(ev.Player.UserId);
            databasePlayer.Name = ev.Player.Nickname;
            var sameIP = Database.LiteDatabase.GetCollection<Player>().FindAll().Where(x => x.Ip == databasePlayer.Ip).ToList();
            if (databasePlayer.Ip != ev.Player.IPAddress)
            {
                pluginInstance.Functions.ChangeIP(ev.Player);
            }
                if (sameIP.Count > 1)
                pluginInstance.Functions.CheckAccount(ev.Player);
            if (databasePlayer.FirstJoin == DateTime.MinValue)
            {
                databasePlayer.FirstJoin = DateTime.Now;
            }
            if(pluginInstance.Config.WelcomeMessage.Show)
            {
                var message = pluginInstance.Config.WelcomeMessage.Content;
                message = message.Replace("%player%", ev.Player.Nickname);                
                ev.Player.Broadcast(pluginInstance.Config.WelcomeMessage.Duration, message, pluginInstance.Config.WelcomeMessage.Type);
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
                Map.Broadcast(pluginInstance.Config.DecontaminationMessage);            
        }
    }

}
