namespace ReportingSystem.Utils
{
    public static class Settings
    {
        private static IConfigurationRoot LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private static string GetValue(string source)
        {
            IConfigurationRoot configuration = LoadConfiguration();
            return configuration.GetValue<string>(source);
        }

        // Режим роботи
        public static string Source()
        {
            return GetValue("Source:source").Equals("1") ? "sql" : "json";
        }

        // Режим роботи
        public static string Mode()
        {
            return GetValue("Source:mode").Equals("1") ? "write" : "read";
        }

        public static string Server()
        {
            return GetValue("ConnectionSettings:Server");
        }

        public static string DB()
        {
            return GetValue("ConnectionSettings:DB");
        }

        public static string Login()
        {
            return GetValue("ConnectionSettings:Login");
        }

        public static string Password()
        {
            return GetValue("ConnectionSettings:Password");
        }
        public static string UseDatabaseCredential()
        {
            return GetValue("ConnectionSettings:IsUseDatabaseCredential");
        }
        
    }
}
