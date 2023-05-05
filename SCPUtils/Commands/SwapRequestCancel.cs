using CommandSystem;
using System;
using PluginAPI.Core;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SwapRequestCancel : ICommand
    {
        public string Command => ScpUtils.StaticInstance.configs.SwapRequestCancelCommand;

        public string[] Aliases => ScpUtils.StaticInstance.configs.SwapRequestCancelCommandAliases;

        public string Description => "Cancel a swap request sent by you";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(((CommandSender)sender).SenderId);
            if (!ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsKey(player))
            {
                response = $"<color=red>You haven't sent any swap request!</color>";
                return false;
            }
            var target = ScpUtils.StaticInstance.EventHandlers.SwapRequest[player];
            target.ClearBroadcasts();
            target.SendBroadcast(ScpUtils.StaticInstance.configs.SwapRequestCanceledBroadcast.Content, ScpUtils.StaticInstance.configs.SwapRequestCanceledBroadcast.Duration);
            ScpUtils.StaticInstance.EventHandlers.SwapRequest.Remove(player);
            response = $"<color=green>Swap request has been canceled</color>";
            return true;
        }
    }
}
