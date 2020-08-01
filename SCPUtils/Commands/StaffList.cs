using System;
using CommandSystem;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class StaffList : ICommand
    {
        public string Command { get;  } = "scputils_staff_list";

        public string[] Aliases { get; } = new[] { "sl", "stafflist" };

        public string Description { get; } = "Show staff list";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!CommandExtensions.IsAllowed(((CommandSender)sender).SenderId, "scputils.stafflist"))
            {
                response = "You need a higher administration level to use this command!";
                return false;
            }
            string text = "";            
            foreach(var player in Exiled.API.Features.Player.List)
            {              
                if(player.ReferenceHub.serverRoles.RaEverywhere || player.ReferenceHub.serverRoles.Staff) text += $"(SCP:SL Staff) Player: {player.Nickname} {player.UserId} Global badge: {player.GlobalBadge}\n";
                else if (player.ReferenceHub.serverRoles.RemoteAdmin) text += $"Player: {player.Nickname} {player.UserId} Rank: {player.GroupName}\n";
            }
            if (string.IsNullOrEmpty(text)) text = "No staff online!";
            response = text;
            return true;
        }
    }
}