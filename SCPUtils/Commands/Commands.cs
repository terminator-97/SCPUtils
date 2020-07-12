using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCPUtils
{

    public class Commands
    {
        private readonly List<string> validColors = new List<string> { "pink", "red", "default", "brown", "silver", "light_green", "crismon", "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta", "blue_green", "orange", "lime", "green", "emerald", "carmine", "nickel", "mint", "army_green", "pumpkin" };
        public void OnRaCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            switch (ev.Name)
            {
                case "scputils_help":
                    {
                        ev.IsAllowed = false;

                        ev.Sender.RemoteAdminMessage($"SCPUtils info:\n" +
                        $"Avaible commands: scputils_help, scputils_player_info, scputils_player_list, scputils_player_reset_preferences, scputils_player_reset, scputils_set_color, scputils_set_name,  scputils_set_badge,  scputils_revoke_badge, scputils_play_time, scputils_whitelist_asn, scputils_unwhitelist_asn", true);
                        break;
                    }

                case "scputils_player_info":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 1)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_player_info <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender.Nickname, "scputils.playerinfo"))
                        {
                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            ev.Sender.RemoteAdminMessage($"\n[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n" +
                  $"Total SCP Suicides/Quits: [ {databasePlayer.ScpSuicideCount} ]\n" +
                  $"Total SCP Suicides/Quits Kicks: [ {databasePlayer.TotalScpSuicideKicks} ]\n" +
                  $"Total SCP Suicides/Quits Bans: [ {databasePlayer.TotalScpSuicideBans} ]\n" +
                  $"Total Games played as SCP: [ {databasePlayer.TotalScpGamesPlayed} ]\n" +
                  $"Total Suicides/Quits Percentage: [ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]\n" +
                  $"First Join: [ {databasePlayer.FirstJoin} ]\n" +
                  $"Last Seen: [ {databasePlayer.LastSeen} ]\n" +
                  $"Custom Color: [ {databasePlayer.ColorPreference} ]\n" +
                  $"Custom Name: [ {databasePlayer.CustomNickName} ]\n" +
                  $"Temporarily Badge: [ {databasePlayer.BadgeName} ]\n" +
                  $"Badge Expire: [ {databasePlayer.BadgeExpire} ]\n" +
                  $"Hide Badge: [ {databasePlayer.HideBadge} ]\n" +
                  $"Asn Whitelisted: [ {databasePlayer.ASNWhitelisted} ]\n" +
                  $"Total Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString() } ]");
                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_play_time":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 2)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_player_info <player name/id> <days range>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender.Nickname, "scputils.playtime"))
                        {
                            int.TryParse(ev.Arguments[1], out int range);

                            if (range < 0)
                            {
                                ev.Sender.RemoteAdminMessage("You have to specify a positive number!", false);
                                return;
                            }

                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            string message = $"\n[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n" +
                  $"Total Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString() } ]\n";


                            for (int i = 0; i <= range; i++)
                            {
                                DateTime.TryParse((DateTime.Now.Date.AddDays(-i)).ToString(), out DateTime date);
                                if (databasePlayer.PlayTimeRecords.ContainsKey(date.Date.ToShortDateString())) message += $"{date.Date.ToShortDateString()} Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()]).ToString() } ]\n";
                                else message += $"{date.Date.ToShortDateString()} Playtime: [ No activity ]\n";
                            }

                            ev.Sender.RemoteAdminMessage(message);
                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_player_list":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 1)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_player_list <minimum quit percentage>", false);
                            break;
                        }


                        if (IsAllowed(ev.Sender.Nickname, "scputils.playerlist"))
                        {

                            var playerListString = "[Quits/Suicides Percentage]\n";

                            if (int.TryParse(ev.Arguments[0], out int minpercentage))
                            {
                                foreach (var databasePlayer in Database.LiteDatabase.GetCollection<Player>().Find(x => x.SuicidePercentage >= minpercentage))
                                {
                                    playerListString += $"\n{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication}) -[ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]";
                                }
                                if (playerListString == "[Quits/Suicides as SCP]\n") ev.Sender.RemoteAdminMessage("No results found!", false);
                                else ev.Sender.RemoteAdminMessage($"{playerListString}");
                            }
                            else
                            {
                                ev.Sender.RemoteAdminMessage("Arg1 is not an integer, Comand usage example: scputils_player_list 50", false);
                                break;
                            }


                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_player_reset":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 1)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_player_reset <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender.Nickname, "scputils.playerreset"))
                        {
                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            databasePlayer.Reset();
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            ev.Sender.RemoteAdminMessage("Success!", false);
                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_player_reset_preferences":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 1)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_player_reset_preferences <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender.Nickname, "scputils.playerresetpreferences"))
                        {
                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            databasePlayer.ResetPreferences();
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            ev.Sender.RemoteAdminMessage("Success!", false);
                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_set_color":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 2)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_set_color <player name/id> <color>", false);
                            break;
                        }
                        ev.Arguments[1].ToLower().ToString();

                        if (IsAllowed(ev.Sender.Nickname, "scputils.playersetcolor"))
                        {
                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            var target = Exiled.API.Features.Player.Get(ev.Arguments[0]);
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            if (ev.Arguments[1] == "none")
                            {
                                databasePlayer.ColorPreference = "";
                                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                                ev.Sender.RemoteAdminMessage("Success, effects will take effect next round!", false);
                                break;
                            }
                            if (validColors.Contains(ev.Arguments[1]))
                            {
                                if (target != null) target.ReferenceHub.serverRoles.SetColor(ev.Arguments[1]);
                                string color = ev.Arguments[1];
                                databasePlayer.ColorPreference = color;
                                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                                ev.Sender.RemoteAdminMessage("Success!", false);
                            }
                            else ev.Sender.RemoteAdminMessage("Invalid color!", false);
                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_set_name":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 2)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_set_name <player name/id> <nick>", false);
                            break;
                        }


                        if (IsAllowed(ev.Sender.Nickname, "scputils.playersetname"))
                        {
                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            var target = Exiled.API.Features.Player.Get(ev.Arguments[0]);
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            if (ev.Arguments[1].ToLower() == "none")
                            {
                                databasePlayer.CustomNickName = "";
                                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                                ev.Sender.RemoteAdminMessage("Success, changes will take effect next round!", false);
                                break;
                            }
                            string name = ev.Arguments[1];
                            databasePlayer.CustomNickName = name;
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            ev.Sender.RemoteAdminMessage("Success, choice has been saved!", false);
                            if (target != null) target.ReferenceHub.nicknameSync.DisplayName = ev.Arguments[1].ToString();

                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }



                case "scputils_set_badge":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 3)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_set_badge <player name/id> <Badge Name> <Minutes>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender.Nickname, "scputils.handlebadges"))
                        {
                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            var player = Exiled.API.Features.Player.Get(ev.Arguments[0]);
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            if (!ServerStatic.GetPermissionsHandler().GetAllGroups().ContainsKey(ev.Arguments[1]))
                            {
                                ev.Sender.RemoteAdminMessage("Invalid role name!", false);
                                break;
                            }

                            else if (int.TryParse(ev.Arguments[2], out int duration))
                            {
                                string badgeName = ev.Arguments[1].ToString();
                                databasePlayer.BadgeName = badgeName;
                                databasePlayer.BadgeExpire = DateTime.Now.AddMinutes(duration);
                                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                                if (player != null) player.ReferenceHub.serverRoles.Group = ServerStatic.GetPermissionsHandler()._groups[ev.Arguments[1]];
                                ev.Sender.RemoteAdminMessage("Badge set!", false);

                            }
                            else ev.Sender.RemoteAdminMessage("Arg3 must be integer!", false);
                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_revoke_badge":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 1)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_revoke_badge <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender.Nickname, "scputils.handlebadges"))
                        {
                            var player = Exiled.API.Features.Player.Get(ev.Arguments[0]);
                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }

                            databasePlayer.BadgeName = "";
                            databasePlayer.BadgeExpire = DateTime.MinValue;
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            if (player != null) player.SetRank(null, null);
                            ev.Sender.RemoteAdminMessage("Badge revoked!", false);

                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_whitelist_asn":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 1)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_whitelist_asn <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender.Nickname, "scputils.whitelist"))
                        {
                            var player = Exiled.API.Features.Player.Get(ev.Arguments[0]);
                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }

                            databasePlayer.ASNWhitelisted = true;
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            ev.Sender.RemoteAdminMessage("Player has been successfully whitelisted!", false);

                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_unwhitelist_asn":
                    {
                        ev.IsAllowed = false;

                        var commandSender = Exiled.API.Features.Player.Get(ev.Sender.Nickname);

                        if (ev.Arguments.Count < 1)
                        {
                            ev.Sender.RemoteAdminMessage("Usage: scputils_unwhitelist_asn <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender.Nickname, "scputils.whitelist"))
                        {
                            var player = Exiled.API.Features.Player.Get(ev.Arguments[0]);
                            var databasePlayer = ev.Arguments[0].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RemoteAdminMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }

                            databasePlayer.ASNWhitelisted = false;
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            ev.Sender.RemoteAdminMessage("Player has been successfully removed from whitelist!", false);

                        }
                        else ev.Sender.RemoteAdminMessage("You need a higher administration level to use this command!", false);
                        break;
                    }
            }
        }


        private bool IsAllowed(string sender, string permission)
        {
            Exiled.API.Features.Player player;
            return sender != null && (sender == "GAME CONSOLE" || (player = Exiled.API.Features.Player.Get(sender)) == null || Exiled.Permissions.Extensions.Permissions.CheckPermission(player, permission));
        }
    }

}
