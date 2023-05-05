using CommandSystem;
using System;
using System.Linq;
using PluginAPI.Core;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SwapRequestDeny : ICommand
    {
        public string Command => ScpUtils.StaticInstance.configs.SwapRequestDenyCommand;

        public string[] Aliases => ScpUtils.StaticInstance.configs.SwapRequestDenyCommandAliases;

        public string Description => "Deny a swap request";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(((CommandSender)sender).SenderId);
            if (!player.IsSCP)
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
            target.SendBroadcast(ScpUtils.StaticInstance.configs.SwapRequestDeniedBroadcast.Content, ScpUtils.StaticInstance.configs.SwapRequestDeniedBroadcast.Duration);
            response = $"<color=green>Swap request has been denied</color>";
            return true;
        }
    }
}
