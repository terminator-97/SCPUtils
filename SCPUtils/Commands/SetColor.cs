using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SetColor : ICommand
    {
        private readonly List<string> validColors = new List<string> { "pink", "red", "default", "brown", "silver", "light_green", "crismon", "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta", "blue_green", "orange", "lime", "green", "emerald", "carmine", "nickel", "mint", "army_green", "pumpkin" };
        public string Command { get; } = "scputils_set_color";

        public string[] Aliases { get; } = new[] { "scl", "scputils_change_color", "su_sc", "su_cc", "su_setc", "sc_scolor", "scpu_sc", "scpu_cc", "scpu_setc", "scpu_scolor" };

        public string Description { get; } = "You can change everyone's color or only your one based on the permissions you have";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            string color;
            if (sender.CheckPermission("scputils.playersetcolor"))
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>Usage: {Command} <player name/id> <Color / None> </color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                    color = arguments.Array[2].ToString().ToLower();
                    if (!validColors.Contains(color) && !color.Equals("none"))
                    {
                        response = "<color=red>Invalid color, type color in console to see valid SCP colors<color>";
                        return false;
                    }
                }
            }
            else if (sender.CheckPermission("scputils.changecolor"))
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <Color / None></color>";
                    return false;
                }
                else
                {
                    target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).UserId;
                    color = arguments.Array[1].ToString().ToLower();

                    if (target.GetDatabasePlayer().IsRestricted())
                    {
                        response = "<color=red>You are banned from executing this command!</color>";
                        return false;
                    }

                    else if (!validColors.Contains(color) && !color.Equals("none"))
                    {
                        response = "<color=red>Invalid color, type color in console to see valid SCP colors</color>";
                        return false;
                    }

                    else if (ScpUtils.StaticInstance.Config.RestrictedRoleColors.Contains(color))
                    {

                        response = "<color=red>This color has been restricted by server owner, please use another color!</color>";
                        return false;
                    }

                }
            }
            else
            {
                response = $"{ScpUtils.StaticInstance.Config.UnauthorizedColorChange} ";
                return false;
            }

            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            if (color == "none")
            {
                databasePlayer.ColorPreference = "";
                databasePlayer.SaveData();
                response = "<color=green>Success, changes will take effect next round!</color>";
                return true;
            }

            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(target);

            if (player != null)
            {
                if(player.GlobalBadge != null)
                {
                    response = "<color=red>You have a global badge, as VSR rules you cannot change global badge colors!";
                    return false;
                }

                player.RankColor = color;
            }

            databasePlayer.ColorPreference = color;
            databasePlayer.SaveData();
            response = "<color=green>Success, choice has been saved!</color>";    
         

            return true;
        }
    }
}
