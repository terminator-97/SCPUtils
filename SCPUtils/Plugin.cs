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
        public static bool IsStarted { get; set; }
        public static string pluginVersion = "2.1.1";
        public override string Author { get; } = "Terminator_9#0507";
        public override string Name { get; } = "SCPUtils";
        public override Version Version { get; } = new Version(2, 1, 1);
        public override Version RequiredExiledVersion { get; } = new Version(2, 0, 7);
        public EventHandlers EventHandlers { get; private set; }
        public Functions Functions { get; private set; }
        public Player Player { get; private set; }
        public Commands.ShowBadge ShowBadge { get; private set; }
        public Commands.HideBadge HideBadge { get; private set; }

        public Database DatabasePlayerData { get; private set; }
        public int PatchesCounter { get; private set; }

        public Harmony Harmony { get; private set; }

        private ScpUtils()
        {

        }

        public void LoadEvents()
        {
            ServerEvents.RoundStarted += EventHandlers.OnRoundStart;
            ServerEvents.RoundEnded += EventHandlers.OnRoundEnd;
            ServerEvents.RestartingRound += EventHandlers.OnRoundRestart;            
            MapEvents.Decontaminating += EventHandlers.OnDecontaminate;
            PlayerEvents.Joined += EventHandlers.OnPlayerJoin;
            PlayerEvents.Left += EventHandlers.OnPlayerLeave;        
            PlayerEvents.Spawning += EventHandlers.OnPlayerSpawn;
            PlayerEvents.Dying += EventHandlers.OnPlayerDeath;           
            Exiled.Events.Handlers.Scp079.InteractingTesla += EventHandlers.On079TeslaEvent;
        }

        public override void OnEnabled()
        {
            if (!Config.IsEnabled) return;
            Functions = new Functions(this);
            EventHandlers = new EventHandlers(this);
            DatabasePlayerData = new Database(this);
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
            ServerEvents.RoundStarted -= EventHandlers.OnRoundStart;
            ServerEvents.RoundEnded -= EventHandlers.OnRoundEnd;
            ServerEvents.RestartingRound -= EventHandlers.OnRoundRestart;
            MapEvents.Decontaminating -= EventHandlers.OnDecontaminate;
            PlayerEvents.Joined -= EventHandlers.OnPlayerJoin;
            PlayerEvents.Left -= EventHandlers.OnPlayerLeave;
            PlayerEvents.Spawning -= EventHandlers.OnPlayerSpawn;
            PlayerEvents.Dying -= EventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Scp079.InteractingTesla -= EventHandlers.On079TeslaEvent;
            Timing.KillCoroutines(Functions.DT);
            EventHandlers = null;
            Functions = null;
            HideBadge = null;
            ShowBadge = null;
            Database.LiteDatabase.Dispose();
            Harmony.UnpatchAll();
        }

        public override void OnReloaded()
        {

        }
    }
}