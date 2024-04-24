using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressionUtility.Services.Interfaces
{
	public interface IMessageLogger
	{
		void LogInformation(string message);

		void LogError(string message);

		void LogWarning(string message);
	}
}
