using CommandLine;
using CommandLine.Text;
using FileCompressionUtility.Services.Interfaces;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressionUtility.Services.Configuration
{
	[Singleton]
	public class CommandLineOptions : IOptions
	{
		
		[Option('p', "Profile", Required = false,
			HelpText = "This app uses the configuration settings from the Master.AppSettings table (Profile=Default). If you would like to use a different profile use the -p option.",
			DefaultValue = "Default"),]
		public string Profile { get; set; }

		[Option('d', "Directories", Required=false, 
			HelpText = "This is a comma delimited list of directories, which we want all the containing files to be compressed")]
		public string DirectoriesWithFilesToCompress { get;set; }

        [HelpOption('?', "help")]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}

	}
}
