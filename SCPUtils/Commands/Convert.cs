using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Log = Exiled.API.Features.Log;

//Command is here in case if you need convert playtime of all players inside database, you must edit the code for your needs. (example: if you change time format in the vps), this this is a DIY, to recompile the plugin you need publicized assembly c# and you must allow unsafe code.
//Before doing anything make a copy of your database

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class Convert : ICommand
    {
        
                public string Command { get; } = "scputils_convert";

                public string[] Aliases { get; } = new[] { "sconv" };

                public string Description { get; } = "Convert english date format to italian";

                public Dictionary<string, int> TempDict { get; private set; } = new Dictionary<string, int>();

                public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
                {
            /*

                    if (!sender.CheckPermission("scputils.convert") && !((CommandSender)sender).FullPermissions)
                    {
                        response = "<color=red>You need a higher administration level to use this command!</color>";
                        return false;
                    }


                    Log.Info("Starting conversion!");

                    foreach (var a in Database.LiteDatabase.GetCollection<Player>().FindAll())
                    {                 
                        foreach (var b in a.PlayTimeRecords)
                        {

                            var Day = b.Key.ToString().Split(new string[] { "/" }, StringSplitOptions.None)[1];
                            var Month = b.Key.ToString().Split(new string[] { "/" }, StringSplitOptions.None)[0];
                            var Year = b.Key.ToString().Split(new string[] { "/" }, StringSplitOptions.None)[2];
                            var value = b.Value;
                            if (int.Parse(Month) > 5 && int.Parse(Day) >= 4 && int.Parse(Year) == 2021)
                            {                      
                                TempDict.Add($"{Month}/{Day}/{Year}", value);
                            }

                            else
                            {
                                TempDict.Add($"{Day}/{Month}/{Year}", value);                              
                            }


                        }

                        a.PlayTimeRecords.Clear();
                        a.PlayTimeRecords = TempDict;
                        Database.LiteDatabase.GetCollection<Player>().Update(a);
                        TempDict.Clear();                    
                    }




                    response = "Playtime converted!";
        */
        response = "Disabled!";
            return true;
        }
    }
}
