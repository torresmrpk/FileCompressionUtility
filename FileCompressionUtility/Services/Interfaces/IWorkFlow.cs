using FileCompressionUtility.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressionUtility.Services.Interfaces
{
	public interface IWorkFlow
	{
		CommandLineOptions options { get; set; }

		void Run();
	}	
}
