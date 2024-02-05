using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class PlayerDnt : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.PlayerdntCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.PlayerdntAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.PlayerdntDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            if (!sender.CheckPermission("scputils.dnt"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            else
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer}</color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                }
            }
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }

            if (databasePlayer.IgnoreDNT == false)
            {
                databasePlayer.IgnoreDNT = true;
                databasePlayer.SaveData();
                response = ScpUtils.StaticInstance.Translation.Enabled;
            }
            else
            {
                databasePlayer.IgnoreDNT = false;
                databasePlayer.SaveData();
                response = ScpUtils.StaticInstance.Translation.Disabled;
            }


            return true;
        }
    }
}
