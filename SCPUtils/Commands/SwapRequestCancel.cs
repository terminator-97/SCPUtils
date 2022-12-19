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
    internal class SwapRequestCancel : ICommand
    {
        public string Command => ScpUtils.StaticInstance.Config.SwapRequestCancelCommand;

        public string[] Aliases => ScpUtils.StaticInstance.Config.SwapRequestCancelCommandAliases;

        public string Description => "Cancel a swap request sent by you";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId);          
            if (!ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsKey(player))
            {
                response = $"<color=red>You haven't sent any swap request!</color>";
                return false;
            }
            var target = ScpUtils.StaticInstance.EventHandlers.SwapRequest[player];
            target.ClearBroadcasts();
            target.Broadcast(ScpUtils.StaticInstance.Config.SwapRequestCanceledBroadcast);
            ScpUtils.StaticInstance.EventHandlers.SwapRequest.Remove(player);
            response = $"<color=green>Swap request has been canceled</color>";
            return true;
        }
    }
}
