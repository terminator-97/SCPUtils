namespace SCPUtils.Config
{
    using System.ComponentModel;

    public class Translation
    {
        [Description("Autowarn message for suiciding as SCP")]
        public Extensions.Broadcast SuicideWarnMessage { get; private set; } = new Extensions.Broadcast("<color=red>WARN:\nAs per server rules SCP's suicide is an offence, doing it too much will result in a ban!</color>", 30, true, Broadcast.BroadcastFlags.Normal);

        [Description("Decontamination message (if enabled)")]
        public Extensions.Broadcast DecontaminationMessage { get; private set; } = new Extensions.Broadcast("<color=yellow>Decontamination has started</color>", 12, false, Broadcast.BroadcastFlags.Normal);

        [Description("Suicide auto-kick reason (if enabled)")]
        public string SuicideKickMessage { get; private set; } = "Suicide as SCP";

        [Description("Auto-kick message for using a restricted nickname")]
        public string AutoKickBannedNameMessage { get; private set; } = "You're using a restricted nickname or too similar to a restricted one, please change it";

        [Description("Suicide auto-ban reason (if enabled)")]
        public string AutoBanMessage { get; private set; } = "Exceeded SCP suicide limit";

        [Description("Message if player is not authorized to use this command")]
        public string UnauthorizedNickNameChange { get; private set; } = "<color=red>Permission denied.</color>";

        [Description("Message if player is not authorized to use this command")]
        public string UnauthorizedColorChange { get; private set; } = "<color=red>Permission denied.</color>";

        [Description("Message if player is not authorized to use this command")]
        public string UnauthorizedBadgeChangeVisibility { get; private set; } = "<color=red>Permission denied.</color>";

        [Description("Message if player is not authorized to use this command")]
        public string NicknameCooldownMessage { get; private set; } = "<color=red>This command is in user cooldown, try again in few minutes.</color>";

        [Description("Message if player try to change his nickname to a restricted one")]
        public string InvalidNicknameText { get; private set; } = "This nickname has been restricted by server owner, please use another nickname!";

        [Description("Which broadcast should be shown when a SCP die?")]
        public Extensions.Broadcast ScpDeathMessage { get; private set; } = new Extensions.Broadcast("<color=blue>SCP %playername% (%scpname%) was killed by %killername%. Cause of death: %reason%</color>", 12, true, Broadcast.BroadcastFlags.Normal);

        [Description("Which broadcast should be shown when a SCP die?")]
        public Extensions.Broadcast ScpSuicideMessage { get; private set; } = new Extensions.Broadcast("<color=blue>SCP %playername% (%scpname%) has killed by themselves. Cause of death: %reason%</color>", 12, true, Broadcast.BroadcastFlags.Normal);

        [Description("Text shown if a player doesn't own the handcuffed player")]
        public Extensions.Broadcast UnhandCuffDenied { get; private set; } = new Extensions.Broadcast("<color=blue>You do not have the ownership of this player therefore you can't un-handcuff him!</color>", 8, true, Broadcast.BroadcastFlags.Normal);

        [Description("Which message non-whitelisted players should get while connecting from blacklisted ASN?")]
        public string AsnKickMessage { get; private set; } = "The ASN you are connecting from is blacklisted from this server, please contact server staff to request to being whitelisted";

        [Description("Which message should be shown to who become SCP-096 target?")]
        public string Scp096TargetNotifyText { get; private set; } = "<color=red>Attention:</color>\n<color=purple>You became a target of SCP-096!</color>";

        [Description("Which message should be shown to last player alive of a team?")]
        public string LastPlayerAliveNotificationText { get; private set; } = "<color=red>Attention:</color>\n<color=purple>You are the last player alive of your team!</color>";

        [Description("Which message should be shown for offline warns when a player rejoin?")]
        public Extensions.Broadcast OfflineWarnNotification { get; private set; } = new Extensions.Broadcast("<color=red>Post-Warning notification:</color>\n<color=yellow>You've been recently warned for your recent quit as SCP in game, continuing this behaviour may cause a ban!</color>", 30, true, Broadcast.BroadcastFlags.Normal);

        [Description("Which message should be shown to a player when he gets banned from playing SCP?")]
        public Extensions.Broadcast RoundBanNotification { get; private set; } = new Extensions.Broadcast("<color=red>You have been banned:</color>\n<color=yellow><size=27>You have been banned from playing SCP. You are excluded from playing SCP %roundnumber% rounds due your past offences!</size></color>", 30, true, Broadcast.BroadcastFlags.Normal);

        [Description("Which message should be shown to a player when he spawns as SCP while being banned and replaced with another player?")]
        public Extensions.Broadcast RoundBanSpawnNotification { get; private set; } = new Extensions.Broadcast("<color=red>You're SCP banned:</color>\n<color=yellow><size=27>You have been removed as SCP because you're currently SCP-Banned! You must be replaced other %roundnumber% time(s) before you will be able to play SCP again!</size></color>", 30, true, Broadcast.BroadcastFlags.Normal);

        [Description("SCP swap request broadcast")]
        public Extensions.Broadcast SwapRequestBroadcast { get; private set; } = new Extensions.Broadcast("<color=blue>%player% (%scp%) wants to swap their role with you, to accept open the console with ò key and type .accept otherwise type .deny, you have %seconds% seconds left to accept this request.</color>", 20, true, Broadcast.BroadcastFlags.Normal);

        [Description("SCP swap request canceled broadcast")]
        public Extensions.Broadcast SwapRequestCanceledBroadcast { get; private set; } = new Extensions.Broadcast("<color=blue>The swap request has been canceled</color>", 12, true, Broadcast.BroadcastFlags.Normal);

        [Description("SCP swap request denied broadcast")]
        public Extensions.Broadcast SwapRequestDeniedBroadcast { get; private set; } = new Extensions.Broadcast("<color=blue>The swap request has been denied</color>", 12, true, Broadcast.BroadcastFlags.Normal);

        [Description("SCP swap request informative broadcast")]
        public Extensions.Broadcast SwapRequestInfoBroadcast { get; private set; } = new Extensions.Broadcast("<color=blue>You are an SCP, for %seconds% seconds you can exchange your role with other SCP player using swap command on ò, you can use .scplist to see who is scp</color>", 15, true, Broadcast.BroadcastFlags.Normal);

        [Description("Command cooldown text")]
        public string CooldownMessage { get; private set; } = "<color=red>Command execution failed! You are under cooldown or command banned, wait 5 seconds and try again, if the error persist you might have been banned from using commands, to see the reason and duration open the console after joining the server, this abusive action has been reported to the staff for futher punishments</color>";

        [Description("Broadcast to send to all online staff when player enter with more than 1 account")]
        public Extensions.Broadcast AlertStaffBroadcastMultiAccount { get; private set; } = new Extensions.Broadcast("<size=40><color=red>Alert</color></size>\n<size=35>Player <color=yellow>{player}</color> has entered with <color=yellow>{accountNumber}</color> accounts</size>\n<size=30>Check console pressing <color=yellow>ò</color></size>", 10);

        [Description("Broadcast to send to all online staff when player change IP")]
        public Extensions.Broadcast AlertStaffBroadcastChangeIP { get; private set; } = new Extensions.Broadcast("<size=40><color=red>Alert</color></size>\n<size=35>Player <color=yellow>{player}</color> has changed IP. <color=yellow>{oldIP}</color> to <color=yellow>{newIP}</color></size>\n<size=35>Check console pressing <color=yellow>ò</color></size>", 10);

        public Extensions.Broadcast AutoBanPlayerMessage { get; private set; } = new Extensions.Broadcast("<color=#4169E1>[SCPUTILS: ADMINISTRATION BROADCAST]</color>\n\n%player.Nickname% (%player.Role%) has been <color=#FF0000>BANNED</color> from the server for exceeding Quits / Suicides (as SCP) limit.\nDuration: %duration% mitutes", 12, true, Broadcast.BroadcastFlags.AdminChat);
    }
}
