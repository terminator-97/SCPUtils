using HarmonyLib;
using System;
using Features = Exiled.API.Features;
using Handlers = Exiled.Events.Handlers;
using Log = Exiled.API.Features.Log;
using MapEvents = Exiled.Events.Handlers.Map;
using PlayerEvents = Exiled.Events.Handlers.Player;
using ServerEvents = Exiled.Events.Handlers.Server;


namespace SCPUtils
{

    public class ScpUtils : Features.Plugin<Configs>
    {
        public override string Author { get; } = "Terminator_97#0507";
        public override string Name { get; } = "SCPUtils";
        public override Version Version { get; } = new Version(4, 1, 1);
        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);
        public EventHandlers EventHandlers { get; private set; }
        public Functions Functions { get; private set; }
        public Player Player { get; private set; }
        public Database DatabasePlayerData { get; private set; }
        public Events.Events Events { get; private set; }
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
            PlayerEvents.PreAuthenticating += EventHandlers.OnPlayerPreauth;
            Handlers.Scp079.InteractingTesla += EventHandlers.On079TeslaEvent;
            ServerEvents.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            ServerEvents.RoundEnded += EventHandlers.OnRoundEnded;
            Handlers.Scp096.AddingTarget += EventHandlers.On096AddTarget;
            ServerEvents.RespawningTeam += EventHandlers.OnTeamRespawn;
            ServerEvents.RestartingRound += EventHandlers.OnRoundRestart;
            PlayerEvents.Handcuffing += EventHandlers.OnPlayerHandcuff;
            PlayerEvents.RemovingHandcuffs += EventHandlers.OnPlayerUnhandCuff;
            PlayerEvents.Joined += EventHandlers.OnPlayerJoined;
        }

        public override void OnEnabled()
        {
            Functions = new Functions(this);
            EventHandlers = new EventHandlers(this);
            DatabasePlayerData = new Database(this);
            Events = new Events.Events(this);

            EventHandlers.TemporarilyDisabledWarns = false;

            if (Config.EnableAutoRestart)
            {
                Functions.CoroutineRestart();
            }

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
            PlayerEvents.PreAuthenticating -= EventHandlers.OnPlayerPreauth;
            Handlers.Scp079.InteractingTesla -= EventHandlers.On079TeslaEvent;
            ServerEvents.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            ServerEvents.RoundEnded -= EventHandlers.OnRoundEnded;
            Handlers.Scp096.AddingTarget -= EventHandlers.On096AddTarget;
            ServerEvents.RespawningTeam -= EventHandlers.OnTeamRespawn;
            ServerEvents.RestartingRound -= EventHandlers.OnRoundRestart;
            PlayerEvents.Handcuffing -= EventHandlers.OnPlayerHandcuff;
            PlayerEvents.RemovingHandcuffs -= EventHandlers.OnPlayerUnhandCuff;
            PlayerEvents.Joined -= EventHandlers.OnPlayerJoined;
            EventHandlers = null;
            Functions = null;
            Functions.LastWarn.Clear();
            Database.LiteDatabase.Dispose();
            Harmony.UnpatchAll();
        }


    }
}
