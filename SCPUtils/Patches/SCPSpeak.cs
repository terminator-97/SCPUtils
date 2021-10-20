using HarmonyLib;
using Mirror;
using PlayableScps;
using PlayableScps.Messages;
using Exiled.Permissions.Extensions;
using System.Linq;
using Log = Exiled.API.Features.Log;

namespace SCPUtils
{
    [HarmonyPatch(typeof(Scp939), nameof(Scp939.ServerReceivedVoiceMsg))]
    public class SCPSpeak
    {
        public static bool Prefix(NetworkConnection conn, Scp939VoiceMessage msg)
        {
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(conn.identity.netId);
            
            if (string.IsNullOrEmpty(player?.UserId) || player.Team != Team.SCP)
            {
                Log.Info("Team or player null!");
                return false;
            }
            else if (ScpUtils.StaticInstance.Config.AllowedScps.Contains(player.Role))
            {
                Log.Info("Ok");
                return player.ReferenceHub.dissonanceUserSetup.MimicAs939 = msg.IsMimicking;
            }
            else if (string.IsNullOrEmpty(ServerStatic.GetPermissionsHandler()._groups.FirstOrDefault(g => g.Value == player.ReferenceHub.serverRoles.Group).Key) && !string.IsNullOrEmpty(player.ReferenceHub.serverRoles.MyText))
            {
                Log.Info("Perm error");
                return false;
            }
            else if (player.CheckPermission($"scputils_speak.{player.Role.ToString().ToLower()}"))
            {
                Log.Info("Ok2");
                return player.ReferenceHub.dissonanceUserSetup.MimicAs939 = msg.IsMimicking;
            }
            else
            {
                Log.Info("Error??");
                return false;
            }
        }
    }
}
