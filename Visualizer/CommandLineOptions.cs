using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Alloclave
{
	public class CommandLineOptions
	{
		[Option("t", "transport", Required = false, HelpText = "Transport type to select.")]
		public string TransportType { get; set; }

		[Option("p", "process", Required = false, HelpText = "Process name for process spy transport.")]
		public string ProcessName { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			var usage = new StringBuilder();
			usage.AppendLine("Alloclave");
			usage.AppendLine("Launch Alloclave with optional command line arguments.");
			usage.AppendLine("\tAlloclave [-t:transport_type] [-p:process_name]");
			usage.AppendLine("\t-t\tTransport type, such as \"Process Spy\"");
			usage.AppendLine("\t-p\tProcess name, such as \"firefox\"");
			usage.AppendLine("Press enter to continue...");
			return usage.ToString();
		}
	}
}
