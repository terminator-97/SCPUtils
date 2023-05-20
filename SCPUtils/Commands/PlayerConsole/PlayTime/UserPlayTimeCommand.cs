namespace SCPUtils.Commands.Console
{
    using CommandSystem;
    using System;
    using System.Linq;
    using System.Text;

    public class UserPlayTimeCommand : ICommand
    {
        public string Command { get; } = "playtime";
        public string[] Aliases { get; } = new[] { "pt" };
        public string Description { get; } = "You can see detailed informations about playtime";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            int range;
            int playtime;
            string target = PluginAPI.Core.Player.Get(((CommandSender)sender).SenderId).UserId;

            if (arguments.Count != 1)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Days}");
                return false;
            }
            else
            {
                if (!int.TryParse(arguments.Array[2], out range))
                {
                    response = ScpUtils.StaticInstance.commandTranslation.DaysInteger;
                    return false;
                }
            }

            if (range >= ScpUtils.StaticInstance.configs.MaxPlaytime)
            {
                response = ScpUtils.StaticInstance.commandTranslation.DaysMaximus.Replace("%maxDays%", $"{ScpUtils.StaticInstance.configs.MaxPlaytime}");
                return false;
            }

            Player databasePlayer = target.GetDatabasePlayer();

            if (range <= 0)
            {
                response = ScpUtils.StaticInstance.commandTranslation.DaysInteger;
                return false;
            }

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                return false;
            }

            StringBuilder message = new StringBuilder($"[{databasePlayer.Name} ({databasePlayer.Id}@{databasePlayer.Authentication})]");
            message.AppendLine();
            message.Append(ScpUtils.StaticInstance.commandTranslation.TotalPlaytime.Replace("%playtime%", $"{new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum())}"));

            playtime = 0;
            for (int i = 0; i <= range; i++)
            {
                databasePlayer.PlayTimeRecords.Count();
                message.AppendLine();
                DateTime.TryParse((DateTime.Now.Date.AddDays(-i)).ToString(), out DateTime date);
                if (databasePlayer.PlayTimeRecords.ContainsKey(date.Date.ToShortDateString()))
                {
                    message.Append($"{date.Date.ToShortDateString()} {ScpUtils.StaticInstance.commandTranslation.Playtime.Replace("%playtime%", $"{new TimeSpan(0, 0, databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()])}")}");
                    playtime += databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()];
                }
                else
                {
                    message.Append($"{date.Date.ToShortDateString()} {ScpUtils.StaticInstance.commandTranslation.PlaytimeNoActivity}");
                }
            }
            message.AppendLine("\n" + ScpUtils.StaticInstance.commandTranslation.SpecifiedPlaytime.Replace("%totalPlaytime%", $"{new TimeSpan(0, 0, playtime)}"));
            response = $"{message}";

            return true;
        }
    }
}