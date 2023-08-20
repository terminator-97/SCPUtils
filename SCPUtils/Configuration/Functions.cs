namespace SCPUtils.Config
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Broadcast = Extensions.Broadcast;
    using FunctionEnums = Enums.FunctionsType;
    using MessageType = Enums.FunctionsMessage;

    public class Functions
    {
        public Dictionary<FunctionEnums, MessageType> FunctionMessageType { get; set; } = new()
        {
            { FunctionEnums.Restarter, MessageType.ServerHint },
        };

        [Description("Definitions\n #\n # \"Restarter\": the message can be only displayed in server console.\n #\n # \"<color=>\": the color can be used for broadcast (#005EBC is blue, #C50000 is red).\n # \"$playerNickname$\": the player's name, without the ID.\n # \"$playerRole$\": the role of the player.\n # \"rounds\": the rounds, in numbers, that the player will be replaced from SCP to any human class.")]
        public Dictionary<FunctionEnums, string> FunctionMessage { get; private set; } = new()
        {
            { FunctionEnums.Restarter, "Warning: Server is auto-restarting!" },
            { FunctionEnums.ScpBanned, "<color=#005EBC>[SCPUtils - AutoModerator]\n$playerNickname$ ($playerRole$) has been <color=#C50000>BANNED</color> from playing SCP for exceeding Quits / Suicides (as SCP) limit for $rounds$ rounds.</color>" },
            { FunctionEnums.IssuesBan, "<color=#005EBC>[SCPUtils - AutoModerator]\n$playerNickname$ has suicided while having an active ban!</color>" },
        };

        public Dictionary<FunctionEnums, bool> FunctionType { get; private set; } = new()
        {
            { FunctionEnums.Restarter, true },
            { FunctionEnums.Sanctions, true },
        };

        public Dictionary<FunctionEnums, Broadcast> FunctionBroadcasts { get; private set; } = new()
        {
            { FunctionEnums.Restarter, new()
                {
                    Show = true,
                    Content = "",
                    Duration = 10,
                    Type = global::Broadcast.BroadcastFlags.Normal,
                    ClearAll = false
                }
            }
        };

        public Dictionary<FunctionEnums, string> FunctionsWarningsMessages { get; private set; } = new()
        {
            { FunctionEnums.ScpBanned, "" },
            { FunctionEnums.IssuesBan, "" }
        };
    }
}
