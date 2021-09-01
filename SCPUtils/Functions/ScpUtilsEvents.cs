using System;

namespace SCPUtils.Events
{
    public class Events
    {
        private readonly ScpUtils pluginInstance;

        public Events(ScpUtils pluginInstance)
        {
            this.pluginInstance = pluginInstance;
        }

        public EventHandler<BadgeRemovedEvent> OnBadgeRemove { get; set; }
        public EventHandler<BadgeSetEvent> OnBadgeIsSet { get; set; }

        public EventHandler<ReplacePlayerEvent> OnReplacePlayer { get; set; }

        public virtual void OnBadgeRemoved(BadgeRemovedEvent e)
        {
            EventHandler<BadgeRemovedEvent> handler = OnBadgeRemove;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public virtual void OnBadgeSet(BadgeSetEvent e)
        {
            EventHandler<BadgeSetEvent> handler = OnBadgeIsSet;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public virtual void OnReplacePlayerEvent(ReplacePlayerEvent e)
        {
            EventHandler<ReplacePlayerEvent> handler = OnReplacePlayer;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class BadgeRemovedEvent : EventArgs
    {
        public Exiled.API.Features.Player Player { get; set; }
        public string BadgeName { get; set; }
    }

    public class BadgeSetEvent : EventArgs
    {
        public Exiled.API.Features.Player Player { get; set; }
        public string NewBadgeName { get; set; }
    }

    public class ReplacePlayerEvent : EventArgs
    {
        public Exiled.API.Features.Player BannedPlayer { get; set; }
        public Exiled.API.Features.Player ReplacedPlayer { get; set; }
        public RoleType ScpRole { get; set; }
        public RoleType NormalRole { get; set; }
    }
}
