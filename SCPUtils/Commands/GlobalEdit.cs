﻿using CommandSystem;
using Exiled.Permissions.Extensions;
using MongoDB.Driver;
using System;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class GlobalEdit : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.GlobaleditCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.GlobaleditAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.GlobaleditDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.globaledit"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            else
            {
                if (arguments.Count < 4)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} <SCPGames to remove> <Suicides to remove> <Kicks to remove> <Bans to remove></color>";
                    return false;
                }
            }

            if (int.TryParse(arguments.Array[1].ToString(), out int scpGamesToRemove) && int.TryParse(arguments.Array[2].ToString(), out int suicidesToRemove) && int.TryParse(arguments.Array[3].ToString(), out int kicksToRemove) && int.TryParse(arguments.Array[4].ToString(), out int bansToRemove))
            {
                foreach (Player databasePlayer in Database.MongoDatabase.GetCollection<Player>("players").AsQueryable().ToList().FindAll(x => x.ScpSuicideCount >= 1))
                {
                    if (databasePlayer.TotalScpGamesPlayed >= scpGamesToRemove)
                    {
                        databasePlayer.TotalScpGamesPlayed -= scpGamesToRemove;
                    }
                    else
                    {
                        databasePlayer.TotalScpGamesPlayed = 0;
                    }

                    if (databasePlayer.ScpSuicideCount >= suicidesToRemove)
                    {
                        databasePlayer.ScpSuicideCount -= suicidesToRemove;
                    }
                    else
                    {
                        databasePlayer.ScpSuicideCount = 0;
                    }

                    if (databasePlayer.TotalScpSuicideKicks >= kicksToRemove)
                    {
                        databasePlayer.TotalScpSuicideKicks -= kicksToRemove;
                    }
                    else
                    {
                        databasePlayer.TotalScpSuicideKicks = 0;
                    }

                    if (databasePlayer.TotalScpSuicideBans >= bansToRemove)
                    {
                        databasePlayer.TotalScpSuicideBans -= bansToRemove;
                    }
                    else
                    {
                        databasePlayer.TotalScpSuicideBans = 0;
                    }

                    databasePlayer.SaveData();
                }
            }

            else
            {
                response = $"{ScpUtils.StaticInstance.Translation.InvalidArgInt}";
                return false;
            }

            response = $"Success, the following edits have been made globally: Removed: {scpGamesToRemove} SCP Game(s), {suicidesToRemove} Suicide(s), {kicksToRemove} Kick(s), {bansToRemove} Ban(s) to each player!";
            return true;
        }
    }
}
