using System;
using System.Collections.Generic;
using Exiled.Events.EventArgs;
using Player = Exiled.API.Features.Player;
using Exiled.Permissions.Extensions;
using System.Linq;

namespace SCPUtils
{
    public class ConsoleCommands
    {
        private readonly ScpUtils pluginInstance;
        public ConsoleCommands(ScpUtils pluginInstance) => this.pluginInstance = pluginInstance;

        private readonly List<string> validColors = new List<string> { "pink", "red", "default", "brown", "silver", "light_green", "crismon", "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta", "blue_green", "orange", "lime", "green", "emerald", "carmine", "nickel", "mint", "army_green", "pumpkin" };
        public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
        {


            switch (ev.Name)
            {
                case "scputils_help":
                    {
                        ev.Allow = false;
                        ev.Color = "green";
                        ev.ReturnMessage = "Avaible commands: \n" +
                            ".scputils_info, .scputils_help, .scputils_change_nickname, .scputils_change_color, .scputils_show_badge, .scputils_hide_badge";
                        break;
                    }


                case "scputils_info":
                    {
                        ev.Allow = false;
                        ev.Color = "green";
                        ev.ReturnMessage = "Plugin Info: \n" +
                            "SCPUtils is a public plugin created by Terminator_97#0507, you can download this plugin at: github.com/terminator-97/SCPUtils \n" +
                            $"This server is running SCPUtils version {ScpUtils.pluginVersion}";
                        break;
                    }


                case "scputils_change_nickname":
                    {
                        ev.Allow = false;
                        var commandSender = Exiled.API.Features.Player.Get(ev.Player.UserId);

                        if (commandSender == null)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "An error has occured while executing this command!";
                            break;
                        }

                        if (ev.Arguments.Count < 1)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "Usage: scputils_changenickname <Nick>";
                            break;
                        }

                        if (IsAllowed(commandSender.Nickname, "scputils.changenickname"))
                        {

                            if (ev.Arguments[0].ToLower() == "none")
                            {
                                ev.Color = "green";
                                ev.ReturnMessage = "Your nickname has been removed, changes will take effect next round!";
                                ev.Player.GetDatabasePlayer().CustomNickName = "";
                            }

                            else
                            {
                                bool allowChange = true;
                                foreach (var player in Exiled.API.Features.Player.List)
                                {
                                    if (player.Nickname.ToLower() == ev.Arguments[0].ToLower())
                                    {
                                        allowChange = false;
                                        break;
                                    }
                                }
                                if (!allowChange)
                                {
                                    ev.Color = "red";
                                    ev.ReturnMessage = "This nickname is already used by another player, please choose another name!";
                                    break;
                                }
                                else if (pluginInstance.Functions.CheckNickname(ev.Arguments[0]) && !IsAllowed(commandSender.Nickname, "scputils.bypassnickrestriction"))
                                {
                                    ev.Color = "red";
                                    ev.ReturnMessage = pluginInstance.Config.InvalidNicknameText;
                                    break;
                                }
                                ev.Color = "green";
                                ev.ReturnMessage = "Your nickname has been changed, changes will take effect next round, use scputils_change_nickname None to remove the nickname";
                                string nickname = ev.Arguments[0];
                                ev.Player.GetDatabasePlayer().CustomNickName = nickname;
                                ev.Player.Nickname = nickname;
                            }
                        }
                        else
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = pluginInstance.Config.UnauthorizedNickNameChange;
                        }
                        break;
                    }

                case "scputils_change_color":
                    {
                        ev.Allow = false;
                        var commandSender = Exiled.API.Features.Player.Get(ev.Player.UserId);

                        if (commandSender == null)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "An error has occured while executing this command!";
                            break;
                        }
                        if (ev.Arguments.Count < 1)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "Usage: scputils_changecolor <Color / None>";
                            break;
                        }
                        if (IsAllowed(commandSender.Nickname, "scputils.changecolor"))
                        {

                            if (ev.Arguments[0].ToLower() == "none")
                            {
                                ev.Color = "green";
                                ev.ReturnMessage = "Your color has been removed, changes will take effect next round!";
                                ev.Player.GetDatabasePlayer().ColorPreference = "";
                            }
                            if (validColors.Contains(ev.Arguments[0]))
                            {
                                if (pluginInstance.Config.RestrictedRoleColors.Contains(ev.Arguments[0]))
                                {
                                    ev.Color = "red";
                                    ev.ReturnMessage = "This color has been restricted by server owner, please use another color!";
                                    break;
                                }
                                else
                                {
                                    ev.Color = "green";
                                    ev.ReturnMessage = "Your color has been changed, use scputils_change_color None to remove the color!";
                                    commandSender.RankColor = ev.Arguments[0];
                                    string colorPreference = ev.Arguments[0];
                                    ev.Player.GetDatabasePlayer().ColorPreference = colorPreference;
                                }
                            }
                            else
                            {
                                ev.Color = "red";
                                ev.ReturnMessage = "Invalid color, type color in console to see valid SCP colors";
                            }
                        }
                        else
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = pluginInstance.Config.UnauthorizedColorChange;
                        }
                        break;
                    }


                case "scputils_hide_badge":
                    {
                        ev.Allow = false;
                        var commandSender = Exiled.API.Features.Player.Get(ev.Player.UserId);

                        if (commandSender == null)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "An error has occured while executing this command!";
                            break;
                        }
                        if (IsAllowed(commandSender.Nickname, "scputils.badgevisibility"))
                        {
                            ev.Player.BadgeHidden = true;
                            commandSender.GetDatabasePlayer().HideBadge = true;
                            ev.Color = "green";
                            ev.ReturnMessage = "Your badge has been hidden!";
                        }
                        else
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = pluginInstance.Config.UnauthorizedBadgeChangeVisibility;
                        }
                        break;
                    }


                case "scputils_show_badge":
                    {
                        ev.Allow = false;
                        var commandSender = Exiled.API.Features.Player.Get(ev.Player.UserId);

                        if (commandSender == null)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "An error has occured while executing this command!";
                            break;
                        }

                        if (IsAllowed(commandSender.Nickname, "scputils.badgevisibility"))
                        {
                            ev.Player.BadgeHidden = false;
                            commandSender.GetDatabasePlayer().HideBadge = false;
                            ev.Color = "green";
                            ev.ReturnMessage = "Your badge has been shown!";
                        }
                        else
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = pluginInstance.Config.UnauthorizedBadgeChangeVisibility;
                        }
                        break;
                    }

                case "scputils_my_info":
                    {
                        ev.Allow = false;
                        var commandSender = Exiled.API.Features.Player.Get(ev.Player.UserId);

                        if (commandSender == null)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "An error has occured while executing this command!";
                            break;
                        }
                        ev.Color = "green";
                        var databasePlayer = commandSender.GetDatabasePlayer();
                        ev.ReturnMessage = "Those are your preferences and informations:\n" +
                        $"Custom Name: [ {databasePlayer.CustomNickName} ]\n" +
                        $"Badge Color: [ {databasePlayer.ColorPreference} ]\n" +
                        $"Hide Badge: [ {databasePlayer.HideBadge} ]\n" +
                        $"Temporarily Badge: [ {databasePlayer.BadgeName} ]\n" +
                        $"Badge Expire: [ {databasePlayer.BadgeExpire} ]\n" +
                        $"First Join: [ {databasePlayer.FirstJoin} ]\n" +
                        $"Total Playtime: [ { new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString() } ]";
                        break;
                    }
            }

        }
        private bool IsAllowed(string sender, string permission)
        {
            Exiled.API.Features.Player player;
            return sender != null && (sender == "GAME CONSOLE" || (player = Exiled.API.Features.Player.Get(sender)) == null || Exiled.Permissions.Extensions.Permissions.CheckPermission(player, permission));
        }


    }
}