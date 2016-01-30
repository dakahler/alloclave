using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;

namespace Alloclave
{
	[DataContract()]
	public abstract class SymbolLookup
	{
		[DataMember]
		Dictionary<UInt64, String> NameCache
		{
			get;
			set;
		}

		internal static SymbolLookup Instance
		{
			get;
			set;
		}

		static String _SymbolsPath;
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
							Instance = symbolLookupAdapter.CreateExport().Value;
						}
					}
				}
				else
				{
					Instance = null;
					_SymbolsPath = null;
				}
			}
		}

		protected SymbolLookup()
		{
			NameCache = new Dictionary<UInt64, String>();
		}

		internal String Lookup(UInt64 address)
		{
			String name;
			if (!NameCache.TryGetValue(address, out name))
			{
				name = GetName(address);
				NameCache.Add(address, name);
			}

			return name;
		}

		protected abstract String GetName(UInt64 address);
	}
}
