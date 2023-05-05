using CommandSystem;
using System;
using System.Linq;
using PluginAPI.Core;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SwapRequestAccept : ICommand
    {
        public string Command => ScpUtils.StaticInstance.configs.SwapRequestAcceptCommand;

        public string[] Aliases => ScpUtils.StaticInstance.configs.SwapRequestAcceptCommandAliases;

        public string Description => "Accept an SCP swap request";

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
            if (PluginAPI.Core.Round.Duration.TotalSeconds  >= ScpUtils.StaticInstance.configs.MaxAllowedTimeScpSwapRequestAccept)
            {
                response = $"<color=red>The round has been started from too much time due this reason our system decided to block this SCP swap request, you are too slow next time try to be faster</color>";
                return false;
            }
            if (!ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsValue(player))
            {
                response = $"<color=red>You haven't any swap request!</color>";
                return false;
            }
            if (ScpUtils.StaticInstance.configs.AllowSCPSwapOnlyFullHealth && player.Health < player.MaxHealth)
            {
                response = $"<color=red>You have taken damage due this reason our system decided to block this SCP swap request, next time be more careful and play better</color>";
                return false;
            }

            var target = ScpUtils.StaticInstance.EventHandlers.SwapRequest.FirstOrDefault(x => x.Value == player).Key;

            if (ScpUtils.StaticInstance.configs.AllowSCPSwapOnlyFullHealth && target.Health < target.MaxHealth)
            {
                response = $"<color=red>Swap is impossible: requester has taken damage.</color>";
                return false;
            }

            if (!target.IsSCP)
            {
                response = $"<color=red>Target is not an SCP</color>";
                return false;
            }

            if (target.CustomInfo != null && ScpUtils.StaticInstance.configs.DeniedSwapCustomInfo?.Any() == true)
            {
                if (ScpUtils.StaticInstance.configs.DeniedSwapCustomInfo.Contains(target.CustomInfo.ToString()))
                {
                    response = $"<color=red>Target is using a custom SCP therefore swap is denied!</color>";
                    return false;
                }
            }

            if (player.CustomInfo != null && ScpUtils.StaticInstance.configs.DeniedSwapCustomInfo?.Any() == true)
            {
                if (ScpUtils.StaticInstance.configs.DeniedSwapCustomInfo.Contains(player.CustomInfo.ToString()))
                {
                    response = $"<color=red>You are using a custom SCP therefore swap is denied!</color>";
                    return false;
                }
            }

            var scp = player.Role;

            player.SetRole(target.Role);
            target.SetRole(scp);
            ScpUtils.StaticInstance.EventHandlers.SwapRequest.Remove(target);
            response = $"<color=green>Swap request has been accepted</color>";
            return true;
        }
    }
}
