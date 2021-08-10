using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Text;

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class PermissionsView : ICommand
    {
        public string Command { get; } = "scputils_permissions_view";

        public string[] Aliases { get; } = new[] { "pmv", "permissionsview", "su_pmv", "scpu_pmview", "scpu_permissionsview" };

        public string Description { get; } = "Show your scputils permissions";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            StringBuilder message = new StringBuilder($"Your permissions (Granted):");
            if (sender.CheckPermission("scputils.help")) message.AppendLine("You MAY see SCPUtils Admin commands!");
            if (sender.CheckPermission("scputils.playerinfo")) message.AppendLine("You MAY see player info of other players!");
            if (sender.CheckPermission("scputils.playerlist")) message.AppendLine("You MAY see the player list!");
            if (sender.CheckPermission("scputils.playerreset")) message.AppendLine("You MAY reset players!");
            if (sender.CheckPermission("scputils.playerresetpreferences")) message.AppendLine("You MAY reset player preferences!");
            if (sender.CheckPermission("scputils.playersetcolor")) message.AppendLine("You MAY set other players badge colors!");
            if (sender.CheckPermission("scputils.playersetname")) message.AppendLine("You MAY set other players nickname!");
            if (sender.CheckPermission("scputils.handlebadges")) message.AppendLine("You MAY add or remove temporarily bades!");
            if (sender.CheckPermission("scputils.playtime")) message.AppendLine("You MAY see other players playtime!");
            if (sender.CheckPermission("scputils.whitelist")) message.AppendLine("You MAY add/remove players from ASN whitelist!");
            if (sender.CheckPermission("scputils.stafflist")) message.AppendLine("You MAY see online staff list!");
            if (sender.CheckPermission("scputils.warnmanagement")) message.AppendLine("You MAY enable or disable automatic warns!");
            if (sender.CheckPermission("scputils.globaledit")) message.AppendLine("You MAY edit players globally!");
            if (sender.CheckPermission("scputils.playeredit")) message.AppendLine("You MAY edit single players!");
            if (sender.CheckPermission("scputils.playerdelete")) message.AppendLine("You MAY delete players from database!");
            if (sender.CheckPermission("scputils.keep")) message.AppendLine("You MAY force the plugin to keep user preferences even without permissions!");
            if (sender.CheckPermission("scputils.moderatecommands")) message.AppendLine("You MAY moderate SCPUtils commands!");
            if (sender.CheckPermission("scputils.dnt")) message.AppendLine("You MAY force to ignore DNT requests from specific players");
            if (sender.CheckPermission("scputils.showwarns")) message.AppendLine("You MAY see detailed automatic warns info!");
            if (sender.CheckPermission("scputils.unwarn")) message.AppendLine("You MAY remove automatic sanctions from a player!");
            if (sender.CheckPermission("scputils.broadcast")) message.AppendLine("You MAY use SCPUtils broadcast feature!");
            if (sender.CheckPermission("scputils.changenickname")) message.AppendLine("You MAY change your own nickname!");
            if (sender.CheckPermission("scputils.changecolor")) message.AppendLine("You MAY change your own color!");
            if (sender.CheckPermission("scputils.badgevisibility")) message.AppendLine("You MAY change your own badge visibility!");
            if (sender.CheckPermission("scputils.ownplaytime")) message.AppendLine("You MAY see your own playtime!");
            if (sender.CheckPermission("scputils.bypassnickrestriction")) message.AppendLine("You MAY bypass nicknames restrictions!");
            if (sender.CheckPermission("scputils.roundinfo.execute")) message.AppendLine("You MAY execute roundinfo command!");
            if (sender.CheckPermission("scputils.roundinfo.roundtime")) message.AppendLine("You MAY see roundtime!");
            if (sender.CheckPermission("scputils.roundinfo.tickets")) message.AppendLine("You MAY see all teams tickets!");
            if (sender.CheckPermission("scputils.roundinfo.nextrespawnteam")) message.AppendLine("You MAY see next respawn info!");
            if (sender.CheckPermission("scputils.roundinfo.respawncount")) message.AppendLine("You MAY see last chaos/mtf respawn info!");
            if (sender.CheckPermission("scputils.roundinfo.lastrespawn")) message.AppendLine("You MAY see when chaos/mtf respawned!");
            if (sender.CheckPermission("scputils.onlinelist.basic")) message.AppendLine("You MAY execute online info command!");
            if (sender.CheckPermission("scputils.onlinelist.userid")) message.AppendLine("You MAY see online info userids!");
            if (sender.CheckPermission("scputils.onlinelist.badge")) message.AppendLine("You MAY see online info badges!");
            if (sender.CheckPermission("scputils.onlinelist.role")) message.AppendLine("You MAY see online info roles!");
            if (sender.CheckPermission("scputils.onlinelist.health")) message.AppendLine("You MAY see online info health!");
            if (sender.CheckPermission("scputils.onlinelist.flags")) message.AppendLine("You MAY see online info flags!");
            if (sender.CheckPermission("scputils_speak.scp049") || ScpUtils.StaticInstance.Config.AllowedScps.Contains(RoleType.Scp049)) message.AppendLine("You MAY speak with SCP-049!");
            if (sender.CheckPermission("scputils_speak.scp0492") || ScpUtils.StaticInstance.Config.AllowedScps.Contains(RoleType.Scp0492)) message.AppendLine("You MAY speak with SCP-0492!");
            if (sender.CheckPermission("scputils_speak.scp079") || ScpUtils.StaticInstance.Config.AllowedScps.Contains(RoleType.Scp079)) message.AppendLine("You MAY speak with SCP-079!");
            if (sender.CheckPermission("scputils_speak.scp096") || ScpUtils.StaticInstance.Config.AllowedScps.Contains(RoleType.Scp096)) message.AppendLine("You MAY speak with SCP-096!");
            if (sender.CheckPermission("scputils_speak.scp106") || ScpUtils.StaticInstance.Config.AllowedScps.Contains(RoleType.Scp106)) message.AppendLine("You MAY speak with SCP-106!");
            if (sender.CheckPermission("scputils_speak.scp173") || ScpUtils.StaticInstance.Config.AllowedScps.Contains(RoleType.Scp173)) message.AppendLine("You MAY speak with SCP-173!");
            message.AppendLine("Permissions not listed here means they are denied, more info about permissions on github.com/terminator-97/SCPUtils");


            response = message.ToString();
            return true;
        }

    }
}
