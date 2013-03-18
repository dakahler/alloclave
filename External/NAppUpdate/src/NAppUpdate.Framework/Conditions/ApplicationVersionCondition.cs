using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Sources;
using NAppUpdate.Framework.Tasks;
using System.Xml;
using NAppUpdate.Framework.FeedReaders;
using NAppUpdate.Framework.Conditions;

namespace NAppUpdate
{
	[Serializable]
	public class ApplicationVersionCondition : IUpdateCondition
    {
		private readonly String _applicationVersion;

		public ApplicationVersionCondition(String applicationVersion)
        {
            _applicationVersion = applicationVersion;
            Attributes = new Dictionary<String, String>();
        }
 
        public bool IsMet(IUpdateTask task)
        {
            if (!Attributes.ContainsKey("version"))
                return false;
 
            var localVersion = new Version(_applicationVersion);
            var updateVersion = new Version(Attributes["version"]);
             
            return updateVersion > localVersion;
        }

		public IDictionary<String, String> Attributes { get; private set; }
    }
}