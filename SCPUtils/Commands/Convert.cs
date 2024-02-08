using CommandSystem;
using System;
using System.Collections.Generic;

//Command is here in case if you need convert playtime of all players inside database, you must edit the code for your needs. (example: if you change time
//in the vps), this this is a DIY, to recompile the plugin you need publicized assembly c# and you must allow unsafe code.
//Before doing anything make a copy of your database

namespace SCPUtils.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class Convert : ICommand
    {

        public string Command { get; } = "scputils_convert";

        public string[] Aliases { get; } = new[] { "sconv", "su_conv", "scpu_conv" };

        public string Description { get; } = "Convert english date format to your date";

        public Dictionary<string, int> TempDict { get; private set; } = new Dictionary<string, int>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {


            response = "Disabled";
            return true;
        }
    }
}
