namespace SCPUtils.Commands.RemoteAdmin.PlayTime
{
    using CommandSystem;
    using System;
    using System.Linq;
    using System.Text;

    public class PlayTimeBadgeCommand : ICommand
    {
        public string Command { get; } = "badge";
        public string[] Aliases { get; } = new[] { "b", "group", "g" };
        public string Description { get; } = "Short playtime with a specified range by using badge as input.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string badge;
            int count = 0;
            int playtime;
            int completedays;
            // TimeSpan playtime;

            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Translation.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils playtime badge"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils playtime badge"]}");
                return false;
            }
            if (arguments.Count < 2)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Badge} {ScpUtils.StaticInstance.commandTranslation.Days}");
                return false;
            }
            else
            {
                badge = arguments.Array[3].ToString();
            }

            int.TryParse(arguments.Array[4], out int range);

            if (!ServerStatic.GetPermissionsHandler().GetAllGroups().ContainsKey(badge))
            {
                response = ScpUtils.StaticInstance.commandTranslation.BadgeNameError;
                return false;
            }

            StringBuilder message = new StringBuilder().AppendLine(ScpUtils.StaticInstance.commandTranslation.BadgePlaytime.Replace("%badge%", badge));
            foreach (var player in ServerStatic.RolesConfig.GetStringDictionary("Members"))
            {
                playtime = 0;
                completedays = 0;
                int days = range + 1;
                if (player.Value.ToString() == badge)
                {
                    var databasePlayer = player.Key.GetDatabasePlayer();
                    if (databasePlayer == null)
                    {
                        response = $"Null player detected! {player.Key}";
                        return false;
                    }

                    for (int i = 0; i <= range; i++)
                    {
                        databasePlayer.PlayTimeRecords.Count();
                        DateTime.TryParse((DateTime.Now.Date.AddDays(-i)).ToString(), out DateTime date);
                        if (databasePlayer.PlayTimeRecords.ContainsKey(date.Date.ToShortDateString()))
                        {
                            playtime += databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()];
                            if (databasePlayer.PlayTimeRecords[date.Date.ToShortDateString()] >= ScpUtils.StaticInstance.Configs.BptMinSeconds) completedays++;
                        }
                    }
                    if (playtime == 0) message.AppendLine($"[{databasePlayer.Name} - {databasePlayer.Id}@{databasePlayer.Authentication}] - {ScpUtils.StaticInstance.commandTranslation.PlaytimeNoActivity}");
                    else message.AppendLine($"[{databasePlayer.Name} - {databasePlayer.Id}@{databasePlayer.Authentication}] - {ScpUtils.StaticInstance.commandTranslation.Playtime.Replace("%playtime%", $"{new TimeSpan(0, 0, playtime)}")} - {ScpUtils.StaticInstance.commandTranslation.DaysJoined.Replace("%joinedDays%", $"{completedays}").Replace("%totalDays%", $"{days}")}");
                    count++;
                }
            }

            if (count == 0)
            {
                response = ScpUtils.StaticInstance.commandTranslation.NoPlayerBadge;
                return false;
            }


            response = $"{message}";

            return true;
        }
    }
}