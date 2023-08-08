namespace SCPUtils
{
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Events;

    public class ScpUtils
    {
        public EventHandlers EventHandlers { get; private set; }
        public Function Functions { get; private set; }
        public Player Player { get; private set; }
        public Events.Events Events { get; private set; }
        public int PatchesCounter { get; private set; }
        public static ScpUtils StaticInstance;

        [PluginPriority(PluginAPI.Enums.LoadPriority.Medium)]
        [PluginEntryPoint("SCPUtils", "0.6.2", "The most famous plugin that offers many additions to the servers.", "Terminator_97, Bay")]
        public void LoadPlugin()
        {
            if (Configs.IsEnabled is false)
            {
                StaticInstance = null;

                Functions = null;
                EventHandlers = null;
                Events = null;

                EventManager.UnregisterEvents<EventHandlers>(this);

                Database.Close();

                Log.Error("SCPUtils has been disabled by server administration. Check Configs if is an error.");

                return;
            }

            StaticInstance = this;

            Functions = new Function(this);
            EventHandlers = new EventHandlers();
            Events = new Events.Events(this);

            EventManager.RegisterEvents<EventHandlers>(this);

            var handler = PluginHandler.Get(this);

            handler.SaveConfig(this, nameof(Config.Configs));
            handler.SaveConfig(this, nameof(Config.Permissions));
            handler.SaveConfig(this, nameof(Config.Command));
            handler.SaveConfig(this, nameof(Config.Database));
            handler.SaveConfig(this, nameof(Config.MOTD));
            handler.SaveConfig(this, nameof(Config.Webhook));

            Database.OpenDatabase();

            if (GetWebhookConfig.Url is "None")
            {
                Log.Error("Webhook configuration is null, plugin cannot load!");
                return;
            }
        }

        [PluginConfig("configuration.yml")] public Config.Configs Configs;
        [PluginConfig("Database/database.yml")] public Config.Database databaseConfig;

        [PluginConfig("Permissions/permissions.yml")] public Config.Permissions perms;

        [PluginConfig("Translations/commands.yml")] public Config.Command commandTranslation;
        [PluginConfig("Translations/functions.yml")] public Config.Functions GetFunctions;
        [PluginConfig("Translations/motd.yml")] public Config.MOTD GetMotd;
        [PluginConfig("Translations/translation.yml")] public Config.Translation Translation;
        [PluginConfig("Translations/webhook.yml")] public Config.Webhook GetWebhookConfig;
    }
}
