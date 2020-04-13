using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using MEC;

namespace SCPUtils
{
    public class ConsoleCommands
    {
        private readonly SCPUtils pluginInstance;
        public ConsoleCommands(SCPUtils pluginInstance) => this.pluginInstance = pluginInstance;

        private readonly List<string> validColors = new List<string> { "pink", "red", "default", "brown", "silver", "light_green", "crismon", "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta", "blue_green", "orange", "lime", "green", "emerald", "carmine", "nickel", "mint", "army_green", "pumpkin" };
        public void OnConsoleCommand(ConsoleCommandEvent ev)
        {
            string[] args = ev.Command.Split(' ');

            switch (args[0].ToLower())
            {
                case "scputils_help":
                    {
                        ev.Color = "green";
                        ev.ReturnMessage = "Avaible commands: \n" +
                            ".scputils_info, .scputils_help, .scputils_change_nickname, .scputils_change_color, .scputils_show_badge, .scputils_hide_badge";
                        break;
                    }


                case "scputils_info":
                    {
                        ev.Color = "green";
                        ev.ReturnMessage = "Plugin Info: \n" +
                            "SCPUtils is a public plugin created by Terminator_97#0507, you can download this plugin at: github.com/terminator-97/SCPUtils \n" +
                            $"This server is running SCPUtils version {SCPUtils.pluginVersion}";
                        break;
                    }


                case "scputils_change_nickname":
                    {
                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Player.GetNickname());

                        if (commandSender == null)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "An error has occured while executing this command!";
                            break;
                        }

                        if (args.Length < 2)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "Usage: scputils_changenickname <Nick>";
                            break;
                        }

                        if (commandSender.CheckPermission("scputils.changenickname"))
                        {
                            args[1] = args[1].ToString();
                            if (args[1].ToLower() == "none")
                            {
                                ev.Color = "green";
                                ev.ReturnMessage = "Your nickname has been removed, changes will take effect next round!";
                                ev.Player.GetDatabasePlayer().CustomNickName = "";
                            }

                            else
                            {
                                bool allowChange = true;
                                foreach (ReferenceHub player in EXILED.Extensions.Player.GetHubs())
                                {
                                    if (player.GetNickname().ToLower() == args[1].ToLower())
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
                                ev.Color = "green";
                                ev.ReturnMessage = "Your nickname has been changed, changes will take effect next round, use scputils_change_nickname None to remove the nickname";
                                string nickname = args[1];
                                ev.Player.GetDatabasePlayer().CustomNickName = nickname;
                            }
                        }
                        else
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = pluginInstance.unauthorizedNickNameChange;
                        }
                        break;
                    }

                case "scputils_change_color":
                    {
                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Player.GetNickname());

                        if (commandSender == null)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "An error has occured while executing this command!";
                            break;
                        }
                        if (args.Length < 2)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "Usage: scputils_changecolor <Color / None>";
                            break;
                        }
                        if (commandSender.CheckPermission("scputils.changecolor"))
                        {
                            args[1] = args[1].ToLower().ToString();

                            if (args[1] == "none")
                            {
                                ev.Color = "green";
                                ev.ReturnMessage = "Your color has been removed, changes will take effect next round!";
                                ev.Player.GetDatabasePlayer().ColorPreference = "";
                            }
                            if (validColors.Contains(args[1]))
                            {
                                if (pluginInstance.restrictedRoleColors.Contains(args[1]))
                                {
                                    ev.Color = "red";
                                    ev.ReturnMessage = "This color has been restricted by server owner, please use another color!";
                                    break;
                                }
                                else
                                {
                                    ev.Color = "green";
                                    ev.ReturnMessage = "Your color has been changed, use scputils_change_color None to remove the color!";
                                    commandSender.SetRankColor(args[1]);
                                    string colorPreference = args[1];
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
                            ev.ReturnMessage = pluginInstance.unauthorizedColorChange;
                        }
                        break;
                    }


                case "scputils_hide_badge":
                    {
                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Player.GetNickname());

                        if (commandSender == null)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "An error has occured while executing this command!";
                            break;
                        }

                        if (commandSender.CheckPermission("scputils.badgevisibility"))
                        {
                            ev.Player.characterClassManager.CallCmdRequestHideTag();
                            Database.PlayerData[commandSender].HideBadge = true;
                            ev.Color = "green";
                            ev.ReturnMessage = "Your badge has been hidden!";
                        }
                        else
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = pluginInstance.unauthorizedBadgeChangeVisibility;
                        }
                        break;
                    }


                case "scputils_show_badge":
                    {
                        var commandSender = EXILED.Extensions.Player.GetPlayer(ev.Player.GetNickname());

                        if (commandSender == null)
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = "An error has occured while executing this command!";
                            break;
                        }

                        if (commandSender.CheckPermission("scputils.badgevisibility"))
                        {
                            ev.Player.characterClassManager.CallCmdRequestShowTag(false);
                            Database.PlayerData[commandSender].HideBadge = false;
                            ev.Color = "green";
                            ev.ReturnMessage = "Your badge has been shown!";
                        }
                        else
                        {
                            ev.Color = "red";
                            ev.ReturnMessage = pluginInstance.unauthorizedBadgeChangeVisibility;
                        }
                        break;
                    }
            }
        }
    }
}