﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	[DataContract()]
	internal class Diff
	{
		[DataMember]
		Snapshot Left;

		[DataMember]
		Snapshot Right;

		public Snapshot Difference { get; private set; }

		public Diff()
		{

		}

		public void SetLeft(Snapshot left)
		{
			Left = new Snapshot(left);
		}

		public void SetRight(Snapshot right)
		{
			Right = new Snapshot(right);

			Debug.Assert(Left != null);
			CalculateDiff();
		}

		public void CalculateDiff()
		{
			Difference = Right - Left;
		}
	}
}
