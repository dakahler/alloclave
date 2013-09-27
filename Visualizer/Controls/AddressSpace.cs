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
using System.Diagnostics;
using System.Threading.Tasks;
using OpenTK;
using System.Threading;
using System.Windows;

namespace Alloclave
{
	internal partial class AddressSpace : UserControl
	{
		bool IsLeftMouseDown;
		bool IsMiddleMouseDown;
		Vector LastMouseLocation;
		Vector MouseDownLocation;
		Vector CurrentMouseLocation;
		const int WheelDelta = 120;

		//RichTextBoxPrintCtrl printCtrl = new RichTextBoxPrintCtrl();

		AddressSpaceRenderer Renderer;

		private Object RebuildDataLock = new Object();
		private AutoResetEvent RecalculateSelectedBlock = new AutoResetEvent(false);
		private AutoResetEvent RecalculateHoverBlock = new AutoResetEvent(false);

		CancellationTokenSource TokenSource;

		// TODO: Probably shouldn't be static
		public static event SelectionChangedEventHandler SelectionChanged;

		private bool _IsPaused = false;
		public bool IsPaused
		{
			get
			{
				return _IsPaused;
			}
			set
			{
				_IsPaused = value;

				if (PauseChanged != null)
				{
					PauseChanged(this, new EventArgs());
				}
			}
		}

		public event EventHandler PauseChanged;

		System.Timers.Timer FrameTimer;
		private const double FrameInterval = 30.0;

		private History _History;
		public History History
		{
			get
			{
				return _History;
			}
			set
			{
				Debug.Assert(_History == null);
				_History = value;
				_History.Rebuilt += Snapshot_Rebuilt;
				SizeChanged += new System.EventHandler(AddressSpace_SizeChanged);
			}
		}

		public AddressSpace()
		{
			InitializeComponent();

			Renderer = new AddressSpaceRenderer_OGL(this);

			//Tooltip.RtbPCtrl = printCtrl;

			this.MouseWheel += AddressSpace_MouseWheel;

			ColorPickerDialog.ColorChanged += ColorPickerDialog_ColorChanged;

			TokenSource = new CancellationTokenSource();
			var cancellationToken = TokenSource.Token;

			Task.Factory.StartNew(() => HoverTask(cancellationToken), cancellationToken);
			Task.Factory.StartNew(() => SelectTask(cancellationToken), cancellationToken);

			this.Load += AddressSpace_Load;
		}

		void AddressSpace_Load(object sender, EventArgs e)
		{
			FrameTimer = new System.Timers.Timer(FrameInterval);
			FrameTimer.Elapsed += TimerElapsed;
			FrameTimer.Start();
		}

		void Snapshot_Rebuilt(object sender, EventArgs e)
		{
			History history = sender as History;

			// TODO: Too circular how history calls this and then it asks history for stuff
			if (!history.Snapshot.Contains(Renderer.SelectedBlock))
			{
				Renderer.SelectedBlock = null;
			}

			if (!history.Snapshot.Contains(Renderer.HoverBlock))
			{
				Renderer.HoverBlock = null;
			}

			RenderManager_OGL.Instance.Rebuild(history.Snapshot, Width);
		}

		private void TimerElapsed(object sender, EventArgs e)
		{
			System.Timers.Timer timer = (System.Timers.Timer)sender;
			timer.Stop();

			if (History.ArtificialMaxTime == 0)
			{
				// Recalculate the percentage
				//UInt64 timeRange = History.Instance.TimeRange.Max - History.Instance.TimeRange.Min;

				// TODO
				//// This just needs to check if everything is initialized
				////if (LastTimestamp.Time > 0)
				//{
				//	// Slowly move forward
				//	// TODO: This should be target-time based, but the tool doesn't really
				//	// know what units the target time is in right now
				//	if (!IsPaused && Scrubber.Position < 1.0)
				//	{
				//		Scrubber.Position += 0.0005;
				//		History.Instance.UpdateRollingSnapshot(false, true);
				//	}
				//}
			}

			timer.Start();
		}

		~AddressSpace()
		{
			TokenSource.Cancel();
			RecalculateHoverBlock.Set();
			RecalculateSelectedBlock.Set();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			Renderer.Dispose();

			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		void ColorPickerDialog_ColorChanged(object sender, EventArgs e)
		{
			History.UpdateRollingSnapshotAsync(true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			return;
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			return;
		}

		public void AddressSpace_MouseMove(object sender, MouseEventArgs e)
		{
			Renderer.CurrentMouseLocation = e.Location.ToVector();
			CurrentMouseLocation = e.Location.ToVector();

			if (IsLeftMouseDown || IsMiddleMouseDown)
			{
				Vector mouseDelta = e.Location.ToVector() - LastMouseLocation;
				LastMouseLocation = e.Location.ToVector();

				Renderer.Offset = Renderer.Offset + mouseDelta;
			}

			RecalculateHoverBlock.Set();
		}

		public void AddressSpace_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = true;
				LastMouseLocation = e.Location.ToVector();
				MouseDownLocation = e.Location.ToVector();
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = true;
				LastMouseLocation = e.Location.ToVector();
				MouseDownLocation = e.Location.ToVector();
			}
		}

		public void AddressSpace_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				IsLeftMouseDown = false;
				if (MouseDownLocation == e.Location.ToVector())
				{
					SelectAt();
				}
			}
			else if (e.Button == MouseButtons.Middle)
			{
				IsMiddleMouseDown = false;
				if (MouseDownLocation == e.Location.ToVector())
				{
					SelectAt();
				}
			}
		}

		public void AddressSpace_MouseWheel(object sender, MouseEventArgs e)
		{
			Matrix currentViewMatrix = new Matrix();
			currentViewMatrix.Translate((float)Renderer.Offset.X, (float)Renderer.Offset.Y);
			currentViewMatrix.Scale(Renderer.Scale, Renderer.Scale);

			Vector pointBefore = CurrentMouseLocation;
			Matrix InvertedTransform = currentViewMatrix.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformVector(ref pointBefore);

			int amountToMove = e.Delta / WheelDelta;
			amountToMove = amountToMove.Clamp(-4, 4);

			float finalScale = 1.0f + (float)amountToMove / 5.0f;
			Renderer.Scale *= finalScale;

			// TODO: Better correction
			if (Renderer.Scale < 10.0)
			{
				currentViewMatrix.Scale(finalScale, finalScale);
			}
			else
			{
				Renderer.Scale = 10.0f;
			}

			// Scale is not allowed to be < 1.0
			if (Renderer.Scale < 1.0f)
			{
				float correctionFactor = 1.0f / Renderer.Scale;
				Renderer.Scale = 1.0f;
				currentViewMatrix.Scale(correctionFactor, correctionFactor);
			}

			Vector pointAfter = CurrentMouseLocation;
			InvertedTransform = currentViewMatrix.Clone();
			InvertedTransform.Invert();
			InvertedTransform.TransformVector(ref pointAfter);

			Vector delta = pointAfter - pointBefore;

			currentViewMatrix.Translate((float)delta.X, (float)delta.Y);

			Renderer.Offset = new Vector(currentViewMatrix.OffsetX, currentViewMatrix.OffsetY);
		}

		void SelectAt()
		{
			RecalculateSelectedBlock.Set();
		}

		public void SelectAt(Allocation targetAllocation)
		{
			if (targetAllocation == null)
			{
				return;
			}

			MemoryBlock block = History.Snapshot.Find(targetAllocation.Address);
			if (block != null)
			{
				Renderer.SelectedBlock = block;

				SelectionChangedEventArgs e = new SelectionChangedEventArgs();
				e.SelectedBlock = Renderer.SelectedBlock;
				this.Invoke((MethodInvoker)(() => SelectionChanged(this, e)));
			}
			else
			{
				// TODO: What should be done here? Scroll the timeline to a point where it exists?
			}

			// TODO: Automatically bring the new selection into view
		}

		// TODO: HoverTask and SelectTask are very similar. Find a way to consolidate them.
		void HoverTask(CancellationToken ct)
		{
			while (true)
			{
				RecalculateHoverBlock.WaitOne();

				if (ct.IsCancellationRequested)
				{
					return;
				}

				lock (RebuildDataLock)
				{
					Renderer.HoverBlock = History.Snapshot.Find(Renderer.GetLocalMouseLocation());
				}
			}
		}

		void SelectTask(CancellationToken ct)
		{
			while (true)
			{
				RecalculateSelectedBlock.WaitOne();

				if (ct.IsCancellationRequested)
				{
					return;
				}

				lock (RebuildDataLock)
				{
					Renderer.SelectedBlock = History.Snapshot.Find(Renderer.GetLocalMouseLocation());
					if (Renderer.SelectedBlock != null)
					{
						SelectionChangedEventArgs e = new SelectionChangedEventArgs();
						e.SelectedBlock = Renderer.SelectedBlock;
						this.Invoke((MethodInvoker)(() => SelectionChanged(this, e)));
					}
				}
			}
		}

		void HoverAt(Vector location)
		{
			// TODO: Tooltip?
		}

		private void AddressSpace_SizeChanged(object sender, EventArgs e)
		{
			History.RebaseBlocks = true;
			History.UpdateRollingSnapshotAsync();
		}

		private void AddressSpace_MouseHover(object sender, EventArgs e)
		{
			HoverAt(CurrentMouseLocation);
		}

		public void CenterAt(Vector location)
		{
			Vector newOffset = location - new Vector(Width / 2, Height / 2);
			newOffset.Negate();
			Renderer.Offset = newOffset;
			Renderer.Scale = 1.0f;
		}

		public void AddressSpace_MouseLeave(object sender, EventArgs e)
		{
			Renderer.CurrentMouseLocation = new Vector(-1, -1);
		}
	}

	internal class SelectionChangedEventArgs : EventArgs
	{
		public MemoryBlock SelectedBlock;
	}

	internal delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
}
