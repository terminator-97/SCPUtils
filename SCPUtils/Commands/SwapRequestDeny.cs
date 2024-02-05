using CommandSystem;
using System;
using System.Linq;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SwapRequestDeny : ICommand
    {
        public string Command => ScpUtils.StaticInstance.Config.SwapRequestDenyCommand;

        public string[] Aliases => ScpUtils.StaticInstance.Config.SwapRequestDenyCommandAliases;

        public string Description => ScpUtils.StaticInstance.Translation.SwaprequestdenyDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId);
            if (!player.IsScp)
            {
                response = ScpUtils.StaticInstance.Translation.SwaprequestNotscp;
                return false;
            }
            if (!ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsValue(player))
            {
                response = ScpUtils.StaticInstance.Translation.SwaprequestacceptNoswaprequest;
                return false;
            }


            var target = ScpUtils.StaticInstance.EventHandlers.SwapRequest.FirstOrDefault(x => x.Value == player).Key;
            ScpUtils.StaticInstance.EventHandlers.SwapRequest.Remove(target);
            target.ClearBroadcasts();
            target.Broadcast(ScpUtils.StaticInstance.Config.SwapRequestDeniedBroadcast);
            response = ScpUtils.StaticInstance.Translation.Success;
            return true;
        }
    }
}
