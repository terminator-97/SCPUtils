using CommandSystem;
using System;
using System.Collections.Generic;

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
                response = ScpUtils.StaticInstance.commandTranslation.SenderError;
                return false;
            }
            int valid = 0;
            int invalid = 0;
            foreach (var a in Database.LiteDatabase.GetCollection<Player>().FindAll())
            {
                try
                {
                    var x = a.Name;
                    valid++;
                }
                catch (Exception e)
                {
                    invalid++;
                }
            }

            response = $"Invalid: {invalid}, Valid: {valid}";
            return true;
        }
    }
}
