using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressionUtility.Services.Interfaces
{
	public interface IOptions
	{
		string Profile { get; set; }

        string DirectoriesWithFilesToCompress { get; set; }

        string GetUsage();
	}
}
