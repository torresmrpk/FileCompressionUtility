using FileCompressionUtility.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompressionUtility.Services
{
	public class TraceLogger : IMessageLogger
	{

		/// <summary>
		/// Logs informational messages
		/// </summary>
		/// <param name="message">Message to log</param>
		public void LogInformation(string message)
		{
			try
			{
				Trace.TraceInformation(DateTime.Now + ": " + message);
			}
			catch
			{
				// don't let tracing disrupt workflow
			}
		}

		/// <summary>
		/// Logs error messages
		/// </summary>
		/// <param name="message">Message to log</param>
		public void LogError(string message)
		{
			try
			{
				Trace.TraceError(DateTime.Now + ": " + message);
			}
			catch
			{
				// don't let tracing disrupt workflow
			}
		}

		/// <summary>
		/// Logs Warnings
		/// </summary>
		/// <param name="message"></param>
		public void LogWarning(string message)
		{
			try
			{
				Trace.TraceWarning(DateTime.Now + ": " + message);
			}
			catch
			{
				// don't let tracing disrupt workflow
			}
		}
	}
}
