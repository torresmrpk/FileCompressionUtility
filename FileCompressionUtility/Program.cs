using FileCompressionUtility.Services;
using FileCompressionUtility.Services.Configuration;
using FileCompressionUtility.Services.Interfaces;
using StructureMap;
using System;

namespace FileCompressionUtility
{
	public class Program
	{

		private const int ERROR_BAD_ARGUMENTS = 0xA0;
		private const int ERROR_ARITHMETIC_OVERFLOW = 0x216;
		private const int ERROR_INVALID_COMMAND_LINE = 0x667;

        /// <summary>
        /// Mauro Torres	2024/04/24
        /// 
        /// I wrote this program to save disk space. You pass it a comma delimited list of directories and it just compresses each file individually. 
        /// This can then be called from a scheduled job. This is meant for a team that works with a lot of files that need to be compressed. 
		/// 
        /// I first had the configuration of directories configured in a table named Master.Appsettings, which is what the ConfigurationRepository is for, 
        /// but to make things simple I now call the class "ConfigurationWrapper" which just uses the -d command line option passed to it. 
		/// 
        /// The purpose of using the library StructureMap was for me to learn dependency injection. 
		/// 
		/// Example Usage:
		/// FileCompressionUtility.exe -d "C:\Users\torre\source\CompressFiles"
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
		{
			IMessageLogger log = null;
			try
			{				
				//	Dependency Injection/IOC configuration.
				Container ctr = ConfigureServices();
				log = ctr.GetInstance<IMessageLogger>();

				log.LogInformation("Program Start");

				//	Parse Command Line Arguments
				IOptions options = ctr.GetInstance<IOptions>();
				if (!CommandLine.Parser.Default.ParseArguments(args, options))
				{
					log.LogInformation($"Invalid Command Line Arguments.");
					Environment.ExitCode = ERROR_INVALID_COMMAND_LINE;
					return;
				}

                //	Start WorkFlow
                //	IWorkFlow was automatically mapped to WorkFlow using "Automatic Mapping" in the container.
                //	By calling it this way we can have objects in the container automatically injected in the constructor for us.
                //	public WorkFlow(IConfigurationRepository configRepository, IMessageLogger log, IOptions options)
                ctr.GetInstance<IWorkFlow>().Run();
				log.LogInformation("Program End");

			}
			catch (Exception ex)
			{
				log.LogError($"Program Level Error: {ex.Message}");
				// A 32-bit signed integer containing the exit code. The default value is 0 (zero)
				// which indicates that the process completed successfully.
				Environment.ExitCode = 1;
			}
			finally
			{
				Environment.Exit(Environment.ExitCode);
			}
		}

		/// <summary>
		/// This uses the StructureMap Depedency Injection class.
		/// http://structuremap.github.io/quickstart/
		/// 
		///	Transient -- The default lifecycle. A new object is created for each logical request to resolve an object graph from the container.
		///	Singleton -- Only one object instance will be created for the container and any children or nested containers created by that container
		///	ContainerScoped -- Only one object instance will be created for unique root, child, or nested container
		///	AlwaysUnique -- A new object instance is created every time, even within the same object graph
		///	ThreadLocal -- Only one object instance will be created for the currently executing Thread
		/// 		
		/// </summary>
		/// <returns></returns>
		private static Container ConfigureServices()
		{
			Container ctr = new Container(config =>
			{
				//	Automatic Mapping: Maps Classes to Interfaces, which have similar names. 
				config.Scan
					(x =>
					{
						x.TheCallingAssembly();
						x.WithDefaultConventions();
					});

				//	Manual Mappings				
				config.For<IMessageLogger>().Use<TraceLogger>().Singleton();
				config.For<IOptions>().Use<CommandLineOptions>().Singleton();

                //ConfigurationRepository would get the configuration from a database.
                //ConfigurationWrapper gets  the configuration from the command line. Its just a wrapper to make depedency injection work.
                config.For<IConfigurationRepository>().Use<ConfigurationWrapper>().Singleton();
			});

			return ctr;
		}
	}
}
