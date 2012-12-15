using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Alloclave
{
	public partial class AddressSpace : UserControl
	{
		bool IsLeftMouseDown;
		bool IsMiddleMouseDown;
		Point LastMouseLocation;
		Point MouseDownLocation;
		Point CurrentMouseLocation;
		const int WheelDelta = 120;
		float MainBitmapOpacity = 1.0f;

		// TODO: This should be exposed in the UI
		UInt64 AddressWidth = 0xFF;

		// TODO: Too inefficient?
		History LastHistory = new History();
		
		List<VisualMemoryChunk> VisualMemoryChunks = new List<VisualMemoryChunk>();

		RichTextBoxPrintCtrl printCtrl = new RichTextBoxPrintCtrl();

		AddressSpaceRenderer Renderer = new AddressSpaceRenderer_GDI();

		VisualMemoryChunk SelectedChunk;

		public event SelectionChangedEventHandler SelectionChanged;

		public void History_Updated(object sender, EventArgs e)
		{
			LastHistory = sender as History;
			Rebuild(ref LastHistory);
		}

		public void Rebuild(ref History history)
		{
			// TODO: Should probably move this to a separate thread and render to a secondary bitmap
			if (Parent == null)
			{
				return;
			}

			VisualMemoryChunks.Clear();
			SortedList<TimeStamp, IPacket> allocations = history.Get(typeof(Allocation));
			SortedList<TimeStamp, IPacket> frees = history.Get(typeof(Free));

			// Combine allocation and free lists
			IEnumerable<KeyValuePair<TimeStamp, IPacket>> combinedList = allocations.Union(frees);

			// Create final list, removing allocations as frees are encountered
			// TODO: This will get slower the longer the profile is running
			// Need to come up with a better way
			SortedList<UInt64, IPacket> finalList = new SortedList<UInt64, IPacket>();
			foreach (var pair in combinedList)
			{
				if (pair.Value is Allocation)
				{
					Allocation allocation = pair.Value as Allocation;
					finalList.Add(allocation.Address, pair.Value);
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

			if (finalList.Count == 0)
			{
				return;
			}

			// Start by determining the lowest address
			Allocation startAllocation = (Allocation)finalList.ElementAt(0).Value;
			Allocation endAllocation = (Allocation)finalList.ElementAt(finalList.Count - 1).Value;

			// Align to the beginning of the row
			UInt64 startAddress = startAllocation.Address & ~AddressWidth;
			UInt64 numRows = (endAllocation.Address - startAddress) / AddressWidth;

			// Now build up the actual polygons
			foreach (var pair in finalList)
			{
				Allocation allocation = pair.Value as Allocation;
				VisualMemoryChunk chunk = new VisualMemoryChunk(allocation, startAllocation.Address, AddressWidth, Width);

				VisualMemoryChunks.Add(chunk);
			}

			SelectedChunk = null;

			Renderer.WorldSize = new Size(this.Width, (int)numRows * 2);
			Renderer.Size = this.Size;
			Renderer.Blocks = VisualMemoryChunks;
			Renderer.SelectedBlock = SelectedChunk;

			Renderer.Update();
			Refresh();
		}

		public AddressSpace()
		{
			InitializeComponent();
			this.DoubleBuffered = true;

			Tooltip.RtbPCtrl = printCtrl;

			this.MouseWheel += AddressSpace_MouseWheel;

			// Set the control style to double buffer.
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			// TODO: Still get tearing here, possibly due to no vsync
			// May have to switch to DirectX to fix this
			IntPtr hdc = e.Graphics.GetHdc();
			Renderer.Blit(hdc);
			e.Graphics.ReleaseHdc(hdc);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			return;
		}

		private void AddressSpace_MouseMove(object sender, MouseEventArgs e)
		{
			Renderer.CurrentMouseLocation = e.Location;
			CurrentMouseLocation = e.Location;

			if (IsLeftMouseDown || IsMiddleMouseDown)
			{
				Point mouseDelta = Point.Subtract(e.Location, new Size(LastMouseLocation));
				LastMouseLocation = e.Location;

				Point[] points = { mouseDelta };

				Matrix InvertedTransform = Renderer.ViewMatrix.Clone();
				InvertedTransform.Invert();
				InvertedTransform.TransformVectors(points);

				Renderer.ViewMatrix.Translate(points[0].X, points[0].Y);
			}

			Renderer.Update();
			Refresh();
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
					SelectAt();
				}
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = false;
				if (MouseDownLocation == e.Location)
				{
					SelectAt();
				}
			}
		}

		void AddressSpace_MouseWheel(object sender, MouseEventArgs e)
		{
			// HACK
			Renderer.ViewMatrix = Renderer.ViewMatrix;

			Point[] pointBefore = { CurrentMouseLocation };
			Matrix InvertedTransform = Renderer.ViewMatrix.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformPoints(pointBefore);

			int amountToMove = e.Delta / WheelDelta;
			float finalScale = 1.0f + (float)amountToMove / 5.0f;
			Renderer.ViewMatrix.Scale(finalScale, finalScale);

			Point[] pointAfter = { CurrentMouseLocation };
			InvertedTransform = Renderer.ViewMatrix.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformPoints(pointAfter);

			Point delta = Point.Subtract(pointAfter[0], new Size(pointBefore[0]));

			Renderer.ViewMatrix.Translate(delta.X, delta.Y);

			Renderer.Update();
			Refresh();
		}

		void SelectAt()
		{
			SelectedChunk = null;
			Point localLocation = Renderer.GetLocalMouseLocation();
			foreach (VisualMemoryChunk chunk in VisualMemoryChunks)
			{
				if (chunk.Contains(localLocation))
				{
					SelectedChunk = chunk;
					break;
				}
			}

			SelectionChangedEventArgs e = new SelectionChangedEventArgs();
			e.SelectedChunk = SelectedChunk;
			SelectionChanged(this, e);

			Renderer.Update();
			Refresh();
		}

		void HoverAt(Point location)
		{
			printCtrl.Text = "TESSSSST";
			Tooltip.Show("", this);
		}

		private void AddressSpace_SizeChanged(object sender, EventArgs e)
		{
			// TODO: Might not be viable to do this dynamically for large datasets
			Rebuild(ref LastHistory);
		}

		private void AddressSpace_MouseHover(object sender, EventArgs e)
		{
			HoverAt(CurrentMouseLocation);
		}
	}

	public class SelectionChangedEventArgs : EventArgs
	{
		public VisualMemoryChunk SelectedChunk;
	}

	public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
}
