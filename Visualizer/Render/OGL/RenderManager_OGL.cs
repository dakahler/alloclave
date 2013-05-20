using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Alloclave
{
	class RenderManager_OGL : IDisposable
	{
		private static readonly RenderManager_OGL _Instance = new RenderManager_OGL();
		public static RenderManager_OGL Instance
		{
			get
			{
				return _Instance;
			}
		}

		public class RenderEventArgs : EventArgs
		{
			public bool IsPreRender = true;
		}

		public delegate void RenderEventHandler(object sender, RenderEventArgs e);

		public event EventHandler OnUpdate;
		public event RenderEventHandler OnRender;
		public event EventHandler OnDispose;

		private const double FrameInterval = 10.0;

		private class BlockMetadata
		{
			public BlockMetadata(uint startVertex, uint endVertex)
			{
				VertexStartIndex = startVertex;
				VertexEndIndex = endVertex;
			}

			public TimeStamp StartTime = new TimeStamp();
			public uint VertexStartIndex;
			public uint VertexEndIndex;
			public const float AliveSeconds = 3.0f;
		}

		private struct VertexData
		{
			public byte R, G, B, A;
			public Vector3 Position;

			public static int SizeInBytes = 16;
		}

		private const int MaxVertices = 1000000;
		private VertexData[] VBO = new VertexData[MaxVertices];
		private uint NumVertices;
		private uint VboHandle;

		private bool BuffersCreated = false;

		Dictionary<VisualMemoryBlock, BlockMetadata> NewBlocks = new Dictionary<VisualMemoryBlock, BlockMetadata>();

		System.Timers.Timer FrameTimer;

		private RenderManager_OGL()
		{
			FrameTimer = new System.Timers.Timer(FrameInterval);
			FrameTimer.Elapsed += TimerElapsed;
			FrameTimer.Start();
		}

		~RenderManager_OGL()
		{
			// TODO
			//GL.DeleteBuffers(1, ref VboHandle);
		}

		public void Dispose()
		{
			OnUpdate = null;
			OnRender = null;

			if (OnDispose != null)
			{
				OnDispose(this, new EventArgs());
			}

			FrameTimer.Stop();
			FrameTimer.Close();

			if (VboHandle != 0)
			{
				GL.DeleteBuffers(1, ref VboHandle);
			}

			BuffersCreated = false;
		}

		public void Pause()
		{
			FrameTimer.Stop();
		}

		public void Unpause()
		{
			FrameTimer.Start();
		}

		private void TimerElapsed(object sender, EventArgs e)
		{
			System.Timers.Timer timer = (System.Timers.Timer)sender;
			timer.Stop();

			Update();
			Render();

			timer.Start();
		}

		/// <summary>
		/// Binds vertex buffer object to the current GL context
		/// </summary>
		public void Bind()
		{
			if (!BuffersCreated)
			{
				GL.GenBuffers(1, out VboHandle);

				ErrorCode errorCode = GL.GetError();
				if (errorCode != ErrorCode.NoError)
				{
					MessageBox.Show("Error creating vertex buffer object. Please email support@circularshift.com.");
				}

				BuffersCreated = true;
			}

			GL.BindBuffer(BufferTarget.ArrayBuffer, VboHandle);
			GL.ColorPointer(4, ColorPointerType.UnsignedByte, VertexData.SizeInBytes, (IntPtr)0);
			GL.VertexPointer(3, VertexPointerType.Float, VertexData.SizeInBytes, (IntPtr)(4 * sizeof(byte)));
		}

		public void Update()
		{
			// TODO: Needs to be much faster
			//lock (NewBlocks)
			//{
			//	for (int i = 0; i < NewBlocks.Count; i++)
			//	{
			//		var block = NewBlocks.ElementAt(i);

			//		double startTimeSeconds = (double)block.Value.StartTime.Time / (double)Stopwatch.Frequency;
			//		double currentTimeSeconds = (double)Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;
			//		float difference = (float)(currentTimeSeconds - startTimeSeconds);

			//		float percentage = (BlockMetadata.AliveSeconds - (float)difference) / BlockMetadata.AliveSeconds;
			//		percentage = 1.0f - percentage;
			//		percentage = Math.Min(percentage, 1.0f);
			//		percentage = Math.Max(percentage, 0.0f);

			//		ChangeBlockColor(block, Color.LightYellow, percentage);

			//		// Delete if old
			//		if (difference > BlockMetadata.AliveSeconds)
			//		{
			//			ChangeBlockColor(block, Color.LightYellow, 1.0f);
			//			NewBlocks.Remove(block.Key);
			//			i--;
			//		}
			//	}
			//}

			if (OnUpdate != null)
			{
				EventArgs e = new EventArgs();
				OnUpdate(this, e);
			}
		}

		public void Render()
		{
			if (OnRender != null)
			{
				MulticastDelegate m = (MulticastDelegate)OnRender;
				Delegate[] delegates = m.GetInvocationList();
				foreach (RenderEventHandler d in delegates)
				{
					// Pre-render callback
					RenderEventArgs e = new RenderEventArgs();
					d.Invoke(this, e);

					// If pre-render above didn't setup a context, don't try to do anything else
					if (OpenTK.Graphics.GraphicsContext.CurrentContext == null)
					{
						continue;
					}

					// Tell OpenGL to discard old VBO when done drawing it and reserve memory _now_ for a new buffer.
					// without this, GL would wait until draw operations on old VBO are complete before writing to it
					GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexData.SizeInBytes * MaxVertices), IntPtr.Zero, BufferUsageHint.StreamDraw);

					// Fill newly allocated buffer
					GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexData.SizeInBytes * MaxVertices), VBO, BufferUsageHint.StreamDraw);

					// Draw everything
					GL.DrawArrays(BeginMode.Triangles, 0, (int)NumVertices);

					// Post-render callback
					e.IsPreRender = false;
					d.Invoke(this, e);
				}
			}
		}

		public void Rebuild(Dictionary<uint, float> offsets = null)
		{
			// TODO: Vertex incremental rebuilding
			NumVertices = 0;

			lock (NewBlocks)
			{
				NewBlocks.Clear();
				bool isSecondaryColor = false;
				foreach (var block in MemoryBlockManager.Instance)
				{
					uint startVertex = NumVertices;
					foreach (Triangle triangle in block.Triangles)
					{
						foreach (Vector vertex in triangle.Vertices)
						{
							if (NumVertices >= VBO.Length)
							{
								Array.Resize(ref VBO, VBO.Length * 2);
							}

							double yVertex = vertex.Y;
							if (offsets != null && offsets.ContainsKey(block.Allocation.HeapId))
							{
								yVertex -= (double)offsets[block.Allocation.HeapId];
							}

							//Color color = Color.Red;
							//switch (block.ColorIndex)
							//{
							//	case 0:
							//		if (!isSecondaryColor)
							//			color = Properties.Settings.Default.Heap1_Allocation1;
							//		else
							//			color = Properties.Settings.Default.Heap1_Allocation2;
							//		break;
							//	case 1:
							//		if (!isSecondaryColor)
							//			color = Properties.Settings.Default.Heap2_Allocation1;
							//		else
							//			color = Properties.Settings.Default.Heap2_Allocation2;
							//		break;
							//	case 2:
							//		if (!isSecondaryColor)
							//			color = Properties.Settings.Default.Heap3_Allocation1;
							//		else
							//			color = Properties.Settings.Default.Heap3_Allocation2;
							//		break;
							//	case 3:
							//		if (!isSecondaryColor)
							//			color = Properties.Settings.Default.Heap4_Allocation1;
							//		else
							//			color = Properties.Settings.Default.Heap4_Allocation2;
							//		break;
							//}

							VBO[NumVertices].R = block._Color.R;
							VBO[NumVertices].G = block._Color.G;
							VBO[NumVertices].B = block._Color.B;
							VBO[NumVertices].A = block._Color.A;
							VBO[NumVertices].Position = new Vector3((float)vertex.X, (float)yVertex, 0);
							NumVertices++;
						}
					}
					uint endVertex = NumVertices - 1;

					if (block.IsNew)
					{
						block.IsNew = false;
						NewBlocks.Add(block, new BlockMetadata(startVertex, endVertex));
					}

					isSecondaryColor = !isSecondaryColor;
				}
			}
		}

		private void ChangeBlockColor(KeyValuePair<VisualMemoryBlock, BlockMetadata> block, Color color, float blend)
		{
			//byte r = (byte)((block.Key._Color.R * blend) + color.R * (1 - blend));
			//byte g = (byte)((block.Key._Color.G * blend) + color.G * (1 - blend));
			//byte b = (byte)((block.Key._Color.B * blend) + color.B * (1 - blend));

			//for (uint j = block.Value.VertexStartIndex; j <= block.Value.VertexEndIndex; j++)
			//{
			//	VBO[j].R = r;
			//	VBO[j].G = g;
			//	VBO[j].B = b;
			//	VBO[j].A = 255;
			//}
		}
	}
}
