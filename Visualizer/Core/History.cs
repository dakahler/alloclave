using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	public class History
	{
		SortedList<TimeStamp, IPacket> PacketList = new SortedList<TimeStamp, IPacket>();

		DateTime TrialStartTime;
		bool SentTrialWarning;

		// Position tracker
		int Position;

		// TODO: Need to find a better way to synchronize this
		public Object AddLock = new Object();

		// TODO: Might not be able to make these static
		public static event EventHandler Updated;
		public static bool SuspendRebuilding = false;
		private static History _Instance;
		public static History Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new History();
				}

				return _Instance;
			}
		}

		public class Range
		{
			public UInt64 Min;
			public UInt64 Max;

			public Range()
			{
				Min = UInt64.MaxValue;
				Max = UInt64.MinValue;
			}
		}

		private Range _AddressRange = new Range();
		public Range AddressRange
		{
			get
			{
				return _AddressRange;
			}
		}

		public Range TimeRange
		{
			get
			{
				Range timeRange = new Range();

				if (PacketList.Count > 0)
				{
					timeRange.Min = PacketList.Keys[0].Time;
					timeRange.Max = PacketList.Keys[PacketList.Count - 1].Time;
				}
				else
				{
					timeRange.Min = 0;
					timeRange.Max = 0;
				}

				return timeRange;
			}
		}

		private History()
		{
			//this.Add(new Allocation(), 0);
			//this.Add(new Allocation(), 1);
			//this.Add(new Allocation(), 2);
			//this.Add(new Allocation(), 3);
			//this.Add(new Allocation(), 4);
			//this.Add(new Allocation(), 5);
			//this.Add(new Allocation(), 6);
			//this.Add(new Allocation(), 7);
			//this.Add(new Allocation(), 8);
			//this.Add(new Allocation(), 9);
			//this.Add(new Allocation(), 10);

			//var result = GetForward(new TimeStamp(5));
			//result = GetBackward(new TimeStamp(2));
			//result = GetForward(new TimeStamp(10));
		}

		public void Reset()
		{
			PacketList.Clear();
			Position = 0;
			_AddressRange = new Range();
			Updated = null;
		}

		public void Add(IPacket packet, UInt64 timeStamp)
		{
			//Task task = new Task(() =>
			{
				lock (AddLock)
				{
					// Trial limitation: Only allow 1 minute of data
					if (Licensing.IsTrial)
					{
						if (PacketList.Count > 0)
						{
							TimeSpan span = DateTime.Now.Subtract(TrialStartTime);
							const double trialTimeLimit = 60.0;
							if (span.TotalSeconds > trialTimeLimit)
							{
								if (!SentTrialWarning)
								{
									SentTrialWarning = true;
									MessagesForm.Add(MessagesForm.MessageType.Warning, null, "Trial version can only collect one minute of data.");
								}

								return;
							}
						}
						else
						{
							TrialStartTime = DateTime.Now;
						}
					}

					lock (PacketList)
					{
						PacketList.Add(new TimeStamp(timeStamp), packet);

						Allocation allocation = packet as Allocation;
						if (allocation != null)
						{
							_AddressRange.Min = Math.Min(_AddressRange.Min, allocation.Address);
							_AddressRange.Max = Math.Max(_AddressRange.Max, allocation.Address);
						}
					}

					if (Updated != null && !SuspendRebuilding)
					{
						EventArgs e = new EventArgs();
						Updated.Invoke(this, e);
					}
				}
			}
			//});

			//task.Start();
		}

		public IEnumerable<KeyValuePair<TimeStamp, IPacket>> Get()
		{
			return PacketList.AsEnumerable();
		}

		public IEnumerable<KeyValuePair<TimeStamp, IPacket>> GetForward(TimeStamp timeStamp)
		{
			lock (PacketList)
			{
				var finalList = PacketList.Skip(Position + 1).TakeWhile(p => p.Key <= timeStamp);
				Position = PacketList.IndexOfValue(finalList.LastOrDefault().Value);

				return finalList;
			}
		}

		public IEnumerable<KeyValuePair<TimeStamp, IPacket>> GetBackward(TimeStamp timeStamp)
		{
			lock (PacketList)
			{
				var finalList = PacketList.Take(Position + 1).SkipWhile(p => p.Key <= timeStamp).Reverse();
				Position = PacketList.IndexOfValue(finalList.LastOrDefault().Value) - 1;

				return finalList;
			}
		}

		public static void ForceRebuild()
		{
			EventArgs e = new EventArgs();
			Updated.Invoke(Instance, e);
		}
	}
}
