using CommandSystem;
using MEC;
using System;
using System.Linq;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Respawn : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Config.RespawnCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Config.RespawnCommandAliases;

        public string Description { get; } = "Send a SCP swap request to a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId);

            if (!player.IsAlive)
            {
                response = $"<color=red>You are not alive!</color>";
                return false;
            }

            if (!Exiled.API.Features.Round.IsStarted)
            {
                response = $"<color=red>Round is not started!</color>";
                return false;
            }

            if (DateTime.Now > EventHandlers.LastRespawn[player])
            {
                response = $"<color=yellow>The respawn time window has ended!</color>";
                return false;
            }


            if (ScpUtils.StaticInstance.Config.RespawnOnlyFullHealth && player.Health < player.MaxHealth)
            {
                response = $"<color=red>You have taken damage therefore respawn is not available.</color>";
                return false;
            }

            else
            {
                var respawndate = EventHandlers.LastRespawn[player];
                var inv = player.Items.ToList();
                var ammo = player.Ammo;
                player.Role.Set(player.Role, PlayerRoles.RoleSpawnFlags.UseSpawnpoint);
                player.ClearInventory(true);
                Timing.CallDelayed(1.1f, () =>
                {
                    player.ClearInventory(true);
                    foreach (var a in inv)
                    {
                        player.AddItem(a.Type);
                    }
                });
                response = $"<color=green>Respawn granted</color>";
                EventHandlers.LastRespawn[player] = respawndate;
                return false;

            }
        }
    }
}
