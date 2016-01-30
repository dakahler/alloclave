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
	public class InstallerAwareAppcastReader : IUpdateFeedReader
	{
		String Description;
		String Version;
		public String ApplicationVersion;

		public InstallerAwareAppcastReader(String applicationVersion)
		{
			ApplicationVersion = applicationVersion;
		}

		public IList<IUpdateTask> Read(string feed)
		{
			var doc = new XmlDocument();
			doc.LoadXml(feed);
			XmlNodeList nodeList = doc.SelectNodes("/rss/channel/item");

			var updateTasks = new List<IUpdateTask>();
			bool foundOneVersion = false;
			foreach (XmlNode node in nodeList)
			{
				var task = new InstallerUpdateTask();
				task.Description = node["description"].InnerText.Trim('\n').Replace("\n", "\r\n");
				Description = task.Description;

				string url = node["enclosure"].Attributes["url"].Value;
				task.UpdateTo = url;

				Version = node["appcast:version"].InnerText;

				if (".exe".Equals(Path.GetExtension(url)) || ".msi".Equals(Path.GetExtension(url)))
				{
					string fileName = Path.GetFileName(url);
					task.LocalPath = fileName;
					//string baseDirectory = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
					//UpdateManager.Instance.ApplicationPath = Path.Combine(baseDirectory, fileName);

					var condition = new ApplicationVersionCondition(ApplicationVersion);
					condition.Attributes.Add("version", Version);
					task.UpdateConditions.AddCondition(condition, BooleanCondition.ConditionType.AND);
					task.UpdateConditions.Attributes.Add("version", Version);
				}
				else
				{
					var condition = new FileVersionCondition();
					condition.Version = Version;
					task.UpdateConditions.AddCondition(condition, BooleanCondition.ConditionType.AND);
					task.UpdateConditions.Attributes.Add("version", Version);
				}

				if (foundOneVersion)
				{
					task.DataOnly = true;
				}

				foundOneVersion = true;

				updateTasks.Add(task);
			}

			return updateTasks;
		}

	}
}