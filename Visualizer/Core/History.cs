using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Alloclave
{
	[DataContract()]
	[KnownType(typeof(Allocation))]
	[KnownType(typeof(Free))]
	public sealed class History
	{
		[DataMember]
		SortedList<TimeStamp, IPacket> PacketList = new SortedList<TimeStamp, IPacket>();

		DateTime TrialStartTime;
		bool SentTrialWarning;

		// Position tracker
		[DataMember]
		int Position = -1;

		// TODO: Need to find a better way to synchronize this
		[DataMember]
		Object AddLock = new Object();

		internal event EventHandler Updated;

		public bool SuspendRebuilding;

		[DataContract()]
		public class Range
		{
			[DataMember]
			public UInt64 Min;

			[DataMember]
			public UInt64 Max;

			public Range()
			{
				Min = UInt64.MaxValue;
				Max = UInt64.MinValue;
			}
		}

		[DataMember]
		Range _AddressRange = new Range();
		internal Range AddressRange
		{
			get
			{
				return _AddressRange;
			}
		}

		internal Range TimeRange
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

		internal bool RebaseBlocks;

        Snapshot _Snapshot;
		internal Snapshot Snapshot
        {
            get
            {
				if (_Snapshot == null)
				{
					_Snapshot = new Snapshot();
				}

                return _Snapshot;
            }
        }

		// TODO: This should be exposed in the UI
		[DataMember]
		const UInt64 AddressWidth = 0xFF;

		[DataMember]
		internal TimeStamp LastTimestamp = new TimeStamp();
		UInt64 LastRange = 0;

		internal event EventHandler Rebuilt;

		[DataMember]
		internal UInt64 ArtificialMaxTime
		{
			get;
			set;
		}

		internal Scrubber Scrubber
		{
			get;
			set;
		}

		public History()
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

		internal History(Snapshot snapshot)
		{
			_Snapshot = snapshot;
		}

		internal void Reset()
		{
			PacketList.Clear();
			Position = -1;
			_AddressRange = new Range();
			Updated = null;
		}

		internal void Add(IPacket packet, UInt64 timeStamp)
		{
			// Trial limitation: Only allow 30 seconds of data
			if (Licensing.IsTrial)
			{
				if (PacketList.Count > 0)
				{
					TimeSpan span = DateTime.Now.Subtract(TrialStartTime);
					const double trialTimeLimit = 30.0;
					if (span.TotalSeconds > trialTimeLimit)
					{
						if (!SentTrialWarning)
						{
							SentTrialWarning = true;
							MessagesForm.Add(MessagesForm.MessageType.Warning, null,
								"Trial version can only collect 30 seconds of data.");
						}

						return;
					}
				}
				else
				{
					TrialStartTime = DateTime.Now;
				}
			}

			lock (AddLock)
			{
				lock (PacketList)
				{
					PacketList.Add(new TimeStamp(timeStamp), packet);

					Allocation allocation = packet as Allocation;
					if (allocation != null)
					{
						UInt64 oldMin = _AddressRange.Min;
						_AddressRange.Min = Math.Min(_AddressRange.Min, allocation.Address);
						_AddressRange.Max = Math.Max(_AddressRange.Max, allocation.Address);

						// Requires a spatial rebuild
						if (oldMin != _AddressRange.Min)
						{
							RebaseBlocks = true;
						}
					}
				}

				if (Updated != null)
				{
					EventArgs e = new EventArgs();
					Updated.Invoke(this, e);
				}

				UpdateSnapshotAsync(Snapshot);
			}
		}

		internal async void UpdateSnapshotAsync(Snapshot snapshot, bool forceFullRebuild = false)
		{
			await Task.Run(() => UpdateSnapshot(snapshot, forceFullRebuild));
		}

		internal void UpdateSnapshot(Snapshot snapshot, bool forceFullRebuild)
		{
            if (SuspendRebuilding)
            {
                return;
            }

			//NBug.Exceptions.Handle(false, () =>
			{
				lock (AddLock)
				{
					// Start by rebasing if necessary
					bool forceUpdate = false;
					if (RebaseBlocks)
					{
						snapshot.Rebase(AddressRange.Min, AddressWidth);
						RebaseBlocks = false;
						forceUpdate = true;
					}

					UInt64 maxTime = ArtificialMaxTime;
					if (maxTime == 0)
					{
						maxTime = TimeRange.Max;
					}

					IEnumerable<KeyValuePair<TimeStamp, IPacket>> packets = null;
					bool isBackward = false;
					if (forceFullRebuild)
					{
						snapshot.Reset();
						packets = Get();
					}
					else
					{
						// Determine what entries we need to get based off scrubber position
						UInt64 timeRange = maxTime - TimeRange.Min;

						// Readjust the position if needed
						if (ArtificialMaxTime == 0 && timeRange > 0)
						{
							double rangeScale = (double)LastRange / (double)timeRange;

							if (rangeScale > 0 && Scrubber._Position < 1.0)
							{
								// Hacky
								Scrubber._Position *= rangeScale;
								Scrubber._Position = Scrubber._Position.Clamp(0.0, 1.0);
								Scrubber.FlagRedraw();
							}
						}

						LastRange = timeRange;

						double position = Scrubber.Position;
						UInt64 currentTime = TimeRange.Min + (UInt64)((double)timeRange * position);

						bool nothingToProcess = false;
						if (currentTime > LastTimestamp.Time)
						{
							packets = GetForward(new TimeStamp(currentTime));
						}
						else if (currentTime < LastTimestamp.Time)
						{
							packets = GetBackward(new TimeStamp(currentTime));
							isBackward = true;
						}
						else
						{
							nothingToProcess = true;
						}

						LastTimestamp = new TimeStamp(currentTime);

						if (nothingToProcess && !forceUpdate)
						{
							return;
						}
					}

					if (AddressRange.Max < AddressRange.Min)
					{
						return;
					}

					// Create final list, removing allocations as frees are encountered
					if (packets != null)
					{
						int index = 0;
						foreach (var pair in packets)
						//Parallel.ForEach(newList, pair =>
						{
							// On a full rebuild, if there's a position specified in
							// the snapshot, only go up to that position
							if (forceFullRebuild && snapshot.Position > 0)
							{
								if (index > snapshot.Position)
								{
									break;
								}
							}

							// TODO: Can allocation and free processing be combined?
							// They should be exact opposites of each other
							if (pair.Value is Allocation)
							{
								Allocation allocation = pair.Value as Allocation;

								if (!isBackward)
								{
									MemoryBlock newBlock = snapshot.Add(
										allocation, AddressRange.Min, AddressWidth);

									if (newBlock == null)
									{
										//MessagesForm.Add(MessagesForm.MessageType.Error, allocation, "Duplicate allocation!");
									}
								}
								else
								{
									snapshot.Remove(allocation.Address);
								}
							}
							else
							{
								Free free = pair.Value as Free;

								if (snapshot.Find(free.Address) != null)
								{
									MemoryBlock removedBlock = snapshot.Remove(free.Address);
									if (removedBlock != null)
									{
										removedBlock.Allocation.AssociatedFree = free;
										free.AssociatedAllocation = removedBlock.Allocation;
									}
									else
									{
										//throw new DataException();
									}
								}
								else
								{
									if (isBackward)
									{
										MemoryBlock newBlock = snapshot.Add(
											free.AssociatedAllocation, AddressRange.Min, AddressWidth);
									}
									else
									{
										//MessagesForm.Add(MessagesForm.MessageType.Error, free.AssociatedAllocation, "Duplicate free!");
									}
								}
							}

							index++;
						} //);

						if (!forceFullRebuild || snapshot.Position == 0)
						{
							snapshot.Position = Position;
						}
					}

                    if (Rebuilt != null)
                    {
                        EventArgs e = new EventArgs();
                        Rebuilt.Invoke(this, e);
                    }
				}
			} //);
		}

		internal IEnumerable<KeyValuePair<TimeStamp, IPacket>> Get()
		{
			return PacketList.AsEnumerable();
		}

		internal IEnumerable<KeyValuePair<TimeStamp, IPacket>> GetForward(TimeStamp timeStamp)
		{
			lock (PacketList)
			{
				var finalList = PacketList.Skip(Position + 1).TakeWhile(p => p.Key <= timeStamp);

				if (finalList.Any())
				{
                    // TODO: IndexOfValue is very expensive
					Position = PacketList.IndexOfValue(finalList.Last().Value);
				}

				return finalList;
			}
		}

		internal IEnumerable<KeyValuePair<TimeStamp, IPacket>> GetBackward(TimeStamp timeStamp)
		{
			lock (PacketList)
			{
				var finalList = PacketList.Take(Position + 1).SkipWhile(p => p.Key <= timeStamp).Reverse();

				if (finalList.Any())
				{
					Position = PacketList.IndexOfValue(finalList.Last().Value) - 1;
				}

				return finalList;
			}
		}

		public void ForceRebuild()
		{
			UpdateSnapshotAsync(Snapshot);
		}

		public void ForceFullSymbolLookup()
		{
			foreach (IPacket packet in PacketList.Values)
			{
				Allocation allocation = packet as Allocation;
				if (allocation != null)
				{
					foreach (var frame in allocation.Stack.Frames)
					{
						// TODO: This shouldn't need to know if there's an instance
						if (SymbolLookup.Instance != null)
						{
							SymbolLookup.Instance.Lookup(frame.Address);
						}
					}
				}
			}
		}
	}
}
