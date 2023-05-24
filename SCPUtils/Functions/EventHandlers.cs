namespace SCPUtils
{
    using MEC;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PluginAPI.Core.Attributes;
    using CommandSystem;
    using ServerEvent = PluginAPI.Enums.ServerEventType;
    using PlayerRoles;
    using LiteNetLib;
    using Respawning;
    using PlayerRoles.PlayableScps.Scp079;
    using PlayerStatsSystem;
    using PluginAPI.Core;

    public class EventHandlers
    {
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

        [PluginEvent(ServerEvent.PlayerDeath)]
        public void OnPlayerDeath(PluginAPI.Core.Player player, PluginAPI.Core.Player attacker, DamageHandlerBase damageHandler)
        {
            //To test: check if player is null while suiciding
            //PluginAPI.Core.Log.Info("NULL");
            //if (player is null) return;
            PluginAPI.Core.Log.Info("TEST 20");
            if (damageHandler is UniversalDamageHandler damage)
            {
                DamageType = (UniversalDamageHandler)damageHandler;
            }
            else return;
            PluginAPI.Core.Log.Info("TEST 21");
            if (Cuffed.ContainsKey(player)) Cuffed.Remove(player);
            /*if ((player.Team == Team.SCPs || (ScpUtils.StaticInstance.Configs.AreTutorialsSCP && player.Role == RoleTypeId.Tutorial)) && PluginAPI.Core.Round.IsRoundStarted && ScpUtils.StaticInstance.Configs.EnableSCPSuicideAutoWarn && !TemporarilyDisabledWarns)
            {
                PluginAPI.Core.Log.Info("TEST 22");
                if (DatabasePlayer.GetDatabasePlayer(player).LastRespawn.AddSeconds(ScpUtils.StaticInstance.Configs.WarnImmunityTeslaRespawn) <= DateTime.Now && DamageType.TranslationId == DeathTranslations.Tesla.Id)
                {
                    return;
                }
                PluginAPI.Core.Log.Info("TEST 23");
                if ((DateTime.Now - lastTeslaEvent).Seconds >= ScpUtils.StaticInstance.Configs.Scp079TeslaEventWait)
                {
                    PluginAPI.Core.Log.Info("TEST 24");
                    //if (DamageType.TranslationId == DeathTranslations.Tesla.Id || DamageType.TranslationId == DeathTranslations.Crushed.Id || (DamageType.TranslationId == DeathTranslations.Unknown.Id && DamageType.Damage >= 50000) || (DamageType.TranslationId == DeathTranslations.Explosion.Id && player.UserId == attacker.UserId))
                    if (DamageType.TranslationId == DeathTranslations.Crushed.Id)
                    {
                        PluginAPI.Core.Log.Info("TEST 25");
                        ScpUtils.StaticInstance.Functions.LogWarn(player, DamageType.TranslationId.ToString());
                        ScpUtils.StaticInstance.Functions.OnQuitOrSuicide(player);
                    }
                    else if ((DamageType.TranslationId == DeathTranslations.Unknown.Id && DamageType.Damage == -1f) && ScpUtils.StaticInstance.Configs.QuitEqualsSuicide)
                    {
                        PluginAPI.Core.Log.Info("TEST 26");
                        ScpUtils.StaticInstance.Functions.LogWarn(player, "Disconnect");
                        ScpUtils.StaticInstance.Functions.OnQuitOrSuicide(player);
                    }
                }
                PluginAPI.Core.Log.Info("TEST 27");
            }*/
            //if ((DateTime.Now - lastTeslaEvent).Seconds >= ScpUtils.StaticInstance.Configs.Scp079TeslaEventWait)
            //{
                PluginAPI.Core.Log.Info("TEST 24");
                if (DamageType.TranslationId == DeathTranslations.Tesla.Id || DamageType.TranslationId == DeathTranslations.Crushed.Id || (DamageType.TranslationId == DeathTranslations.Unknown.Id && DamageType.Damage >= 50000) || (DamageType.TranslationId == DeathTranslations.Explosion.Id && player.UserId == attacker.UserId))
                //if (DamageType.TranslationId == DeathTranslations.Crushed.Id)
                {
                    PluginAPI.Core.Log.Info("TEST 25");
                    ScpUtils.StaticInstance.Functions.LogWarn(player, DamageType.TranslationId.ToString());
                    ScpUtils.StaticInstance.Functions.OnQuitOrSuicide(player);
                }
                else if (DamageType.TranslationId == DeathTranslations.Unknown.Id && ScpUtils.StaticInstance.Configs.QuitEqualsSuicide)
                {
                    PluginAPI.Core.Log.Info("TEST 26");
                    ScpUtils.StaticInstance.Functions.LogWarn(player, "Disconnect");
                    ScpUtils.StaticInstance.Functions.OnQuitOrSuicide(player);
                }
            //}

            if (ScpUtils.StaticInstance.Configs.NotifyLastPlayerAlive)
            {           

                List<PluginAPI.Core.Player> team = PluginAPI.Core.Player.GetPlayers().FindAll(x => x.Team == player.Team);

             
              
                if (team.Count - 1 == 1)
                {
                    if (team[0] == player)
                    {
                        team[1].ReceiveHint(ScpUtils.StaticInstance.Translation.LastPlayerAliveNotificationText, ScpUtils.StaticInstance.Configs.LastPlayerAliveMessageDuration);
                    }
                    else
                    {
                        team[0].ReceiveHint(ScpUtils.StaticInstance.Translation.LastPlayerAliveNotificationText, ScpUtils.StaticInstance.Configs.LastPlayerAliveMessageDuration);
                    }
                }
            }


            if (player.IsSCP || player.Role == PlayerRoles.RoleTypeId.Tutorial && ScpUtils.StaticInstance.Configs.AreTutorialsSCP)
            {
                if (!ScpUtils.StaticInstance.Configs.ShowDeathMessage0492 && player.Role == PlayerRoles.RoleTypeId.Scp0492) return;
                if (player.UserId == attacker.UserId)
                {
                    var message = ScpUtils.StaticInstance.Translation.ScpSuicideMessage.Content;
                    message = message.Replace("%playername%", player.Nickname).Replace("%scpname%", player.Role.ToString()).Replace("%reason%", DamageType.TranslationId.ToString());
                    PluginAPI.Core.Server.SendBroadcast(message, ScpUtils.StaticInstance.Translation.ScpSuicideMessage.Duration, ScpUtils.StaticInstance.Translation.ScpSuicideMessage.Type);
                }

                else if (player.UserId != attacker.UserId)
                {                  
                        if (ScpUtils.StaticInstance.Translation.ScpSuicideMessage.Show)
                        {
                            var message = ScpUtils.StaticInstance.Translation.ScpSuicideMessage.Content;
                            message = message.Replace("%playername%", player.Nickname).Replace("%scpname%", player.Role.ToString()).Replace("%reason%", DamageType.TranslationId.ToString());
                          
                        PluginAPI.Core.Server.SendBroadcast(message, ScpUtils.StaticInstance.Translation.ScpSuicideMessage.Duration, ScpUtils.StaticInstance.Translation.ScpSuicideMessage.Type);
                        }                    
                }
                else
                {
                    if (ScpUtils.StaticInstance.Translation.ScpDeathMessage.Show)
                    {
                        var message = ScpUtils.StaticInstance.Translation.ScpDeathMessage.Content;
                        message = message.Replace("%playername%", player.Nickname).Replace("%scpname%", player.Role.ToString()).Replace("%killername%", attacker.Nickname).Replace("%reason%", DamageType.TranslationId.ToString());
                        PluginAPI.Core.Server.SendBroadcast(message, ScpUtils.StaticInstance.Translation.ScpDeathMessage.Duration, ScpUtils.StaticInstance.Translation.ScpDeathMessage.Type);
                    }
                }

                }
            }
        

        [PluginEvent(ServerEvent.RoundStart)]
        public void OnRoundStarted()
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
        public void OnChangingRole(PluginAPI.Core.Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
        {
            Player databasePlayer = player.GetDatabasePlayer();
            //databasePlayer.LastRespawn = DateTime.Now;
            PluginAPI.Core.Log.Info($"{player.Nickname} - {changeReason}");
            if (newRole is RoleTypeId.Overwatch && player.RemoteAdminAccess && changeReason is RoleChangeReason.RemoteAdmin)
            {
                databasePlayer.OverwatchActive = true;
            }
            else if (newRole is RoleTypeId.Spectator)
            {
                databasePlayer.OverwatchActive = false;
            }
        }

        [PluginEvent(ServerEvent.PlayerKicked)]
        public void OnKicking(PluginAPI.Core.Player player, ICommandSender sender, string reason)
        {
            if (!KickedList.Contains(player)) KickedList.Add(player);
        }

        [PluginEvent(ServerEvent.PlayerBanned)]
        public void OnBanned(PluginAPI.Core.Player player, ICommandSender issuer, string reason, long duration)
        {
            if (!KickedList.Contains(player)) KickedList.Add(player);
        }

        [PluginEvent(ServerEvent.PlayerRemoveHandcuffs)]
        public void OnPlayerUnhandCuff(PluginAPI.Core.Player player, PluginAPI.Core.Player target)
        {
            if (ScpUtils.StaticInstance.Configs.HandCuffOwnership)
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
                    if (ScpUtils.StaticInstance.Translation.UnhandCuffDenied.Show) player.ReceiveHint(ScpUtils.StaticInstance.Translation.UnhandCuffDenied.Content, ScpUtils.StaticInstance.Translation.UnhandCuffDenied.Duration);
                    // Let's try to disallow player cuffing 
                    return;
                }
            }
        }

        [PluginEvent(ServerEvent.PlayerHandcuff)]
        public void OnPlayerHandcuff(PluginAPI.Core.Player player, PluginAPI.Core.Player target)
        {
            if (ScpUtils.StaticInstance.Configs.HandCuffOwnership)
            {
                if (Cuffed.ContainsKey(target)) Cuffed.Remove(target);
                Cuffed.Add(target, player.UserId);
            }
        }

        [PluginEvent(ServerEvent.RoundStart)]
        public void OnRoundRestart()
        {
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                ScpUtils.StaticInstance.Functions.SaveData(player);
            }
        }


        [PluginEvent(ServerEvent.PlayerPreauth)]
        public void OnPlayerPreauth(string userId, string ipAddress, long expiration, CentralAuthPreauthFlags centralFlags, string region, byte[] signature, ConnectionRequest connectionRequest, int readerStartPosition)
        {
            if (PreauthTime.ContainsKey(userId))
            {
                PreauthTime.Remove(userId);
            }
            PreauthTime.Add(userId, DateTime.Now);
        }


        [PluginEvent(ServerEvent.RoundEnd)]
        public void OnRoundEnded(RoundSummary.LeadingTeam leadingTeam)
        {
            TemporarilyDisabledWarns = true;

            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                ScpUtils.StaticInstance.Functions.SaveData(player);
            }
            Cuffed.Clear();
        }

        [PluginEvent(ServerEvent.TeamRespawn)]
        public void OnTeamRespawn(SpawnableTeamType team)
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
        public void OnPlayerDestroy(PluginAPI.Core.Player player)
        {
            ScpUtils.StaticInstance.Functions.SaveData(player);
            if (!Cuffed.ContainsKey(player)) Cuffed.Remove(player);
        }

        [PluginEvent(ServerEvent.Scp096AddingTarget)]
        public void On096AddTarget(PluginAPI.Core.Player player, PluginAPI.Core.Player target, bool isForLook)
        {
            if (ScpUtils.StaticInstance.Configs.Scp096TargetNotifyEnabled)
            {
                target.ReceiveHint(ScpUtils.StaticInstance.Translation.Scp096TargetNotifyText, ScpUtils.StaticInstance.Configs.Scp096TargetMessageDuration);
            }
        }

        [PluginEvent(ServerEvent.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            TemporarilyDisabledWarns = false;
            ChaosRespawnCount = 0;
            MtfRespawnCount = 0;
            SwapRequest.Clear();
        }

        [PluginEvent(ServerEvent.Scp079UseTesla)]
        public void On079TeslaEvent(PluginAPI.Core.Player player, TeslaGate teslaGate)
        {
            lastTeslaEvent = DateTime.Now;
        }

        [PluginEvent(ServerEvent.PlayerDamage)]
        public bool OnPlayerHurt(PluginAPI.Core.Player player, PluginAPI.Core.Player attacker, DamageHandlerBase damageHandler)
        {
            if ( player == null || attacker == null) return true;

            if (ScpUtils.StaticInstance.Configs.CuffedImmunityPlayers?.ContainsKey(player.Role.GetTeam()) == true)
            {            
                return !(ScpUtils.StaticInstance.Functions.IsTeamImmune(player, attacker) && ScpUtils.StaticInstance.Functions.CuffedCheck(player) && ScpUtils.StaticInstance.Functions.CheckSafeZones(player));
            }
            else return true;
        }

        [PluginEvent(ServerEvent.PlayerJoined)]
        public void OnPlayerVerify(PluginAPI.Core.Player joinedPlayer)
        {
            var databasePlayer = joinedPlayer.GetDatabasePlayer();
            if (databasePlayer == null)
            {
                joinedPlayer.AddPlayer();
                databasePlayer = joinedPlayer.GetDatabasePlayer();
            }

            if (Database.PlayerData.ContainsKey(joinedPlayer))
            {
                Database.PlayerData.Remove(joinedPlayer);
            }
            Database.PlayerData.Add(joinedPlayer, databasePlayer);

            databasePlayer = Database.PlayerData[joinedPlayer];

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

            if (ScpUtils.StaticInstance.GetMotd.Motd.Show)
            {
                string message = ScpUtils.StaticInstance.GetMotd.Motd.Content
                .Replace("$player", joinedPlayer.Nickname)
                .Replace("$serverName", ServerConsole._serverName)
                .Replace("$roundDurationMinutes", $"{Round.Duration.Minutes}")
                .Replace("$roundDurationSeconds", $"{Round.Duration.Seconds}")
                .Replace("%players%", $"{PluginAPI.Core.Player.GetPlayers().Count}");
                
                joinedPlayer.SendBroadcast(message, ScpUtils.StaticInstance.GetMotd.Motd.Duration, ScpUtils.StaticInstance.GetMotd.Motd.Type, ScpUtils.StaticInstance.GetMotd.Motd.ClearAll);
            }

            if (ScpUtils.StaticInstance.Configs.ASNBlacklist.Contains(joinedPlayer.ReferenceHub.characterClassManager.Asn) && !databasePlayer.ASNWhitelisted)
            {
                joinedPlayer.Kick($"{ScpUtils.StaticInstance.commandTranslation.Kick} {ScpUtils.StaticInstance.Translation.AsnKickMessage}");
            }
            else ScpUtils.StaticInstance.Functions.PostLoadPlayer(joinedPlayer);

            ScpUtils.StaticInstance.Functions.IpCheck(joinedPlayer);
        }

        [PluginEvent(ServerEvent.PlayerSpawn)]
        public void OnPlayerSpawn(PluginAPI.Core.Player player, RoleTypeId role)
        {
            if (player.IsServer || !player.IsSCP || player == null) return;

            Player databasePlayer = player.GetDatabasePlayer();
            if (player.Team == Team.SCPs || (ScpUtils.StaticInstance.Configs.AreTutorialsSCP && player.Role == RoleTypeId.Tutorial))
            {

                if (databasePlayer.RoundBanLeft >= 1 && player.Role != RoleTypeId.Scp0492)
                {
                    Timing.CallDelayed(1.5f, () => ScpUtils.StaticInstance.Functions.ReplacePlayer(player));
                }
                else player.GetDatabasePlayer().TotalScpGamesPlayed++;
            }
            if (player.IsSCP && ScpUtils.StaticInstance.Configs.AllowSCPSwap)
            {
                if (Round.Duration.TotalSeconds < ScpUtils.StaticInstance.Configs.MaxAllowedTimeScpSwapRequest)
                {
                    var seconds = Math.Round(ScpUtils.StaticInstance.Configs.MaxAllowedTimeScpSwapRequest - Round.Duration.TotalSeconds + 1);
                    var message = ScpUtils.StaticInstance.Translation.SwapRequestInfoBroadcast.Content;

                    message = message.Replace("%seconds%", seconds.ToString());
                    player.SendBroadcast(message, ScpUtils.StaticInstance.Translation.SwapRequestInfoBroadcast.Duration, ScpUtils.StaticInstance.Translation.SwapRequestInfoBroadcast.Type, false);
                }
            }
        }

        [PluginEvent(ServerEvent.PlayerLeft)]
        public void OnPlayerLeave(PluginAPI.Core.Player player)
        {
            ScpUtils.StaticInstance.Functions.SaveData(player);
        }

        [PluginEvent(ServerEvent.LczDecontaminationStart)]
        public void OnDecontaminate() => Server.SendBroadcast(ScpUtils.StaticInstance.Translation.DecontaminationMessage.Content, ScpUtils.StaticInstance.Translation.DecontaminationMessage.Duration, ScpUtils.StaticInstance.Translation.DecontaminationMessage.Type);
    }
}