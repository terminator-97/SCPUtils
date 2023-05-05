using PluginAPI.Core;
using System.Linq;
using CommandSystem;

namespace SCPUtils
{
    public static class Extension
    {

        public static bool CheckPermission(this PluginAPI.Core.Player player, string permission)
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
                Log.Error($"SCPUtils permissions error! Badge {badge} is not present in configs!");
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
                Log.Error($"SCPUtils permissions error! Badge {badge} is not present in configs!");
                return false;
            }
        }

        /*public static bool IsScpBanned(this PluginAPI.Core.Player player)
        {
            if 
        }*/

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
        }

        public static string GetGroupName(this PluginAPI.Core.Player player)
        {
            if (ServerStatic.PermissionsHandler._members.TryGetValue(player.UserId, out string name))
            {
                return name;
            }
            else
            {
                return "none";
            }

        }           

    }
}
