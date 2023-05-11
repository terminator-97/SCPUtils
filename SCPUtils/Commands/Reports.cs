using CommandSystem;
using Exiled.Permissions.Extensions;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Reports : ICommand
    {

        public string Command { get; } = "scputils_reports";

        public string[] Aliases { get; } = new[] { "scpu_reports", "rep", "sreports", "reports" };

        public string Description { get; } = "Show useful stats about users inside SCPUtils database.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string report;
            bool showarg = false;
            // TimeSpan playtime;

            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }


            if (!sender.CheckPermission("scputils.reports") && !((CommandSender)sender).FullPermissions)
            {
                response = "<color=red>You need a higher administration level to use this command!</color>";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = $"<color=yellow>Usage: {Command} <report-type> (use help argument to see avaible reports)</color>";
                return false;
            }
            else
            {
                report = arguments.Array[1].ToString().ToLower();
            }


            if (report == "help")
            {
                response = "Avaible reports: totalusers, scpbanned, banshigherthan, kickshigherthan, scpbannedhigherthan, quitshigherthan, playedscphigherthan, bancounthigherthan, tempbadge";
                return false;
            }

            if (report == "totalusers")
            {
                response = $"Total users: {Database.MongoDatabase.GetCollection<Player>("players").EstimatedDocumentCount()}";
                return true;
            }

            if (report == "scpbanned")
            {
                if (arguments.Array.Length >= 3)
                {
                    if (arguments.Array[2].ToLower().ToString() == "show") showarg = true;
                }

                if (Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.RoundBanLeft >= 1).CountDocuments() >= 1)
                {
                    StringBuilder message = new StringBuilder($"There are currenly {Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.RoundBanLeft >= 1).CountDocuments() } users SCP-Banend, use argument show after scpbanned to see the banned users.").AppendLine();
                    if (showarg)
                    {
                        foreach (var player in Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.RoundBanLeft >= 1).ToList().OrderByDescending(x => x.RoundBanLeft))
                        {
                            message.AppendLine($"[{player.Name}({player.Id}@{player.Authentication})] - Round bans left: {player.RoundBanLeft}");
                        }

                    }
                    response = $"{message}";
                    return true;

                }
                else
                {
                    response = $"No user is currently SCP-Banned!";
                    return true;
                }

            }

            if (report == "banshigherthan")
            {
                if (arguments.Count < 2)
                {
                    response = $"<color=yellow>Usage: {Command} {report} <number-of-bans></color>";
                    return false;
                }
                if (int.TryParse(arguments.Array[2], out int num))
                {
                    if (Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpSuicideBans >= num).CountDocuments() >= 1)
                    {
                        StringBuilder message = new StringBuilder($"There are currenly {Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpSuicideBans >= num).CountDocuments() } users Suicide-Banned, use argument show after {report} to see the banned users (i advise to do not do it if you don't know what you are doing, large database may freeze the server).").AppendLine();
                        if (showarg)
                        {
                            foreach (var player in Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpSuicideBans >= num).ToList().OrderByDescending(x => x.TotalScpSuicideBans))
                            {
                                message.AppendLine($"[{player.Name}({player.Id}@{player.Authentication})] - Suicide bans: {player.TotalScpSuicideBans}");
                            }
                        }
                        response = $"{message}";
                        return true;

                    }
                    else
                    {
                        response = $"No user is matching the current filter!";
                        return true;
                    }
                }
                else
                {
                    response = $"Arg2 must be integer!";
                    return true;
                }
            }

            if (report == "kickshigherthan")
            {
                if (arguments.Array.Length >= 4)
                {
                    if (arguments.Array[3].ToLower().ToString() == "show") showarg = true;
                }

                if (int.TryParse(arguments.Array[2], out int num))
                {
                    if (Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpSuicideKicks >= num).Count() >= 1)
                    {
                        StringBuilder message = new StringBuilder($"There are currenly {Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpSuicideKicks >= num).CountDocuments() } users Kicked due suicide/disconnect, use argument show after {report} to see the banned users (i advise to do not do it if you don't know what you are doing, large database may freeze the server).").AppendLine();
                        if (showarg)
                        {
                            foreach (var player in Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpSuicideKicks >= num).ToList().OrderByDescending(x => x.TotalScpSuicideKicks))
                            {
                                message.AppendLine($"[{player.Name}({player.Id}@{player.Authentication})] - Suicide kicks: {player.TotalScpSuicideKicks}");
                            }
                        }
                        response = $"{message}";
                        return true;

                    }
                    else
                    {
                        response = $"No user is matching the current filter!";
                        return true;
                    }
                }
                else
                {
                    response = $"Arg2 must be integer!";
                    return true;
                }
            }

            if (report == "scpbannedhigherthan")
            {
                if (arguments.Array.Length >= 4)
                {
                    if (arguments.Array[3].ToLower().ToString() == "show") showarg = true;
                }
                if (int.TryParse(arguments.Array[2], out int num))
                {
                    if (Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.RoundBanLeft >= num).CountDocuments() >= 1)
                    {
                        StringBuilder message = new StringBuilder($"There are currenly {Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.RoundBanLeft >= num).CountDocuments() } users are Round-Banned, use argument show after {report} to see the banned users (i advise to do not do it if you don't know what you are doing, large database may freeze the server).").AppendLine();
                        if (showarg)
                        {
                            foreach (var player in Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.RoundBanLeft >= num).ToList().OrderByDescending(x => x.RoundBanLeft))
                            {
                                message.AppendLine($"[{player.Name}({player.Id}@{player.Authentication})] - Round bans left: {player.RoundBanLeft}");
                            }
                        }
                        response = $"{message}";
                        return true;

                    }
                    else
                    {
                        response = $"No user is matching the current filter!";
                        return true;
                    }
                }
                else
                {
                    response = $"Arg2 must be integer!";
                    return true;
                }
            }

            if (report == "quitshigherthan")
            {
                if (arguments.Array.Length >= 4)
                {
                    if (arguments.Array[3].ToLower().ToString() == "show") showarg = true;
                }

                if (int.TryParse(arguments.Array[2], out int num))
                {
                    if (Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.ScpSuicideCount >= num).CountDocuments() >= 1)
                    {
                        StringBuilder message = new StringBuilder($"There are currenly {Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.ScpSuicideCount >= num).CountDocuments() } users have {num} suicides, use argument show after {report} to see the banned users (i advise to do not do it if you don't know what you are doing, large database may freeze the server).").AppendLine();
                        if (showarg)
                        {
                            foreach (var player in Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.ScpSuicideCount >= num).ToList().OrderByDescending(x => x.ScpSuicideCount))
                            {
                                message.AppendLine($"[{player.Name}({player.Id}@{player.Authentication})] - Suicide/Disconnect count: {player.ScpSuicideCount}");
                            }
                        }
                        response = $"{message}";
                        return true;
                    }
                    else
                    {
                        response = $"No user is matching the current filter!";
                        return true;
                    }
                }
                else
                {
                    response = $"Arg2 must be integer!";
                    return true;
                }
            }

            if (report == "playedscphigherthan")
            {
                if (arguments.Array.Length >= 4)
                {
                    if (arguments.Array[3].ToLower().ToString() == "show") showarg = true;
                }

                if (int.TryParse(arguments.Array[2], out int num))
                {
                    if (Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpGamesPlayed >= num).CountDocuments() >= 1)
                    {
                        StringBuilder message = new StringBuilder($"There are currenly {Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpGamesPlayed >= num).CountDocuments() } users have played SCP {num} times, use argument show after {report} to see the banned users (i advise to do not do it if you don't know what you are doing, large database may freeze the server).").AppendLine();
                        if (showarg)
                        {
                            foreach (var player in Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpGamesPlayed >= num).ToList().OrderByDescending(x => x.TotalScpGamesPlayed))
                            {
                                message.AppendLine($"[{player.Name}({player.Id}@{player.Authentication})] - Total games played as SCP: {player.TotalScpGamesPlayed}");
                            }
                        }
                        response = $"{message}";
                        return true;

                    }
                    else
                    {
                        response = $"No user is matching the current filter!";
                        return true;
                    }
                }
                else
                {
                    response = $"Arg2 must be integer!";
                    return true;
                }
            }

            if (report == "bancounthigherthan")
            {
                if (arguments.Array.Length >= 4)
                {
                    if (arguments.Array[3].ToLower().ToString() == "show") showarg = true;
                }

                if (int.TryParse(arguments.Array[2], out int num))
                {
                    if (Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpSuicideBans >= num).CountDocuments() >= 1)
                    {
                        StringBuilder message = new StringBuilder($"There are currenly {Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpSuicideBans >= num).CountDocuments() } users have been banned {num} times for suicides/quits as SCP, use argument show after {report} to see the banned users (i advise to do not do it if you don't know what you are doing, large database may freeze the server).").AppendLine();
                        if (showarg)
                        {
                            foreach (var player in Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.TotalScpSuicideBans >= num).ToList().OrderByDescending(x => x.TotalScpSuicideBans))
                            {
                                message.AppendLine($"[{player.Name}({player.Id}@{player.Authentication})] - Ban count: {player.TotalScpSuicideBans }");
                            }
                        }
                        response = $"{message}";
                        return true;

                    }
                    else
                    {
                        response = $"No user is matching the current filter!";
                        return true;
                    }
                }
                else
                {
                    response = $"Arg2 must be integer!";
                    return true;
                }
            }

            if (report == "tempbadge")
            {
                if (arguments.Array.Length >= 3)
                {
                    if (arguments.Array[2].ToLower().ToString() == "show") showarg = true;
                }
                if (Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.BadgeExpire >= DateTime.Now).CountDocuments() >= 1)
                {
                    StringBuilder message = new StringBuilder($"There are currenly {Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.BadgeExpire >= DateTime.Now).CountDocuments() } have an active temporarily badge, use argument show after {report} to see the specific users.").AppendLine();
                    if (showarg)
                    {
                        foreach (var player in Database.MongoDatabase.GetCollection<Player>("players").Find(x => x.BadgeExpire >= DateTime.Now).ToList().OrderByDescending(x => x.BadgeExpire))
                        {
                            message.AppendLine($"[{player.Name}({player.Id}@{player.Authentication})] - Badge name: {player.BadgeName} - Badge expire {player.BadgeExpire}");
                        }
                    }
                    response = $"{message}";
                    return true;

                }
                else
                {
                    response = $"No user currently have a temporarily badge";
                    return true;
                }

            }

            response = $"Invalid report, Avaible reports: totalusers, scpbanned, banshigherthan, kickshigherthan, scpbannedhigherthan, quitshigherthan, playedscphigherthan, bancounthigherthan, tempbadge";
            return false;

        }
    }
}
