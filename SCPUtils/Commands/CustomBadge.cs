using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class CustomBadge : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.CustomBadgeCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.CustomBadgeAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.CustomBadgeDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            string badge = "";
            if (sender.CheckPermission("scputils.custombadge"))
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} {ScpUtils.StaticInstance.Translation.ArgText}</color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                    badge = string.Join(" ", arguments.Array, 2, arguments.Array.Length - 2);
                }
            }

            else
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }



            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }

            if (badge.ToLower() == "none" || badge.ToLower() == ScpUtils.StaticInstance.Translation.None)
            {
                databasePlayer.CustomBadgeName = "";
                databasePlayer.SaveData();
                var plr = Exiled.API.Features.Player.Get(target);
                plr.BadgeHidden = plr.BadgeHidden;
                response = ScpUtils.StaticInstance.Translation.Success;
                return true;
            }


            databasePlayer.CustomBadgeName = badge;
            databasePlayer.SaveData();
            var player = Exiled.API.Features.Player.Get(target);

            if (player != null)
            {
                if (player.Group != null)
                {
                    player.RankName = badge;
                }
                else
                {
                    response = ScpUtils.StaticInstance.Translation.Success;
                    return true;
                }
            }

            response = ScpUtils.StaticInstance.Translation.Success;
            return true;
        }
    }
}
