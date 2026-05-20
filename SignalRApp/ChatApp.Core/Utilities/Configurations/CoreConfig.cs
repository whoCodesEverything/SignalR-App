using Microsoft.Extensions.Configuration;

namespace ChatApp.Core.Utilities.Configuration
{
    public class CoreConfig
    {
        public static IConfigurationBuilder _configurationBuilder;
        public static IConfiguration _configuration;

        public static IConfiguration GetConfiguration()
        {

            _configurationBuilder = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsetting.json");
            _configuration = _configurationBuilder.Build();
            return _configuration;


        }


        public static string GetValue(string key) => GetConfiguration().GetValue<string>(key);

        public static string GetConnectionString(string connection) => GetConfiguration().GetConnectionString(connection);

        public static IConfigurationSection GetSection(string sectionName)=>GetConfiguration().GetSection(sectionName);



    }
}
