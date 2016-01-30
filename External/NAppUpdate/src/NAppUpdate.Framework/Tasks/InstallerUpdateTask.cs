using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Sources;
using NAppUpdate.Framework.Tasks;

namespace NAppUpdate
{
	[Serializable]
	[UpdateTaskAlias("InstallerUpdate")]
	public class InstallerUpdateTask : UpdateTaskBase
	{
		private string _installerDownloadPath;

		public bool DataOnly;

		[NauField("localPath", "The local path of the installer to execute", true)]
		public string LocalPath { get; set; }

		[NauField("updateTo",
			"File name on the remote location; same name as local path will be used if left blank"
			, false)]
		public string UpdateTo { get; set; }

		public override void Prepare(IUpdateSource source)
		{
			if (DataOnly)
			{
				return;
			}

			if (string.IsNullOrEmpty(LocalPath))
			{
				UpdateManager.Instance.Logger.Log(Logger.SeverityLevel.Warning,
												  "InstallerUpdateTask: LocalPath is empty.");
				// Errorneous case, but there's nothing to prepare to, and by default we prefer a noop over an error
				return;
			}

			// NAppUpdate needs the assembly for this type in order to deserialize it when running cold updates
			//var assemblyFile = new FileInfo(GetType().Assembly.Location);
			//UpdateManager.Instance.Config.DependenciesForColdUpdate = new List<string> { assemblyFile.Name };
			UpdateManager.Instance.Config.UpdateExecutableName = LocalPath;
			UpdateManager.Instance.Config.UpdateProcessName = LocalPath;

			string fileName;
			if (!string.IsNullOrEmpty(UpdateTo))
			{
				fileName = UpdateTo;
			}
			else
			{
				fileName = LocalPath;
			}

			_installerDownloadPath = null;
			string tempFileLocal = Path.Combine(UpdateManager.Instance.Config.TempFolder, LocalPath);
			if (!source.GetData(fileName, string.Empty, OnProgress, ref tempFileLocal))
			{
				throw new UpdateProcessFailedException("InstallerUpdateTask: Failed to get file from source");
			}

			_installerDownloadPath = tempFileLocal;
			if (_installerDownloadPath == null)
			{
				throw new UpdateProcessFailedException("InstallerUpdateTask: Failed to get file from source");
			}

			UpdateManager.Instance.Logger.Log("InstallerUpdateTask: Prepared successfully; installer file: {0}",
											  _installerDownloadPath);
		}

		public override TaskExecutionStatus Execute(bool coldRun)
		{
			if (DataOnly)
			{
				return TaskExecutionStatus.Successful;
			}

			if (!coldRun)
			{
				// We won't do anything untill we're running cold
				return TaskExecutionStatus.RequiresPrivilegedAppRestart;
			}

			Process process = Process.Start( _installerDownloadPath);
			if (process == null)
			{
				return TaskExecutionStatus.Failed;
			}

			bool success = process.WaitForExit(60000);
			if (success)
			{
				return TaskExecutionStatus.Successful;
			}
			return TaskExecutionStatus.Failed;
		}

		public override bool Rollback()
		{
			// No rollback supported
			return true;
		}
	}
}