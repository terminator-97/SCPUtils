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

        public string[] Aliases { get; } = new[] { "streamer", "stm"};

        public string Description { get; } = "Removes your usergrup.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.streamermode"))
            {
                response = $"{ScpUtils.StaticInstance.Config.UnauthorizedBadgeChangeVisibility} ";
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
                databaseplayer.StreamerMode = !databaseplayer.StreamerMode;
                ScpUtils.StaticInstance.Functions.PostLoadPlayer(player);
                response = $"<color=green>Streamer mode turned to {databaseplayer.StreamerMode}</color>";
                return true;
            }
        }
    }
}
