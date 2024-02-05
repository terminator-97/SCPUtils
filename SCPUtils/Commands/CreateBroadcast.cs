using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class CreateBroadcast : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.BroadcastcreateCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.BroadcastcreateAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.BroadcastcreateDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.broadcastcreate"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgId} {ScpUtils.StaticInstance.Translation.ArgSeconds} {ScpUtils.StaticInstance.Translation.ArgText}";
                return false;
            }
            else
            {
                if (int.TryParse(arguments.Array[2].ToString(), out int duration))
                {
                    if (GetBroadcast.FindBroadcast(arguments.Array[1].ToString()) != null)
                    {
                        response = ScpUtils.StaticInstance.Translation.InvalidId;
                        return false;
                    }
                    else
                    {
                        var broadcast = string.Join(" ", arguments.Array, 3, arguments.Array.Length - 3);
                        GetBroadcast.AddBroadcast(arguments.Array[1].ToString(), duration, broadcast.ToString(), sender.LogName);
                        response = ScpUtils.StaticInstance.Translation.Success;
                        return true;
                    }
                }
                else
                {
                    response = ScpUtils.StaticInstance.Translation.InvalidArgInt;
                    return false;
                }
            }


        }
    }
}
