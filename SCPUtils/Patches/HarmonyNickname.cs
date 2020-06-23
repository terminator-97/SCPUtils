using System;
using System.Text.RegularExpressions;
using HarmonyLib;


namespace SCPUtils
{
    [HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.UpdateNickname))]
    public class CallCmdSetNick
    {
        public static void Prefix(NicknameSync __instance, ref string n)
        {
            var player = Exiled.API.Features.Player.Get(__instance.gameObject);
            var databasePlayer = player.GetDatabasePlayer();
            if (databasePlayer == null) return;
            if (!string.IsNullOrEmpty(databasePlayer.CustomNickName) && databasePlayer.CustomNickName != "None")
            {
                n = databasePlayer.CustomNickName;
            }
        }
    }
}
