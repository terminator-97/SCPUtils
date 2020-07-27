using System;
using System.Linq;
using CommandSystem;
using HarmonyLib;
using Log = Exiled.API.Features.Log;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    class SetName : ICommand
    {

        public string Command { get; } = "scputils_set_name";

        public string[] Aliases { get; } = new string[] { "un", "scputils_change_nickname" };

        public string Description { get; } = "You can change everyone name or only your name based on the permissions you have";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string target;
            string nickname = "";
            if (CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.playersetname") || ((CommandSender)sender).FullPermissions)
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>Usage: {Command} <player name/id> <Nickname> </color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                    nickname = string.Join(" ", arguments.Array, 2, arguments.Array.Length - 2);
                }
            }
            else if (CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.changenickname"))
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>Usage: {Command} <Nickname> </color>";
                    return false;
                }
                else
                {
                    target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).UserId;
                    nickname = string.Join(" ", arguments.Array, 1, arguments.Array.Length - 1);
                    bool allowChange = true;
                    foreach (var playerList in Exiled.API.Features.Player.List)
                    {
                        if (playerList.Nickname.ToLower() == nickname.ToLower())
                        {
                            allowChange = false;
                            break;
                        }
                    }

                    if (!allowChange)
                    {

                        response = "<color=red>This nickname is already used by another player, please choose another name!</color>";
                        return false;
                    }
                    else if (ScpUtils.StaticInstance.Functions.CheckNickname(nickname) && !CommandExtensions.IsAllowed(target, "scputils.bypassnickrestriction"))
                    {

                        response = $"{ScpUtils.StaticInstance.Config.InvalidNicknameText} ";
                        return false;
                    }


                }
            }
            else
            {
                response = $"{ScpUtils.StaticInstance.Config.UnauthorizedNickNameChange} ";
                return false;
            }


            var databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = "<color=yellow>Player not found on Database or Player is loading data!</color>";
                return false;
            }

            if (nickname.ToLower() == "none")
            {
                databasePlayer.CustomNickName = "";
                Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
                response = "<color=green>Success, changes will take effect next round!</color>";
                return true;
            }

            if (nickname.Length > ScpUtils.StaticInstance.Config.NicknameMaxLength)
            {
                response = "<color=red>Nickname is too long!</color>";
                return false;
            }


            databasePlayer.CustomNickName = nickname;
            Database.LiteDatabase.GetCollection<Player>().Update(databasePlayer);
            response = "<color=green>Success, choice has been saved!</color>";
            var player = Exiled.API.Features.Player.Get(target);
            if (player != null) player.Nickname = nickname;

            return true;
        }
    }
}
