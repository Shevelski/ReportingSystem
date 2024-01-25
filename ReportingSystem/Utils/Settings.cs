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
            //IConfigurationRoot configuration = LoadConfiguration();
            //string source = configuration.GetValue<string>("Source:source");
            //return source.Equals("1") ? "sql" : "json";
            return GetValue("Source:source").Equals("1") ? "sql" : "json";
            
        }

        // Режим роботи
        public static string Mode()
        {
            //IConfigurationRoot configuration = LoadConfiguration();
            //string mode = configuration.GetValue<string>("Source:mode");
            //return mode.Equals("1") ? "write" : "read";
            return GetValue("Source:mode").Equals("1") ? "write" : "read";
        }

        public static string Server()
        {
            //IConfigurationRoot configuration = LoadConfiguration();
            //return configuration.GetValue<string>("ConnectionSettings:Server");
            return GetValue("ConnectionSettings:Server");
        }

        public static string DB()
        {
            //IConfigurationRoot configuration = LoadConfiguration();
            //return configuration.GetValue<string>("ConnectionSettings:DB");
            return GetValue("ConnectionSettings:DB");
        }

        public static string Login()
        {
            //IConfigurationRoot configuration = LoadConfiguration();
            //return configuration.GetValue<string>("ConnectionSettings:Server");
            return GetValue("ConnectionSettings:Login");
        }

        public static string Password()
        {
            //IConfigurationRoot configuration = LoadConfiguration();
            //return configuration.GetValue<string>("ConnectionSettings:DB");
            return GetValue("ConnectionSettings:Password");
        }
    }
}
