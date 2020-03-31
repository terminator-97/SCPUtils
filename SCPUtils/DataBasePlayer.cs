using EXILED.Extensions;

namespace SCPUtils
{
    public static class DatabasePlayer
    {
        public static string GetAuthentication(this ReferenceHub player) => player.GetUserId().Split('@')[1];

        public static string GetRawUserId(this ReferenceHub player) => player.GetUserId().Split('@')[0];
    }
}
