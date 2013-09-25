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

		private const double FrameInterval = 30.0;

		//private class BlockMetadata
		//{
		//	public BlockMetadata(uint startVertex, uint endVertex)
		//	{
		//		VertexStartIndex = startVertex;
		//		VertexEndIndex = endVertex;
		//	}

		//	public TimeStamp StartTime = new TimeStamp();
		//	public uint VertexStartIndex;
		//	public uint VertexEndIndex;
		//	public const float AliveSeconds = 3.0f;
		//}

		private struct VertexData
		{
			public byte R, G, B, A;
			public Vector3 Position;

			public static int SizeInBytes = 16;
		}

		private const int MaxVertices = 1000000;
		private VertexData[] VBO = new VertexData[MaxVertices];

		bool IsVbo1 = true;
		private VertexData[] VBO1 = new VertexData[MaxVertices];
		private VertexData[] VBO2 = new VertexData[MaxVertices];

		private uint NumVertices = MaxVertices;
		private uint VboHandle;

		private bool BuffersCreated = false;
		private bool BuffersDirty = true;

		System.Timers.Timer FrameTimer;

		private RenderManager_OGL()
		{
			FrameTimer = new System.Timers.Timer(FrameInterval);
			FrameTimer.Elapsed += TimerElapsed;
			FrameTimer.Start();
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
			if (OnUpdate != null)
			{
				EventArgs e = new EventArgs();
				OnUpdate(this, e);
			}
		}

		public void Render()
		{
			bool callBufferData = BuffersDirty;

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

					if (callBufferData)
					{
						// Tell OpenGL to discard old VBO when done drawing it and reserve memory _now_ for a new buffer.
						// without this, GL would wait until draw operations on old VBO are complete before writing to it
						GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexData.SizeInBytes * NumVertices), IntPtr.Zero, BufferUsageHint.StreamDraw);

						// Fill newly allocated buffer
						GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexData.SizeInBytes * NumVertices), VBO, BufferUsageHint.StreamDraw);

						BuffersDirty = false;
					}

					// Draw everything
					GL.DrawArrays(BeginMode.Triangles, 0, (int)NumVertices);

					// Post-render callback
					e.IsPreRender = false;
					d.Invoke(this, e);
				}
			}
		}

		public void Rebuild(Snapshot snapshot, int width)
		{
			// TODO: Vertex incremental rebuilding
			uint numVertices = 0;

			// Use temporary vertex buffers, and just swap out the real one at the end
			VertexData[] vbo;
			if (IsVbo1)
			{
				vbo = VBO1;
			}
			else
			{
				vbo = VBO2;
			}

			foreach (var block in snapshot)
			{
				foreach (Triangle triangle in block.Triangles)
				{
					foreach (Vector vertex in triangle.Vertices)
					{
						if (numVertices >= vbo.Length)
						{
							Array.Resize(ref vbo, vbo.Length * 2);
						}

						vbo[numVertices].R = block._Color.R;
						vbo[numVertices].G = block._Color.G;
						vbo[numVertices].B = block._Color.B;
						vbo[numVertices].A = block._Color.A;
						vbo[numVertices].Position = new Vector3((float)vertex.X * width, (float)vertex.Y, 0);
						numVertices++;
					}
				}
			}

			// Swap
			VBO = vbo;
			NumVertices = numVertices;
			IsVbo1 = !IsVbo1;
			BuffersDirty = true;
		}

		//private void ChangeBlockColor(KeyValuePair<MemoryBlock, BlockMetadata> block, Color color, float blend)
		//{
		//	//byte r = (byte)((block.Key._Color.R * blend) + color.R * (1 - blend));
		//	//byte g = (byte)((block.Key._Color.G * blend) + color.G * (1 - blend));
		//	//byte b = (byte)((block.Key._Color.B * blend) + color.B * (1 - blend));

		//	//for (uint j = block.Value.VertexStartIndex; j <= block.Value.VertexEndIndex; j++)
		//	//{
		//	//	VBO[j].R = r;
		//	//	VBO[j].G = g;
		//	//	VBO[j].B = b;
		//	//	VBO[j].A = 255;
		//	//}
		//}
	}
}
