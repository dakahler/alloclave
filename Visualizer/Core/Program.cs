using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Runtime.InteropServices;
using NAppUpdate.Framework;
using NAppUpdate;
using System.Threading.Tasks;

namespace Alloclave
{
	public interface ITransportName
	{
		String Name { get; }
	}

	public interface ISymbolLookupExtension
	{
		String Extension { get; }
	}

	class Program
	{
		private static CompositionContainer _container;

		public static IEnumerable<ExportFactory<Transport, ITransportName>> TransportAdapters;
		public static IEnumerable<ExportFactory<SymbolLookup, ISymbolLookupExtension>> SymbolLookupAdapters;
		
		[ImportMany]
		private IEnumerable<ExportFactory<Transport, ITransportName>> InternalTransportAdapters = null;

		[ImportMany]
		private IEnumerable<ExportFactory<SymbolLookup, ISymbolLookupExtension>> InternalSymbolLookupAdapters = null;

		[DllImport("kernel32.dll")]
		static extern bool AttachConsole(int dwProcessId);

		[DllImport("kernel32.dll")]
		static extern bool FreeConsole();

		private const int ATTACH_PARENT_PROCESS = -1;

		private static void ImportPlugins()
		{
			// Figure out proper plugins location
			String executablePath = Application.StartupPath;
			String pluginsPath = Path.Combine(executablePath, "plugins");
			if (!Directory.Exists(pluginsPath))
			{
				pluginsPath = Directory.GetParent(executablePath).Parent.Parent.FullName;
				pluginsPath = Path.Combine(pluginsPath, "plugins");
			}

			// Plugins path must exist by this point
			String transportPluginsPath = Path.Combine(pluginsPath, "transport");
			String symbolLookupPluginsPath = Path.Combine(pluginsPath, "symbollookup");
			if (!Directory.Exists(pluginsPath) || !Directory.Exists(transportPluginsPath) ||
				!Directory.Exists(symbolLookupPluginsPath))
			{
				MessageBox.Show("Plugins not found! Please reinstall Alloclave.");
				Application.Exit();
			}

			// An aggregate catalog that combines multiple catalogs
			var catalog = new AggregateCatalog();
			// Adds all the parts found in the same assembly as the Program class
			catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
			catalog.Catalogs.Add(new DirectoryCatalog(transportPluginsPath));
			catalog.Catalogs.Add(new DirectoryCatalog(symbolLookupPluginsPath));


			// Create the CompositionContainer with the parts in the catalog
			_container = new CompositionContainer(catalog);

			// Fill the imports of this object
			try
			{
				Program program = new Program();
				_container.ComposeParts(program);
				TransportAdapters = program.InternalTransportAdapters;
				SymbolLookupAdapters = program.InternalSymbolLookupAdapters;
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
			bool is45 = Type.GetType("System.Reflection.ReflectionContext", false) != null;
			if (!is45)
			{
				FrameworkMessage message = new FrameworkMessage();
				message.ShowDialog();
				return;
			}

			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
			AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
			Application.ThreadException += NBug.Handler.ThreadException;
			TaskScheduler.UnobservedTaskException += NBug.Handler.UnobservedTaskException;

			ImportPlugins();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Licensing licensing = new Licensing();
			if (!licensing.Disposing && !licensing.IsDisposed)
			{
				Application.Run(licensing);
			}

			if (Licensing.CurrentLicenseStatus != Licensing.LicenseStatus.Invalid)
			{
				AttachConsole(ATTACH_PARENT_PROCESS);

				Main main = new Main();
				if (!main.Disposing && !main.IsDisposed)
				{
					UpdateManager.Instance.UpdateFeedReader = new InstallerAwareAppcastReader(Application.ProductVersion);
					UpdateManager.Instance.UpdateSource = new NAppUpdate.Framework.Sources.SimpleWebSource(
						Common.CompanyWebsiteUrl + "/alloclave/updatefeed.xml");
					UpdateManager.Instance.ReinstateIfRestarted();

					Application.Run(main);
				}

				FreeConsole();
			}
		}
	}
}
