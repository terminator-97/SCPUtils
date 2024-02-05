using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class SetRoundBan : ICommand
    {
        public string Command { get; } = ScpUtils.StaticInstance.Translation.RoundbanCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.RoundbanAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.RoundbanDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;

            if (!sender.CheckPermission("scputils.roundban"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;


            }

            if (arguments.Count < 2)
            {
                response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} {ScpUtils.StaticInstance.Translation.ArgNumber}</color>";
                return false;
            }
            else target = arguments.Array[1].ToString();


            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }

            if (int.TryParse(arguments.Array[2].ToString(), out int rounds))
            {
                databasePlayer.RoundBanLeft = rounds;
                databasePlayer.SaveData();
                response = ScpUtils.StaticInstance.Translation.Success;
                return true;
            }

            else
            {
                response = ScpUtils.StaticInstance.Translation.InvalidArgInt;
                return false;
            }
        }
    }
}
