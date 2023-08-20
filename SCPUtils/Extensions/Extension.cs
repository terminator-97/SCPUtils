namespace SCPUtils
{
    using System.Linq;
    using FunctionEnums = Enums.FunctionsType;
    using MessageType = Enums.FunctionsMessage;

    public static class Extension
    {
        /*public static bool CheckPermission(this PluginAPI.Core.Player player, string permission)
        {
            var badge = player.GetGroupName();
            if (ScpUtils.StaticInstance.perms.PermissionsList[badge]?.Any() == true)
            {
                if (ScpUtils.StaticInstance.perms.PermissionsList[badge].Contains(permission) || ScpUtils.StaticInstance.perms.PermissionsList[badge].Contains("scputils.*"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Log.Error($"SCPUtils permissions error! Badge {badge} is not present in Configs!");
                return false;
            }
        }

        public static bool CheckPermission(this string badge, string permission)
        {
            if (ScpUtils.StaticInstance.perms.PermissionsList[badge]?.Any() == true)
            {
                if (ScpUtils.StaticInstance.perms.PermissionsList[badge].Contains(permission) || ScpUtils.StaticInstance.perms.PermissionsList[badge].Contains("scputils.*"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Log.Error($"SCPUtils permissions error! Badge {badge} is not present in Configs!");
                return false;
            }
        }

        public static bool IsScpBanned(this PluginAPI.Core.Player player)
        {
            if 
        }

        public static bool CheckPermission(this ICommandSender sender, string permission)
        {
            if(PluginAPI.Core.Player.Get(((CommandSender)sender).SenderId) != null)
            {
                var player = PluginAPI.Core.Player.Get(((CommandSender)sender).SenderId);

                var badge = player.GetGroupName();

                if (ScpUtils.StaticInstance.perms.PermissionsList[badge].Contains(permission) || ScpUtils.StaticInstance.perms.PermissionsList[badge].Contains("scputils.*"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
         
            else if(sender.LogName == "SERVER CONSOLE")
            {
                return true;
            }
            
            else
            {               
                return false;
            }
        }*/

        public static string GetGroupName(this PluginAPI.Core.Player player)
        {
            if (ServerStatic.PermissionsHandler._members.TryGetValue(player.UserId, out string name))
                return name;
            else
                return "none";
        }

        public static void ShowHint(this PluginAPI.Core.Server _, string issuer, string content, float duration = 7)
        {
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
                player.ReceiveHint($"<color=orange>{ScpUtils.StaticInstance.Translation.ExtensionTranslations[0]}</color>\n<color=green>{content}</color>\n ~{issuer}", duration);
        }

        public static void ShowHint(this PluginAPI.Core.Server _, PluginAPI.Core.Player issuer, string content, float duration = 7)
        {
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
                player.ReceiveHint($"<color=orange>{ScpUtils.StaticInstance.Translation.ExtensionTranslations[0]}</color>\n<color=green>{content}</color>\n ~{issuer}", duration);
        }

        public static void SendStaffMessage(this PluginAPI.Core.Player _, Extensions.Broadcast broadcast)
        {
            foreach (var staffer in PluginAPI.Core.Player.GetPlayers())
            {
                if (staffer.RemoteAdminAccess || staffer != null)
                    staffer.SendBroadcast(broadcast.Content, broadcast.Duration, broadcast.Type, broadcast.ClearAll);
            }
        }

        public static void SendSanctionMessage(this PluginAPI.Core.Player player, string message, ushort duration = 15, Broadcast.BroadcastFlags flag = Broadcast.BroadcastFlags.Normal, bool clearOtherBroadcasts = true) => player.SendBroadcast(message, duration, flag, clearOtherBroadcasts);

        public static void SendAdminHint(this PluginAPI.Core.Player _, string issuer, string content, float duration = 7)
        {
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                if (player.RemoteAdminAccess || player != null)
                    player.ReceiveHint($"<color=orange>{ScpUtils.StaticInstance.Translation.ExtensionTranslations[0]}</color>\n<color=green>{content}</color>\n ~{issuer}", duration);
            }
        }

        public static void SendAdminHint(this PluginAPI.Core.Player _, PluginAPI.Core.Player issuer, string content, float duration = 7)
        {
            foreach (PluginAPI.Core.Player player in PluginAPI.Core.Player.GetPlayers())
            {
                if (player.RemoteAdminAccess || player != null)
                    player.ReceiveHint($"<color=orange>{ScpUtils.StaticInstance.Translation.ExtensionTranslations[0]}</color>\n<color=green>{content}</color>\n ~{issuer}", duration);
            }
        }
    }

}

/*switch (functions)
{
                case FunctionEnums.ScpBanned:
                    player.SendBroadcast(ScpUtils.StaticInstance.Configs.FunctionsWarningsMessages[FunctionEnums.ScpBanned], 15, Broadcast.BroadcastFlags.Normal, true);
                    break;
                case FunctionEnums.IssuesBan:
                    player.SendBroadcast(ScpUtils.StaticInstance.Configs.FunctionsWarningsMessages[FunctionEnums.IssuesBan], 15, Broadcast.BroadcastFlags.Normal, true);
                    break;
            }*/