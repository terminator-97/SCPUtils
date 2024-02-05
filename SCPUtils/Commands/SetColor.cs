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
        private readonly List<string> validColors = new List<string> { "pink", "red", "default", "brown", "silver", "light_green", "crismon", "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta", "blue_green", "orange", "lime", "green", "emerald", "carmine", "nickel", "mint", "army_green", "pumpkin", "rainbow", "random" };
        public string Command { get; } = ScpUtils.StaticInstance.Translation.SetcolorCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.SetcolorAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.SetcolorDescription;

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
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} {ScpUtils.StaticInstance.Translation.SetcolorArgColor}</color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                    color = arguments.Array[2].ToString().ToLower();
                    if (!validColors.Contains(color) && !color.Equals("none") && color != ScpUtils.StaticInstance.Translation.SetcolorRainbow.ToLower())
                    {
                        response = ScpUtils.StaticInstance.Translation.SetcolorInvalidcolor;
                        return false;
                    }
                }
            }
            else if (sender.CheckPermission("scputils.changecolor"))
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.SetcolorArgColor}</color>";
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

                    else if (!validColors.Contains(color) && !color.Equals("none") && color != ScpUtils.StaticInstance.Translation.SetcolorRainbow.ToLower())
                    {                     
                        response = ScpUtils.StaticInstance.Translation.SetcolorInvalidcolor;
                        return false;
                    }

                    else if (ScpUtils.StaticInstance.Config.RestrictedRoleColors.Contains(color))
                    {

                        response = ScpUtils.StaticInstance.Translation.SetcolorRestrictedcolor;
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
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }

            if (color == "none" || color == ScpUtils.StaticInstance.Translation.None)
            {
                databasePlayer.ColorPreference = "";
                databasePlayer.SaveData();
                response = ScpUtils.StaticInstance.Translation.SuccessNextRound;
                return true;
            }

            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(target);

            if (color == "rainbow" || color == "random" || color == ScpUtils.StaticInstance.Translation.SetcolorRainbow.ToLower())
            {
                if (!ScpUtils.StaticInstance.Config.AllowRainbowTags)
                {
                    response = ScpUtils.StaticInstance.Translation.SetcolorRainbowdisabled;
                    return false;
                }
            }

            if (player != null)
            {
                if (player.GlobalBadge != null)
                {
                    response = ScpUtils.StaticInstance.Translation.SetcolorGlobalbadge;
                    return false;
                }

                if (color != "rainbow"  || color != "random" || color != ScpUtils.StaticInstance.Translation.SetcolorRainbow.ToLower()) player.RankColor = color;
            }

            databasePlayer.ColorPreference = color;
            databasePlayer.SaveData();
            response = ScpUtils.StaticInstance.Translation.Success;


            return true;
        }
    }
}
