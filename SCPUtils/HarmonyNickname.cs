using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using EXILED.Extensions;

namespace SCPUtils
{
    [HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.UpdateNickname))]
    public class CallCmdSetNick
    {
        public static void Prefix(NicknameSync __instance, ref string n)
        {
            var databasePlayer = __instance.gameObject.GetPlayer().GetDatabasePlayer();
            if (databasePlayer == null) return;
            if (!string.IsNullOrEmpty(databasePlayer.CustomNickName) && databasePlayer.CustomNickName!="None")
            {
                n = databasePlayer.CustomNickName;
            }
        }
    }
}
