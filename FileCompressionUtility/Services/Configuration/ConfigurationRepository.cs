using FileCompressionUtility.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace FileCompressionUtility.Services.Configuration
{
	/// <summary>
	/// This was used to pull the configuration from a table in a database. 
	/// </summary>
	public class ConfigurationRepository : IConfigurationRepository
	{
		private IMessageLogger _log;
		private IOptions _options;
		private string _connString;

		public IDictionary<string, string> AppNameValuePairs { get; private set; }

		public ConfigurationRepository(IOptions options, IMessageLogger log)
		{
			_connString = ConfigurationManager.ConnectionStrings["ConfigDatabase"].ConnectionString;
			_log = log;
			_options = options;

			RefreshAppNameValuePairs();
		}

		private void RefreshAppNameValuePairs()
		{
			IDictionary<string, string> appNameValuePairs = GetConfigNameValuePairs(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, "All", 1, _options.Profile);
			AppNameValuePairs = appNameValuePairs;
			if (AppNameValuePairs.Count == 0)
			{
				string error = "Error: Invalid Profile. Profile returned 0 configuration values.";
				_log.LogError(error);
				throw new Exception(error);
			}
		}

		public string GetConfigValue(string appName, string configName, int isEnabled)
			=> GetConfigValue(appName, configName, isEnabled, "Default");

		public string GetConfigValue(string appName, string configName, int isEnabled, string profile)
		{
			SqlConnection conn;
			string ret = string.Empty;
			using (conn = new SqlConnection(_connString))
			{
				using (var cmd = new SqlCommand("Master.GetAppSettings", conn))
				{
					conn.Open();

					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@AppName", SqlDbType.VarChar).Value = appName;
					cmd.Parameters.Add("@ConfigName", SqlDbType.VarChar).Value = configName;
					cmd.Parameters.Add("@IsEnabled", SqlDbType.VarChar).Value = isEnabled;
					cmd.Parameters.Add("@Profile", SqlDbType.VarChar).Value = profile;

					SqlDataReader result = cmd.ExecuteReader();

					if (result.HasRows)
					{
						while (result.Read())
						{
							ret = result["ConfigValue"].ToString();
						}
					}
				}
			}

			return ret;
		}

		public IDictionary<string, string> GetConfigNameValuePairs(string appName, string configName, int isEnabled)
			=> GetConfigNameValuePairs(appName, configName, isEnabled, "Default");

		public IDictionary<string, string> GetConfigNameValuePairs(string appName, string configName, int isEnabled, string profile)
		{
			IDictionary<string, string> nameValues = new Dictionary<string, string>();

			SqlConnection conn;
			try
			{
				using (conn = new SqlConnection(_connString))
				{
					using (var cmd = new SqlCommand("Master.GetAppSettings", conn))
					{
						conn.Open();

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.Add("@AppName", SqlDbType.VarChar).Value = appName;
						cmd.Parameters.Add("@ConfigName", SqlDbType.VarChar).Value = configName;
						cmd.Parameters.Add("@IsEnabled", SqlDbType.VarChar).Value = isEnabled;
						cmd.Parameters.Add("@Profile", SqlDbType.VarChar).Value = profile;

						SqlDataReader result = cmd.ExecuteReader();

						if (result.HasRows)
						{
							while (result.Read())
							{
								nameValues.Add(result["ConfigName"].ToString(), result["ConfigValue"]?.ToString() ?? String.Empty);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				_log.LogInformation($"Error: {e.Message}");
			}

			return nameValues;
		}

		public string GetNewReferenceID(string profile)
		{
			SqlConnection conn;
			string ret = string.Empty;
			using (conn = new SqlConnection(_connString))
			{
				using (var cmd = new SqlCommand("Master.GetNewReferenceID", conn))
				{
					conn.Open();

					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ConfigProfile", SqlDbType.VarChar).Value = _options.Profile;

					SqlDataReader result = cmd.ExecuteReader();

					if (result.HasRows)
					{
						while (result.Read())
						{
							ret = result["ConfigValue"].ToString();
						}
					}
				}
			}

			return ret;
		}
	}
}
