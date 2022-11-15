using Exiled.API.Features;
using Exiled.Events.EventArgs;
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

        public bool ptEnabled;

        private static Dictionary<string, DateTime> PreauthTime { get; set; } = new Dictionary<string, DateTime>();

        private static Dictionary<Exiled.API.Features.Player, string> Cuffed { get; set; } = new Dictionary<Exiled.API.Features.Player, string>();
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
            if (ev.Target == null) return;
            if (Cuffed.ContainsKey(ev.Target)) Cuffed.Remove(ev.Target);
            if ((ev.Target.Role.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && ev.Target.Role.Team == Team.TUT)) && Round.IsStarted && pluginInstance.Config.EnableSCPSuicideAutoWarn && !TemporarilyDisabledWarns)
            {
                if ((DateTime.Now - lastTeslaEvent).Seconds >= pluginInstance.Config.Scp079TeslaEventWait)
                {
                    if (ev.Handler.Type == DamageTypes.Tesla || ev.Handler.Type == DamageTypes.Crushed || (ev.Handler.Type == DamageTypes.Unknown && ev.Handler.Damage >= 50000) || (ev.Handler.Type == DamageTypes.Explosion && ev.Handler.IsSuicide))
                    {
                        pluginInstance.Functions.LogWarn(ev.Target, ev.Handler.Type.ToString());
                        pluginInstance.Functions.OnQuitOrSuicide(ev.Target);
                    }
                    else if ((ev.Handler.Type == DamageTypes.Unknown && ev.Handler.Damage == -1f) && pluginInstance.Config.QuitEqualsSuicide)
                    {
                        pluginInstance.Functions.LogWarn(ev.Target, "Disconnect");
                        pluginInstance.Functions.OnQuitOrSuicide(ev.Target);
                    }
                }
            }

            if (pluginInstance.Config.NotifyLastPlayerAlive)
            {

                List<Features.Player> team = Features.Player.Get(ev.Target.Role.Team).ToList();
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
                if (ev.Handler.IsSuicide)
                {
                    var message = pluginInstance.Config.ScpSuicideMessage.Content;
                    message = message.Replace("%playername%", ev.Target.Nickname).Replace("%scpname%", ev.Target.Role.Type.ToString()).Replace("%reason%", ev.Handler.Type.ToString());
                    Map.Broadcast(pluginInstance.Config.ScpSuicideMessage.Duration, message, pluginInstance.Config.ScpSuicideMessage.Type);
                }

                else if (!ev.Handler.IsSuicide)
                {
                    if (pluginInstance.Config.ScpSuicideMessage.Show)
                    {
                        if (ev.Killer == null)
                        {
                            var message = pluginInstance.Config.ScpSuicideMessage.Content;
                            message = message.Replace("%playername%", ev.Target.Nickname).Replace("%scpname%", ev.Target.Role.Type.ToString()).Replace("%reason%", ev.Handler.Type.ToString());
                            Map.Broadcast(pluginInstance.Config.ScpSuicideMessage.Duration, message, pluginInstance.Config.ScpSuicideMessage.Type);
                        }
                        else
                        {
                            var message = pluginInstance.Config.ScpDeathMessage.Content;
                            message = message.Replace("%playername%", ev.Target.Nickname).Replace("%scpname%", ev.Target.Role.Type.ToString()).Replace("%killername%", ev.Killer.Nickname).Replace("%reason%", ev.Handler.Type.ToString());
                            Map.Broadcast(pluginInstance.Config.ScpDeathMessage.Duration, message, pluginInstance.Config.ScpDeathMessage.Type);
                        }
                    }
                }
            }
        }

        internal void OnPlayerJoined(JoinedEventArgs ev)
        {
            if((Features.Player.Dictionary.Count >= pluginInstance.Config.MinPlayersPtCount) && (!ptEnabled))
            {
                foreach(var player in Exiled.API.Features.Player.List)
                {
                    var databasePlayer = player.GetDatabasePlayer();
                    databasePlayer.LastSeen = DateTime.Now;                    
                }
                ptEnabled = true;
            }
            else if ((Features.Player.Dictionary.Count >= pluginInstance.Config.MinPlayersPtCount) && (ptEnabled))
            {
                foreach (var player in Exiled.API.Features.Player.List)
                {
                    pluginInstance.Functions.SavePlaytime(player);
                }
                ptEnabled = false;
            }
        }

        internal void OnPlayerUnhandCuff(RemovingHandcuffsEventArgs ev)
        {
            if (pluginInstance.Config.HandCuffOwnership)
            {
                if (!Cuffed.ContainsKey(ev.Target)) ev.IsAllowed = true;

                else if (Cuffed.FirstOrDefault(x => x.Key == ev.Target).Key.Role.Team == Team.CDP && ev.Cuffer.Role.Team == Team.CHI)
                {
                    ev.IsAllowed = true;
                    Cuffed.Remove(ev.Target);
                }

                else if (Cuffed.FirstOrDefault(x => x.Key == ev.Target).Key.Role.Team == Team.RSC && ev.Cuffer.Role.Team == Team.MTF)
                {
                    ev.IsAllowed = true;
                    Cuffed.Remove(ev.Target);
                }

                else if (Cuffed.FirstOrDefault(x => x.Key == ev.Target).Key.Role.Team == ev.Cuffer.Role.Team)
                {
                    ev.IsAllowed = true;
                    Cuffed.Remove(ev.Target);
                }


                else if (Cuffed[ev.Target] == ev.Cuffer.UserId)
                {
                    ev.IsAllowed = true;
                    Cuffed.Remove(ev.Target);
                }

                else
                {
                    if (pluginInstance.Config.UnhandCuffDenied.Show) ev.Cuffer.ShowHint(pluginInstance.Config.UnhandCuffDenied.Content, pluginInstance.Config.UnhandCuffDenied.Duration);
                    ev.IsAllowed = false;
                }
            }
        }

        internal void OnPlayerHandcuff(HandcuffingEventArgs ev)
        {
            if (pluginInstance.Config.HandCuffOwnership)
            {
                if (Cuffed.ContainsKey(ev.Target)) Cuffed.Remove(ev.Target);
                Cuffed.Add(ev.Target, ev.Cuffer.UserId);
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
        }

        internal void On079TeslaEvent(InteractingTeslaEventArgs _)
        {
            lastTeslaEvent = DateTime.Now;
        }

        internal void OnPlayerHurt(HurtingEventArgs ev)
        {
            if (ev.Attacker == null || ev.Target == null) return;
            if (pluginInstance.Config.CuffedImmunityPlayers?.ContainsKey(ev.Target.Role.Team) == true)
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
                ev.Player.Kick($"Auto-Kick: {pluginInstance.Config.AsnKickMessage}", "SCPUtils");
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
            if (ev.Player.Role.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && ev.Player.Role.Team == Team.TUT))
            {

                if (databasePlayer.RoundBanLeft >= 1 && ev.Player.Role != RoleType.Scp0492)
                {
                    Timing.CallDelayed(1.5f, () => pluginInstance.Functions.ReplacePlayer(ev.Player));

                }
                else ev.Player.GetDatabasePlayer().TotalScpGamesPlayed++;


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
