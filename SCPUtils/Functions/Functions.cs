﻿using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using SCPUtils.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SCPUtils
{
    public class Functions : EventArgs
    {
        public CoroutineHandle RS;
        public CoroutineHandle RainbowRoleCoroutine;
        public int i = 0;
        private readonly ScpUtils pluginInstance;


        public Functions(ScpUtils pluginInstance)
        {
            this.pluginInstance = pluginInstance;
        }


        public void CoroutineRainbow()
        {
            RainbowRoleCoroutine = Timing.RunCoroutine(RainbowRole(pluginInstance.Config.RainbowTagSeconds), Segment.FixedUpdate);
        }

        private IEnumerator<float> RainbowRole(float seconds)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(seconds);

                if (pluginInstance.Config.AllowRainbowTags)
                {
                    foreach (var player in Exiled.API.Features.Player.List)
                    {
                        var databasePlayer = player.GetDatabasePlayer();
                        if (databasePlayer.ColorPreference == "rainbow")
                        {
                            if (player.GlobalBadge == null)
                            {
                                int id = UnityEngine.Random.Range(0, pluginInstance.Config.AllowedRainbowtagColors.Count() - 1);
                                player.RankColor = pluginInstance.Config.AllowedRainbowtagColors[id];
                            }
                        }
                    }
                }
            }
        }

        public void CoroutineRestart()
        {
            TimeSpan timeParts = TimeSpan.Parse(pluginInstance.Config.AutoRestartTimeTask);
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
            Log.Info("Warning: Server is auto-restarting");
            Server.Restart();
        }

        public Dictionary<string, DateTime> LastWarn { get; private set; } = new Dictionary<string, DateTime>();

        public void AutoRoundBanPlayer(Exiled.API.Features.Player player)
        {
            int rounds;
            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideBans++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Round-Ban";
            if (pluginInstance.Config.MultiplyBanDurationEachBan == true)
            {
                rounds = databasePlayer.TotalScpSuicideBans * pluginInstance.Config.AutoBanRoundsCount;
            }
            else
            {
                rounds = pluginInstance.Config.AutoBanRoundsCount;

            }
            if (pluginInstance.Config.BroadcastSanctions)
            {
                var message = ScpUtils.StaticInstance.Translation.ScpRoundBanStaffBc.Replace("%user%", player.Nickname).Replace("%role%", player.Role.Type.ToString()).Replace("%rounds%", rounds.ToString());
                var messageSuicideAgain = ScpUtils.StaticInstance.Translation.ScpSuicideBroadcastActiveBan.Replace("%user%", player.Nickname);
                BroadcastSuicideQuitAction(message);
                if (databasePlayer.RoundBanLeft >= 1) BroadcastSuicideQuitAction(messageSuicideAgain);
            }
            databasePlayer.RoundsBan[databasePlayer.RoundsBan.Count() - 1] = rounds;
            databasePlayer.RoundBanLeft += rounds;
            if (pluginInstance.Config.RoundBanNotification.Show)
            {
                player.ClearBroadcasts();
                var message = pluginInstance.Config.RoundBanNotification.Content;
                message = message.Replace("%roundnumber%", databasePlayer.RoundBanLeft.ToString());
                player.Broadcast(pluginInstance.Config.WelcomeMessage.Duration, message, pluginInstance.Config.WelcomeMessage.Type, false);
            }

        }

        public void AutoBanPlayer(Exiled.API.Features.Player player)
        {
            int duration;
            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideBans++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Ban";

            if (pluginInstance.Config.MultiplyBanDurationEachBan == true)
            {
                duration = databasePlayer.TotalScpSuicideBans * pluginInstance.Config.AutoBanDuration * 60;
            }
            else
            {
                duration = pluginInstance.Config.AutoBanDuration * 60;
            }

            if (pluginInstance.Config.BroadcastSanctions)
            {
                var durationBroadcast = duration / 60;
                var message = ScpUtils.StaticInstance.Translation.ScpRoundBanStaffBc.Replace("%user%", player.Nickname).Replace("%role%", player.Role.Type.ToString()).Replace("%duration%", durationBroadcast.ToString());
                BroadcastSuicideQuitAction(message);
            }
            if (pluginInstance.Config.MultiplyBanDurationEachBan == true) databasePlayer.Expire[databasePlayer.Expire.Count() - 1] = DateTime.Now.AddMinutes((duration / 60) * databasePlayer.TotalScpSuicideBans);
            else databasePlayer.Expire[databasePlayer.Expire.Count() - 1] = DateTime.Now.AddMinutes(duration / 60);
            player.Ban(duration, $"Auto-Ban: {string.Format(pluginInstance.Config.AutoBanMessage, duration)}");
        }

        public void AutoKickPlayer(Exiled.API.Features.Player player)
        {
            if (pluginInstance.Config.BroadcastSanctions)
            {
                var message = ScpUtils.StaticInstance.Translation.ScpKickStaffBc.Replace("%user%", player.Nickname).Replace("%role%", player.Role.Type.ToString());
                BroadcastSuicideQuitAction(message);
            }

            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideKicks++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Kick";
            player.Kick($"Auto-Kick: {pluginInstance.Config.SuicideKickMessage}");
        }

        public void AutoWarnPlayer(Exiled.API.Features.Player player)
        {
            if (pluginInstance.Config.BroadcastWarns)
            {
                var message = ScpUtils.StaticInstance.Translation.ScpWarnStaffBc.Replace("%user%", player.Nickname).Replace("%role%", player.Role.Type.ToString());
                BroadcastSuicideQuitAction(message);
            }

            player.GetDatabasePlayer().ScpSuicideCount++;
            player.ClearBroadcasts();
            player.Broadcast(pluginInstance.Config.SuicideWarnMessage);
        }

        public void OnQuitOrSuicide(Exiled.API.Features.Player player)
        {

            if (!pluginInstance.Config.EnableSCPSuicideAutoWarn || pluginInstance.EventHandlers.KickedList.Contains(player) || EventHandlers.TemporarilyDisabledWarns)
            {
                return;
            }
            if (!LastWarn.ContainsKey(player.UserId))
            {
                LastWarn.Add(player.UserId, DateTime.MinValue);
            }
            else if (LastWarn[player.UserId] >= DateTime.Now)
            {
                return;
            }
            Player databasePlayer = player.GetDatabasePlayer();
            float suicidePercentage = databasePlayer.SuicidePercentage;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Warn";
            AutoWarnPlayer(player);
            if (pluginInstance.Config.EnableSCPSuicideAutoBan && suicidePercentage >= pluginInstance.Config.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Config.ScpSuicideTollerance)
            {
                AutoBanPlayer(player);
            }
            else if (pluginInstance.Config.EnableSCPSuicideSoftBan && suicidePercentage >= pluginInstance.Config.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Config.ScpSuicideTollerance)
            {
                AutoRoundBanPlayer(player);
            }
            else if (pluginInstance.Config.AutoKickOnSCPSuicide && suicidePercentage >= pluginInstance.Config.AutoKickThreshold && suicidePercentage < pluginInstance.Config.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Config.ScpSuicideTollerance)
            {
                AutoKickPlayer(player);
            }

            LastWarn[player.UserId] = DateTime.Now.AddSeconds(5);
        }

        public void PostLoadPlayer(Exiled.API.Features.Player player)
        {

            Player databasePlayer = player.GetDatabasePlayer();

            if (!string.IsNullOrEmpty(databasePlayer.BadgeName))
            {
                UserGroup group = ServerStatic.GetPermissionsHandler()._groups[databasePlayer.BadgeName];


                if (databasePlayer.BadgeExpire >= DateTime.Now)
                {
                    player.ReferenceHub.serverRoles.SetGroup(group, false, true);
                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                    {
                        ServerStatic.PermissionsHandler._members.Remove(player.UserId);
                    }

                    ServerStatic.PermissionsHandler._members.Add(player.UserId, databasePlayer.BadgeName);
                    BadgeSetEvent args = new BadgeSetEvent();
                    args.Player = player;
                    args.NewBadgeName = databasePlayer.BadgeName;
                    pluginInstance.Events.OnBadgeSet(args);

                }
                else
                {

                    BadgeRemovedEvent args = new BadgeRemovedEvent();
                    args.Player = player;
                    args.BadgeName = databasePlayer.BadgeName;
                    databasePlayer.BadgeName = "";

                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                    {
                        ServerStatic.PermissionsHandler._members.Remove(player.UserId);
                    }
                    if (ServerStatic.RolesConfig.GetStringDictionary("Members").ContainsKey(player.UserId))
                    {
                        UserGroup previous = ServerStatic.GetPermissionsHandler()._groups[ServerStatic.RolesConfig.GetStringDictionary("Members")[player.UserId]];
                        ServerStatic.PermissionsHandler._members.Add(player.UserId, ServerStatic.RolesConfig.GetStringDictionary("Members")[player.UserId]);
                        player.ReferenceHub.serverRoles.SetGroup(previous, false, true);
                    }
                    pluginInstance.Events.OnBadgeRemoved(args);
                }
            }

            Timing.CallDelayed(1.5f, () =>
            {

                if (!string.IsNullOrEmpty(databasePlayer.ColorPreference) && databasePlayer.ColorPreference != "None")
                {
                    if (player.CheckPermission("scputils.changecolor") || player.CheckPermission("scputils.playersetcolor") || databasePlayer.KeepPreferences || pluginInstance.Config.KeepColorWithoutPermission)
                    {
                        if (player.GlobalBadge == null)
                        {
                            if (databasePlayer.ColorPreference != "rainbow")
                            {
                                player.RankColor = databasePlayer.ColorPreference;
                            }
                        }
                        else
                        {
                            player.SendConsoleMessage(ScpUtils.StaticInstance.Translation.GlobalBadgeConsoleError, "red");
                        }
                    }
                    else
                    {
                        databasePlayer.ColorPreference = "";
                    }
                }

                if (databasePlayer.HideBadge == true)
                {
                    if (player.CheckPermission("scputils.badgevisibility") || databasePlayer.KeepPreferences || pluginInstance.Config.KeepBadgeVisibilityWithoutPermission)
                    {
                        player.BadgeHidden = true;
                    }
                    else
                    {
                        databasePlayer.HideBadge = false;
                    }
                }


                if (!string.IsNullOrEmpty(databasePlayer.CustomNickName) && databasePlayer.CustomNickName != "None")
                {
                    if (player.CheckPermission("scputils.changenickname") || player.CheckPermission("scputils.playersetname") || databasePlayer.KeepPreferences || pluginInstance.Config.KeepNameWithoutPermission)
                    {
                        player.DisplayNickname = databasePlayer.CustomNickName;
                    }
                    else
                    {
                        databasePlayer.CustomNickName = "";
                    }
                }

                if (!string.IsNullOrEmpty(databasePlayer.CustomBadgeName))
                {
                    if (player.Group != null)
                    {
                        player.RankName = databasePlayer.CustomBadgeName;
                    }
                    else
                    {
                        player.SendConsoleMessage(ScpUtils.StaticInstance.Translation.NoUsergroupError, "red");
                    }
                }

                if (pluginInstance.Config.AutoKickBannedNames && pluginInstance.Functions.CheckNickname(player.Nickname) && !player.CheckPermission("scputils.bypassnickrestriction"))
                {
                    Timing.CallDelayed(2f, () =>
                    {
                        player.Kick("Auto-Kick: " + pluginInstance.Config.AutoKickBannedNameMessage);
                    });
                }

            });

            if (databasePlayer.UserNotified.Count() <= 0)
            {
                return;
            }

            if (databasePlayer.UserNotified[databasePlayer.UserNotified.Count() - 1] == false)
            {
                if (databasePlayer.SuicidePunishment[databasePlayer.UserNotified.Count() - 1] == "None")
                {
                    databasePlayer.UserNotified[databasePlayer.UserNotified.Count() - 1] = true;
                }
                else
                {
                    player.ClearBroadcasts();
                    player.Broadcast(pluginInstance.Config.OfflineWarnNotification);
                    databasePlayer.UserNotified[databasePlayer.UserNotified.Count() - 1] = true;
                }
            }

            /*    if (databasePlayer.StreamerMode)
                {
                    player.ReferenceHub.serverRoles.SetGroup(new UserGroup()
                    {
                        BadgeColor = "default",
                        BadgeText = "",
                        HiddenByDefault = true
                    }, false, true, false);
                    player.SendConsoleMessage($"You have a badge but you have choosen to do not use it, if you wish to get your badge back run the .scputils_streamer_mode command again", "yellow");
                } */


            SetCommandBan(player);
        }

        public bool CheckNickname(string name)
        {
            if (pluginInstance.Config.BannedNickNames == null)
            {
                return false;
            }

            foreach (string nickname in pluginInstance.Config.BannedNickNames)
            {
                if (Regex.Match(name.ToLower(), nickname.ToLower()).Success)
                {
                    return true;
                }
            }
            return false;
        }
        public void LogWarn(Exiled.API.Features.Player player, string suicidetype)
        {

            if (Round.IsEnded) return;
            Player databasePlayer = player.GetDatabasePlayer();
            FixBanTime(databasePlayer);
            databasePlayer.SuicideDate.Add(DateTime.Now);
            databasePlayer.SuicideType.Add(suicidetype);
            databasePlayer.SuicideScp.Add(player.Role.Type.ToString());
            databasePlayer.Expire.Add(DateTime.Now);
            databasePlayer.RoundsBan.Add(0);
            databasePlayer.SuicidePunishment.Add("None");
            databasePlayer.LogStaffer.Add("SCPUtils");
            if (suicidetype == "Disconnect")
            {
                databasePlayer.UserNotified.Add(false);
            }
            else
            {
                databasePlayer.UserNotified.Add(true);
            }

        }
        public void SaveData(Exiled.API.Features.Player player)
        {
            if (player.Nickname != "Dedicated Server" && player != null)
            {
                if ((player.Role.Team == PlayerRoles.Team.SCPs || (pluginInstance.Config.AreTutorialsSCP && player.Role == PlayerRoles.RoleTypeId.Tutorial)) && pluginInstance.Config.QuitEqualsSuicide && !Round.IsEnded)
                {
                    if (pluginInstance.Config.EnableSCPSuicideAutoWarn && pluginInstance.Config.QuitEqualsSuicide && !pluginInstance.EventHandlers.KickedList.Contains(player))
                    {
                        pluginInstance.Functions.OnQuitOrSuicide(player);
                    }
                }
                Player databasePlayer = player.GetDatabasePlayer();

                if (player.DoNotTrack && !pluginInstance.Config.IgnoreDntRequests && !pluginInstance.Config.DntIgnoreList.Contains(player.GroupName) && !databasePlayer.IgnoreDNT)
                {
                    databasePlayer.PlayTimeRecords.Clear();
                    //   databasePlayer.PlaytimeSessionsLog.Clear();
                    databasePlayer.ResetPreferences();
                    databasePlayer.FirstJoin = DateTime.MinValue;
                    databasePlayer.LastSeen = DateTime.MinValue;
                }
                else if (!player.DoNotTrack)
                {
                    databasePlayer.SetCurrentDayPlayTime();
                    databasePlayer.SetCurrentDayOwPlayTime();
                }
                else
                {
                    databasePlayer.SetCurrentDayPlayTime();
                    databasePlayer.SetCurrentDayOwPlayTime();
                }

                if (!string.IsNullOrEmpty(databasePlayer.BadgeName))
                {
                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                    {
                        ServerStatic.PermissionsHandler._members.Remove(player.UserId);
                    }
                }

                databasePlayer.Ip = player.IPAddress;
                databasePlayer.SaveData();
                Database.PlayerData.Remove(player);
            }
            if (pluginInstance.EventHandlers.KickedList.Contains(player)) pluginInstance.EventHandlers.KickedList.Remove(player);
        }

        public void SavePlaytime(Exiled.API.Features.Player player)
        {
            if (player.Nickname != "Dedicated Server" && player != null)
            {
                Player databasePlayer = player.GetDatabasePlayer();
                databasePlayer.SetCurrentDayPlayTime();
                databasePlayer.LastSeen = DateTime.Now;
                databasePlayer.SaveData();
            }
        }


        private void BroadcastSuicideQuitAction(string text)
        {
            foreach (Exiled.API.Features.Player admin in Exiled.API.Features.Player.List)
            {
                if (pluginInstance.Config.BroadcastSanctions)
                {
                    if (admin.ReferenceHub.serverRoles.RemoteAdmin)
                        if (admin.Sender.CheckPermission(PlayerPermissions.AdminChat))
                        {
                            admin.Broadcast(12, text, Broadcast.BroadcastFlags.AdminChat, false);
                        }
                }
            }
        }

        public void AdminMessage(string text)
        {
            foreach (Exiled.API.Features.Player admin in Exiled.API.Features.Player.List)
            {
                if (admin.ReferenceHub.serverRoles.RemoteAdmin)
                    if (admin.Sender.CheckPermission(PlayerPermissions.AdminChat))
                    {
                        admin.Broadcast(15, text, Broadcast.BroadcastFlags.AdminChat, false);
                    }
            }
        }

        public bool IsTeamImmune(Exiled.API.Features.Player player, Exiled.API.Features.Player attacker)
        {
            if (pluginInstance.Config.CuffedImmunityPlayers[player.Role.Team]?.Any() == true)
            {

                if (pluginInstance.Config.CuffedImmunityPlayers[player.Role.Team].Contains(attacker.Role.Team))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Log.Error($"Detected invalid setting on cuffed_immunity_players! Key: {player.Role.Team}, List cannot be null!");
                return false;
            }

        }

        public bool CuffedCheck(Exiled.API.Features.Player player)
        {
            if (pluginInstance.Config.CuffedProtectedTeams?.Any() == true)
            {
                if (pluginInstance.Config.CuffedProtectedTeams.Contains(player.Role.Team) && player.IsCuffed)
                {
                    return true;
                }
                else if (!pluginInstance.Config.CuffedProtectedTeams.Contains(player.Role.Team))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }


        public bool CheckSafeZones(Exiled.API.Features.Player player)
        {
            if (pluginInstance.Config.CuffedSafeZones == null)
            {
                Log.Error($"Detected invalid setting on cuffed_safe_zones! Key cannot be null!");
                return false;
            }

            else if (pluginInstance.Config.CuffedSafeZones[player.Role.Team]?.Any() == true)
            {
                if (pluginInstance.Config.CuffedSafeZones[player.Role.Team].Contains(player.CurrentRoom.Zone))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            else
            {
                Log.Error($"Detected invalid setting on cuffed_safe_zones! Key: {player.Role.Team}, List cannot be null!");
                return false;
            }

        }

        public bool CheckAsnPlayer(Exiled.API.Features.Player player)
        {
            Player databasePlayer = player.GetDatabasePlayer();
            if (pluginInstance.Config.ASNBlacklist == null)
            {
                return false;
            }

            /*if (pluginInstance.Config.ASNBlacklist.Contains(player.ReferenceHub.characterClassManager.Asn) && !databasePlayer.ASNWhitelisted)
              {
                  return true;
              } */
            else
            {
                return false;
            }
        }


        public void FixBanTime(SCPUtils.Player databasePlayer)
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

        public void FixBanRounds(SCPUtils.Player databasePlayer)
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

        public void ReplacePlayer(Exiled.API.Features.Player player)
        {
            Player databasePlayer = player.GetDatabasePlayer();


            var list = Exiled.API.Features.Player.List.ToList();
            list.Remove(player);
            list.RemoveAll(x => x.IsScp);
            list.RemoveAll(x => x.Role == PlayerRoles.RoleTypeId.Tutorial);
            list.RemoveAll(x => x.IsOverwatchEnabled);
            if (list.Count() == 0)
            {
                Log.Info("[SCPUtils] Couldnt find a player to replace the banned one!");
                return;
            }
            var id = UnityEngine.Random.Range(0, list.Count - 1);
            var role = player.Role;
            ReplacePlayerEvent args = new ReplacePlayerEvent();
            args.BannedPlayer = player;
            args.ReplacedPlayer = list[id];
            args.ScpRole = player.Role;
            args.NormalRole = list[id].Role;
            player.Role.Set(list[id].Role);
            list[id].Role.Set(role);
            pluginInstance.Events.OnReplacePlayerEvent(args);


            databasePlayer.RoundBanLeft--;
            if (pluginInstance.Config.RoundBanNotification.Show)
            {
                player.ClearBroadcasts();
                var message = pluginInstance.Config.RoundBanSpawnNotification.Content;
                message = message.Replace("%roundnumber%", databasePlayer.RoundBanLeft.ToString());
                player.Broadcast(pluginInstance.Config.RoundBanSpawnNotification.Duration, message, pluginInstance.Config.RoundBanSpawnNotification.Type, false);
            }

        }

        public void RandomScp(Exiled.API.Features.Player player, PlayerRoles.RoleTypeId role)
        {
            if (role == PlayerRoles.RoleTypeId.None) return;
            Player databasePlayer = player.GetDatabasePlayer();
            var list = Exiled.API.Features.Player.List.ToList();
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
                    list[id].Role.Set(role);
                }
                else RandomScp2(player, role);
            });
        }

        public void RandomScp2(Exiled.API.Features.Player player, PlayerRoles.RoleTypeId role)
        {
            Player databasePlayer = player.GetDatabasePlayer();
            var list = Exiled.API.Features.Player.List.ToList();
            list.Remove(player);
            list.RemoveAll(x => x.IsScp);
            list.RemoveAll(x => x.Role == PlayerRoles.RoleTypeId.Tutorial);
            if (list.Count == 0) return;
            var id = UnityEngine.Random.Range(0, list.Count - 1);

            if (list[id] != null)
            {
                list[id].Role.Set(role);
            }

        }

        public void IpCheck(Exiled.API.Features.Player player)
        {
            var databaseIp = GetIp.GetIpAddress(player.IPAddress);
            if (databaseIp == null)
            {
                player.AddIp();
                databaseIp = GetIp.GetIpAddress(player.IPAddress);
            }

            if (!databaseIp.UserIds.Contains(player.UserId))
            {
                databaseIp.UserIds.Add(player.UserId);
                databaseIp.SaveIp();

            }
            if (pluginInstance.Config.ASNWhiteslistMultiAccount?.Any() ?? true)
            {
                if (player.GetDatabasePlayer().MultiAccountWhiteList) return;
                CheckIp(player);
                return;
            }
            // if (!pluginInstance.Config.ASNWhiteslistMultiAccount.Contains(player.ReferenceHub.characterClassManager.Asn) && !player.GetDatabasePlayer().MultiAccountWhiteList) CheckIp(player);
            if (!player.GetDatabasePlayer().MultiAccountWhiteList) CheckIp(player);
        }


        public void CheckIp(Exiled.API.Features.Player player)
        {
            var databaseIp = GetIp.GetIpAddress(player.IPAddress);
            if (databaseIp.UserIds.Count() > 1)
            {
                MultiAccountEvent args = new MultiAccountEvent();
                args.Player = player;
                args.UserIds = databaseIp.UserIds;
                pluginInstance.Events.OnMultiAccountEvent(args);

                if (pluginInstance.Config.MultiAccountBroadcast)
                {
                    var message = ScpUtils.StaticInstance.Translation.MultiAccountBc.Replace("%user%", player.Nickname).Replace("%userid%", player.Id.ToString()).Replace("%count%", databaseIp.UserIds.Count().ToString());
                    AdminMessage(message);
                }

                foreach (var userId in databaseIp.UserIds)
                {
                    if (player.IsMuted) return;
                    if (!pluginInstance.Config.ReportMuteEvasions) return;
                    if (VoiceChat.VoiceChatMutes.QueryLocalMute(userId))
                    {
                        if (!string.Equals(ScpUtils.StaticInstance.Config.WebhookUrl, "None")) DiscordWebHook.Message(userId, player);
                        var message = ScpUtils.StaticInstance.Translation.MuteEvasionBc.Replace("%user%", player.Nickname).Replace("%useid%", player.Id.ToString()).Replace("%userid2%", userId);
                        AdminMessage(message);
                        if (pluginInstance.Config.AutoMute) player.IsMuted = true;
                    }
                }
            }
        }

        public bool CheckCommandCooldown(ICommandSender sender)
        {
            if (((CommandSender)sender).Nickname.Equals("SERVER CONSOLE"))
            {
                return false;
            }
            var player = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId);
            if (player.CheckPermission("scputils.bypasscooldown"))
            {
                return false;
            }
            if (!pluginInstance.EventHandlers.LastCommand.ContainsKey(player))
            {
                pluginInstance.EventHandlers.LastCommand.Add(player, DateTime.Now.AddSeconds(pluginInstance.Config.CommandCooldownSeconds));
                return false;
            }
            else if (DateTime.Now <= pluginInstance.EventHandlers.LastCommand[player])
            {
                if (pluginInstance.Config.CommandAbuseReport)
                {
                    Log.Info($"[ABUSE-REPORT] {player.Nickname} - {player.UserId}@{player.AuthenticationType} tried to spam commands!");
                }
                return true;
            }
            else
            {
                pluginInstance.EventHandlers.LastCommand[player] = DateTime.Now.AddSeconds(pluginInstance.Config.CommandCooldownSeconds);
                return false;
            }
        }

        public void SetCommandBan(Exiled.API.Features.Player player)
        {
            var databasePlayer = player.GetDatabasePlayer();

            foreach (KeyValuePair<DateTime, string> a in databasePlayer.Restricted)
            {
                if (a.Key >= DateTime.Now)
                {
                    player.SendConsoleMessage($"{pluginInstance.Translation.CommandBanString} {a.Key} {pluginInstance.Translation.CommandBanStringReason} {a.Value}", "red");
                    if (!pluginInstance.EventHandlers.LastCommand.ContainsKey(player))
                    {
                        pluginInstance.EventHandlers.LastCommand.Add(player, a.Key);
                        return;
                    }
                    else
                    {
                        pluginInstance.EventHandlers.LastCommand[player] = a.Key;
                        return;
                    }
                }
            }
        }


        /*     public void CheckPtStatus()
             {
                 if ((Features.Player.Dictionary.Count >= pluginInstance.Config.MinPlayersPtCount) && (!pluginInstance.EventHandlers.ptEnabled))
                 {
                     foreach (var player in Features.Player.List)
                     {
                         var databasePlayer = player.GetDatabasePlayer();
                         databasePlayer.LastSeen = DateTime.Now;
                     }
                     pluginInstance.EventHandlers.ptEnabled = true;
                 }
                 else if ((Features.Player.Dictionary.Count >= pluginInstance.Config.MinPlayersPtCount) && (pluginInstance.EventHandlers.ptEnabled))
                 {
                     foreach (var player in Features.Player.List)
                     {
                         pluginInstance.Functions.SavePlaytime(player);
                     }
                     pluginInstance.EventHandlers.ptEnabled = false;
                 }
             } */

    }
}
