using EXILED;

namespace SCPUtils
{
    public static class Extensions
    {

        public static void RAMessage(this CommandSender sender, string message, bool success = true) =>
        sender.RaReply("SCPUtils#" + message, success, true, string.Empty);


    }
}