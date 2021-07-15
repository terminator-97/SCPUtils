using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SCPUtils
{
    public class Functions : EventArgs
    {
        public CoroutineHandle RS;
        public int i = 0;
        private readonly ScpUtils pluginInstance;
        public Functions(ScpUtils pluginInstance)
        {
            this.pluginInstance = pluginInstance;
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
                BroadcastSuicideQuitAction($"<color=blue><SCPUtils> {player.Nickname} ({player.Role}) has been <color=red>BANNED</color> from the server for exceeding Quits / Suicides (as SCP) limit. Duration: {duration / 60} mitutes</color>");
            }

            player.Ban(duration, $"Auto-Ban: {string.Format(pluginInstance.Config.AutoBanMessage, duration)}", "SCPUtils");
        }

        public void AutoKickPlayer(Exiled.API.Features.Player player)
        {
            if (pluginInstance.Config.BroadcastSanctions)
            {
                BroadcastSuicideQuitAction($"<color=blue><SCPUtils> {player.Nickname} ({player.Role}) has been <color=red>KICKED</color> from the server for exceeding Quits / Suicides (as SCP) limit</color>");
            }

            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.TotalScpSuicideKicks++;
            databasePlayer.SuicidePunishment[databasePlayer.SuicidePunishment.Count() - 1] = "Kick";
            player.Kick($"Auto-Kick: {pluginInstance.Config.SuicideKickMessage}", "SCPUtils");
        }

        public void AutoWarnPlayer(Exiled.API.Features.Player player)
        {
            if (pluginInstance.Config.BroadcastWarns)
            {
                BroadcastSuicideQuitAction($"<color=blue><SCPUtils> {player.Nickname} ({player.Role}) has been <color=red>WARNED</color> for Quitting or Suiciding as SCP</color>");
            }

            player.GetDatabasePlayer().ScpSuicideCount++;
            player.ClearBroadcasts();
            player.Broadcast(pluginInstance.Config.SuicideWarnMessage);
        }

        public void OnQuitOrSuicide(Exiled.API.Features.Player player)
        {
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
                    player.ReferenceHub.serverRoles.SetGroup(group, false, true, true);
                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                    {
                        ServerStatic.PermissionsHandler._members.Remove(player.UserId);
                    }

                    ServerStatic.PermissionsHandler._members.Add(player.UserId, databasePlayer.BadgeName);
                }
                else
                {
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
                }
            }

            Timing.CallDelayed(1.5f, () =>
            {

                if (!string.IsNullOrEmpty(databasePlayer.ColorPreference) && databasePlayer.ColorPreference != "None")
                {
                    if (player.CheckPermission("scputils.changecolor") || player.CheckPermission("scputils.playersetcolor") || databasePlayer.KeepPreferences || pluginInstance.Config.KeepColorWithoutPermission)
                    {
                        player.RankColor = databasePlayer.ColorPreference;
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

                if (pluginInstance.Config.AutoKickBannedNames && pluginInstance.Functions.CheckNickname(player.Nickname) && !player.CheckPermission("scputils.bypassnickrestriction"))
                {
                    Timing.CallDelayed(2f, () =>
                    {
                        player.Kick("Auto-Kick: " + pluginInstance.Config.AutoKickBannedNameMessage, "SCPUtils");
                    });
                }

            });

            if (databasePlayer.UserNotified.Count() <= 0)
            {
                return;
            }

            if (databasePlayer.UserNotified[databasePlayer.UserNotified.Count() - 1] == false)
            {
                player.ClearBroadcasts();
                player.Broadcast(pluginInstance.Config.OfflineWarnNotification);
                databasePlayer.UserNotified[databasePlayer.UserNotified.Count() - 1] = true;
            }

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
            Player databasePlayer = player.GetDatabasePlayer();
            databasePlayer.SuicideDate.Add(DateTime.Now);
            databasePlayer.SuicideType.Add(suicidetype);
            databasePlayer.SuicideScp.Add(player.Role.ToString());
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
            if (player.Nickname != "Dedicated Server" && player != null && Database.PlayerData.ContainsKey(player))
            {
                if ((player.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && player.Team == Team.TUT)) && pluginInstance.Config.QuitEqualsSuicide && Round.IsStarted)
                {
                    if (pluginInstance.Config.EnableSCPSuicideAutoWarn && pluginInstance.Config.QuitEqualsSuicide)
                    {
                        pluginInstance.Functions.OnQuitOrSuicide(player);
                    }
                }
                Player databasePlayer = player.GetDatabasePlayer();


                if (player.DoNotTrack && !pluginInstance.Config.IgnoreDntRequests && !pluginInstance.Config.DntIgnoreList.Contains(player.GroupName) && !databasePlayer.IgnoreDNT)
                {
                    databasePlayer.PlayTimeRecords.Clear();
                    databasePlayer.PlaytimeSessions.Clear();
                    databasePlayer.ResetPreferences();
                    databasePlayer.FirstJoin = DateTime.MinValue;
                    databasePlayer.LastSeen = DateTime.MinValue;
                }
                else if (!player.DoNotTrack)
                {
                    databasePlayer.SetCurrentDayPlayTime();
                }
                else
                {
                    databasePlayer.SetCurrentDayPlayTime();
                }

                if (!string.IsNullOrEmpty(databasePlayer.BadgeName))
                {
                    if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                    {
                        ServerStatic.PermissionsHandler._members.Remove(player.UserId);
                    }
                }

                databasePlayer.Ip = player.IPAddress;
                Database.LiteDatabase.GetCollection<Player>().Update(Database.PlayerData[player]);
                Database.PlayerData.Remove(player);
            }
        }

        private void BroadcastSuicideQuitAction(string text)
        {
            foreach (Exiled.API.Features.Player admin in Exiled.API.Features.Player.List)
            {
                if (pluginInstance.Config.BroadcastSanctions)
                {
                    if (admin.Sender.CheckPermission(PlayerPermissions.AdminChat))
                    {
                        admin.Broadcast(12, text, Broadcast.BroadcastFlags.AdminChat);
                    }
                }
            }
        }

        public bool IsTeamImmune(Exiled.API.Features.Player player, Exiled.API.Features.Player attacker)
        {
            if (pluginInstance.Config.CuffedImmunityPlayers[player.Team]?.Any() == true)
            {

                if (pluginInstance.Config.CuffedImmunityPlayers[player.Team].Contains(attacker.Team))
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
                Log.Error($"Detected invalid setting on cuffed_immunity_players! Key: {player.Team}, List cannot be null!");
                return false;
            }

        }

        public bool CuffedCheck(Exiled.API.Features.Player player)
        {
            if (pluginInstance.Config.CuffedProtectedTeams?.Any() == true)
            {
                if (pluginInstance.Config.CuffedProtectedTeams.Contains(player.Team) && player.IsCuffed)
                {
                    return true;
                }
                else if (!pluginInstance.Config.CuffedProtectedTeams.Contains(player.Team))
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

            else if (pluginInstance.Config.CuffedSafeZones[player.Team]?.Any() == true)
            {
                if (pluginInstance.Config.CuffedSafeZones[player.Team].Contains(player.CurrentRoom.Zone))
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
                Log.Error($"Detected invalid setting on cuffed_safe_zones! Key: {player.Team}, List cannot be null!");
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

            if (pluginInstance.Config.ASNBlacklist.Contains(player.ReferenceHub.characterClassManager.Asn) && !databasePlayer.ASNWhitelisted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
