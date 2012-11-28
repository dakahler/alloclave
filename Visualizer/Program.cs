using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace Alloclave
{
	public interface ITransportName
	{
		String Name { get; }
	}

	class Program
	{
		private static CompositionContainer _container;

		public static IEnumerable<ExportFactory<Transport, ITransportName>> TransportAdapters;

		[ImportMany]
		private IEnumerable<ExportFactory<Transport, ITransportName>> InternalTransportAdapters;

		private static void ImportPlugins()
		{
			// Figure out proper plugins location
			String executablePath = Application.StartupPath;
			String pluginsPath = Path.Combine(executablePath, "plugins");
			if (!Directory.Exists(pluginsPath))
			{
				pluginsPath = Directory.GetParent(executablePath).Parent.FullName;
				pluginsPath = Path.Combine(pluginsPath, "plugins");
			}

			// Plugins path must exist by this point
			String transportPluginsPath = Path.Combine(pluginsPath, "transport");
			String callstackPluginsPath = Path.Combine(pluginsPath, "callstack");
			String userdataPluginsPath = Path.Combine(pluginsPath, "userdata");
			if (!Directory.Exists(pluginsPath) || !Directory.Exists(transportPluginsPath) ||
				!Directory.Exists(callstackPluginsPath) || !Directory.Exists(userdataPluginsPath))
			{
				// TODO: Better messaging
				MessageBox.Show("Plugins not found! Please reinstall Alloclave.");
				Application.Exit();
			}

			// An aggregate catalog that combines multiple catalogs
			var catalog = new AggregateCatalog();
			// Adds all the parts found in the same assembly as the Program class
			catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
			catalog.Catalogs.Add(new DirectoryCatalog(transportPluginsPath));


			// Create the CompositionContainer with the parts in the catalog
			_container = new CompositionContainer(catalog);

			// Fill the imports of this object
			try
			{
				Program program = new Program();
				_container.ComposeParts(program);
				TransportAdapters = program.InternalTransportAdapters;
			}
			catch (CompositionException compositionException)
			{
				Console.WriteLine(compositionException.ToString());
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if (!System.Diagnostics.Debugger.IsAttached)
			{
				AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
				Application.ThreadException += NBug.Handler.ThreadException;

				NBug.Settings.StoragePath = NBug.Enums.StoragePath.CurrentDirectory;
				NBug.Settings.UIMode = NBug.Enums.UIMode.Full;
				//NBug.Settings.Destination1 = "Type=Mail;From=me@mycompany.com;To=bugtracker@mycompany.com;SmtpServer=smtp.mycompany.com;";
			}

			ImportPlugins();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Main());
		}
	}
}
