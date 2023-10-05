namespace ReportingSystem.Utils
{
    public static class Mode
    {
        //режим роботи
        public static string Read()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            configuration.Reload();
            string source = configuration.GetValue<string>("Source:mode");

            if (source.Equals("2"))
            {
                return "sql";
            }
            else
            {
                return "json";
            }

        }
    }
}
