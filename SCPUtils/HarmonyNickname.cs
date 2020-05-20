using System;
using System.Text.RegularExpressions;
using Harmony;
using EXILED.Extensions;

namespace SCPUtils
{
    [HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.UpdateNickname))]
    public class CallCmdSetNick
    {
        public static void Prefix(NicknameSync __instance, ref string n)
        {
            var player = __instance.gameObject.GetPlayer();
            var databasePlayer = player.GetDatabasePlayer();
            if (databasePlayer == null) return;
            if (!string.IsNullOrEmpty(databasePlayer.CustomNickName) && databasePlayer.CustomNickName != "None")
            {
                n = databasePlayer.CustomNickName;
            }
        }
    }
}
