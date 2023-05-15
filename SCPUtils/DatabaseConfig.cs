using System.ComponentModel;

namespace SCPUtils
{
    public class DatabaseConfig
    {
        [Description("DATABASE CONFIGURATION\n# database_name: Set the database Name, will get used for name in MongoDBCompass.\n# database_ip: Set the database IP, if is a local server don't change it.\n# database_port: Set the database Port, don't change if you don't know.\n# database_user: Set the name of the user.\n# database_password: Set the Password for Database, if is a localhost REMOVE IT or will doesn't work.\n# database_auth_type: Set the Auth Type, don't change it if you don't know.")]
        public string DatabaseName { get; private set; } = "ScpUtils";
        public string DatabaseIp { get; private set; } = "localhost";
        public ushort DatabasePort { get; private set; } = 27017;
        public string DatabaseUser { get; private set; } = "user";
        public string DatabasePassword { get; private set; } = "StrongPassword";
        public string DatabaseAuthType { get; private set; } = "SCRAM-SHA-256";
    }
}