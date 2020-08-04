using MEC;
using RemoteAdmin;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System.Linq;

namespace SCPUtils
{
    public class Functions
    {
        public CoroutineHandle DT;
        public int i = 0;
        private readonly ScpUtils pluginInstance;

        public Functions(ScpUtils pluginInstance) => this.pluginInstance = pluginInstance;

        public void AutoBanPlayer(Exiled.API.Features.Player player)
        {
            int duration;
            player.GetDatabasePlayer().TotalScpSuicideBans++;
            if (pluginInstance.Config.MultiplyBanDurationEachBan == true) duration = player.GetDatabasePlayer().TotalScpSuicideBans * pluginInstance.Config.AutoBanDuration;
            else duration = pluginInstance.Config.AutoBanDuration;
            if (pluginInstance.Config.BroadcastSanctions) BroadcastSuicideQuitAction($"<color=blue><SCPUtils> {player.Nickname} has been <color=red>BANNED</color> from the server for exceeding Quits / Suicides (as SCP) limit. Duration: {duration} mitutes</color>");
            player.Ban(duration, $"Auto-Ban: {string.Format(pluginInstance.Config.AutoBanMessage, duration * 60)}", "SCPUtils");
        }

        public void AutoKickPlayer(Exiled.API.Features.Player player)
        {
            if (pluginInstance.Config.BroadcastSanctions) BroadcastSuicideQuitAction($"<color=blue><SCPUtils> {player.Nickname} has been <color=red>KICKED</color> from the server for exceeding Quits / Suicides (as SCP) limit</color>");
            player.GetDatabasePlayer().TotalScpSuicideKicks++;
            player.Kick($"Auto-Kick: {pluginInstance.Config.SuicideKickMessage}", "SCPUtils");
        }

        public void AutoWarnPlayer(Exiled.API.Features.Player player)
        {
            if (pluginInstance.Config.BroadcastWarns) BroadcastSuicideQuitAction($"<color=blue><SCPUtils> {player.Nickname} has been <color=red>WARNED</color> for Quitting or Suiciding as SCP</color>");
            player.GetDatabasePlayer().ScpSuicideCount++;
            player.ClearBroadcasts();
            player.Broadcast(pluginInstance.Config.AutoWarnMessageDuration, pluginInstance.Config.SuicideWarnMessage, Broadcast.BroadcastFlags.Normal);
        }

        public void OnQuitOrSuicide(Exiled.API.Features.Player player)
        {
            var suicidePercentage = player.GetDatabasePlayer().SuicidePercentage;
            AutoWarnPlayer(player);
            if (pluginInstance.Config.EnableSCPSuicideAutoBan && suicidePercentage >= pluginInstance.Config.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Config.ScpSuicideTollerance) AutoBanPlayer(player);
            else if (pluginInstance.Config.AutoKickOnSCPSuicide && suicidePercentage >= pluginInstance.Config.AutoKickThreshold && suicidePercentage < pluginInstance.Config.AutoBanThreshold && player.GetDatabasePlayer().TotalScpGamesPlayed > pluginInstance.Config.ScpSuicideTollerance) AutoKickPlayer(player);
        }

        public void PostLoadPlayer(Exiled.API.Features.Player player)
        {
            var databasePlayer = player.GetDatabasePlayer();


            if (!string.IsNullOrEmpty(databasePlayer.BadgeName))
            {
                Timing.CallDelayed(1f, () =>
                {
                    if (databasePlayer.BadgeExpire >= DateTime.Now)
                    {
                        var group = ServerStatic.GetPermissionsHandler()._groups[databasePlayer.BadgeName];
                        player.ReferenceHub.serverRoles.SetGroup(group, false, true, true);
                    }
                    else
                    {
                        databasePlayer.BadgeName = "";
                        databasePlayer.ResetPreferences();
                    }
                });
            }

            if (!string.IsNullOrEmpty(databasePlayer.ColorPreference) && databasePlayer.ColorPreference != "None")
            {
                Timing.CallDelayed(1.15f, () =>
                {
                    player.RankColor = databasePlayer.ColorPreference;
                });
            }

            if (databasePlayer.HideBadge == true)
            {
                Timing.CallDelayed(1.25f, () =>
                {
                    player.BadgeHidden = true;
                });
            }

            if (pluginInstance.Config.AutoKickBannedNames && pluginInstance.Functions.CheckNickname(player.Nickname) && !player.CheckPermission("scputils.bypassnickrestriction"))
            {
                Timing.CallDelayed(3f, () =>
                {
                    player.Kick("Auto-Kick: " + pluginInstance.Config.AutoKickBannedNameMessage, "SCPUtils");
                });
            }

        }

        public bool CheckNickname(string name)
        {
            foreach (var nickname in pluginInstance.Config.BannedNickNames)
            {
                name = Regex.Replace(name, "[^a-zA-Z0-9]", "").ToLower();
                string pattern = Regex.Replace(nickname.ToLower(), "[^a-zA-Z0-9]", "");
                if (Regex.Match(name, pattern).Success) return true;
            }
            return false;
        }

        public void SaveData(Exiled.API.Features.Player player)
        {
            if (player.Nickname != "Dedicated Server" && player != null && Database.PlayerData.ContainsKey(player))
            {
                if ((player.Team == Team.SCP || (pluginInstance.Config.AreTutorialsSCP && player.Team == Team.TUT)) && pluginInstance.Config.QuitEqualsSuicide && Round.IsStarted)
                {
                    if (pluginInstance.Config.EnableSCPSuicideAutoWarn && pluginInstance.Config.QuitEqualsSuicide) pluginInstance.Functions.OnQuitOrSuicide(player);
                }
                player.GetDatabasePlayer().SetCurrentDayPlayTime();
                Database.LiteDatabase.GetCollection<Player>().Update(Database.PlayerData[player]);
                Database.PlayerData.Remove(player);
                Log.Debug($"Saving data of {player.Nickname}");
            }
        }

        private void BroadcastSuicideQuitAction(string text)
        {
            foreach (var admin in Exiled.API.Features.Player.List)
            {
                if (pluginInstance.Config.BroadcastSanctions)
                {
                    if (admin.ReferenceHub.serverRoles.RemoteAdmin) Map.Broadcast(12, text, Broadcast.BroadcastFlags.AdminChat);
                }
            }
        }

        public bool IsTeamImmune(Exiled.API.Features.Player player, Exiled.API.Features.Player attacker)
        {
            if (pluginInstance.Config.CuffedImmunityPlayers[player.Team]?.Any() == true)
            {

                if (pluginInstance.Config.CuffedImmunityPlayers[player.Team].Contains(attacker.Team)) return true;
                else return false;
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
                if (pluginInstance.Config.CuffedProtectedTeams.Contains(player.Team) && player.IsCuffed) return true;
                else if (!pluginInstance.Config.CuffedProtectedTeams.Contains(player.Team)) return true;
                else return false;
            }
            else return true;
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
                if (pluginInstance.Config.CuffedSafeZones[player.Team].Contains(player.CurrentRoom.Zone)) return true;
                else return false;
            }

            else
            {
                Log.Error($"Detected invalid setting on cuffed_safe_zones! Key: {player.Team}, List cannot be null!");
                return false;
            }

        }

    }
}