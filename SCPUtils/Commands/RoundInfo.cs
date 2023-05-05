using CommandSystem;
using PluginAPI.Core;
using System;
using System.Text;
using System.Linq;
using Respawning;

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
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            if (!(PluginAPI.Core.Player.Get((CommandSender)sender) is PluginAPI.Core.Player player))
            {
                player = PluginAPI.Core.Player.GetPlayers().FirstOrDefault(x => x.IsServer);
            } 

            if (!sender.CheckPermission("scputils.roundinfo.execute") && !ScpUtils.StaticInstance.configs.AllowedMtfInfoTeam.Contains(player.Team) && !ScpUtils.StaticInstance.configs.AllowedChaosInfoTeam.Contains(player.Team))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }

            if (!PluginAPI.Core.Round.IsRoundStarted)
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
                    message.AppendLine($"Round time: {PluginAPI.Core.Round.Duration.ToString(@"hh\:mm\:ss")}");
                }
                if (sender.CheckPermission("scputils.roundinfo.tickets") || ScpUtils.StaticInstance.configs.AllowedChaosInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Number of Chaos Tickets: {PluginAPI.Core.Respawn.ChaosTickets}");
                }
                if (sender.CheckPermission("scputils.roundinfo.tickets") || ScpUtils.StaticInstance.configs.AllowedMtfInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Number of MTF Tickets: {PluginAPI.Core.Respawn.NtfTickets}");
                }

                if (sender.CheckPermission("scputils.roundinfo.nextrespawnteam") || ScpUtils.StaticInstance.configs.AllowedChaosInfoTeam.Contains(player.Team) || ScpUtils.StaticInstance.configs.AllowedMtfInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Next known Respawn Team: {RespawnManager.Singleton.NextKnownTeam}");
                    message.AppendLine($"Time until respawn:" + TimeSpan.FromSeconds(RespawnManager.Singleton._timeForNextSequence - (float)RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds));
                }

                if (sender.CheckPermission("scputils.roundinfo.respawncount") || ScpUtils.StaticInstance.configs.AllowedChaosInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Number of Chaos Respawn Waves: {ScpUtils.StaticInstance.EventHandlers.ChaosRespawnCount}");
                }
                if (sender.CheckPermission("scputils.roundinfo.respawncount") || ScpUtils.StaticInstance.configs.AllowedMtfInfoTeam.Contains(player.Team))
                {
                    message.AppendLine($"Number of Mtf Respawn Waves: {ScpUtils.StaticInstance.EventHandlers.MtfRespawnCount}");
                }
                if (sender.CheckPermission("scputils.roundinfo.lastrespawn") || ScpUtils.StaticInstance.configs.AllowedChaosInfoTeam.Contains(player.Team))
                {
                    if (ScpUtils.StaticInstance.EventHandlers.ChaosRespawnCount >= 1)
                    {
                        TimeSpan timespan = (DateTime.Now - ScpUtils.StaticInstance.EventHandlers.LastChaosRespawn);
                        message.AppendLine($"Last Chaos wave respawn elapsed time: { timespan.ToString(@"hh\:mm\:ss")} ").AppendLine();

                    }
                }
                if (sender.CheckPermission("scputils.roundinfo.lastrespawn") || ScpUtils.StaticInstance.configs.AllowedMtfInfoTeam.Contains(player.Team))
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
