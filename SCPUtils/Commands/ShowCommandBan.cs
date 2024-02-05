using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class ShowCommandBans : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.ShowcommandbansCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.ShowcommandbansAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.ShowbadgeDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;

            }
            string target;
            if (!sender.CheckPermission("scputils.moderatecommands"))
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

            string message = $"\n[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]\n\n" +
  $"Total Command restrictions: [ {databasePlayer.Restricted.Count} ]\n";


            if (databasePlayer.IsRestricted())
            {
                message += $"*** Active Restrictions: ***\n\n" +
                               $"Reason: [ {databasePlayer.Restricted.Values.Last()} ]\n" +
                               $"Expire: [ {databasePlayer.Restricted.Keys.Last()} ]\n";
            }

            if (databasePlayer.Restricted.Count >= 1)
            {
                message += $"\n*** Restrictions History: ***\n\n";
                foreach (System.Collections.Generic.KeyValuePair<DateTime, string> a in databasePlayer.Restricted)
                {
                    message += $"Reason: [ {a.Value} ]\n";
                    message += $"Expire: [ {a.Key} ]\n\n";
                }
            }
            else
            {
                message += "No restrictions!";
            }

            response = $"{message}";

            return true;
        }
    }
}
