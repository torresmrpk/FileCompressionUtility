using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressionUtility.Services.Interfaces
{
	public interface IConfigurationRepository
	{
		IDictionary<string, string> AppNameValuePairs { get; }

		string GetConfigValue(string appName, string configName, int isEnabled);

		string GetConfigValue(string appName, string configName, int IsEnabled, string profile);

		IDictionary<string, string> GetConfigNameValuePairs(string appName, string configName, int isEnabled);

		IDictionary<string, string> GetConfigNameValuePairs(string appName, string configName, int isEnabled, string profile);

		string GetNewReferenceID(string profile);
	}
}
