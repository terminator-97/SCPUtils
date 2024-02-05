using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SetName : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.SetnameCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.SetnameAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.SetnameDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            string target;
            string nickname = "";
            if (sender.CheckPermission("scputils.playersetname"))
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.ArgPlayer} {ScpUtils.StaticInstance.Translation.SetnameArgnickname}</color>";
                    return false;
                }
                else
                {
                    target = arguments.Array[1].ToString();
                    nickname = string.Join(" ", arguments.Array, 2, arguments.Array.Length - 2);
                }
            }
            else if (sender.CheckPermission("scputils.changenickname"))
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} {ScpUtils.StaticInstance.Translation.SetnameArgnickname}</color>";
                    return false;
                }
                else
                {
                    target = Exiled.API.Features.Player.Get(((CommandSender)sender).SenderId).UserId;
                    if (target.GetDatabasePlayer().IsRestricted())
                    {
                        response = "<color=red>You are banned from executing this command!</color>";
                        return false;
                    }
                    if (target.GetDatabasePlayer().NicknameCooldown > DateTime.Now)
                    {
                        response = ScpUtils.StaticInstance.Config.NicknameCooldownMessage;
                        return false;
                    }
                    nickname = string.Join(" ", arguments.Array, 1, arguments.Array.Length - 1);
                    bool allowChange = true;
                    foreach (Exiled.API.Features.Player playerList in Exiled.API.Features.Player.List)
                    {
                        if (playerList.Nickname.ToLower() == nickname.ToLower())
                        {
                            allowChange = false;
                            break;
                        }
                    }

                    if (!allowChange)
                    {

                        response = ScpUtils.StaticInstance.Translation.SetnameTaken;
                        return false;
                    }
                    else if (ScpUtils.StaticInstance.Functions.CheckNickname(nickname) && !sender.CheckPermission("scputils.bypassnickrestriction"))
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


            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.Translation.NoDbPlayer;
                return false;
            }

            if (nickname.ToLower() == "none" || nickname.ToLower() == ScpUtils.StaticInstance.Translation.None.ToLower())
            {
                databasePlayer.CustomNickName = "";
                databasePlayer.SaveData();

                Exiled.API.Features.Player plr = Exiled.API.Features.Player.Get(target);

                if (plr != null)
                {
                    plr.DisplayNickname = plr.Nickname;
                }

                response = ScpUtils.StaticInstance.Translation.Success;
                return true;
            }

            if (nickname.Length > ScpUtils.StaticInstance.Config.NicknameMaxLength)
            {
                response = ScpUtils.StaticInstance.Translation.SetnameToolong;
                return false;
            }


            databasePlayer.CustomNickName = nickname;
            databasePlayer.NicknameCooldown = DateTime.Now.AddSeconds(ScpUtils.StaticInstance.Config.ChangeNicknameCooldown);
            databasePlayer.SaveData();
            response = ScpUtils.StaticInstance.Translation.Success;
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(target);

            if (player != null)
            {
                player.DisplayNickname = nickname;
            }

            return true;
        }
    }
}
