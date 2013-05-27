using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;

namespace Alloclave
{
	public abstract class SymbolLookup
	{
		private static SymbolLookup _Instance;
		public static SymbolLookup Instance
		{
			get
			{
				return _Instance;
			}
		}

		private static String _SymbolsPath;
		public static String SymbolsPath
		{
			get
			{
				return _SymbolsPath;
			}
			set
			{
				_SymbolsPath = value;

				if (_SymbolsPath != null && File.Exists(_SymbolsPath))
				{
					foreach (ExportFactory<SymbolLookup, ISymbolLookupExtension> symbolLookupAdapter in Program.SymbolLookupAdapters)
					{
						String extension = symbolLookupAdapter.Metadata.Extension;
						if (String.Compare(Path.GetExtension(_SymbolsPath), "." + extension, true) == 0)
						{
							_Instance = symbolLookupAdapter.CreateExport().Value;
						}
					}
				}
				else
				{
					_Instance = null;
					_SymbolsPath = null;
				}
			}
		}

		public abstract String GetName(UInt64 address);
	}
}
