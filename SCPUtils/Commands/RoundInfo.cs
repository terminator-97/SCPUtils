using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class RoundInfo : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.RoundinfoCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.RoundinfoAliases;
        public string Description { get; } = ScpUtils.StaticInstance.Translation.RoundinfoDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!(Exiled.API.Features.Player.Get((CommandSender)sender) is Exiled.API.Features.Player player))
            {
                player = Exiled.API.Features.Server.Host;
            }

            if (!sender.CheckPermission("scputils.roundinfo.execute") && !ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Role.Team) && !ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Role.Team))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }

            if (!Exiled.API.Features.Round.IsStarted)
            {
                response = ScpUtils.StaticInstance.Translation.RoundNotStarted;
                return true;
            }
            else
            {
                StringBuilder message = new StringBuilder($"{ScpUtils.StaticInstance.Translation.Roundinfo}");
                if (sender.CheckPermission("scputils.roundinfo.roundtime"))
                {
                    message.AppendLine();
                    message.AppendLine($"{ScpUtils.StaticInstance.Translation.Roundinfo} {Exiled.API.Features.Round.ElapsedTime.ToString(@"hh\:mm\:ss")}");
                }
                if (sender.CheckPermission("scputils.roundinfo.tickets") || ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Role.Team))
                {
                    message.AppendLine($"{ScpUtils.StaticInstance.Translation.Roundinfochaostickets} {Exiled.API.Features.Respawn.ChaosTickets}");
                }
                if (sender.CheckPermission("scputils.roundinfo.tickets") || ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Role.Team))
                {
                    message.AppendLine($"{ScpUtils.StaticInstance.Translation.Roundinfomtftickets} {Exiled.API.Features.Respawn.NtfTickets}");
                }

                if (sender.CheckPermission("scputils.roundinfo.nextrespawnteam") || ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Role.Team) || ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Role.Team))
                {
                    message.AppendLine($"{ScpUtils.StaticInstance.Translation.Roundinforespawnteam} {Exiled.API.Features.Respawn.NextKnownTeam}");
                    message.AppendLine($"{ScpUtils.StaticInstance.Translation.Roundinforespawntime} {TimeSpan.FromSeconds(Exiled.API.Features.Respawn.TimeUntilSpawnWave.TotalSeconds).ToString(@"hh\:mm\:ss")}");
                }

                if (sender.CheckPermission("scputils.roundinfo.respawncount") || ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Role.Team))
                {
                    message.AppendLine($"{ScpUtils.StaticInstance.Translation.Roundinfonumberchaosrespawn} {ScpUtils.StaticInstance.EventHandlers.ChaosRespawnCount}");
                }
                if (sender.CheckPermission("scputils.roundinfo.respawncount") || ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Role.Team))
                {
                    message.AppendLine($"{ScpUtils.StaticInstance.Translation.Roundinfonumbermtfrespawn} {ScpUtils.StaticInstance.EventHandlers.MtfRespawnCount}");
                }
                if (sender.CheckPermission("scputils.roundinfo.lastrespawn") || ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Role.Team))
                {
                    if (ScpUtils.StaticInstance.EventHandlers.ChaosRespawnCount >= 1)
                    {
                        TimeSpan timespan = (DateTime.Now - ScpUtils.StaticInstance.EventHandlers.LastChaosRespawn);
                        message.AppendLine($"{ScpUtils.StaticInstance.Translation.Roundinfolastchaoswave} {timespan.ToString(@"hh\:mm\:ss")} ").AppendLine();

                    }
                }
                if (sender.CheckPermission("scputils.roundinfo.lastrespawn") || ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Role.Team))
                {
                    if (ScpUtils.StaticInstance.EventHandlers.MtfRespawnCount >= 1)
                    {
                        TimeSpan timespan = (DateTime.Now - ScpUtils.StaticInstance.EventHandlers.LastMtfRespawn);

                        message.AppendLine($"{ScpUtils.StaticInstance.Translation.Roundinfolastmtfwave} {timespan.ToString(@"hh\:mm\:ss")}");
                    }
                }
                response = $"{message}";

            }
            return true;
        }
    }
}
