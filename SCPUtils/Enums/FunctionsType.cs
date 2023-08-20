namespace SCPUtils.Enums
{
    public enum FunctionsType
    {
        //Functions, not in alphabet order
        Restarter = 1,
        ScpBanned,
        IssuesBan,

        //Only for bools purprose
        Sanctions = 1000
    }

    public enum FunctionsMessage
    {
        PlayerConsole,
        ServerConsole,
        AdminBroadcast,
        ServerBroadcast,
        AdminHint,
        ServerHint
    }
}
