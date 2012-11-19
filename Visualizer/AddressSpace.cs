using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Alloclave
{
	public partial class AddressSpace : UserControl
	{
		Bitmap MainBitmap;
		VisualConstraints VisualConstraints = new VisualConstraints();
		Matrix GlobalTransform = new Matrix();
		bool IsLeftMouseDown;
		bool IsMiddleMouseDown;
		Point LastMouseLocation;
		Point MouseDownLocation;
		const int WheelDelta = 120;

		

		List<VisualMemoryChunk> VisualMemoryChunks = new List<VisualMemoryChunk>();

		public void History_Updated(object sender, EventArgs e)
		{
			History history = sender as History;
			Rebuild(ref history);
		}

		public void Rebuild(ref History history)
		{
			VisualMemoryChunks.Clear();
			SortedList<TimeStamp, IPacket> allocations = history.Get(typeof(Allocation));
			SortedList<TimeStamp, IPacket> frees = history.Get(typeof(Free));

			// Combine allocation and free lists
			IEnumerable<KeyValuePair<TimeStamp, IPacket>> combinedList = allocations.Union(frees);

			// Create final list, removing allocations as frees are encountered
			SortedList<TimeStamp, IPacket> finalList = new SortedList<TimeStamp, IPacket>();
			foreach (var pair in combinedList)
			{
				if (pair.Value is Allocation)
				{
					finalList.Add(pair.Key, pair.Value);
				}
				else
				{
					int index = finalList.IndexOfValue(pair.Value);
					if (index != -1)
					{
						finalList.RemoveAt(index);
					}
					else
					{
						// This indicates a memory problem on the target side
						// TODO: User-facing error reporting
						throw new InvalidConstraintException();
					}
				}
			}

			// Start by determining the lowest address
			VisualConstraints.StartAddress = UInt64.MaxValue;
			foreach (var pair in combinedList)
			{
				Allocation allocation = pair.Value as Allocation;
				VisualConstraints.StartAddress = Math.Min(VisualConstraints.StartAddress, allocation.Address);
			}
			VisualConstraints.StartAddress = VisualConstraints.StartAddress & ~VisualConstraints.RowAddressWidth;

			// Then actually build the visual data
			foreach (var pair in combinedList)
			{
				Allocation allocation = pair.Value as Allocation;
				VisualMemoryChunk chunk = new VisualMemoryChunk(allocation.Address,
					allocation.Size, VisualConstraints);

				VisualMemoryChunks.Add(chunk);
			}

			Invalidate();
		}

		public AddressSpace()
		{
			InitializeComponent();
			this.DoubleBuffered = true;

			MainBitmap = new Bitmap(this.Width, this.Height);
			this.MouseWheel += AddressSpace_MouseWheel;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics gForm = e.Graphics;
			gForm.Clear(Color.White);

			Graphics g = Graphics.FromImage(MainBitmap);
			g.Clear(Color.White);

			foreach (VisualMemoryChunk chunk in VisualMemoryChunks)
			{
				foreach (VisualMemoryBox box in chunk.Boxes)
				{
					Rectangle rectangle = box.DefaultBox;
					Region region = new Region(rectangle);
					region.Transform(box.Transform);
					region.Transform(GlobalTransform);
					
					SolidBrush brush = new SolidBrush(Color.Red);
					g.FillRegion(brush, region);
				}
			}

			gForm.DrawImage(MainBitmap, 0, 0, MainBitmap.Width, MainBitmap.Height);

			g.Dispose();

			base.OnPaint(e);
		}

		private void AddressSpace_MouseMove(object sender, MouseEventArgs e)
		{
			if (IsLeftMouseDown || IsMiddleMouseDown)
			{
				Point mouseDelta = Point.Subtract(e.Location, new Size(LastMouseLocation));
				LastMouseLocation = e.Location;

				GlobalTransform.Translate(mouseDelta.X, mouseDelta.Y);
				Invalidate();
			}
		}

		private void AddressSpace_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = true;
				LastMouseLocation = e.Location;
				MouseDownLocation = e.Location;
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = true;
				LastMouseLocation = e.Location;
				MouseDownLocation = e.Location;
			}
		}

		private void AddressSpace_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = false;
				if (MouseDownLocation == e.Location)
				{
					SelectAt(MouseDownLocation);
				}
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = false;
				if (MouseDownLocation == e.Location)
				{
					SelectAt(MouseDownLocation);
				}
			}
		}

		void AddressSpace_MouseWheel(object sender, MouseEventArgs e)
		{

			int amountToMove = e.Delta / WheelDelta;
			float finalScale = 1.0f + (float)amountToMove / 5.0f;
			GlobalTransform.Scale(finalScale, finalScale);
			Invalidate();
		}

		void SelectAt(Point location)
		{
			// TODO
		}
	}
}
