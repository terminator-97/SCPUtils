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
        public string Command { get; } = "scputils_round_info";

        public string[] Aliases { get; } = new[] { "ri", "roundinfo", "round_info", "su_ri", "su_roundinfo", "su_round_info", "scpu_ri", "scpu_roundinfo", "scpu_round_info" };
        public string Description { get; } = "Show round info";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!(Exiled.API.Features.Player.Get((CommandSender)sender) is Exiled.API.Features.Player player))
            {
                player = Exiled.API.Features.Server.Host;
            }

            if (!sender.CheckPermission("scputils.roundinfo.execute") && !ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Team) && !ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Team))
            {
                response = "You need a higher administration level to use this command!";
                return false;
            }

            if (!Exiled.API.Features.Round.IsStarted)
            {
                response = "Round is not started yet!";
                return true;
            }
            else
            {
                StringBuilder message = new StringBuilder($"Round Info:");
                if (sender.CheckPermission("scputils.roundinfo.roundtime"))
                {
                    message.AppendLine();
                    message.AppendLine($"Round time: {Exiled.API.Features.Round.ElapsedTime.ToString(@"hh\:mm\:ss")}");
                }
                if (sender.CheckPermission("scputils.roundinfo.tickets") || ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Number of Chaos Tickets: {Exiled.API.Features.Respawn.ChaosTickets}");
                }
                if (sender.CheckPermission("scputils.roundinfo.tickets") || ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Number of MTF Tickets: {Exiled.API.Features.Respawn.NtfTickets}");
                }

                if (sender.CheckPermission("scputils.roundinfo.nextrespawnteam") || ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Team) || ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Next known Respawn Team: {Exiled.API.Features.Respawn.NextKnownTeam}");
                    message.AppendLine($"Time until respawn: {TimeSpan.FromSeconds(Exiled.API.Features.Respawn.TimeUntilRespawn).ToString(@"hh\:mm\:ss")}");
                }

                if (sender.CheckPermission("scputils.roundinfo.respawncount") || ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Number of Chaos Respawn Waves: {ScpUtils.StaticInstance.EventHandlers.ChaosRespawnCount}");
                }
                if (sender.CheckPermission("scputils.roundinfo.respawncount") || ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Number of Mtf Respawn Waves: {ScpUtils.StaticInstance.EventHandlers.MtfRespawnCount}");
                }
                if (sender.CheckPermission("scputils.roundinfo.lastrespawn") || ScpUtils.StaticInstance.Config.AllowedChaosInfoTeam.Contains(player.Team))
                {
                    if (ScpUtils.StaticInstance.EventHandlers.ChaosRespawnCount >= 1)
                    {
                        TimeSpan timespan = (DateTime.Now - ScpUtils.StaticInstance.EventHandlers.LastChaosRespawn);
                        message.AppendLine($"Last Chaos wave respawn elapsed time: { timespan.ToString(@"hh\:mm\:ss")} ").AppendLine();

                    }
                }
                if (sender.CheckPermission("scputils.roundinfo.lastrespawn") || ScpUtils.StaticInstance.Config.AllowedMtfInfoTeam.Contains(player.Team))
                {
                    if (ScpUtils.StaticInstance.EventHandlers.MtfRespawnCount >= 1)
                    {
                        TimeSpan timespan = (DateTime.Now - ScpUtils.StaticInstance.EventHandlers.LastMtfRespawn);

                        message.AppendLine($"Last MTF wave respawn elapsed time: { timespan.ToString(@"hh\:mm\:ss")}");
                    }
                }
                response = $"{message}";

            }
            return true;
        }
    }
}
