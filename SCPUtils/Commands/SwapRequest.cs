using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SwapRequest : ICommand
    {      
        public string Command { get; } = ScpUtils.StaticInstance.Config.SwapRequestCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Config.SwapRequestCommandAliases;

        public string Description { get; } = "Send a SCP swap request to a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {                      
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId);

            if (!ScpUtils.StaticInstance.Config.AllowSCPSwap)            
            {
                response = $"<color=yellow>SCP swap module is disabled on this server!</color>";
                return false;
            }

            if (!player.IsScp)
            {               
                    response = $"<color=yellow>Only SCPs are allowed to use this command!</color>";
                    return false;              
            }
       
            else
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <player></color>";
                    return false;
                }
                else if (Exiled.API.Features.Round.ElapsedTime.TotalSeconds >= ScpUtils.StaticInstance.Config.MaxAllowedTimeScpSwapRequest)
                {
                    response = $"<color=red>The round has been started from too much time due this reason our system decided to deny this SCP swap request, you are too slow next time try to be faster</color>";
                    return false;
                }
                else if (ScpUtils.StaticInstance.Config.AllowSCPSwapOnlyFullHealth && player.Health < player.MaxHealth)
                {
                    response = $"<color=red>You have taken damage due this reason our system decided to deny this SCP swap request, next time be more careful and play better</color>";
                    return false;
                }                
                else
                {
                    
                    var target = Exiled.API.Features.Player.Get(arguments.Array[1].ToString());
                    var seconds = Math.Round(ScpUtils.StaticInstance.Config.MaxAllowedTimeScpSwapRequestAccept - Exiled.API.Features.Round.ElapsedTime.TotalSeconds);
                    if (target == null)
                    {
                        response = $"<color=red>Invalid player!</color>";
                        return false;
                    }
                    if (ScpUtils.StaticInstance.Config.AllowSCPSwapOnlyFullHealth && target.Health < target.MaxHealth)
                    {
                        response = $"<color=red>Target has not full health!</color>";
                        return false;
                    }
                    if (ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsKey(target) || ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsValue(target))
                    {
                        response = $"<color=red>Target already sent a request or has a pending request</color>";
                        return false;
                    }

                    if (ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsKey(player) || ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsValue(player))
                    {
                        response = $"<color=red>You already sent or received a swap request, verify that</color>";
                        return false;
                    }
                    if(target==player)
                    {
                        response = $"<color=red>You can't send swap request to yourself!</color>";
                        return false;
                    }
                    if(!target.IsScp)
                    {
                        response = $"<color=red>Target is not SCP!</color>";
                        return false;
                    }
                    if(target.Role == player.Role)
                    {
                        response = $"<color=red>You have the same role of another player!</color>";
                        return false;
                    }

                    ScpUtils.StaticInstance.EventHandlers.SwapRequest.Add(player, target);
                    target.ClearBroadcasts();
                    var message = ScpUtils.StaticInstance.Config.SwapRequestBroadcast.Content;
                    message = message.Replace("%player%", player.DisplayNickname).Replace("%scp%", player.Role.Name).Replace("%seconds%", seconds.ToString());
                    target.Broadcast(ScpUtils.StaticInstance.Config.SwapRequestBroadcast.Duration, message, ScpUtils.StaticInstance.Config.SwapRequestBroadcast.Type, false);
                    response = $"<color=green>Request has been sent successfully, player has {seconds} seconds to accept the request. Use .cancel to cancel the request</color>";
                    return true;
                }
            }            
        }
    }
}
