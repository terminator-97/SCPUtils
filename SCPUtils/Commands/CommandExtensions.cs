using Log = Exiled.API.Features.Log;

namespace SCPUtils.Commands
{
    public static class CommandExtensions
    {
        public static bool IsAllowed(string sender, string permission)
        {
            Exiled.API.Features.Player player;
            return sender != null && (sender == "GAME CONSOLE" || (player = Exiled.API.Features.Player.Get(sender)) == null || Exiled.Permissions.Extensions.Permissions.CheckPermission(player, permission));
        }
    }
}
