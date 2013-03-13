using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;


namespace Alloclave_Plugin
{
	[Export(typeof(Alloclave.CallStack))]
	[ExportMetadata("Name", "Call Stack PDB")]
	public class CallStack_PDB : Alloclave.CallStack
	{
		Alloclave.PdbParser pdbParser = new Alloclave.PdbParser();

		public CallStack_PDB()
		{
			// TODO: Temp location
			// Need to find a way to only read this in once across plugin instances
			pdbParser.Open(@"C:\dev\Alloclave\Debug_Collector\TestCollector.pdb");
		}

		protected override String TranslateAddress(UInt64 address)
		{
			return pdbParser.GetFunctionName(address);
		}
	}

}
