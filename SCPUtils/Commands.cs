using EXILED;
using System;
using System.Collections.Generic;

namespace SCPUtils
{

    public class Commands
    {
        private readonly List<string> validColors = new List<string> { "pink", "red", "default", "brown", "silver", "light_green", "crismon", "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta", "blue_green", "orange", "lime", "green", "emerald", "carmine", "nickel", "mint", "army_green", "pumpkin" };
        public void OnRaCommand(ref RACommandEvent ev)
        {
            string[] args = ev.Command.Split(' ');

            switch (args[0].ToLower())
            {
                case "scputils_help":
                    {
                        ev.Allow = false;

                        ev.Sender.RAMessage($"SCPUtils info:\n" +
                        $"Avaible commands: scputils_help, scputils_player_info, scputils_player_list, scputils_player_reset_preferences, scputils_player_reset, scputils_set_color, scputils_set_name,  scputils_set_badge,  scputils_revoke_badge", true);
                        break;
                    }

                case "scputils_player_info":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (args.Length < 2)
                        {
                            ev.Sender.RAMessage("Usage: scputils_player_info <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender, "scputils.playerinfo"))
                        {
                            var databasePlayer = args[1].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RAMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            ev.Sender.RAMessage($"\n[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n" +
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
                  $"Hide Badge: [ {databasePlayer.HideBadge} ]");
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_player_list":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (args.Length < 2)
                        {
                            ev.Sender.RAMessage("Usage: scputils_player_list <minimum quit percentage>", false);
                            break;
                        }


                        if (IsAllowed(ev.Sender, "scputils.playerlist"))
                        {

                            var playerListString = "[Quits/Suicides Percentage]\n";

                            if (int.TryParse(args[1], out int minpercentage))
                            {
                                foreach (var databasePlayer in Database.LiteDatabase.GetCollection<Player>().Find(x => x.SuicidePercentage >= minpercentage))
                                {
                                    playerListString += $"\n{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication}) -[ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]";
                                }
                                if (playerListString == "[Quits/Suicides as SCP]\n") ev.Sender.RAMessage("No results found!", false);
                                else ev.Sender.RAMessage($"{playerListString}");
                            }
                            else
                            {
                                ev.Sender.RAMessage("Arg1 is not an integer, Comand usage example: scputils_player_list 50", false);
                                break;
                            }


                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_player_reset":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (args.Length < 2)
                        {
                            ev.Sender.RAMessage("Usage: scputils_player_reset <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender, "scputils.playerreset"))
                        {
                            var databasePlayer = args[1].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RAMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            databasePlayer.Reset();
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            ev.Sender.RAMessage("Success!", false);
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_player_reset_preferences":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (args.Length < 2)
                        {
                            ev.Sender.RAMessage("Usage: scputils_player_reset_preferences <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender, "scputils.playerresetpreferences"))
                        {
                            var databasePlayer = args[1].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RAMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            databasePlayer.ResetPreferences();
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            ev.Sender.RAMessage("Success!", false);
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_set_color":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (args.Length < 3)
                        {
                            ev.Sender.RAMessage("Usage: scputils_set_color <player name/id> <color>", false);
                            break;
                        }
                        args[2] = args[2].ToLower().ToString();

                        if (IsAllowed(ev.Sender, "scputils.playersetcolor"))
                        {
                            var databasePlayer = args[1].GetDatabasePlayer();
                            var target = EXILED.Extensions.Player.GetPlayer(args[1]);
                            if (databasePlayer == null)
                            {
                                ev.Sender.RAMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            if (args[2] == "none")
                            {
                                databasePlayer.ColorPreference = "";
                                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                                ev.Sender.RAMessage("Success, effects will take effect next round!", false);
                                break;
                            }
                            if (validColors.Contains(args[2]))
                            {
                                if (target != null) target.serverRoles.SetColor(args[2]);
                                string color = args[2];
                                databasePlayer.ColorPreference = color;
                                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                                ev.Sender.RAMessage("Success!", false);
                            }
                            else ev.Sender.RAMessage("Invalid color!", false);
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_set_name":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (args.Length < 3)
                        {
                            ev.Sender.RAMessage("Usage: scputils_set_name <player name/id> <nick>", false);
                            break;
                        }


                        if (IsAllowed(ev.Sender, "scputils.playersetname"))
                        {
                            var databasePlayer = args[1].GetDatabasePlayer();
                            var target = EXILED.Extensions.Player.GetPlayer(args[1]);
                            if (databasePlayer == null)
                            {
                                ev.Sender.RAMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            if (args[2] == "none")
                            {
                                databasePlayer.CustomNickName = "";
                                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                                ev.Sender.RAMessage("Success, changes will take effect next round!", false);
                                break;
                            }
                            string name = args[2];
                            databasePlayer.CustomNickName = name;
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            ev.Sender.RAMessage("Success, changes will take effect next round!", false);

                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }



                case "scputils_set_badge":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (args.Length < 4)
                        {
                            ev.Sender.RAMessage("Usage: scputils_set_badge <player name/id> <Badge Name> <Minutes>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender, "scputils.handlebadges"))
                        {
                            var databasePlayer = args[1].GetDatabasePlayer();
                            var player = EXILED.Extensions.Player.GetPlayer(args[1]);
                            if (databasePlayer == null)
                            {
                                ev.Sender.RAMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }
                            if (!ServerStatic.GetPermissionsHandler().GetAllGroups().ContainsKey(args[2]))
                            {
                                ev.Sender.RAMessage("Invalid role name!", false);
                                break;
                            }

                            else if (int.TryParse(args[3], out int duration))
                            {
                                string badgeName = args[2].ToString();
                                databasePlayer.BadgeName = badgeName;
                                databasePlayer.BadgeExpire = DateTime.Now.AddMinutes(duration);
                                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                                if (player) EXILED.Extensions.Player.SetRank(player, ServerStatic.GetPermissionsHandler()._groups[args[2]]);
                                ev.Sender.RAMessage("Badge set!", false);

                            }
                            else ev.Sender.RAMessage("Arg3 must be integer!", false);
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_revoke_badge":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (args.Length < 2)
                        {
                            ev.Sender.RAMessage("Usage: scputils_revoke_badge <player name/id>", false);
                            break;
                        }

                        if (IsAllowed(ev.Sender, "scputils.handlebadges"))
                        {
                            var player = EXILED.Extensions.Player.GetPlayer(args[1]);
                            var databasePlayer = args[1].GetDatabasePlayer();
                            if (databasePlayer == null)
                            {
                                ev.Sender.RAMessage("Player not found on Database or Player is loading data!", false);
                                break;
                            }

                            databasePlayer.BadgeName = "";
                            databasePlayer.BadgeExpire = DateTime.MinValue;
                            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                            if (player) EXILED.Extensions.Player.SetRank(player, null);
                            ev.Sender.RAMessage("Badge revoked!", false);
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

            }
        }

        private bool IsAllowed(CommandSender sender, string permission)
        {
            ReferenceHub player;
            return sender != null && (sender.SenderId == "GAME CONSOLE" || (player = EXILED.Extensions.Player.GetPlayer(sender.Nickname)) == null || player.CheckPermission(permission));
        }
    }

}
