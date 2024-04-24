using FileCompressionUtility.Services.Configuration;
using FileCompressionUtility.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressionUtility.Services
{
	public class WorkFlow : IWorkFlow
	{

		private IConfigurationRepository _configuration;		
		private IMessageLogger _log;
		private IOptions _options;		

		//	properties
		public CommandLineOptions options { get; set; }

		public WorkFlow(IConfigurationRepository configRepository, IMessageLogger log, IOptions options)
		{
			_configuration = configRepository;			
			_log = log;
			_options = options;			
		}

		public void Run()
		{

			string dirList = _configuration.AppNameValuePairs["DirectoryList"];

			if (dirList == string.Empty)
			{
				throw new Exception("No directories specified. Either use the -d option to specify a comma delimited list of directories or configure the program to look them up from a database.");
			}

			string[] dirs = dirList.ToString().Split(',');						

			foreach (var directoryPath in dirs)
			{
				if ( directoryPath.Trim().Length == 0 ) { continue;  }
				_log.LogInformation($"Processing Dir: {directoryPath}");

				try
				{
					DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
					ProcessDir(directorySelected);
				}
				catch (Exception ex)
				{
					_log.LogError($"Unable to process Dir: {directoryPath}, error: {ex.Message}");
					continue;
				}				
			}			
		}

		private void ProcessDir(DirectoryInfo directorySelected)
		{
			var files = directorySelected.GetFiles("*.*").Where(f => !(f.Name.EndsWith(".zip") || f.Name.EndsWith(".gz")));

			for (int i = 0; i < files.Count(); i++)
			{

				string file = string.Empty, zipFile = string.Empty;				

				try
				{
					file = files.ElementAtOrDefault(i).FullName;
					zipFile = $"{file}.zip";

					if (!File.Exists(zipFile) && (file != zipFile))
					{
						_log.LogInformation($"Compresing file: {file}, to Zip file name: {zipFile}");
						using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
						{
							archive.CreateEntryFromFile(file, Path.GetFileName(file));
						}

						if (File.Exists(zipFile))
						{
							File.Delete(file);
						}
					}
				}
				catch (Exception ex)
				{
					_log.LogError($"Unable to process File: {file}, error: {ex.Message}");
					continue;
				}
			}
		}
	}
}
