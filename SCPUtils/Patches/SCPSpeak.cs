using Assets._Scripts.Dissonance;
using Exiled.Permissions.Extensions;
using HarmonyLib;
using System.Linq;
using PlayableScps;
using PlayableScps.Messages;
using Mirror;

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
                return false;
            }
            else if (ScpUtils.StaticInstance.Config.AllowedScps.Contains(player.Role)) { return player.ReferenceHub.dissonanceUserSetup.MimicAs939 = msg.IsMimicking; }
            else if (string.IsNullOrEmpty(ServerStatic.GetPermissionsHandler()._groups.FirstOrDefault(g => g.Value == player.ReferenceHub.serverRoles.Group).Key) && !string.IsNullOrEmpty(player.ReferenceHub.serverRoles.MyText))
            {
                return false;
            }
            else if (player.CheckPermission($"scputils_speak.{player.Role.ToString().ToLower()}"))
            {
                return player.ReferenceHub.dissonanceUserSetup.MimicAs939 = msg.IsMimicking;
            }
            else return false;
        }
    }
}