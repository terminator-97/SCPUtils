using System;
using Features = Exiled.API.Features;
using Handlers = Exiled.Events.Handlers;
using MapEvents = Exiled.Events.Handlers.Map;
using PlayerEvents = Exiled.Events.Handlers.Player;
using ServerEvents = Exiled.Events.Handlers.Server;
using Log = Exiled.API.Features.Log;

namespace SCPUtils
{

    public class ScpUtils : Features.Plugin<Configs>
    {
        public override string Author { get; } = "Terminator_97#0507";
        public override string Name { get; } = "SCPUtils";
        public override Version Version { get; } = new Version(5, 2, 2);
        public override Version RequiredExiledVersion { get; } = new Version(6, 0, 0);
        public EventHandlers EventHandlers { get; private set; }
        public Functions Functions { get; private set; }
        public Player Player { get; private set; }
        public Database DatabasePlayerData { get; private set; }
        public Events.Events Events { get; private set; }
        public int PatchesCounter { get; private set; }

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
            PlayerEvents.Banning += EventHandlers.OnBanned;
            PlayerEvents.Kicking += EventHandlers.OnKicking;            
            //  PlayerEvents.ChangingRole += EventHandlers.OnChangingRole;
            //  ServerEvents.RoundStarted += EventHandlers.OnRoundStarted;
            //PlayerEvents.TogglingOverwatch += EventHandlers.OnOverwatchToggle;                
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
            Log.Warn($"[NOTICE] LiteDB edition of SCPUtils (v. {Version}) has limited support, it will receive fixes only for critical bugs and won't receive any new feature, please upgrade to version 6.0.0 or higher which uses MongoDB, for more info read the pinned message on SCPUtils channel on Exiled server.");            
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
            PlayerEvents.Banning -= EventHandlers.OnBanned;
            PlayerEvents.Kicking -= EventHandlers.OnKicking;
            // PlayerEvents.TogglingOverwatch -= EventHandlers.OnOverwatchToggle;
            // PlayerEvents.ChangingRole -= EventHandlers.OnChangingRole;
            //  ServerEvents.RoundStarted -= EventHandlers.OnRoundStarted;
            EventHandlers = null;
            Functions = null;
            Functions.LastWarn.Clear();
            Database.LiteDatabase.Dispose();
        }


    }
}
