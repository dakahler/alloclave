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

		public CallStack_PDB()
		{
			LoadSymbols();
		}

		private void LoadSymbols()
		{
			if (!IsLoaded && File.Exists(SymbolPath))
			{
				IsLoaded = pdbParser.Open(SymbolPath);
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
