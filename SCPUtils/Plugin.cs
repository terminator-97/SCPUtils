namespace SCPUtils
{
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Events;
    using SCPUtils.Commands;

    public class ScpUtils
    {
        public EventHandlers EventHandlers { get; private set; }
        public Functions Functions { get; private set; }
        public Player Player { get; private set; }
        public Events.Events Events { get; private set; }
        public int PatchesCounter { get; private set; }
        public static ScpUtils StaticInstance;

        [PluginPriority(PluginAPI.Enums.LoadPriority.Medium)]
        [PluginEntryPoint("SCPUtils", "0.4.6", "The most famous plugin that offers many additions to the servers.", "Terminator_97")]
        public void LoadPlugin()
        {
            if (configs.IsEnabled is false)
            {
                StaticInstance = null;

                Functions = null;
                EventHandlers = null;
                Events = null;

                EventManager.UnregisterEvents<EventHandlers>(this);

                Database.Close();

                Log.Error("SCPUtils has been disabled by server administration. Check configs if is an error.");

                return;
            }

            StaticInstance = this;

            Functions = new Functions(this);
            EventHandlers = new EventHandlers();
            //DatabasePlayerData = new Database(this);
            Events = new Events.Events(this);

            EventManager.RegisterEvents<EventHandlers>(this);

            var handler = PluginHandler.Get(this);

            handler.SaveConfig(this, nameof(Configs));
            //handler.SaveConfig(this, nameof(Permissions));
            handler.SaveConfig(this, nameof(CommandTranslation));
            handler.SaveConfig(this, nameof(DatabaseConfig));

            Database.OpenDatabase();
        }

        [PluginConfig] public Configs configs;

        //[PluginConfig("permissions.yml")] public Permissions perms;

        [PluginConfig("commands.yml")] public CommandTranslation commandTranslation;

        [PluginConfig("database.yml")] public DatabaseConfig databaseConfig;
    }
}
