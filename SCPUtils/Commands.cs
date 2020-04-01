using EXILED;
using EXILED.Extensions;



namespace SCPUtils
{
    public class Commands
    {
        private readonly Functions functionsInstance;
        private readonly Utils pluginInstance;



        public Commands(Functions functionsInstance, Utils pluginInstance)
        {
            this.functionsInstance = functionsInstance;
            this.pluginInstance = pluginInstance;
        }


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
                        if (args.Length < 2)
                        {
                            ev.Sender.RAMessage("Usage: scputils_player_info <player name/id>", false);
                            break;
                        }
                        var commandsender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (commandsender.CheckPermission("scputils.playerreset") && commandsender != null)
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
                  $"Total Suicides/Quits Percentage: [ {databasePlayer.SuicidePercentage}% ]\n");
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }


                case "scputils_player_reset":
                    {
                        ev.Allow = false;
                        if (args.Length < 2)
                        {
                            ev.Sender.RAMessage("Usage: scputils_player_reset <player name/id>", false);
                            break;
                        }
                        var commandsender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);

                        if (commandsender.CheckPermission("scputils.playerreset") && commandsender != null)
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
