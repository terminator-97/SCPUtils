namespace SCPUtils
{
    public static class GetBroadcast
    {
        public static BroadcastDb FindBroadcast(string id) => Database.LiteDatabase.GetCollection<BroadcastDb>().FindOne(queryBc => queryBc.Id == id);

    }
}
