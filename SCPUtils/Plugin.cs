using Log = Exiled.API.Features.Log;
using ServerEvents = Exiled.Events.Handlers.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using MapEvents = Exiled.Events.Handlers.Map;
using Features = Exiled.API.Features;
using MEC;
using HarmonyLib;
using System;
using System.IO;

namespace SCPUtils
{

    public class ScpUtils : Features.Plugin<Configs>
    {
        public static bool IsStarted { get; set; }
        public static string pluginVersion = "2.0.2";

        public EventHandlers EventHandlers { get; private set; }
        public Commands Commands { get; private set; }
        public Functions Functions { get; private set; }
        public Player Player { get; private set; }
        public ConsoleCommands PlayerConsoleCommands { get; private set; }

        public Database DatabasePlayerData { get; private set; }
        public int PatchesCounter { get; private set; }

        public Harmony Harmony { get; private set; }






        public void LoadEvents()
        {

            ServerEvents.RoundStarted += EventHandlers.OnRoundStart;
            ServerEvents.RoundEnded += EventHandlers.OnRoundEnd;
            ServerEvents.RestartingRound += EventHandlers.OnRoundRestart;
            MapEvents.Decontaminating += EventHandlers.OnDecontaminate;
            PlayerEvents.Joined += EventHandlers.OnPlayerJoin;
            PlayerEvents.Died += EventHandlers.OnPlayerDeath;
            PlayerEvents.TriggeringTesla += EventHandlers.OnTeslaEvent;
            PlayerEvents.Left += EventHandlers.OnPlayerLeave;
            PlayerEvents.Spawning += EventHandlers.OnPlayerSpawn;

        }

        public void LoadCommands()
        {
            ServerEvents.SendingRemoteAdminCommand += Commands.OnRaCommand;
            ServerEvents.SendingConsoleCommand += PlayerConsoleCommands.OnConsoleCommand;

        }



        public override void OnEnabled()
        {
            if (!Config.IsEnabled) return;
            Commands = new Commands();
            Functions = new Functions(this);
            EventHandlers = new EventHandlers(this);
            PlayerConsoleCommands = new ConsoleCommands(this);
            DatabasePlayerData = new Database(this);
            LoadEvents();
            LoadCommands();
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
            PlayerEvents.Died -= EventHandlers.OnPlayerDeath;
            PlayerEvents.TriggeringTesla -= EventHandlers.OnTeslaEvent;
            PlayerEvents.Left -= EventHandlers.OnPlayerLeave;
            PlayerEvents.Spawning -= EventHandlers.OnPlayerSpawn;
            ServerEvents.SendingRemoteAdminCommand -= Commands.OnRaCommand;
            ServerEvents.SendingConsoleCommand -= PlayerConsoleCommands.OnConsoleCommand;
            Timing.KillCoroutines(Functions.DT);
            EventHandlers = null;
            Commands = null;
            Functions = null;
            Database.LiteDatabase.Dispose();
            Harmony.UnpatchAll();
        }

        public override void OnReloaded()
        {

        }
    }
}