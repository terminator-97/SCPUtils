namespace SCPUtils.Commands.RemoteAdmin.PlayTime
{
    using CommandSystem;
    using System;
    using System.Linq;
    using System.Text;

    public class StaffPlayTimeCommand : ICommand
    {
        public string Command { get; } = "user";
        public string[] Aliases { get; } = new[] { "u", "member", "staff" };
        public string Description { get; } = "You can see detailed informations about playtime";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            string target;
            int range;
            int playtime;
            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils playtime user"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils playtime user"]}");
                return false;
            }

            {
                if (arguments.Count < 2)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player} {ScpUtils.StaticInstance.commandTranslation.Days}");
                    return false;
                }
                else
                {
                    target = arguments.Array[3].ToString();
                }

                if (!int.TryParse(arguments.Array[4], out range))
                {
                    response = ScpUtils.StaticInstance.commandTranslation.DaysInteger;
                    return false;
                }

                if (range >= ScpUtils.StaticInstance.configs.MaxPlaytime)
                {
                    response = ScpUtils.StaticInstance.commandTranslation.DaysMaximus.Replace("%maxDays%", $"{ScpUtils.StaticInstance.configs.MaxPlaytime}");
                    return false;
                }
            }
            Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                return false;
            }

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
            message.AppendLine("\n\n" + ScpUtils.StaticInstance.commandTranslation.SpecifiedPlaytime.Replace("%totalPlaytime%", $"{new TimeSpan(0, 0, playtime)}"));
            response = $"{message}";
    
            return true;
        }
    }
}
