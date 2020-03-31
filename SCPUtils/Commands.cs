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

            Log.Info("TEST OK");
        }


        public void OnRaCommand(ref RACommandEvent ev)
        {

            string[] args = ev.Command.Split(' ');


            switch (args[0].ToLower())
            {

                case "scp_utils_help":
                    {
                        ev.Allow = false;
                        ev.Sender.RAMessage($"SCPUtils info:\n" +
                        $"Avaible commands: scputils_help, scputils_player_info, scputils_player_reset", true);
                        break;
                    }

                case "scputils_player_info":
                    {
                        ev.Allow = false;
                        var commandsender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);
                        if (commandsender.CheckPermission("scputils.playerinfo") && commandsender != null)
                        {
                            var player = EXILED.Extensions.Player.GetPlayer(args[1]);

                            if (player == null) ev.Sender.RAMessage("Invalid Player!", false);
                            else
                            {
                                var databasePlayer = pluginInstance.PlayerData[player];
                                ev.Sender.RAMessage($"\n[{player.GetNickname()} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n" +
                                    $"Total SCP Suicides/Quits: [ {databasePlayer.ScpSuicideCount} ]\n" +
                                    $"Total SCP Suicides/Quits Kicks: [ {databasePlayer.TotalScpSuicideKicks} ]\n" +
                                    $"Total SCP Suicides/Quits Bans: [ {databasePlayer.TotalScpSuicideBans} ]\n" +
                                    $"Total Games played as SCP: [ {databasePlayer.TotalScpGamesPlayed} ]\n" +
                                    $"Total Suicides/Quit Percentage: [ {databasePlayer.SuicidePercentage}% ]\n");
                            }
                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);


                        break;
                    }

                case "scputils_player_reset":
                    {
                        ev.Allow = false;
                        var commandsender = EXILED.Extensions.Player.GetPlayer(ev.Sender.Nickname);
                        if (commandsender.CheckPermission("scputils.playerreset") && commandsender != null)
                        {
                            var player = EXILED.Extensions.Player.GetPlayer(args[1]);

                            if (player == null) ev.Sender.RAMessage("Invalid Player!", false);
                            else
                            {
                                var databasePlayer = pluginInstance.PlayerData[player];
                                databasePlayer.Name = player.GetNickname();
                                databasePlayer.ScpSuicideCount = 0;
                                databasePlayer.TotalScpSuicideKicks = 0;
                                databasePlayer.TotalScpSuicideBans = 0;
                                databasePlayer.TotalScpGamesPlayed = 0;
                                ev.Sender.RAMessage("Success!", false);

                            }

                        }
                        else ev.Sender.RAMessage("You need a higher administration level to use this command!", false);
                        break;
                    }

            }
        }


    }
}