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

        public string Description => ScpUtils.StaticInstance.Translation.SwaprequestacceptDescription;

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
            if (Exiled.API.Features.Round.ElapsedTime.TotalSeconds >= ScpUtils.StaticInstance.Config.MaxAllowedTimeScpSwapRequestAccept)
            {
                response = ScpUtils.StaticInstance.Translation.OutofTime;
                return false;
            }
            if (!ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsValue(player))
            {
                response = ScpUtils.StaticInstance.Translation.SwaprequestacceptNoswaprequest;
                return false;
            }
            if (ScpUtils.StaticInstance.Config.AllowSCPSwapOnlyFullHealth && player.Health < player.MaxHealth)
            {
                response = ScpUtils.StaticInstance.Translation.Damaged;
                return false;
            }

            var target = ScpUtils.StaticInstance.EventHandlers.SwapRequest.FirstOrDefault(x => x.Value == player).Key;

            if (ScpUtils.StaticInstance.Config.AllowSCPSwapOnlyFullHealth && target.Health < target.MaxHealth)
            {
                response = ScpUtils.StaticInstance.Translation.TargetDamaged;
                return false;
            }

            if (!target.IsScp)
            {
                response = ScpUtils.StaticInstance.Translation.SwaprequestTargetnoscp;
                return false;
            }

            if (target.CustomInfo != null && ScpUtils.StaticInstance.Config.DeniedSwapCustomInfo?.Any() == true)
            {
                if (ScpUtils.StaticInstance.Config.DeniedSwapCustomInfo.Contains(target.CustomInfo.ToString()))
                {
                    response = ScpUtils.StaticInstance.Translation.SwaprequestTargetcustomscperror;
                    return false;
                }
            }

            if (player.CustomInfo != null && ScpUtils.StaticInstance.Config.DeniedSwapCustomInfo?.Any() == true)
            {
                if (ScpUtils.StaticInstance.Config.DeniedSwapCustomInfo.Contains(player.CustomInfo.ToString()))
                {
                    response = ScpUtils.StaticInstance.Translation.SwaprequestCustomscperror;
                    return false;
                }
            }

            var scp = player.Role.Type;

            player.Role.Set(target.Role.Type);
            target.Role.Set(scp);
            ScpUtils.StaticInstance.EventHandlers.SwapRequest.Remove(target);
            response = ScpUtils.StaticInstance.Translation.Success;
            return true;
        }
    }
}
