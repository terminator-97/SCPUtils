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

        public string Description { get; } = ScpUtils.StaticInstance.Translation.RespawnDescription;

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
                response = ScpUtils.StaticInstance.Translation.NotAlive;
                return false;
            }

            if (!Exiled.API.Features.Round.IsStarted)
            {
                response = ScpUtils.StaticInstance.Translation.RoundNotStarted;
                return false;
            }

            if (DateTime.Now > EventHandlers.LastRespawn[player])
            {
                response = ScpUtils.StaticInstance.Translation.OutofTime;
                return false;
            }


            if (ScpUtils.StaticInstance.Config.RespawnOnlyFullHealth && player.Health < player.MaxHealth)
            {
                response = ScpUtils.StaticInstance.Translation.Damaged;
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
                response = ScpUtils.StaticInstance.Translation.Success;
                EventHandlers.LastRespawn[player] = respawndate;
                return false;

            }
        }
    }
}
