using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SwapRequestDeny : ICommand
    {
        public string Command => ScpUtils.StaticInstance.Config.SwapRequestDenyCommand;

        public string[] Aliases => ScpUtils.StaticInstance.Config.SwapRequestDenyCommandAliases;

        public string Description => "Deny a swap request";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId);
            if (!player.IsScp)
            {
                response = "<color=red>You are not SCP</color>";
                return false;
            }           
            if (!ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsValue(player))
            {
                response = $"<color=red>You haven't any swap request!</color>";
                return false;
            }
        

            var target = ScpUtils.StaticInstance.EventHandlers.SwapRequest.FirstOrDefault(x => x.Value == player).Key;          
            ScpUtils.StaticInstance.EventHandlers.SwapRequest.Remove(target);
            target.ClearBroadcasts();
            target.Broadcast(ScpUtils.StaticInstance.Config.SwapRequestDeniedBroadcast);
            response = $"<color=green>Swap request has been denied</color>";
            return true;
        }
    }
}
