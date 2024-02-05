using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PlayerResetReset : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.ResetpreferencesCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.ResetpreferencesAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.ResetpreferencesDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.playerresetpreferences"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            else if (arguments.Count < 1)
            {
                response = $"<color=red>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer}</color>";
                return false;
            }
            else
            {
                string target = arguments.Array[1].ToString();
                Player databasePlayer = target.GetDatabasePlayer();

                if (databasePlayer == null)
                {
                    response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                    return false;
                }

                databasePlayer.ResetPreferences();
                databasePlayer.SaveData();
                response = ScpUtils.StaticInstance.Translation.Success;

                return true;
            }
        }
    }
}
