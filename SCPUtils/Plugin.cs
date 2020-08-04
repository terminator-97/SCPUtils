using Log = Exiled.API.Features.Log;
using ServerEvents = Exiled.Events.Handlers.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using MapEvents = Exiled.Events.Handlers.Map;
using Features = Exiled.API.Features;
using MEC;
using HarmonyLib;
using System;

namespace SCPUtils
{

    public class ScpUtils : Features.Plugin<Configs>
    {
        private static readonly Lazy<ScpUtils> LazyInstance = new Lazy<ScpUtils>(() => new ScpUtils());
        public static ScpUtils StaticInstance => LazyInstance.Value;
        public static string pluginVersion = "2.3.0";
        public override string Author { get; } = "Terminator_9#0507";
        public override string Name { get; } = "SCPUtils";
        public override Version Version { get; } = new Version(2, 3, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 0, 10);
        public EventHandlers EventHandlers { get; private set; }
        public Functions Functions { get; private set; }
        public Player Player { get; private set; }
        public Database DatabasePlayerData { get; private set; }
        public int PatchesCounter { get; private set; }

        public Harmony Harmony { get; private set; }

        private ScpUtils()
        {

        }

        public void LoadEvents()
        {
            MapEvents.Decontaminating += EventHandlers.OnDecontaminate;
            PlayerEvents.Joined += EventHandlers.OnPlayerJoin;
            PlayerEvents.Left += EventHandlers.OnPlayerLeave;
            PlayerEvents.Spawning += EventHandlers.OnPlayerSpawn;
            PlayerEvents.Dying += EventHandlers.OnPlayerDeath;
            PlayerEvents.ChangingRole += EventHandlers.OnChangeRole;
            PlayerEvents.Hurting += EventHandlers.OnPlayerHurt;
            Exiled.Events.Handlers.Scp079.InteractingTesla += EventHandlers.On079TeslaEvent;
            ServerEvents.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
        }

        public override void OnEnabled()
        {
            if (!Config.IsEnabled) return;
            Functions = new Functions(this);
            EventHandlers = new EventHandlers(this);
            DatabasePlayerData = new Database(this);
            EventHandlers.TemporarilyDisabledWarns = false;
            LoadEvents();
            DatabasePlayerData.CreateDatabase();
            DatabasePlayerData.OpenDatabase();
            try
            {
                Harmony = new Harmony($"com.terminator97.scputils.{PatchesCounter++}");
                Harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"Patching failed!, " + e);
            }

            Log.Debug("Events patched successfully!");
        }

        public override void OnDisabled()
        {
            MapEvents.Decontaminating -= EventHandlers.OnDecontaminate;
            PlayerEvents.Joined -= EventHandlers.OnPlayerJoin;
            PlayerEvents.Left -= EventHandlers.OnPlayerLeave;
            PlayerEvents.Spawning -= EventHandlers.OnPlayerSpawn;
            PlayerEvents.Dying -= EventHandlers.OnPlayerDeath;
            PlayerEvents.ChangingRole -= EventHandlers.OnChangeRole;
            PlayerEvents.Hurting -= EventHandlers.OnPlayerHurt;
            Exiled.Events.Handlers.Scp079.InteractingTesla -= EventHandlers.On079TeslaEvent;
            ServerEvents.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            EventHandlers = null;
            Functions = null;
            Database.LiteDatabase.Dispose();
            Harmony.UnpatchAll();
        }

        public override void OnReloaded()
        {

        }
    }
}