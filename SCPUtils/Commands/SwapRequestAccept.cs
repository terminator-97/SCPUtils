using CommandSystem;
using System;
using System.Linq;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SwapRequestAccept : ICommand
    {
        public string Command => ScpUtils.StaticInstance.Config.SwapRequestAcceptCommand;

        public string[] Aliases => ScpUtils.StaticInstance.Config.SwapRequestAcceptCommandAliases;

        public string Description => "Accept an SCP swap request";

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
                response = "<color=red>You are not SCP</color>";
                return false;
            }
            if (Exiled.API.Features.Round.ElapsedTime.TotalSeconds >= ScpUtils.StaticInstance.Config.MaxAllowedTimeScpSwapRequestAccept)
            {
                response = $"<color=red>The round has been started from too much time due this reason our system decided to block this SCP swap request, you are too slow next time try to be faster</color>";
                return false;
            }
            if (!ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsValue(player))
            {
                response = $"<color=red>You haven't any swap request!</color>";
                return false;
            }
            if (ScpUtils.StaticInstance.Config.AllowSCPSwapOnlyFullHealth && player.Health < player.MaxHealth)
            {
                response = $"<color=red>You have taken damage due this reason our system decided to block this SCP swap request, next time be more careful and play better</color>";
                return false;
            }

            var target = ScpUtils.StaticInstance.EventHandlers.SwapRequest.FirstOrDefault(x => x.Value == player).Key;

            if (ScpUtils.StaticInstance.Config.AllowSCPSwapOnlyFullHealth && target.Health < target.MaxHealth)
            {
                response = $"<color=red>Swap is impossible: requester has taken damage.</color>";
                return false;
            }

            if (target.CustomInfo != null && ScpUtils.StaticInstance.Config.DeniedSwapCustomInfo?.Any() == true)
            {
                if (ScpUtils.StaticInstance.Config.DeniedSwapCustomInfo.Contains(target.CustomInfo.ToString()))
                {
                    response = $"<color=red>Target is using a custom SCP therefore swap is denied!</color>";
                    return false;
                }
            }

            if (player.CustomInfo != null && ScpUtils.StaticInstance.Config.DeniedSwapCustomInfo?.Any() == true)
            {
                if (ScpUtils.StaticInstance.Config.DeniedSwapCustomInfo.Contains(player.CustomInfo.ToString()))
                {
                    response = $"<color=red>You are using a custom SCP therefore swap is denied!</color>";
                    return false;
                }
            }

            var scp = player.Role.Type;

            player.Role.Set(target.Role.Type);
            target.Role.Set(scp);
            ScpUtils.StaticInstance.EventHandlers.SwapRequest.Remove(target);
            response = $"<color=green>Swap request has been accepted</color>";
            return true;
        }
    }
}
