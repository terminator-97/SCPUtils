using CommandSystem;
using System;
using System.Linq;
using Eplayer = Exiled.API.Features.Player;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SwapRequest : ICommand
    {
        Eplayer target;
        PlayerRoles.RoleTypeId role;
        public string Command { get; } = ScpUtils.StaticInstance.Config.SwapRequestCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Config.SwapRequestCommandAliases;

        public string Description { get; } = "Send a SCP swap request to a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

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

            if (!Exiled.API.Features.Round.IsStarted)
            {
                response = $"<color=red>Round is not started!</color>";
                return false;
            }


            else
            {

                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <player id/nickname or SCP-NUMBER (example: swap SCP-939)></color>";
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

                    switch (arguments.Array[1].ToString().ToUpper())
                    {
                        case "SCP-939":
                        case "SCP939":
                        case "939":

                            target = Eplayer.List.FirstOrDefault(x => x.Role.Type == PlayerRoles.RoleTypeId.Scp939);
                            role = PlayerRoles.RoleTypeId.Scp939;
                            break;

                        case "SCP-049":
                        case "SCP049":
                        case "049":

                            target = Eplayer.List.FirstOrDefault(x => x.Role.Type == PlayerRoles.RoleTypeId.Scp049);
                            role = PlayerRoles.RoleTypeId.Scp049;
                            break;

                        case "SCP-0492":
                        case "SCP0492":
                        case "0492":

                            target = Eplayer.List.FirstOrDefault(x => x.Role.Type == PlayerRoles.RoleTypeId.Scp0492);
                            role = PlayerRoles.RoleTypeId.Scp0492;
                            break;

                        case "SCP-106":
                        case "SCP106":
                        case "106":

                            target = Eplayer.List.FirstOrDefault(x => x.Role.Type == PlayerRoles.RoleTypeId.Scp106);
                            role = PlayerRoles.RoleTypeId.Scp106;
                            break;

                        case "SCP-096":
                        case "SCP096":
                        case "096":

                            target = Eplayer.List.FirstOrDefault(x => x.Role.Type == PlayerRoles.RoleTypeId.Scp096);
                            role = PlayerRoles.RoleTypeId.Scp096;
                            break;

                        case "SCP-079":
                        case "SCP079":
                        case "079":

                            target = Eplayer.List.FirstOrDefault(x => x.Role.Type == PlayerRoles.RoleTypeId.Scp079);
                            role = PlayerRoles.RoleTypeId.Scp079;
                            break;

                        case "SCP-173":
                        case "SCP173":
                        case "173":
                            target = Eplayer.List.FirstOrDefault(x => x.Role.Type == PlayerRoles.RoleTypeId.Scp173);
                            role = PlayerRoles.RoleTypeId.Scp173;
                            break;


                        default:
                            target = Eplayer.Get(arguments.Array[1].ToString());
                            role = PlayerRoles.RoleTypeId.None;
                            break;

                    }

                    var seconds = Math.Round(ScpUtils.StaticInstance.Config.MaxAllowedTimeScpSwapRequestAccept - Exiled.API.Features.Round.ElapsedTime.TotalSeconds);

                    if (target == null)
                    {

                        if (role != PlayerRoles.RoleTypeId.None)
                        {
                            if (ScpUtils.StaticInstance.Config.DisallowedScpsSwapGenerationList?.Any() ?? false)
                            {
                                if (ScpUtils.StaticInstance.Config.DisallowedScpsSwapGenerationList.Contains(player.Role.Type))
                                {
                                    response = $"<color=red>This SCP cannot become {role.ToString()} using this feature because it's disabled by server owner.</color>";
                                    return false;
                                }
                            }

                            if (ScpUtils.StaticInstance.Config.AllowedSwapGenerationList?.Any() ?? false)
                            {
                                if (ScpUtils.StaticInstance.Config.AllowedSwapGenerationList.Any(x => x == role))
                                {
                                    if (ScpUtils.StaticInstance.EventHandlers.SwapCount.ContainsKey(player))
                                    {
                                        if (ScpUtils.StaticInstance.EventHandlers.SwapCount[player] >= ScpUtils.StaticInstance.Config.MaxAllowedSwaps)
                                        {
                                            response = $"<color=red>You have reached swaps requests limit for this round, another player should send it to you if he wish to swap!</color>";
                                            return false;
                                        }
                                        ScpUtils.StaticInstance.EventHandlers.SwapCount[player]++;
                                    }
                                    else
                                    {
                                        ScpUtils.StaticInstance.EventHandlers.SwapCount.Add(player, 1);
                                    }


                                    player.Role.Set(role);
                                    response = $"<color=green>Swap request has been granted by system</color>";
                                    return true;
                                }
                                else
                                {
                                    response = $"<color=red>This SCP is not allowed to auto-swap and no player is currently playing this SCP, please choose another SCP.</color>";
                                    return false;
                                }
                            }
                            else
                            {
                                response = $"<color=red>Invalid player nickname/id or invalid SCP name!</color>";
                                return false;
                            }
                        }
                        else
                        {
                            response = $"<color=red>Invalid player nickname/id or invalid SCP name!</color>";
                            return false;
                        }

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

                    if (target == player)
                    {
                        response = $"<color=red>You can't send swap request to yourself!</color>";
                        return false;
                    }

                    if (!target.IsScp)
                    {
                        response = $"<color=red>Target is not SCP!</color>";
                        return false;
                    }

                    if (target.Role == player.Role)
                    {
                        response = $"<color=red>You have the same role of another player!</color>";
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

                    ScpUtils.StaticInstance.EventHandlers.SwapRequest.Add(player, target);

                    target.ClearBroadcasts();

                    var message = ScpUtils.StaticInstance.Config.SwapRequestBroadcast.Content;

                    message = message.Replace("%player%", player.DisplayNickname).Replace("%scp%", player.Role.Name).Replace("%seconds%", seconds.ToString());
                    target.Broadcast(ScpUtils.StaticInstance.Config.SwapRequestBroadcast.Duration, message, ScpUtils.StaticInstance.Config.SwapRequestBroadcast.Type, false);

                    response = $"<color=green>Request has been sent successfully to {target.DisplayNickname}, player has {seconds} seconds to accept the request. Use .cancel to cancel the request</color>";

                    return true;
                }
            }
        }
    }
}
