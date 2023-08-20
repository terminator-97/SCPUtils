using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class StreamerMode : ICommand
    {

        public string Command { get; } = "scputils_streamer_mode";

        public string[] Aliases { get; } = new[] { "streamer", "stm", "smode", "streamermode" };

        public string Description { get; } = "Removes your usergrup.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {


      //      response = "Command disabled";
           // return false;

            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            else if (((CommandSender)sender).Nickname.Equals("SERVER CONSOLE"))
            {
                response = "This command cannot be executed from console!";
                return false;
            }
            else
            {

                Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId);
                var databaseplayer = player.GetDatabasePlayer();
                if (!sender.CheckPermission("scputils.streamermode") && !databaseplayer.StreamerMode)
                {
                    response = "<color=red> You need a higher administration level to use this command!</color>";
                    return false;
                }
                databaseplayer.StreamerMode = !databaseplayer.StreamerMode;
                if (databaseplayer.StreamerMode)
                {
               /*     player.ReferenceHub.serverRoles.SetGroup(new UserGroup()
                    {
                        BadgeColor = "default",
                        BadgeText = "",
                        HiddenByDefault = true
                    }, false, true, false);*/
                    player.SetRank("", new UserGroup()
                    {
                        BadgeColor = "default",
                        BadgeText = "",
                        HiddenByDefault = true
                    });
                    

                }
                else
                {
                    databaseplayer.LastSeen = DateTime.Now;
                    ScpUtils.StaticInstance.Functions.PostLoadPlayer(player);
                }
                response = $"<color=green>Streamer mode turned to {databaseplayer.StreamerMode}</color>";
                return true;
            }
        }
    }
}
