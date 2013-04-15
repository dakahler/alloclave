using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.IO;


namespace Alloclave_Plugin
{
	[Export(typeof(Alloclave.CallStack))]
	[ExportMetadata("Name", "Call Stack PDB")]
	public class CallStack_PDB : Alloclave.CallStack
	{
		static Alloclave.PdbParser pdbParser = new Alloclave.PdbParser();
		static bool IsLoaded;

		private String TempPdbPath;

		public CallStack_PDB()
		{
			LoadSymbols();
		}

		private void LoadSymbols()
		{
			if (!IsLoaded && File.Exists(SymbolsPath))
			{
				// Copy file to temp to avoid locking the real file
				TempPdbPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(SymbolsPath));
				File.Copy(SymbolsPath, TempPdbPath, true);

				IsLoaded = pdbParser.Open(TempPdbPath);
				if (!IsLoaded)
				{
					// TODO: Better exception
					throw new FileNotFoundException();
				}
			}
		}

		public override String TranslateAddress(UInt64 address)
		{
			LoadSymbols();

			if (IsLoaded)
			{
				return pdbParser.GetFunctionName(address);
			}
			else
			{
				return "Unknown";
			}
		}
	}

}
