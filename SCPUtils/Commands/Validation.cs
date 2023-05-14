using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

//Command is here in case if you need convert playtime of all players inside database, you must edit the code for your needs. (example: if you change time format in the vps), this this is a DIY, to recompile the plugin you need publicized assembly c# and you must allow unsafe code.
//Before doing anything make a copy of your database

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Validation : ICommand
    {

        public string Command { get; } = "scputils_validation";

        public string[] Aliases { get; } = new[] { "scv", "su_v", "scpu_v" };

        public string Description { get; } = "Check player data in DB";

        public Dictionary<string, int> TempDict { get; private set; } = new Dictionary<string, int>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scputils.dev"))
            {
                response = "<color=red>You need a higher administration level to use this command!</color>";
                return false;
            }
            int valid = 0;
            int invalid = 0;
            foreach (var a in Database.LiteDatabase.GetCollection<Player>().FindAll())
            {
                try
                {
                    var x = a.Name;

                    if (a.SuicideDate.Count() != a.SuicideDate.Count())
                    {
                        a.RoundsBan.Clear();
                        for (var i = 0; i < a.SuicideDate.Count(); i++)
                        {
                            a.RoundsBan.Add(0);
                        }
                        Database.LiteDatabase.GetCollection<Player>().Update(a);
                    }

                    if (a.SuicideDate.Count() != a.Expire.Count())
                    {
                        a.Expire.Clear();
                        for (var i = 0; i < a.SuicideDate.Count(); i++)
                        {
                            a.Expire.Add(DateTime.MinValue);
                        }
                        Database.LiteDatabase.GetCollection<Player>().Update(a);
                    }

                    if (a.SuicideDate.Count() != a.RoundsBan.Count())
                    {
                        a.RoundsBan.Clear();
                        for (var i = 0; i < a.SuicideDate.Count(); i++)
                        {
                            a.RoundsBan.Add(0);
                        }
                        Database.LiteDatabase.GetCollection<Player>().Update(a);
                    }

                    if (a.SuicideDate.Count() != a.UserNotified.Count())
                    {
                        a.UserNotified.Clear();
                        for (var i = 0; i < a.SuicideDate.Count(); i++)
                        {
                            a.UserNotified.Add(true);
                        }
                        Database.LiteDatabase.GetCollection<Player>().Update(a);
                    }

                    if (a.SuicideDate.Count() != a.LogStaffer.Count())
                    {
                        a.LogStaffer.Clear();
                        for (var i = 0; i < a.SuicideDate.Count(); i++)
                        {
                            a.LogStaffer.Add("Unknown");
                        }
                        Database.LiteDatabase.GetCollection<Player>().Update(a);
                    }

                        if (a.SuicideDate.Count() != a.SuicidePunishment.Count())
                        {
                            a.SuicidePunishment.Clear();
                            for (var i = 0; i < a.SuicideDate.Count(); i++)
                            {
                                a.SuicidePunishment.Add("Unknown");
                            }
                            Database.LiteDatabase.GetCollection<Player>().Update(a);
                        }

                        if (a.SuicideDate.Count() != a.SuicideScp.Count())
                        {
                            a.SuicideDate.Clear();
                            for (var i = 0; i < a.SuicideDate.Count(); i++)
                            {
                                a.SuicideScp.Add("Unknown");
                            }
                            Database.LiteDatabase.GetCollection<Player>().Update(a);
                        }

                        if (a.SuicideDate.Count() != a.SuicideType.Count())
                        {
                            a.SuicideType.Clear();
                            for (var i = 0; i < a.SuicideDate.Count(); i++)
                            {
                                a.SuicideType.Add("Unknown");
                            }
                            Database.LiteDatabase.GetCollection<Player>().Update(a);
                        }                        


                        valid++;
                }
                catch (Exception e)
                {
                    invalid++;
                    Exiled.API.Features.Log.Info($"Invalid player: {a.Id} - {a.Name}");
                }            
            }

            response = $"Invalid: {invalid}, Valid: {valid}";
            return true;
        }
    }
}
