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
            if (string.IsNullOrEmpty(player?.GetUserId())) return;
            else if (player.IsHost()) return;
            else if (player.GetTeam() != Team.SCP) return;     
            else if (player.CheckPermission($"scputils_speak.{player.GetRole().ToString().ToLower()}"))
            {
                __instance.MimicAs939 = value;
            }            
            // Log.Debug($"PlayerName: {player.GetNickname()}");
        }
    }
}