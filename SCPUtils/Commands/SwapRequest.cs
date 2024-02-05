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

        public string Description { get; } = ScpUtils.StaticInstance.Translation.SwaprequestDescription;

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
                response = ScpUtils.StaticInstance.Translation.SwaprequestDisabled;
                return false;
            }

            if (!player.IsScp)
            {
                response = ScpUtils.StaticInstance.Translation.ScplistNoScp;
                return false;
            }

            if (!Exiled.API.Features.Round.IsStarted)
            {
                response = ScpUtils.StaticInstance.Translation.RoundNotStarted;
                return false;
            }


            else
            {

                if (arguments.Count < 1)
                {
                    var message = ScpUtils.StaticInstance.Translation.SwaprequestUsage.Replace("%command%", Command);
                    response = message;
                    return false;
                }
                else if (Exiled.API.Features.Round.ElapsedTime.TotalSeconds >= ScpUtils.StaticInstance.Config.MaxAllowedTimeScpSwapRequest)
                {
                    response = ScpUtils.StaticInstance.Translation.OutofTime;
                    return false;
                }
                else if (ScpUtils.StaticInstance.Config.AllowSCPSwapOnlyFullHealth && player.Health < player.MaxHealth)
                {
                    response = ScpUtils.StaticInstance.Translation.Damaged;
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

                        case "SCP-3114":
                        case "SCP3114":
                        case "3114":
                            target = Eplayer.List.FirstOrDefault(x => x.Role.Type == PlayerRoles.RoleTypeId.Scp3114);
                            role = PlayerRoles.RoleTypeId.Scp3114;
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
                                    response = ScpUtils.StaticInstance.Translation.SwaprequestRoledisabled;
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
                                            response = ScpUtils.StaticInstance.Translation.SwaprequestLimit;
                                            return false;
                                        }
                                        ScpUtils.StaticInstance.EventHandlers.SwapCount[player]++;
                                    }
                                    else
                                    {
                                        ScpUtils.StaticInstance.EventHandlers.SwapCount.Add(player, 1);
                                    }


                                    player.Role.Set(role);
                                    response = ScpUtils.StaticInstance.Translation.Success;
                                    return true;
                                }
                                else
                                {
                                    response = ScpUtils.StaticInstance.Translation.SwaprequestNoautoswap;
                                    return false;
                                }
                            }
                            else
                            {
                                response = ScpUtils.StaticInstance.Translation.SwaprequestInvalidplayer;
                                return false;
                            }
                        }
                        else
                        {
                            response = ScpUtils.StaticInstance.Translation.SwaprequestInvalidplayer;
                            return false;
                        }

                    }


                    if (ScpUtils.StaticInstance.Config.AllowSCPSwapOnlyFullHealth && target.Health < target.MaxHealth)
                    {
                        response = ScpUtils.StaticInstance.Translation.TargetDamaged;
                        return false;
                    }

                    if (ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsKey(target) || ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsValue(target))
                    {
                        response = ScpUtils.StaticInstance.Translation.SwaprequestPendingtargeterror;
                        return false;
                    }

                    if (ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsKey(player) || ScpUtils.StaticInstance.EventHandlers.SwapRequest.ContainsValue(player))
                    {
                        response = ScpUtils.StaticInstance.Translation.SwaprequestPendingerror;
                        return false;
                    }

                    if (target == player)
                    {
                        response = ScpUtils.StaticInstance.Translation.SwaprequestSelferror;
                        return false;
                    }

                    if (!target.IsScp)
                    {
                        response = ScpUtils.StaticInstance.Translation.SwaprequestTargetnoscp;
                        return false;
                    }

                    if (target.Role == player.Role)
                    {
                        response = ScpUtils.StaticInstance.Translation.SwaprequestSameroleerror;
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

                    ScpUtils.StaticInstance.EventHandlers.SwapRequest.Add(player, target);

                    target.ClearBroadcasts();

                    var message = ScpUtils.StaticInstance.Config.SwapRequestBroadcast.Content;

                    message = message.Replace("%player%", player.DisplayNickname).Replace("%scp%", player.Role.Name).Replace("%seconds%", seconds.ToString());
                    target.Broadcast(ScpUtils.StaticInstance.Config.SwapRequestBroadcast.Duration, message, ScpUtils.StaticInstance.Config.SwapRequestBroadcast.Type, false);


                    var messageSuccess = ScpUtils.StaticInstance.Translation.SwaprequestSuccess;
                    messageSuccess = messageSuccess.Replace("%target", target.DisplayNickname).Replace("%seconds%", seconds.ToString()).Replace("%cancelcommand", ScpUtils.StaticInstance.Config.SwapRequestCancelCommand);
                    response = messageSuccess;

                    return true;
                }
            }
        }
    }
}
