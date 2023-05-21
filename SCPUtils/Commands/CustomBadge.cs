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

        public string Command { get; } = "scputils_custom_badge";

        public string[] Aliases { get; } = new[] { "cb", "scputils_cbadge", "su_cb", "su_customb" };

        public string Description { get; } = "You can set a custom badge name to any player";

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
                    response = $"<color=yellow>Usage: {Command} <player name/id> <Custom badge name / None> </color>";
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
                response = "<color=red> You need a higher administration level to use this command!</color>";
                return false;
            }



            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            if (badge.ToLower() == "none")
            {
                databasePlayer.CustomBadgeName = "";
                databasePlayer.SaveData();
                var plr = Exiled.API.Features.Player.Get(target);              
                response = "<color=green>Custom badge removed, changes will take effect next round!</color>";
                return true;
            }     


            databasePlayer.CustomBadgeName = badge;           
            databasePlayer.SaveData();        
            var player = Exiled.API.Features.Player.Get(target);

            if (player != null)
            {
                if (player.Group != null)
                {
                    player.Group.BadgeText = badge;
                    player.BadgeHidden = player.BadgeHidden;                    
                }
                else
                {
                    response = "<color=green>Badge set, it won't be visible until a player has an assigned usergroup!</color>";
                    return true;
                }
            }

            response = "<color=green>Success, custom badge updated!</color>";
            return true;
        }
    }
}
