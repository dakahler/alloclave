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

		public AddressSpaceRenderer Renderer { get; set; }

		Object RebuildDataLock = new Object();
		AutoResetEvent RecalculateSelectedBlock = new AutoResetEvent(false);
		AutoResetEvent RecalculateHoverBlock = new AutoResetEvent(false);

		CancellationTokenSource TokenSource;

		// TODO: Probably shouldn't be static
		public static event SelectionChangedEventHandler SelectionChanged;

		bool _IsPaused = false;
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
		const double FrameInterval = 30.0;

		// TODO: This class shouldn't need a history reference at all
		History _History;
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

                if (_History != null)
                {
                    _History.Rebuilt += Snapshot_Rebuilt;
                    SizeChanged += new System.EventHandler(AddressSpace_SizeChanged);
                }
			}
		}

		public Snapshot SnapshotOverride { get; set; }

		public AddressSpace()
		{
			InitializeComponent();

			this.MouseWheel += AddressSpace_MouseWheel;

			ColorPickerDialog.ColorChanged += ColorPickerDialog_ColorChanged;

			TokenSource = new CancellationTokenSource();
			var cancellationToken = TokenSource.Token;

			Task.Factory.StartNew(() => HoverTask(cancellationToken), cancellationToken);
			Task.Factory.StartNew(() => SelectTask(cancellationToken), cancellationToken);

			this.Load += AddressSpace_Load;
			this.NoDataPanel.Paint += NoDataPanel_Paint;
			this.EnabledChanged += AddressSpace_EnabledChanged;
		}

		void AddressSpace_EnabledChanged(object sender, EventArgs e)
		{
			NoDataPanel.Visible = !Enabled;
		}

		void NoDataPanel_Paint(object sender, PaintEventArgs e)
		{
			if (!Enabled)
			{
				Graphics g = e.Graphics;
				Font font = new Font("Arial", 30);
				String text = "Waiting For Data...";
				SizeF stringSize = g.MeasureString(text, font);

				float x = (float)((NoDataPanel.Width / 2) - (stringSize.Width / 2));
				float y = (float)((NoDataPanel.Height / 2) - (stringSize.Height / 2));
				g.DrawString(text, font, new SolidBrush(Color.FromArgb(180, 180, 180)), new PointF(x, y));
			}
		}

		void AddressSpace_Load(object sender, EventArgs e)
		{
			FrameTimer = new System.Timers.Timer(FrameInterval);
			FrameTimer.Elapsed += TimerElapsed;
			FrameTimer.Start();

			NoDataPanel.BringToFront();
			NoDataPanel.Visible = !Enabled;
		}

		void Snapshot_Rebuilt(object sender, EventArgs e)
		{
			History history = sender as History;
			Renderer.Rebuilt(History);
		}

		void TimerElapsed(object sender, EventArgs e)
		{
			if (History == null)
			{
				return;
			}

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

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			Renderer.Dispose();

			TokenSource.Cancel();
			RecalculateHoverBlock.Set();
			RecalculateSelectedBlock.Set();

			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		void ColorPickerDialog_ColorChanged(object sender, EventArgs e)
		{
			History.UpdateSnapshotAsync(History.Snapshot, true);
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

			Snapshot snapshot = SnapshotOverride ?? History.Snapshot;

			MemoryBlock block = snapshot.Find(targetAllocation.Address);
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
					// TODO: This shouldn't even need history
					Snapshot snapshot = SnapshotOverride ?? History.Snapshot;
					if (snapshot != null)
					{
						Renderer.HoverBlock = snapshot.Find(Renderer.GetLocalMouseLocation());
						if (Renderer.HoverBlock != null && !Renderer.HoverBlock.IsValid)
						{
							Renderer.HoverBlock = null;
						}
					}
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
					// TODO: This shouldn't even need history
					Snapshot snapshot = SnapshotOverride ?? History.Snapshot;
					if (snapshot != null)
					{
						Renderer.SelectedBlock = snapshot.Find(Renderer.GetLocalMouseLocation());
						if (Renderer.SelectedBlock != null && !Renderer.SelectedBlock.IsValid)
						{
							Renderer.SelectedBlock = null;
						}

						if (Renderer.SelectedBlock != null)
						{
							SelectionChangedEventArgs e = new SelectionChangedEventArgs();
							e.SelectedBlock = Renderer.SelectedBlock;
							this.Invoke((MethodInvoker)(() => SelectionChanged(this, e)));
						}
					}
				}
			}
		}

		void HoverAt(Vector location)
		{
			// TODO: Tooltip?
		}

		void AddressSpace_SizeChanged(object sender, EventArgs e)
		{
			if (History != null)
			{
				History.RebaseBlocks = true;
				History.UpdateSnapshotAsync(History.Snapshot);
			}

			NoDataPanel.Refresh();
		}

		void AddressSpace_MouseHover(object sender, EventArgs e)
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
