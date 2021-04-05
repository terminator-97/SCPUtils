using Log = Exiled.API.Features.Log;
using ServerEvents = Exiled.Events.Handlers.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using MapEvents = Exiled.Events.Handlers.Map;
using Handlers = Exiled.Events.Handlers;
using Features = Exiled.API.Features;
using MEC;
using HarmonyLib;
using System;

namespace SCPUtils
{

    public class ScpUtils : Features.Plugin<Configs>
    {    
        public override string Author { get; } = "Terminator_97#0507";
        public override string Name { get; } = "SCPUtils";
        public override Version Version { get; } = new Version(2, 5, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 8, 0);
        public EventHandlers EventHandlers { get; private set; }
        public Functions Functions { get; private set; }
        public Player Player { get; private set; }
        public Database DatabasePlayerData { get; private set; }
        public int PatchesCounter { get; private set; } 

        public Harmony Harmony { get; private set; }

        private static readonly ScpUtils InstanceValue = new ScpUtils();

        private ScpUtils()
        {

        }

        public static ScpUtils StaticInstance => InstanceValue;

        public void LoadEvents()
        {
            MapEvents.Decontaminating += EventHandlers.OnDecontaminate;
            PlayerEvents.Verified += EventHandlers.OnPlayerVerify;
            PlayerEvents.Destroying += EventHandlers.OnPlayerDestroy;
            PlayerEvents.Spawning += EventHandlers.OnPlayerSpawn;
            PlayerEvents.Dying += EventHandlers.OnPlayerDeath;
            PlayerEvents.Hurting += EventHandlers.OnPlayerHurt;
            Handlers.Scp079.InteractingTesla += EventHandlers.On079TeslaEvent;
            ServerEvents.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            ServerEvents.RoundEnded += EventHandlers.OnRoundEnded;
            Handlers.Scp096.AddingTarget += EventHandlers.On096AddTarget;
            ServerEvents.RespawningTeam += EventHandlers.OnTeamRespawn;

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
            PlayerEvents.Verified -= EventHandlers.OnPlayerVerify;
            PlayerEvents.Destroying -= EventHandlers.OnPlayerDestroy;
            PlayerEvents.Spawning -= EventHandlers.OnPlayerSpawn;
            PlayerEvents.Dying -= EventHandlers.OnPlayerDeath;
            PlayerEvents.Hurting -= EventHandlers.OnPlayerHurt;
            Handlers.Scp079.InteractingTesla -= EventHandlers.On079TeslaEvent;
            ServerEvents.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            ServerEvents.RoundEnded -= EventHandlers.OnRoundEnded;
            Handlers.Scp096.AddingTarget -= EventHandlers.On096AddTarget;
            ServerEvents.RespawningTeam -= EventHandlers.OnTeamRespawn;
            EventHandlers = null;
            Functions = null;
            Functions.LastWarn.Clear();
            Database.LiteDatabase.Dispose();
            Harmony.UnpatchAll();
        }

       
    }
}