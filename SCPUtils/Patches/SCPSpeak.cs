using Exiled.Permissions.Extensions;
using HarmonyLib;
using System.Linq;


namespace SCPUtils
{
    [HarmonyPatch(typeof(PlayableScps.PlayableScp), nameof(PlayerRoles.Voice.HumanVoiceModule.Transmitting))]
    public class SCPSpeak
    {
        public static bool Prefix(PlayerRoles.Voice.HumanVoiceModule __instance, bool b)
        {
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(__instance.name);
            if (string.IsNullOrEmpty(player?.UserId)) return false;
            if (player.HasItem(ItemType.Radio)) return __instance.Transmitting = b;
            if (player.Role.Team != PlayerRoles.Team.SCPs) return false;
            else if (ScpUtils.StaticInstance.Config.AllowedScps.Contains(player.Role)) return __instance.Transmitting = b;
            else if (string.IsNullOrEmpty(ServerStatic.GetPermissionsHandler()._groups.FirstOrDefault(g => g.Value == player.ReferenceHub.serverRoles.Group).Key) && !string.IsNullOrEmpty(player.ReferenceHub.serverRoles.MyText)) return false;
            else if (player.CheckPermission($"scputils_speak.{player.Role.Type.ToString().ToLower()}")) return __instance.Transmitting = b;
            else return false;
        }
    }
}


