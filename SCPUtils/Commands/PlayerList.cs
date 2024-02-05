using CommandSystem;
using Exiled.Permissions.Extensions;
using MongoDB.Driver;
using System;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class PlayerList : ICommand
    {

        public string Command { get; } = ScpUtils.StaticInstance.Translation.PlayerlistCommand;

        public string[] Aliases { get; } = ScpUtils.StaticInstance.Translation.PlayerlistAliases;

        public string Description { get; } = ScpUtils.StaticInstance.Translation.PlayerlistDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.playerlist"))
            {
                response = ScpUtils.StaticInstance.Translation.NoPermissions;
                return false;
            }
            else
            {
                if (arguments.Count < 1)
                {
                    response = $"<color=yellow>{ScpUtils.StaticInstance.Translation.Usage} {Command} <Minimun SCP quit percentage></color>";
                    return false;
                }
            }
            StringBuilder playerListString = new StringBuilder("[Quits/Suicides Percentage]");
            playerListString.AppendLine();
            if (int.TryParse(arguments.Array[1].ToString(), out int minpercentage))
            {
                foreach (Player databasePlayer in Database.MongoDatabase.GetCollection<Player>("players").AsQueryable().ToList().FindAll(x => x.SuicidePercentage >= minpercentage))
                {
                    playerListString.AppendLine();
                    playerListString.Append($"{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication}) -[ {Math.Round(databasePlayer.SuicidePercentage, 2)}% ]");
                }
                response = $"{playerListString}";
            }

            else
            {
                response = ScpUtils.StaticInstance.Translation.InvalidArgInt;
                return false;
            }


            return true;
        }
    }
}
