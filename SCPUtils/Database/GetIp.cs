namespace SCPUtils
{
    public static class GetIp
    {
        public static DatabaseIp GetIpAddress(string ip) => Database.LiteDatabase.GetCollection<DatabaseIp>().FindOne(queryIp => queryIp.Ip == ip);

    }
}
