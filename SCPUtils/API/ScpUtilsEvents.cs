namespace SCPUtils.Events
{
    using System;
    using System.Collections.Generic;

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

        public EventHandler<MultiAccountEvent> OnMultiAccount { get; set; }

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

        public virtual void OnMultiAccountEvent(MultiAccountEvent e)
        {
            EventHandler<MultiAccountEvent> handler = OnMultiAccount;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class BadgeRemovedEvent : EventArgs
    {
        public PluginAPI.Core.Player Player { get; set; }
        public string BadgeName { get; set; }
    }

    public class BadgeSetEvent : EventArgs
    {
        public PluginAPI.Core.Player Player { get; set; }
        public string NewBadgeName { get; set; }
    }

    public class ReplacePlayerEvent : EventArgs
    {
        public PluginAPI.Core.Player BannedPlayer { get; set; }
        public PluginAPI.Core.Player ReplacedPlayer { get; set; }
        public PlayerRoles.RoleTypeId ScpRole { get; set; }
        public PlayerRoles.RoleTypeId NormalRole { get; set; }
    }

    public class MultiAccountEvent : EventArgs
    {
        public PluginAPI.Core.Player Player;
        public List<string> UserIds;
    }
}
