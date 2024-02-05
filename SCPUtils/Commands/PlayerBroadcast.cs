using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class PlayerBroadcast : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.PlayerbroadcastCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.PlayerbroadcastAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.PlayerbroadcastDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.broadcast"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} {ScpUtils.StaticInstance.Translation.ArgBroadcast} {ScpUtils.StaticInstance.Translation.ArgId} {ScpUtils.StaticInstance.Translation.ArgSeconds}{ScpUtils.StaticInstance.Translation.Optional}</color>";
                return false;
            }
            else
            {
                var databaseBroadcast = GetBroadcast.FindBroadcast(arguments.Array[3]);

                if (databaseBroadcast == null)
                {
                    response = ScpUtils.StaticInstance.Translation.InvalidId;
                    return false;
                }

                Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(arguments.Array[1].ToString());
                if (player == null)
                {
                    response = ScpUtils.StaticInstance.Translation.InvalidPlayer;
                    return false;
                }

                int duration = databaseBroadcast.Seconds;
                if (arguments.Count == 4)
                {
                    if (int.TryParse(arguments.Array[4].ToString(), out duration)) { }
                    else
                    {
                        response = ScpUtils.StaticInstance.Translation.InvalidArgInt;
                        return false;
                    }
                }

                switch (arguments.Array[2].ToString())
                {
                    case "broadcast":
                    case "bc":
                        player.Broadcast((ushort)duration, databaseBroadcast.Text, global::Broadcast.BroadcastFlags.Normal, false);
                        response = ScpUtils.StaticInstance.Translation.Success;
                        break;
                    case "hint":
                    case "h":
                        player.ShowHint(databaseBroadcast.Text, duration);
                        response = ScpUtils.StaticInstance.Translation.Success;
                        break;
                    default:
                        response = ScpUtils.StaticInstance.Translation.InvalidArg;
                        break;
                }
            }

            return true;
        }
    }
}
