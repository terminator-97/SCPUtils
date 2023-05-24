namespace SCPUtils.Config
{
    using System.ComponentModel;

    public class MotdConfig
    {
        [Description("List of variables!\n # $player: Get the player name\n # $serverName: Get the name of server\n # $roundDurationMinutes: Get the minutes elapsed since the start of the round\n # $roundDurationSeconds: Get the Seconds, after minutes, elapsed since the start of the round\n # $players: Get the number of player\n # $")] 
        public Extensions.Broadcast Motd { get; private set; } = new Extensions.Broadcast("Welcome $player to $serverName!\nIn this moments they're %players% players!", 12, true, Broadcast.BroadcastFlags.Normal, false);
    }
}