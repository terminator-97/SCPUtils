using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using Log = Exiled.API.Features.Log;

//Command is here in case if you need convert playtime of all players inside database, you must edit the code for your needs. (example: if you change time format in the vps), this this is a DIY, to recompile the plugin you need publicized assembly c# and you must allow unsafe code.
//Before doing anything make a copy of your database

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class AddIps : ICommand
    {

        public string Command { get; } = "scputils_add_ips";

        public string[] Aliases { get; } = new[] { "addips" };

        public string Description { get; } = "Add ips from old system into new system, execute this command only once!";

        public Dictionary<string, int> TempDict { get; private set; } = new Dictionary<string, int>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (ScpUtils.StaticInstance.Functions.CheckCommandCooldown(sender) == true)
            {
                response = ScpUtils.StaticInstance.Config.CooldownMessage;
                return false;
            }

            if (!sender.CheckPermission("scputils.convert") && !((CommandSender)sender).FullPermissions)
            {
                response = "<color=red>You need a higher administration level to use this command!</color>";
                return false;
            }
            Log.Info($"Starting conversion, starting checking {Database.LiteDatabase.GetCollection<Player>().Count() } Accounts");
            foreach (var a in Database.LiteDatabase.GetCollection<Player>().FindAll())
            {
                if (!string.IsNullOrEmpty(a.Ip))
                {
                    string ipaddress = a.Ip;
                    string userId = string.Concat(a.Id, "@", a.Authentication);
                    if (!Database.LiteDatabase.GetCollection<DatabaseIp>().Exists(ip => ip.Id == ipaddress))
                    {
                        ScpUtils.StaticInstance.DatabasePlayerData.AddIp(ipaddress, userId);
                    }
                    else
                    {
                        var databaseIp = GetIp.GetIpAddress(a.Ip);
                        if (!databaseIp.UserIds.Contains(userId))
                        {
                            databaseIp.UserIds.Add(userId);
                            Database.LiteDatabase.GetCollection<DatabaseIp>().Update(databaseIp);

                        }
                    }
                }

            }
            response = "Operation completed!";


            return true;
        }
    }
}

