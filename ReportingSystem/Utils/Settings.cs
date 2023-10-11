namespace ReportingSystem.Utils
{
    public static class Settings
    {
        //режим роботи
        public static string Source()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            configuration.Reload();
            string source = configuration.GetValue<string>("Source:source");
            if (source.Equals("0"))
            {
                return "json";
            }

            if (source.Equals("1"))
            {
                return "sql";
            }

            return "json";
        }
        
        //режим роботи
        public static string Mode()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            configuration.Reload();
            string source = configuration.GetValue<string>("Source:mode");

            if (source.Equals("0"))
            {
                return "read";
            }

            if (source.Equals("1"))
            {
                return "write";
            }

            return "read";

        }
    }
}
