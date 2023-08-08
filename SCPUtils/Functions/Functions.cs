namespace SCPUtils
{
    using CommandSystem;
    using MEC;
    using PluginAPI.Core;
    using SCPUtils.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Function : EventArgs
    {
        public CoroutineHandle RS;
        public int i = 0;
        private readonly ScpUtils pluginInstance;

        public Function(ScpUtils pluginInstance)
        {
            this.pluginInstance = pluginInstance;
        }

        public void CoroutineRestart()
        {
            TimeSpan timeParts = TimeSpan.Parse(pluginInstance.Configs.AutoRestartTimeTask);
            double timeCalc;
            timeCalc = (timeParts - DateTime.Now.TimeOfDay).TotalSeconds;
            if (timeCalc <= 0)
            {
                timeCalc += 86400;
            }

            RS = Timing.RunCoroutine(Restarter((float)timeCalc), Segment.FixedUpdate);
        }

        private IEnumerator<float> Restarter(float second)
        {
            yield return Timing.WaitForSeconds(second);
            Log.Info(pluginInstance.GetFunctions.Function[FunctionEnums.Restarter]);
            Server.Restart();
        }

        public Dictionary<string, DateTime> LastWarn { get; private set; } = new Dictionary<string, DateTime>();

        public void AutoRoundBanPlayer(PluginAPI.Core.Player player)
        {
            int rounds;
            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideBans++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Round-Ban";
            if (pluginInstance.Configs.MultiplyBanDurationEachBan == true)
            {
                rounds = databasePlayer.TotalScpSuicideBans * pluginInstance.Configs.AutoBanRoundsCount;
            }
            else
            {
                rounds = pluginInstance.Configs.AutoBanDuration;

            }
            if (pluginInstance.Configs.BroadcastSanctions)
            {
                BroadcastSuicideQuitAction(pluginInstance.GetFunctions.Function[FunctionEnums.ScpBanned].Replace("$playerNickname$", player.Nickname).Replace("$playerRole$", player.RoleName).Replace("$rounds$", rounds.ToString()));
                if (databasePlayer.RoundBanLeft >= 1) BroadcastSuicideQuitAction(pluginInstance.GetFunctions.Function[FunctionEnums.IssuesBan].Replace("$playerNickname$", player.Nickname));
            }
            databasePlayer.RoundsBan[databasePlayer.RoundsBan.Count() - 1] = rounds;
            databasePlayer.RoundBanLeft += rounds;
            if (pluginInstance.Translation.RoundBanNotification.Show)
            {
                player.ClearBroadcasts();
                var message = pluginInstance.Translation.RoundBanNotification.Content;
                message = message.Replace("%roundnumber%", databasePlayer.RoundBanLeft.ToString());
                player.SendBroadcast(message, pluginInstance.GetMotd.Motd.Duration, pluginInstance.GetMotd.Motd.Type, pluginInstance.GetMotd.Motd.ClearAll);
            }

        }

        public void AutoBanPlayer(PluginAPI.Core.Player player)
        {
            int duration;
            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideBans++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Ban";

            if (pluginInstance.Configs.MultiplyBanDurationEachBan == true) duration = databasePlayer.TotalScpSuicideBans * pluginInstance.Configs.AutoBanDuration * 60;
            else duration = pluginInstance.Configs.AutoBanDuration * 60;

            if (pluginInstance.Configs.BroadcastSanctions) BroadcastSuicideQuitAction(pluginInstance.Translation.AutoBanPlayerMessage.Content.Replace("%player.Nickname%", player.Nickname).Replace("%player.Role%", player.Role.ToString()).Replace("%duration%", (duration / 60).ToString()));

            if (pluginInstance.Configs.MultiplyBanDurationEachBan == true) databasePlayer.Expire[databasePlayer.Expire.Count() - 1] = DateTime.Now.AddMinutes((duration / 60) * databasePlayer.TotalScpSuicideBans);
            else databasePlayer.Expire[databasePlayer.Expire.Count() - 1] = DateTime.Now.AddMinutes(duration / 60);
            
            player.Ban($"Auto-Ban: {string.Format(pluginInstance.Translation.AutoBanMessage)}", duration);
        }

        public void AutoKickPlayer(PluginAPI.Core.Player player)
        {
            if (pluginInstance.Configs.BroadcastSanctions) BroadcastSuicideQuitAction($"<color=blue><SCPUtils> {player.Nickname} ({player.Role}) has been <color=red>KICKED</color> from the server for exceeding Quits / Suicides (as SCP) limit</color>");

            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideKicks++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Kick";
            player.Kick($"Auto-Kick: {pluginInstance.Translation.SuicideKickMessage}");
        }

        public void AutoWarnPlayer(PluginAPI.Core.Player player)
        {
            if (pluginInstance.Configs.BroadcastWarns) BroadcastSuicideQuitAction($"<color=blue><SCPUtils> {player.Nickname} ({player.Role}) has been <color=red>WARNED</color> for Quitting or Suiciding as SCP</color>");

            player.GetDatabasePlayer().ScpSuicideCount++;
            player.ClearBroadcasts();
            player.SendBroadcast(pluginInstance.Translation.SuicideWarnMessage.Content, pluginInstance.Translation.SuicideWarnMessage.Duration, pluginInstance.Translation.SuicideWarnMessage.Type);
        }

        public void OnQuitOrSuicide(PluginAPI.Core.Player player)
        {
            if (!pluginInstance.Configs.EnableSCPSuicideAutoWarn || pluginInstance.EventHandlers.KickedList.Contains(player) || EventHandlers.TemporarilyDisabledWarns)
                return;
            
            if (!LastWarn.ContainsKey(player.UserId)) LastWarn.Add(player.UserId, DateTime.MinValue);
            else if (LastWarn[player.UserId] >= DateTime.Now) return;
            
            Player databasePlayer = player.GetDatabasePlayer();
            float suicidePercentage = databasePlayer.SuicidePercentage;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Warn";
            
            AutoWarnPlayer(player);
            if (pluginInstance.Configs.EnableSCPSuicideAutoBan && suicidePercentage >= pluginInstance.Configs.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Configs.ScpSuicideTollerance)
                AutoBanPlayer(player);
            else if (pluginInstance.Configs.EnableSCPSuicideSoftBan && suicidePercentage >= pluginInstance.Configs.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Configs.ScpSuicideTollerance)
                AutoRoundBanPlayer(player);
            else if (pluginInstance.Configs.AutoKickOnSCPSuicide && suicidePercentage >= pluginInstance.Configs.AutoKickThreshold && suicidePercentage < pluginInstance.Configs.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Configs.ScpSuicideTollerance)
                AutoKickPlayer(player);

            LastWarn[player.UserId] = DateTime.Now.AddSeconds(5);
        }

        public void PostLoadPlayer(PluginAPI.Core.Player player)
        {

            Player databasePlayer = player.GetDatabasePlayer();

            if (!string.IsNullOrEmpty(databasePlayer.BadgeName))
            {
                UserGroup group = ServerStatic.GetPermissionsHandler()._groups[databasePlayer.BadgeName];


                if (databasePlayer.BadgeExpire >= DateTime.Now)
                {
                    player.ReferenceHub.serverRoles.SetGroup(group, false, true, true);
                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId)) ServerStatic.PermissionsHandler._members.Remove(player.UserId);

                    ServerStatic.PermissionsHandler._members.Add(player.UserId, databasePlayer.BadgeName);
                    BadgeSetEvent args = new BadgeSetEvent
                    {
                        Player = player,
                        NewBadgeName = databasePlayer.BadgeName
                    };
                    pluginInstance.Events.OnBadgeSet(args);
                }
                else
                {

                    BadgeRemovedEvent args = new BadgeRemovedEvent
                    {
                        Player = player,
                        BadgeName = databasePlayer.BadgeName
                    };

                    databasePlayer.BadgeName = "";

                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                    {
                        ServerStatic.PermissionsHandler._members.Remove(player.UserId);
                    }
                    
                    if (ServerStatic.RolesConfig.GetStringDictionary("Members").ContainsKey(player.UserId))
                    {
                        UserGroup previous = ServerStatic.GetPermissionsHandler()._groups[ServerStatic.RolesConfig.GetStringDictionary("Members")[player.UserId]];
                        ServerStatic.PermissionsHandler._members.Add(player.UserId, ServerStatic.RolesConfig.GetStringDictionary("Members")[player.UserId]);
                        player.ReferenceHub.serverRoles.SetGroup(previous, false, true, true);
                    }

                    pluginInstance.Events.OnBadgeRemoved(args);
                }
            }

            Timing.CallDelayed(1.5f, () =>
            {
                if (!string.IsNullOrEmpty(databasePlayer.ColorPreference) && databasePlayer.ColorPreference != "None")
                {
                    if (databasePlayer.KeepPreferences || pluginInstance.Configs.KeepColorWithoutPermission) player.ReferenceHub.serverRoles.SetColor(databasePlayer.ColorPreference);
                    else databasePlayer.ColorPreference = "";
                }

                if (databasePlayer.HideBadge == true)
                {
                    if (databasePlayer.KeepPreferences || pluginInstance.Configs.KeepBadgeVisibilityWithoutPermission) player.ReferenceHub.characterClassManager.UserCode_CmdRequestHideTag();
                    else databasePlayer.HideBadge = false;
                }

                if (!string.IsNullOrEmpty(databasePlayer.CustomNickName) && databasePlayer.CustomNickName != "None")
                {
                    if (databasePlayer.KeepPreferences || pluginInstance.Configs.KeepNameWithoutPermission)
                    {
                        player.DisplayNickname = databasePlayer.CustomNickName;
                    }
                    else
                    {
                        databasePlayer.CustomNickName = "";
                    }
                }

                if (pluginInstance.Configs.AutoKickBannedNames && pluginInstance.Functions.CheckNickname(player.Nickname)) Timing.CallDelayed(2f, () => player.Kick("Auto-Kick: " + pluginInstance.Translation.AutoKickBannedNameMessage));
            });

            if (databasePlayer.UserNotified.Count() <= 0) return;

            if (databasePlayer.UserNotified[databasePlayer.UserNotified.Count() - 1] == false)
            {
                if (databasePlayer.SuicidePunishment[databasePlayer.UserNotified.Count() - 1] == "None") databasePlayer.UserNotified[databasePlayer.UserNotified.Count() - 1] = true;
                else
                {
                    player.ClearBroadcasts();
                    player.SendBroadcast(pluginInstance.Translation.OfflineWarnNotification.Content, pluginInstance.Translation.OfflineWarnNotification.Duration);
                    databasePlayer.UserNotified[databasePlayer.UserNotified.Count() - 1] = true;
                }
            }
            SetCommandBan(player);
        }

        public bool CheckNickname(string name)
        {
            if (pluginInstance.Configs.BannedNickNames == null)
                return false;

            foreach (string nickname in pluginInstance.Configs.BannedNickNames)
            {
                if (Regex.Match(name.ToLower(), nickname.ToLower()).Success)
                    return true;
            }
            return false;
        }

        public void LogWarn(PluginAPI.Core.Player player, string suicidetype)
        {
            if (!Round.IsRoundStarted) return;

            Player databasePlayer = player.GetDatabasePlayer();
            FixBanTime(databasePlayer);

            databasePlayer.SuicideDate.Add(DateTime.Now);
            databasePlayer.SuicideType.Add(suicidetype);
            databasePlayer.SuicideScp.Add(player.Role.ToString());
            databasePlayer.Expire.Add(DateTime.Now);
            databasePlayer.RoundsBan.Add(0);
            databasePlayer.SuicidePunishment.Add("None");
            databasePlayer.LogStaffer.Add("SCPUtils");

            if (suicidetype == "Disconnect") databasePlayer.UserNotified.Add(false);
            else databasePlayer.UserNotified.Add(true);

        }

        public void SaveData(PluginAPI.Core.Player player)
        {
            if (player.Nickname != "Dedicated Server" && player != null && Database.PlayerData.ContainsKey(player))
            {
                if ((player.Team == PlayerRoles.Team.SCPs || (pluginInstance.Configs.AreTutorialsSCP && player.Role == PlayerRoles.RoleTypeId.Tutorial)) && pluginInstance.Configs.QuitEqualsSuicide && Round.IsRoundStarted)
                {
                    if (pluginInstance.Configs.EnableSCPSuicideAutoWarn && pluginInstance.Configs.QuitEqualsSuicide && !pluginInstance.EventHandlers.KickedList.Contains(player))
                        pluginInstance.Functions.OnQuitOrSuicide(player);
                }
                Player databasePlayer = player.GetDatabasePlayer();


                if (player.DoNotTrack && !pluginInstance.Configs.IgnoreDntRequests && !pluginInstance.Configs.DntIgnoreList.Contains(player.GetGroupName()) && !databasePlayer.IgnoreDNT)
                {
                    databasePlayer.PlayTimeRecords.Clear();
                    //databasePlayer.PlaytimeSessionsLog.Clear();
                    databasePlayer.ResetPreferences();
                    databasePlayer.FirstJoin = DateTime.MinValue;
                    databasePlayer.LastSeen = DateTime.MinValue;
                }
                else if (!player.DoNotTrack) databasePlayer.SetCurrentDayPlayTime();
                else databasePlayer.SetCurrentDayPlayTime();

                if (!string.IsNullOrEmpty(databasePlayer.BadgeName))
                if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                        ServerStatic.PermissionsHandler._members.Remove(player.UserId);

                databasePlayer.Ip = player.IpAddress;
                databasePlayer.SaveData();
                Database.PlayerData.Remove(player);
            }
            if (pluginInstance.EventHandlers.KickedList.Contains(player)) pluginInstance.EventHandlers.KickedList.Remove(player);
        }

        public void SavePlaytime(PluginAPI.Core.Player player)
        {
            if (player.Nickname != "Dedicated Server" && player != null && Database.PlayerData.ContainsKey(player))
            {
                Player databasePlayer = player.GetDatabasePlayer();
                databasePlayer.SetCurrentDayPlayTime();
                databasePlayer.LastSeen = DateTime.Now;
                databasePlayer.SaveData();
            }
        }

        private void BroadcastSuicideQuitAction(string text)
        {
            foreach (PluginAPI.Core.Player user in PluginAPI.Core.Player.GetPlayers())
            {
                if (pluginInstance.Configs.BroadcastSanctions)
                {
                    if (user.RemoteAdminAccess || user.ReferenceHub.serverRoles.RemoteAdmin)
                        user.SendBroadcast(text, 12, Broadcast.BroadcastFlags.AdminChat, false);
                }
            }
        }

        public void AdminMessage(string text)
        {
            foreach (PluginAPI.Core.Player user in PluginAPI.Core.Player.GetPlayers()) 
                if (user.RemoteAdminAccess || user.ReferenceHub.serverRoles.RemoteAdmin) 
                    user.SendBroadcast(text, 15, Broadcast.BroadcastFlags.AdminChat, false);
        }

        public bool IsTeamImmune(PluginAPI.Core.Player player, PluginAPI.Core.Player attacker)
        {
            if (pluginInstance.Configs.CuffedImmunityPlayers[player.Team]?.Any() == true)
            {

                if (pluginInstance.Configs.CuffedImmunityPlayers[player.Team].Contains(attacker.Team)) return true;
                else return false;
            }
            else
            {
                Log.Error($"Detected invalid setting on cuffed_immunity_players! Key: {player.Team}, List cannot be null!");
                return false;
            }

        }

        public bool CuffedCheck(PluginAPI.Core.Player player)
        {
            if (pluginInstance.Configs.CuffedProtectedTeams?.Any() == true)
            {
                if (pluginInstance.Configs.CuffedProtectedTeams.Contains(player.Team) && player.IsDisarmed) return true;
                else if (!pluginInstance.Configs.CuffedProtectedTeams.Contains(player.Team)) return true;
                else return false;
            }
            else return true;
        }


        public bool CheckSafeZones(PluginAPI.Core.Player player)
        {
            if (pluginInstance.Configs.CuffedSafeZones == null)
            {
                Log.Error($"Detected invalid setting on cuffed_safe_zones! Key cannot be null!");
                return false;
            }

            else if (pluginInstance.Configs.CuffedSafeZones[player.Team]?.Any() == true)
            {
                if (pluginInstance.Configs.CuffedSafeZones[player.Team].Contains(player.Room.Zone)) return true;
                else return false;
            }

            else
            {
                Log.Error($"Detected invalid setting on cuffed_safe_zones! Key: {player.Team}, List cannot be null!");
                return false;
            }

        }

        public bool CheckAsnPlayer(PluginAPI.Core.Player player)
        {
            Player databasePlayer = player.GetDatabasePlayer();
            if (pluginInstance.Configs.ASNBlacklist.IsEmpty()) return false;

            if (pluginInstance.Configs.ASNBlacklist.Contains(player.ReferenceHub.characterClassManager.Asn) && !databasePlayer.ASNWhitelisted) return true;
            else return false;
        }

        public void FixBanTime(Player databasePlayer)
        {
            if (databasePlayer.SuicideDate.Count() != databasePlayer.Expire.Count())
            {
                databasePlayer.Expire.Clear();
                for (var i = 0; i < databasePlayer.SuicideDate.Count(); i++)
                {
                    databasePlayer.Expire.Add(DateTime.MinValue);
                }
                databasePlayer.SaveData();
            }
        }

        public void FixBanRounds(Player databasePlayer)
        {
            if (databasePlayer.SuicideDate.Count() != databasePlayer.SuicideDate.Count())
            {
                databasePlayer.RoundsBan.Clear();
                for (var i = 0; i < databasePlayer.SuicideDate.Count(); i++)
                {
                    databasePlayer.RoundsBan.Add(0);
                }
                databasePlayer.SaveData();
            }
        }

        public void ReplacePlayer(PluginAPI.Core.Player player)
        {
            Player databasePlayer = player.GetDatabasePlayer();

            var list = PluginAPI.Core.Player.GetPlayers();
            list.Remove(player);
            list.RemoveAll(x => x.IsSCP);
            list.RemoveAll(x => x.Role == PlayerRoles.RoleTypeId.Tutorial);

            if (list.Count() == 0)
            {
                Log.Info("[SCPUtils] Couldnt find a player to replace the banned one!");
                return;
            }

            var id = UnityEngine.Random.Range(0, list.Count - 1);
            var role = player.Role;
            ReplacePlayerEvent args = new ReplacePlayerEvent
            {
                BannedPlayer = player,
                ReplacedPlayer = list[id],
                ScpRole = player.Role,
                NormalRole = list[id].Role
            };

            player.SetRole(list[id].Role);
            list[id].SetRole(role);

            pluginInstance.Events.OnReplacePlayerEvent(args);

            databasePlayer.RoundBanLeft--;
            if (pluginInstance.Translation.RoundBanNotification.Show)
            {
                player.ClearBroadcasts();
                var message = pluginInstance.Translation.RoundBanSpawnNotification.Content;
                message = message.Replace("%roundnumber%", databasePlayer.RoundBanLeft.ToString());
                player.SendBroadcast(message, pluginInstance.Translation.RoundBanSpawnNotification.Duration, pluginInstance.Translation.RoundBanSpawnNotification.Type, false);
            }
        }

        public void RandomScp(PluginAPI.Core.Player player, PlayerRoles.RoleTypeId role)
        {
            if (role == PlayerRoles.RoleTypeId.None) return;
            Player databasePlayer = player.GetDatabasePlayer();
            var list = PluginAPI.Core.Player.GetPlayers();
            list.RemoveAll(x => x.Role != PlayerRoles.RoleTypeId.ClassD);
            if (list.Count == 0)
            {
                RandomScp2(player, role);
                return;
            }

            var id = UnityEngine.Random.Range(0, list.Count - 1);

            Timing.CallDelayed(2f, () =>
            {
                if (list[id] != null)
                {
                    list[id].SetRole(role);
                }
                else RandomScp2(player, role);
            });
        }

        public void RandomScp2(PluginAPI.Core.Player player, PlayerRoles.RoleTypeId role)
        {
            Player databasePlayer = player.GetDatabasePlayer();
            var list = PluginAPI.Core.Player.GetPlayers();
            list.Remove(player);
            list.RemoveAll(x => x.IsSCP);
            list.RemoveAll(x => x.Role == PlayerRoles.RoleTypeId.Tutorial);
            if (list.Count == 0) return;
            var id = UnityEngine.Random.Range(0, list.Count - 1);

            list[id]?.SetRole(role);
        }

        public void IpCheck(PluginAPI.Core.Player player)
        {
            var databaseIp = GetIp.GetIpAddress(player.IpAddress);
            if (databaseIp == null)
            {
                player.AddIp();
                databaseIp = GetIp.GetIpAddress(player.IpAddress);
            }

            if (!databaseIp.UserIds.Contains(player.UserId))
            {
                databaseIp.UserIds.Add(player.UserId);
                databaseIp.SaveIp();

            }
            if (pluginInstance.Configs.ASNWhiteslistMultiAccount?.Any() ?? true)
            {
                if (player.GetDatabasePlayer().MultiAccountWhiteList) return;
                CheckIp(player);
                return;
            }
            if (!pluginInstance.Configs.ASNWhiteslistMultiAccount.Contains(player.ReferenceHub.characterClassManager.Asn) && !player.GetDatabasePlayer().MultiAccountWhiteList) CheckIp(player);
        }

        public void CheckIp(PluginAPI.Core.Player player)
        {
            var databaseIp = GetIp.GetIpAddress(player.IpAddress);
            if (databaseIp.UserIds.Count() > 1)
            {
                MultiAccountEvent args = new MultiAccountEvent
                {
                    Player = player,
                    UserIds = databaseIp.UserIds
                };

                pluginInstance.Events.OnMultiAccountEvent(args);

                if (pluginInstance.Configs.MultiAccountBroadcast)
                {
                    AdminMessage($"Multi-Account detected on {player.Nickname} - ID: {player.PlayerId} Number of accounts: {databaseIp.UserIds.Count()}");
                }

                foreach (var userId in databaseIp.UserIds)
                {
                    if (player.IsMuted) return;
                    if (VoiceChat.VoiceChatMutes.QueryLocalMute(userId))
                    {
                        DiscordWebHook.Message(userId, player);
                        AdminMessage($"<color=red><size=25>Mute evasion detected on {player.Nickname} ID: {player.PlayerId} Userid of muted user: {userId}</size></color>");
                        if (pluginInstance.Configs.AutoMute) player.Mute(false);
                    }
                }
            }
        }

        public bool CheckCommandCooldown(ICommandSender sender)
        {
            if (((CommandSender)sender).Nickname.Equals("SERVER CONSOLE") || sender.CheckPermission(PlayerPermissions.AdminChat))
                return false;

            var player = PluginAPI.Core.Player.Get(((CommandSender)sender).SenderId);
            if (!pluginInstance.EventHandlers.LastCommand.ContainsKey(player))
            {
                pluginInstance.EventHandlers.LastCommand.Add(player, DateTime.Now.AddSeconds(pluginInstance.Configs.CommandCooldownSeconds));
                return false;
            }
            else if (DateTime.Now <= pluginInstance.EventHandlers.LastCommand[player])
            {
                if (pluginInstance.Configs.CommandAbuseReport)
                {
                    Log.Info($"[ABUSE-REPORT] {player.Nickname} - {player.UserId} tried to spam commands!");
                }
                return true;
            }
            else
            {
                pluginInstance.EventHandlers.LastCommand[player] = DateTime.Now.AddSeconds(pluginInstance.Configs.CommandCooldownSeconds);
                return false;
            }
        }

        public void SetCommandBan(PluginAPI.Core.Player player)
        {
            var databasePlayer = player.GetDatabasePlayer();

            foreach (KeyValuePair<DateTime, string> time in databasePlayer.Restricted)
            {
                if (time.Key >= DateTime.Now)
                {
                    player.SendConsoleMessage($"You are banned from using commands until {time.Key}!\nReason: {time.Value}", "red");
                    
                    if (!pluginInstance.EventHandlers.LastCommand.ContainsKey(player)) pluginInstance.EventHandlers.LastCommand.Add(player, time.Key);
                    else pluginInstance.EventHandlers.LastCommand[player] = time.Key;
                    
                    return;
                }
            }
        }

        public bool IsSuicide(PluginAPI.Core.Player player, PluginAPI.Core.Player attacker)
        {
            return player.UserId == attacker.UserId;
        }

        public string GetGroupName(PluginAPI.Core.Player player)
        {
            if (ServerStatic.PermissionsHandler._members.TryGetValue(player.UserId, out string name)) return name;
            else return "none";
        }
    }
}
