using FileCompressionUtility.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressionUtility.Services.Configuration
{
    public class ConfigurationWrapper : IConfigurationRepository
    {
        public IDictionary<string, string> AppNameValuePairs { get; private set; } = new Dictionary<string, string>();

        public ConfigurationWrapper(IOptions options, IMessageLogger log)
        {
            string directoriesWithFilesToCompress = options.DirectoriesWithFilesToCompress ?? string.Empty;
            AppNameValuePairs.Add("DirectoryList", directoriesWithFilesToCompress);
        }

        public IDictionary<string, string> GetConfigNameValuePairs(string appName, string configName, int isEnabled)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetConfigNameValuePairs(string appName, string configName, int isEnabled, string profile)
        {
            throw new NotImplementedException();
        }

        public string GetConfigValue(string appName, string configName, int isEnabled)
        {
            throw new NotImplementedException();
        }

        public string GetConfigValue(string appName, string configName, int IsEnabled, string profile)
        {
            throw new NotImplementedException();
        }

        public string GetNewReferenceID(string profile)
        {
            throw new NotImplementedException();
        }
    }
}
