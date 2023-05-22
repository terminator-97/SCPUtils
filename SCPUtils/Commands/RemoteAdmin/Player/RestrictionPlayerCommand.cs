namespace SCPUtils.Commands.RemoteAdmin.Player
{
    using CommandSystem;
    using System;

    public class RestrictionPlayerCommand : ICommand
    {
        public string Command { get; } = "restrict";
        public string[] Aliases { get; } = new[]
        {
            "rs"
        };
        public string Description { get; } = "This command restricts a player from using the: scputils_change_nickname and scputils_change_color.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.configs.CooldownMessage;
                return false;
            }

            string target;
            string reason;
            if (!sender.CheckPermission(ScpUtils.StaticInstance.perms.PermissionsList["scputils player restriction"]))
            {
                response = ScpUtils.StaticInstance.commandTranslation.SenderError.Replace("%permission%", $"{ScpUtils.StaticInstance.perms.PermissionsList["scputils player restriction"]}");
                return false;
            }

            else if (arguments.Count < 3)
            {
                response = ScpUtils.StaticInstance.commandTranslation.UsageError.Replace("%command%", $"{arguments.Array[0]} {arguments.Array[1]} {arguments.Array[2]}").Replace("%arguments%", $"{ScpUtils.StaticInstance.commandTranslation.Player} {ScpUtils.StaticInstance.commandTranslation.Time} {ScpUtils.StaticInstance.commandTranslation.Reason}");
                return false;
            }
            target = arguments.Array[3].ToString();

            PluginAPI.Core.Player player = PluginAPI.Core.Player.Get(target);
            SCPUtils.Player databasePlayer = target.GetDatabasePlayer();

            if (databasePlayer == null)
            {
                response = ScpUtils.StaticInstance.commandTranslation.PlayerDatabaseError;
                return false;
            }

            else if (databasePlayer.IsRestricted())
            {
                response = ScpUtils.StaticInstance.commandTranslation.AlreadyRestrict.Replace("%player%", arguments.Array[3]);
                return false;
            }

            else if (TimeSpan.TryParse(arguments.Array[4], out var duration))
            {
                reason = string.Join(" ", arguments.Array, 4, arguments.Array.Length - 4);
                databasePlayer.Restricted.Add(DateTime.Now.Add(duration), reason);
                if (target != null)
                {
                    if (!ScpUtils.StaticInstance.EventHandlers.LastCommand.ContainsKey(player))
                    {
                        if (duration != TimeSpan.Zero)
                        {
                            ScpUtils.StaticInstance.EventHandlers.LastCommand.Add(player, DateTime.Now.Add(duration));
                        }
                        else
                        {
                            ScpUtils.StaticInstance.EventHandlers.LastCommand.Add(player, DateTime.MaxValue);
                        }
                    }
                    else
                    {
                        if (duration != TimeSpan.Zero)
                        {
                            ScpUtils.StaticInstance.EventHandlers.LastCommand[player] = DateTime.Now.Add(duration);
                        }
                        else
                        {
                            ScpUtils.StaticInstance.EventHandlers.LastCommand[player] = DateTime.MaxValue;
                        }
                    }
                }

                databasePlayer.SaveData();
                response = ScpUtils.StaticInstance.commandTranslation.RestrictionResponse.Replace("%player%", arguments.Array[3]);
            }
            else
            {
                response = ScpUtils.StaticInstance.commandTranslation.Integer.Replace("%argument%", arguments.Array[4]);
            }

            return true;
        }
    }
}
