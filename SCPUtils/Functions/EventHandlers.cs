/*using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Server;*/
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
//using DamageTypes = Exiled.API.Enums.DamageType;
//using Features = Exiled.API.Features;
//using Round = Exiled.API.Features.Round;
//using PluginAPI.Enums.ServerEventType;
using PluginAPI.Core.Attributes;
using CommandSystem;
using ServerEvent = PluginAPI.Enums.ServerEventType;
using PlayerRoles;
using LiteNetLib;
using Respawning;
using PlayerRoles.PlayableScps.Scp079;
using PlayerStatsSystem;

namespace SCPUtils
{
    public class EventHandlers
    {
        private readonly ScpUtils pluginInstance;

        public DateTime lastTeslaEvent;

        public static bool TemporarilyDisabledWarns;

        //   public bool ptEnabled;

        public Dictionary<PluginAPI.Core.Player, DateTime> LastCommand { get; set; } = new Dictionary<PluginAPI.Core.Player, DateTime>();
        private static Dictionary<string, DateTime> PreauthTime { get; set; } = new Dictionary<string, DateTime>();

        private static Dictionary<PluginAPI.Core.Player, string> Cuffed { get; set; } = new Dictionary<PluginAPI.Core.Player, string>();

        public List<PluginAPI.Core.Player> KickedList { get; set; } = new List<PluginAPI.Core.Player>();
        public Dictionary<PluginAPI.Core.Player, PluginAPI.Core.Player> SwapRequest { get; set; } = new Dictionary<PluginAPI.Core.Player, PluginAPI.Core.Player>();
        public int ChaosRespawnCount { get; set; }

        public int MtfRespawnCount { get; set; }

        public DateTime LastChaosRespawn { get; set; }

        public DateTime LastMtfRespawn { get; set; }
        
        UniversalDamageHandler DamageType { get; set; }

        public EventHandlers(ScpUtils pluginInstance)
        {
            this.pluginInstance = pluginInstance;
        }

        [PluginEvent(ServerEvent.PlayerDeath)]
        internal void OnPlayerDeath(PluginAPI.Core.Player player, PluginAPI.Core.Player attacker, DamageHandlerBase damageHandler)
        {
            //To test: check if player is null while suiciding
            if (player == null) return;
            if (damageHandler is UniversalDamageHandler damage)
            {
                DamageType = (UniversalDamageHandler)damageHandler;
            }
            else return;
          
            if (Cuffed.ContainsKey(player)) Cuffed.Remove(player);
            if ((player.Team == Team.SCPs || (pluginInstance.configs.AreTutorialsSCP && player.Role == PlayerRoles.RoleTypeId.Tutorial)) && PluginAPI.Core.Round.IsRoundStarted && pluginInstance.configs.EnableSCPSuicideAutoWarn && !TemporarilyDisabledWarns)
            {
                if (DatabasePlayer.GetDatabasePlayer(player).LastRespawn.AddSeconds(pluginInstance.configs.WarnImmunityTeslaRespawn) <= DateTime.Now && DamageType.TranslationId == DeathTranslations.Tesla.Id)
                {
                    return;
                }
                if ((DateTime.Now - lastTeslaEvent).Seconds >= pluginInstance.configs.Scp079TeslaEventWait)
                {
                    if (DamageType.TranslationId == DeathTranslations.Tesla.Id || DamageType.TranslationId == DeathTranslations.Crushed.Id || (DamageType.TranslationId == DeathTranslations.Unknown.Id && DamageType.Damage >= 50000) || (DamageType.TranslationId == DeathTranslations.Explosion.Id && player.UserId == attacker.UserId))
                    {
                        pluginInstance.Functions.LogWarn(player, DamageType.TranslationId.ToString());
                        pluginInstance.Functions.OnQuitOrSuicide(player);
                    }
                    else if ((DamageType.TranslationId == DeathTranslations.Unknown.Id && DamageType.Damage == -1f) && pluginInstance.configs.QuitEqualsSuicide)
                    {
                        pluginInstance.Functions.LogWarn(player, "Disconnect");
                        pluginInstance.Functions.OnQuitOrSuicide(player);
                    }
                }
            }

            if (pluginInstance.configs.NotifyLastPlayerAlive)
            {           

                List<PluginAPI.Core.Player> team = PluginAPI.Core.Player.GetPlayers().FindAll(x => x.Team == player.Team);

             
              
                if (team.Count - 1 == 1)
                {
                    if (team[0] == player)
                    {
                        team[1].ReceiveHint(pluginInstance.configs.LastPlayerAliveNotificationText, pluginInstance.configs.LastPlayerAliveMessageDuration);
                    }
                    else
                    {
                        team[0].ReceiveHint(pluginInstance.configs.LastPlayerAliveNotificationText, pluginInstance.configs.LastPlayerAliveMessageDuration);
                    }
                }
            }


            if (player.IsSCP || player.Role == PlayerRoles.RoleTypeId.Tutorial && pluginInstance.configs.AreTutorialsSCP)
            {
                if (!pluginInstance.configs.ShowDeathMessage0492 && player.Role == PlayerRoles.RoleTypeId.Scp0492) return;
                if (player.UserId == attacker.UserId)
                {
                    var message = pluginInstance.configs.ScpSuicideMessage.Content;
                    message = message.Replace("%playername%", player.Nickname).Replace("%scpname%", player.Role.ToString()).Replace("%reason%", DamageType.TranslationId.ToString());
                    PluginAPI.Core.Server.SendBroadcast(message, pluginInstance.configs.ScpSuicideMessage.Duration, pluginInstance.configs.ScpSuicideMessage.Type);
                }

                else if (player.UserId != attacker.UserId)
                {                  
                        if (pluginInstance.configs.ScpSuicideMessage.Show)
                        {
                            var message = pluginInstance.configs.ScpSuicideMessage.Content;
                            message = message.Replace("%playername%", player.Nickname).Replace("%scpname%", player.Role.ToString()).Replace("%reason%", DamageType.TranslationId.ToString());
                          
                        PluginAPI.Core.Server.SendBroadcast(message, pluginInstance.configs.ScpSuicideMessage.Duration, pluginInstance.configs.ScpSuicideMessage.Type);
                        }                    
                }
                else
                {
                    if (pluginInstance.configs.ScpDeathMessage.Show)
                    {
                        var message = pluginInstance.configs.ScpDeathMessage.Content;
                        message = message.Replace("%playername%", player.Nickname).Replace("%scpname%", player.Role.ToString()).Replace("%killername%", attacker.Nickname).Replace("%reason%", DamageType.TranslationId.ToString());
                        PluginAPI.Core.Server.SendBroadcast(message, pluginInstance.configs.ScpDeathMessage.Duration, pluginInstance.configs.ScpDeathMessage.Type);
                    }
                }

                }
            }
        

        [PluginEvent(ServerEvent.RoundStart)]
        internal void OnRoundStarted()
        {
            Timing.CallDelayed(7f, () =>
            {
                foreach (var player in PluginAPI.Core.Player.GetPlayers())
                {
                    if (player.Role == PlayerRoles.RoleTypeId.None) player.SetRole(PlayerRoles.RoleTypeId.ClassD, PlayerRoles.RoleChangeReason.RemoteAdmin);
                }
            });
        }

        [PluginEvent(ServerEvent.PlayerChangeRole)]
        internal void OnChangingRole(PluginAPI.Core.Player player, PlayerRoles.PlayerRoleBase oldRole, PlayerRoles.RoleTypeId newRole, PlayerRoles.RoleChangeReason changeReason)
        {
            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.LastRespawn = DateTime.Now;
            //    Log.Info($"{ev.Player.Nickname} - {ev.Reason}");
            /*    if (ev.Player.IsOverwatchEnabled && ev.Reason != Exiled.API.Enums.SpawnReason.ForceClass)
                {
                    if ((PlayerRoles.Team)ev.NewRole == PlayerRoles.Team.SCPs)
                    {
                        pluginInstance.Functions.RandomScp(ev.Player, ev.NewRole);
                    }
                    ev.IsAllowed = false;
                    return;
                }
                if (ev.Player.Role == PlayerRoles.RoleTypeId.Overwatch || ev.NewRole == PlayerRoles.RoleTypeId.Overwatch)
                {
                    if ((PlayerRoles.Team)ev.NewRole == PlayerRoles.Team.FoundationForces || (PlayerRoles.Team)ev.NewRole == PlayerRoles.Team.ChaosInsurgency && Respawn.IsSpawning)
                    {
                        ev.IsAllowed = false;
                    }                
                    databasePlayer.OverwatchActive = ev.NewRole == PlayerRoles.RoleTypeId.Overwatch;
                } */
        }

        [PluginEvent(ServerEvent.PlayerKicked)]
        void OnKicking(PluginAPI.Core.Player player, ICommandSender sender, string reason)
        {
            if (!KickedList.Contains(player)) KickedList.Add(player);
        }

        [PluginEvent(ServerEvent.PlayerBanned)]
        void OnBanned(PluginAPI.Core.Player player, ICommandSender issuer, string reason, long duration)
        {
            if (!KickedList.Contains(player)) KickedList.Add(player);
        }

        [PluginEvent(ServerEvent.PlayerRemoveHandcuffs)]
        void OnPlayerUnhandCuff(PluginAPI.Core.Player player, PluginAPI.Core.Player target)
        {
            if (pluginInstance.configs.HandCuffOwnership)
            {
                if (!Cuffed.ContainsKey(target)) return;

                else if (Cuffed.FirstOrDefault(x => x.Key == target).Key.Role == PlayerRoles.RoleTypeId.ClassD && player.Role.GetTeam() == Team.ChaosInsurgency)
                {
                    Cuffed.Remove(target);
                }

                else if (Cuffed.FirstOrDefault(x => x.Key == target).Key.Role == PlayerRoles.RoleTypeId.Scientist && player.Role.GetTeam() == Team.FoundationForces)
                {
                    Cuffed.Remove(target);
                }

                else if (Cuffed.FirstOrDefault(x => x.Key == target).Key.Role == player.Role)
                {
                    Cuffed.Remove(target);
                }


                else if (Cuffed[target] == player.UserId)
                {
                    Cuffed.Remove(target);
                }

                else
                {
                    if (pluginInstance.configs.UnhandCuffDenied.Show) player.ReceiveHint(pluginInstance.configs.UnhandCuffDenied.Content, pluginInstance.configs.UnhandCuffDenied.Duration);
                    // Let's try to disallow player cuffing 
                    return;
                }
            }
        }

        [PluginEvent(ServerEvent.PlayerHandcuff)]
        internal void OnPlayerHandcuff(PluginAPI.Core.Player player, PluginAPI.Core.Player target)
        {
            if (pluginInstance.configs.HandCuffOwnership)
            {
                if (Cuffed.ContainsKey(target)) Cuffed.Remove(target);
                Cuffed.Add(target, player.UserId);
            }
        }

        [PluginEvent(ServerEvent.RoundStart)]
        void OnRoundRestart()
        {
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                pluginInstance.Functions.SaveData(player);
            }
        }


        [PluginEvent(ServerEvent.PlayerPreauth)]
        internal void OnPlayerPreauth(string userId, string ipAddress, string expiration, CentralAuthPreauthFlags centralFlags, string region, byte[] signature, ConnectionRequest connectionRequest, int readerStartPosition)
        {
            if (PreauthTime.ContainsKey(userId))
            {
                PreauthTime.Remove(userId);
            }
            PreauthTime.Add(userId, DateTime.Now);
        }


        [PluginEvent(ServerEvent.RoundEnd)]
        internal void OnRoundEnded(RoundSummary.LeadingTeam leadingTeam)
        {
            TemporarilyDisabledWarns = true;

            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                pluginInstance.Functions.SaveData(player);
            }
            Cuffed.Clear();
        }

        [PluginEvent(ServerEvent.TeamRespawn)]
        internal void OnTeamRespawn(SpawnableTeamType team)
        {

            if (team == SpawnableTeamType.ChaosInsurgency)
            {
                ChaosRespawnCount++;
                LastChaosRespawn = DateTime.Now;
            }

            else if (team == SpawnableTeamType.NineTailedFox)
            {
                MtfRespawnCount++;
                LastMtfRespawn = DateTime.Now;
            }

        }

        [PluginEvent(ServerEvent.PlayerLeft)]
        internal void OnPlayerDestroy(PluginAPI.Core.Player player)
        {
            pluginInstance.Functions.SaveData(player);
            if (!Cuffed.ContainsKey(player)) Cuffed.Remove(player);
        }

        [PluginEvent(ServerEvent.Scp096AddingTarget)]
        internal void On096AddTarget(PluginAPI.Core.Player player, PluginAPI.Core.Player target, bool isForLook)
        {
            if (pluginInstance.configs.Scp096TargetNotifyEnabled)
            {
                target.ReceiveHint(pluginInstance.configs.Scp096TargetNotifyText, pluginInstance.configs.Scp096TargetMessageDuration);
            }
        }

        [PluginEvent(ServerEvent.WaitingForPlayers)]
        internal void OnWaitingForPlayers()
        {
            TemporarilyDisabledWarns = false;
            ChaosRespawnCount = 0;
            MtfRespawnCount = 0;
            SwapRequest.Clear();
        }

        [PluginEvent(ServerEvent.Scp079UseTesla)]
        internal void On079TeslaEvent(PluginAPI.Core.Player player, int amount, Scp079HudTranslation reason)
        {
            lastTeslaEvent = DateTime.Now;
        }

        [PluginEvent(ServerEvent.PlayerDamage)]
        internal bool OnPlayerHurt(PluginAPI.Core.Player player, PluginAPI.Core.Player attacker, DamageHandlerBase damageHandler)
        {
            if ( player == null || attacker == null) return true;

            if (pluginInstance.configs.CuffedImmunityPlayers?.ContainsKey(player.Role.GetTeam()) == true)
            {            
                return !(pluginInstance.Functions.IsTeamImmune(player, attacker) && pluginInstance.Functions.CuffedCheck(player) && pluginInstance.Functions.CheckSafeZones(player));
            }
            else return true;
        }

        [PluginEvent(ServerEvent.PlayerJoined)]
        internal void OnPlayerVerify(PluginAPI.Core.Player joinedPlayer)
        {
            if (!Database.LiteDatabase.GetCollection<Player>().Exists(player => player.Id == DatabasePlayer.GetRawUserId(joinedPlayer)))
            {
                pluginInstance.DatabasePlayerData.AddPlayer(joinedPlayer);
            }

            Player databasePlayer = joinedPlayer.GetDatabasePlayer();

            if (!Database.LiteDatabase.GetCollection<DatabaseIp>().Exists(alias => alias.Id == DatabasePlayer.GetRawUserId(joinedPlayer.IpAddress)))
            {                
                pluginInstance.DatabasePlayerData.AddIp(joinedPlayer.IpAddress, joinedPlayer.UserId);
            }




            if (Database.PlayerData.ContainsKey(joinedPlayer))
            {
                return;
            }
            Database.PlayerData.Add(joinedPlayer, databasePlayer);
            if (PreauthTime.ContainsKey(joinedPlayer.UserId))
            {
                databasePlayer.LastSeen = PreauthTime[joinedPlayer.UserId];
                PreauthTime.Remove(joinedPlayer.UserId);
            }
            else databasePlayer.LastSeen = DateTime.Now;
            databasePlayer.Name = joinedPlayer.Nickname;
            databasePlayer.Ip = joinedPlayer.IpAddress;

            if (databasePlayer.FirstJoin == DateTime.MinValue)
            {
                databasePlayer.FirstJoin = DateTime.Now;
            }

            if (pluginInstance.configs.WelcomeMessage.Show)
            {
                var message = pluginInstance.configs.WelcomeMessage.Content;
                message = message.Replace("%player%", joinedPlayer.Nickname);
                joinedPlayer.SendBroadcast(message, pluginInstance.configs.WelcomeMessage.Duration, pluginInstance.configs.WelcomeMessage.Type, false);
            }

            if (pluginInstance.Functions.CheckAsnPlayer(joinedPlayer))
            {               
                joinedPlayer.Kick($"Auto-Kick: {pluginInstance.configs.AsnKickMessage}");
            }
            else
            {
                pluginInstance.Functions.PostLoadPlayer(joinedPlayer);
            }

            pluginInstance.Functions.IpCheck(joinedPlayer);
            //  if (databasePlayer.OverwatchActive) ev.Player.IsOverwatchEnabled = true;
        }

        [PluginEvent(ServerEvent.PlayerSpawn)]
        internal void OnPlayerSpawn(PluginAPI.Core.Player player, PlayerRoles.RoleTypeId role)
        {
            Player databasePlayer = player.GetDatabasePlayer();
            //   if (databasePlayer.OverwatchActive) ev.Player.IsOverwatchEnabled = true;
            if (player.Team == PlayerRoles.Team.SCPs || (pluginInstance.configs.AreTutorialsSCP && player.Role == PlayerRoles.RoleTypeId.Tutorial))
            {

                if (databasePlayer.RoundBanLeft >= 1 && player.Role != PlayerRoles.RoleTypeId.Scp0492)
                {
                    Timing.CallDelayed(1.5f, () => pluginInstance.Functions.ReplacePlayer(player));
                }
                else player.GetDatabasePlayer().TotalScpGamesPlayed++;
            }
            if (player.IsSCP && pluginInstance.configs.AllowSCPSwap)
            {
                if (PluginAPI.Core.Round.Duration.TotalSeconds < pluginInstance.configs.MaxAllowedTimeScpSwapRequest)
                {
                    var seconds = Math.Round(pluginInstance.configs.MaxAllowedTimeScpSwapRequest - PluginAPI.Core.Round.Duration.TotalSeconds + 1);
                    var message = pluginInstance.configs.SwapRequestInfoBroadcast.Content;
                    message = message.Replace("%seconds%", seconds.ToString());
                    player.SendBroadcast(message, pluginInstance.configs.SwapRequestInfoBroadcast.Duration, pluginInstance.configs.SwapRequestInfoBroadcast.Type, false);
                }
            }
        }

        [PluginEvent(ServerEvent.PlayerLeft)]
        void OnPlayerLeave(PluginAPI.Core.Player player)
        {
            pluginInstance.Functions.SaveData(player);
        }

        [PluginEvent(ServerEvent.LczDecontaminationStart)]
        void OnDecontaminate() => PluginAPI.Core.Server.SendBroadcast(pluginInstance.configs.DecontaminationMessage.Content, pluginInstance.configs.DecontaminationMessage.Duration, pluginInstance.configs.DecontaminationMessage.Type);
    }

}
