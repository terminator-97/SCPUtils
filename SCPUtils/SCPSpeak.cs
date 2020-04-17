using Assets._Scripts.Dissonance;
using Harmony;
using EXILED.Extensions;
using EXILED;

namespace SCPUtils
{
    [HarmonyPatch(typeof(DissonanceUserSetup), nameof(DissonanceUserSetup.CallCmdAltIsActive))]
    public class SCPSpeak
    {
        public static void Prefix(DissonanceUserSetup __instance, bool value)
        {
            var player = EXILED.Extensions.Player.GetPlayer(__instance.gameObject);
            if (player.GetRole() == RoleType.None) return;
            if (player.CheckPermission($"scputils_speak.{player.GetRole().ToString().ToLower()}"))
            {
                __instance.MimicAs939 = value;
            }
            Log.Debug($"GroupName: {EXILED.Extensions.Player.GetGroupName(player)}");
        }

    }
}