namespace SCPUtils.Extensions
{
    using System.ComponentModel;

    public class Broadcast
    {
        public Broadcast()
            : this(string.Empty)
        {
        }

        public Broadcast(string content, ushort duration = 10, bool show = true, global::Broadcast.BroadcastFlags type = global::Broadcast.BroadcastFlags.Normal, bool clearOther = false)
        {
            Content = content;
            Duration = duration;
            Show = show;
            Type = type;
            ClearAll = clearOther;
        }

        [Description("The broadcast content")]
        public string Content { get; set; }

        [Description("The broadcast duration")]
        public ushort Duration { get; set; }

        [Description("The broadcast type")]
        public global::Broadcast.BroadcastFlags Type { get; set; }

        [Description("Indicates whether the broadcast should be shown or not")]
        public bool Show { get; set; }

        public bool ClearAll { get; set; }

        public override string ToString() => $"({Content}) {Duration} {Type}";
    }
}