using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Server;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using DamageTypes = Exiled.API.Enums.DamageType;
using Features = Exiled.API.Features;
using Round = Exiled.API.Features.Round;

namespace SCPUtils
{
    public class EventHandlers
    {
        private readonly ScpUtils pluginInstance;

        public DateTime lastTeslaEvent;

        public static bool TemporarilyDisabledWarns;

     //   public bool ptEnabled;

        private static Dictionary<string, DateTime> PreauthTime { get; set; } = new Dictionary<string, DateTime>();

        private static Dictionary<Features.Player, string> Cuffed { get; set; } = new Dictionary<Features.Player, string>();

        public List<Features.Player> KickedList { get; set; } = new List<Features.Player>();
        public Dictionary<Features.Player, Features.Player> SwapRequest { get; set; } = new Dictionary<Features.Player, Features.Player>();
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
            if (ev.Player == null) return;
            if (Cuffed.ContainsKey(ev.Player)) Cuffed.Remove(ev.Player);
            if ((ev.Player.Role.Team == PlayerRoles.Team.SCPs || (pluginInstance.Config.AreTutorialsSCP && ev.Player.Role == PlayerRoles.RoleTypeId.Tutorial)) && Round.IsStarted && pluginInstance.Config.EnableSCPSuicideAutoWarn && !TemporarilyDisabledWarns)
            {
                if ((DateTime.Now - lastTeslaEvent).Seconds >= pluginInstance.Config.Scp079TeslaEventWait)
                {
                    if (ev.DamageHandler.Type == DamageTypes.Tesla || ev.DamageHandler.Type == DamageTypes.Crushed || (ev.DamageHandler.Type == DamageTypes.Unknown && ev.DamageHandler.Damage >= 50000) || (ev.DamageHandler.Type == DamageTypes.Explosion && ev.DamageHandler.IsSuicide))
                    {
                        pluginInstance.Functions.LogWarn(ev.Player, ev.DamageHandler.Type.ToString());
                        pluginInstance.Functions.OnQuitOrSuicide(ev.Player);
                    }
                    else if ((ev.DamageHandler.Type == DamageTypes.Unknown && ev.DamageHandler.Damage == -1f) && pluginInstance.Config.QuitEqualsSuicide)
                    {
                        pluginInstance.Functions.LogWarn(ev.Player, "Disconnect");
                        pluginInstance.Functions.OnQuitOrSuicide(ev.Player);
                    }
                }
            }

            if (pluginInstance.Config.NotifyLastPlayerAlive)
            {

                List<Features.Player> team = Features.Player.Get(ev.Player.Role.Team).ToList();
                if (team.Count - 1 == 1)
                {
                    if (team[0] == ev.Player)
                    {
                        team[1].ShowHint(pluginInstance.Config.LastPlayerAliveNotificationText, pluginInstance.Config.LastPlayerAliveMessageDuration);
                    }
                    else
                    {
                        team[0].ShowHint(pluginInstance.Config.LastPlayerAliveNotificationText, pluginInstance.Config.LastPlayerAliveMessageDuration);
                    }
                }
            }


            if (ev.Player.IsScp || ev.Player.Role == PlayerRoles.RoleTypeId.Tutorial && pluginInstance.Config.AreTutorialsSCP)
            {
                if (ev.DamageHandler.IsSuicide)
                {
                    var message = pluginInstance.Config.ScpSuicideMessage.Content;
                    message = message.Replace("%playername%", ev.Player.Nickname).Replace("%scpname%", ev.Player.Role.Type.ToString()).Replace("%reason%", ev.DamageHandler.Type.ToString());
                    Map.Broadcast(pluginInstance.Config.ScpSuicideMessage.Duration, message, pluginInstance.Config.ScpSuicideMessage.Type);
                }

                else if (!ev.DamageHandler.IsSuicide)
                {
                    if (pluginInstance.Config.ScpSuicideMessage.Show)
                    {
                        if (ev.Attacker == null)
                        {
                            var message = pluginInstance.Config.ScpSuicideMessage.Content;
                            message = message.Replace("%playername%", ev.Player.Nickname).Replace("%scpname%", ev.Player.Role.Type.ToString()).Replace("%reason%", ev.DamageHandler.Type.ToString());
                            Map.Broadcast(pluginInstance.Config.ScpSuicideMessage.Duration, message, pluginInstance.Config.ScpSuicideMessage.Type);
                        }
                        else
                        {
                            var message = pluginInstance.Config.ScpDeathMessage.Content;
                            message = message.Replace("%playername%", ev.Player.Nickname).Replace("%scpname%", ev.Player.Role.Type.ToString()).Replace("%killername%", ev.Attacker.Nickname).Replace("%reason%", ev.DamageHandler.Type.ToString());
                            Map.Broadcast(pluginInstance.Config.ScpDeathMessage.Duration, message, pluginInstance.Config.ScpDeathMessage.Type);
                        }
                    }
                }
            }
        }


        internal void OnKicking(KickingEventArgs ev)
        {
            if (!KickedList.Contains(ev.Target)) KickedList.Add(ev.Target);            
        }

        internal void OnBanned(BanningEventArgs ev)
        {
            if (!KickedList.Contains(ev.Target)) KickedList.Add(ev.Target);
        }
        internal void OnPlayerUnhandCuff(RemovingHandcuffsEventArgs ev)
        {           
            if (pluginInstance.Config.HandCuffOwnership)
            {
                if (!Cuffed.ContainsKey(ev.Target)) ev.IsAllowed = true;

                else if (Cuffed.FirstOrDefault(x => x.Key == ev.Target).Key.Role.Team == PlayerRoles.Team.ClassD && ev.Player.Role.Team == PlayerRoles.Team.ChaosInsurgency)
                {
                    ev.IsAllowed = true;
                    Cuffed.Remove(ev.Target);
                }

                else if (Cuffed.FirstOrDefault(x => x.Key == ev.Target).Key.Role.Team == PlayerRoles.Team.Scientists && ev.Player.Role.Team == PlayerRoles.Team.FoundationForces)
                {
                    ev.IsAllowed = true;
                    Cuffed.Remove(ev.Target);
                }

                else if (Cuffed.FirstOrDefault(x => x.Key == ev.Target).Key.Role.Team == ev.Player.Role.Team)
                {
                    ev.IsAllowed = true;
                    Cuffed.Remove(ev.Target);
                }


                else if (Cuffed[ev.Target] == ev.Player.UserId)
                {
                    ev.IsAllowed = true;
                    Cuffed.Remove(ev.Target);
                }

                else
                {
                    if (pluginInstance.Config.UnhandCuffDenied.Show) ev.Player.ShowHint(pluginInstance.Config.UnhandCuffDenied.Content, pluginInstance.Config.UnhandCuffDenied.Duration);
                    ev.IsAllowed = false;
                }
            }
        }

        internal void OnPlayerHandcuff(HandcuffingEventArgs ev)
        {
            if (pluginInstance.Config.HandCuffOwnership)
            {
                if (Cuffed.ContainsKey(ev.Target)) Cuffed.Remove(ev.Target);
                Cuffed.Add(ev.Target, ev.Player.UserId);
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
            TemporarilyDisabledWarns = true;

            foreach (Features.Player player in Exiled.API.Features.Player.List)
            {
                pluginInstance.Functions.SaveData(player);
            }            
            Cuffed.Clear();
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
            if (!Cuffed.ContainsKey(ev.Player)) Cuffed.Remove(ev.Player);
        }

        internal void On096AddTarget(AddingTargetEventArgs ev)
        {
            if (pluginInstance.Config.Scp096TargetNotifyEnabled)
            {
                ev.Target.ShowHint(pluginInstance.Config.Scp096TargetNotifyText, pluginInstance.Config.Scp096TargetMessageDuration);
            }
        }

        internal void OnWaitingForPlayers()
        {
            TemporarilyDisabledWarns = false;
            ChaosRespawnCount = 0;
            MtfRespawnCount = 0;
            SwapRequest.Clear();
        }

        internal void On079TeslaEvent(InteractingTeslaEventArgs _)
        {
            lastTeslaEvent = DateTime.Now;
        }

        internal void OnPlayerHurt(HurtingEventArgs ev)
        {            
            if (ev.Player == null || ev.Attacker== null) return;
       
            if (pluginInstance.Config.CuffedImmunityPlayers?.ContainsKey(ev.Player.Role.Team) == true)
            {                
                ev.IsAllowed = !(pluginInstance.Functions.IsTeamImmune(ev.Player, ev.Attacker) && pluginInstance.Functions.CuffedCheck(ev.Player) && pluginInstance.Functions.CheckSafeZones(ev.Player));
            }
        }


        internal void OnPlayerVerify(VerifiedEventArgs ev)
        {
            if (!Database.LiteDatabase.GetCollection<Player>().Exists(player => player.Id == DatabasePlayer.GetRawUserId(ev.Player)))
            {
                pluginInstance.DatabasePlayerData.AddPlayer(ev.Player);
            }

            Player databasePlayer = ev.Player.GetDatabasePlayer();

            if (!Database.LiteDatabase.GetCollection<DatabaseIp>().Exists(alias => alias.Id == DatabasePlayer.GetRawUserId(ev.Player.IPAddress)))
            {
                pluginInstance.DatabasePlayerData.AddIp(ev.Player.IPAddress, ev.Player.UserId);
            }




            if (Database.PlayerData.ContainsKey(ev.Player))
            {
                return;
            }
            Database.PlayerData.Add(ev.Player, databasePlayer);
            if (PreauthTime.ContainsKey(ev.Player.UserId))
            {
                databasePlayer.LastSeen = PreauthTime[ev.Player.UserId];
                PreauthTime.Remove(ev.Player.UserId);
            }
            else databasePlayer.LastSeen = DateTime.Now;
            databasePlayer.Name = ev.Player.Nickname;
            databasePlayer.Ip = ev.Player.IPAddress;

            if (databasePlayer.FirstJoin == DateTime.MinValue)
            {
                databasePlayer.FirstJoin = DateTime.Now;
            }

            if (pluginInstance.Config.WelcomeMessage.Show)
            {
                var message = pluginInstance.Config.WelcomeMessage.Content;
                message = message.Replace("%player%", ev.Player.Nickname);
                ev.Player.Broadcast(pluginInstance.Config.WelcomeMessage.Duration, message, pluginInstance.Config.WelcomeMessage.Type, false);
            }

            if (pluginInstance.Functions.CheckAsnPlayer(ev.Player))
            {
                ev.Player.Kick($"Auto-Kick: {pluginInstance.Config.AsnKickMessage}");
            }
            else
            {
                pluginInstance.Functions.PostLoadPlayer(ev.Player);
            }

            pluginInstance.Functions.IpCheck(ev.Player);
        }

        internal void OnPlayerSpawn(SpawningEventArgs ev)
        {
            Player databasePlayer = ev.Player.GetDatabasePlayer();
            if (ev.Player.Role.Team == PlayerRoles.Team.SCPs || (pluginInstance.Config.AreTutorialsSCP && ev.Player.Role == PlayerRoles.RoleTypeId.Tutorial))
            {

                if (databasePlayer.RoundBanLeft >= 1 && ev.Player.Role != PlayerRoles.RoleTypeId.Scp0492)
                {
                    Timing.CallDelayed(1.5f, () => pluginInstance.Functions.ReplacePlayer(ev.Player));

                }
                else ev.Player.GetDatabasePlayer().TotalScpGamesPlayed++;
            }  
            if(ev.Player.IsScp && pluginInstance.Config.AllowSCPSwap)
            {                
                if (Round.ElapsedTime.TotalSeconds < ScpUtils.StaticInstance.Config.MaxAllowedTimeScpSwapRequest)
                {
                    var seconds = Math.Round(pluginInstance.Config.MaxAllowedTimeScpSwapRequest - Round.ElapsedTime.TotalSeconds + 1);
                    var message = pluginInstance.Config.SwapRequestInfoBroadcast.Content;
                    message = message.Replace("%seconds%", seconds.ToString());
                    ev.Player.Broadcast(pluginInstance.Config.SwapRequestInfoBroadcast.Duration, message, pluginInstance.Config.SwapRequestInfoBroadcast.Type, false);
                  
                }
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
