using Assets._Scripts.Dissonance;
using Harmony;
using EXILED.Extensions;
using EXILED;
using System;
using System.Linq;

namespace SCPUtils
{
    [HarmonyPatch(typeof(DissonanceUserSetup), nameof(DissonanceUserSetup.CallCmdAltIsActive))]
    public class SCPSpeak
    {

        public static void Prefix(DissonanceUserSetup __instance, bool value)
        {
            var player = EXILED.Extensions.Player.GetPlayer(__instance.gameObject);
            if (string.IsNullOrEmpty(player?.GetUserId()) || player.GetTeam() != Team.SCP) return;
            else if (string.IsNullOrEmpty(ServerStatic.GetPermissionsHandler()._groups.FirstOrDefault(g => g.Value == player.serverRoles.Group).Key) && !string.IsNullOrEmpty(player.serverRoles.MyText)) return;
            else if (player.CheckPermission($"scputils_speak.{player.GetRole().ToString().ToLower()}"))
            {
                __instance.MimicAs939 = value;
            }
        }
    }
}