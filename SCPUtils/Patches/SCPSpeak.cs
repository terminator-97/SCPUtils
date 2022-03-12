using Exiled.Permissions.Extensions;
using HarmonyLib;
using System.Linq;


namespace SCPUtils
{
    [HarmonyPatch(typeof(Radio), nameof(Radio.UserCode_CmdSyncTransmissionStatus))]
    public class SCPSpeak
    {
        public static bool Prefix(Radio __instance, bool b)
        {
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(__instance._hub);
            if (string.IsNullOrEmpty(player?.UserId)) return false;
            if (player.HasItem(ItemType.Radio)) return __instance._dissonanceSetup.RadioAsHuman = b;
            if (player.Role.Team != Team.SCP) return false;
            else if (ScpUtils.StaticInstance.Config.AllowedScps.Contains(player.Role)) return __instance._dissonanceSetup.MimicAs939 = b;
            else if (string.IsNullOrEmpty(ServerStatic.GetPermissionsHandler()._groups.FirstOrDefault(g => g.Value == player.ReferenceHub.serverRoles.Group).Key) && !string.IsNullOrEmpty(player.ReferenceHub.serverRoles.MyText)) return false;
            else if (player.CheckPermission($"scputils_speak.{player.Role.ToString().ToLower()}")) return __instance._dissonanceSetup.MimicAs939 = b;
            else return false;
        }
    }
}


