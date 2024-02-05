using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class Broadcast : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.BroadcastCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.BroadcastAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.BroadcastDescription;

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

            else if (arguments.Count < 2)
            {
                response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgBroadcast} {ScpUtils.StaticInstance.Translation.ArgId} {ScpUtils.StaticInstance.Translation.ArgSeconds}{ScpUtils.StaticInstance.Translation.Optional}</color>";
                return false;
            }
            else
            {
                var databaseBroadcast = GetBroadcast.FindBroadcast(arguments.Array[2]);

                if (databaseBroadcast == null)
                {
                    response = ScpUtils.StaticInstance.Translation.InvalidId;
                    return false;
                }
                int duration = databaseBroadcast.Seconds;
                if (arguments.Count == 3)
                {
                    if (int.TryParse(arguments.Array[3].ToString(), out duration)) { }
                    else
                    {
                        response = ScpUtils.StaticInstance.Translation.InvalidArgInt;
                        return false;
                    }
                }

                switch (arguments.Array[1].ToString())
                {
                    case "broadcast":
                    case "bc":
                        Map.Broadcast((ushort)duration, databaseBroadcast.Text);
                        response = ScpUtils.StaticInstance.Translation.Success;
                        break;
                    case "hint":
                    case "h":
                        Map.ShowHint(databaseBroadcast.Text, duration);
                        response = ScpUtils.StaticInstance.Translation.Success;
                        break;
                    default:
                        response = $"{ScpUtils.StaticInstance.Translation.InvalidArg}, {ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgBroadcast} {ScpUtils.StaticInstance.Translation.ArgId} {ScpUtils.StaticInstance.Translation.ArgSeconds}{ScpUtils.StaticInstance.Translation.Optional}";
                        break;
                }
            }

            return true;
        }
    }
}
