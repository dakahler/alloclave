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
			usage.AppendLine("Alloclave Visualizer");
			usage.AppendLine("Temp usage instructions...");
			return usage.ToString();
		}
	}
}
