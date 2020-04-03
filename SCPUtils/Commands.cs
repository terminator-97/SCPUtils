using EXILED;
using System;

namespace SCPUtils
{
    public class Commands
    {
        public void OnRaCommand(ref RACommandEvent ev)
        {

            string[] args = ev.Command.Split(' ');


            switch (args[0].ToLower())
            {

                case "scputils_help":
                    {
                        ev.Allow = false;
                        ev.Sender.RAMessage($"SCPUtils info:\n" +
                        $"Avaible commands: scputils_help, scputils_player_info, scputils_player_reset", true);
                        break;
                    }

                case "scputils_player_info":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);
                        if (commandSender == null)
                        {
                            ev.Sender.RAMessage("An error has occured while executing the command!", false);
                            break;
                        }

                        if (args.Length < 2)
                        {
                            ev.Sender.RAMessage("Usage: scputils_player_info <player name/id>", false);
                            break;
                        }

                        if (commandSender.CheckPermission("scputils.playerinfo"))
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
                  $"Total Suicides/Quits Percentage: [ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]\n");
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_player_list":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);
                        if (commandSender == null)
                        {
                            ev.Sender.RAMessage("An error has occured while executing the command!", false);
                            return;
                        }

                        if (commandSender.CheckPermission("scputils.playerlist"))
                        {

                            var playerListString = "";
                            foreach (var databasePlayer in Database.LiteDatabase.GetCollection<Player>().FindAll())
                            {
                                playerListString += $"\n[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n Total SCP Suicides/Quits: [ {databasePlayer.ScpSuicideCount} ]\n Total SCP Suicides/Quits Kicks: [ {databasePlayer.TotalScpSuicideKicks} ]\n Total SCP Suicides/Quits Bans: [ {databasePlayer.TotalScpSuicideBans} ]\n Total Games played as SCP: [ {databasePlayer.TotalScpGamesPlayed} ]\n Total Suicides/Quits Percentage: [ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]\n";

                            }
                            ev.Sender.RAMessage($"{playerListString}");
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

                case "scputils_player_reset":
                    {
                        ev.Allow = false;

                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);
                        if (commandSender == null)
                        {
                            ev.Sender.RAMessage("An error has occured while executing the command!", false);
                            return;
                        }

                        if (args.Length < 2)
                        {
                            ev.Sender.RAMessage("Usage: scputils_player_reset <player name/id>", false);
                            break;
                        }

                        if (commandSender.CheckPermission("scputils.playerreset"))
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
            }
        }
    }
}
