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
		public class RenderEventArgs : EventArgs
		{
			public bool IsPreRender = true;
		}

		public delegate void RenderEventHandler(object sender, RenderEventArgs e);

		public event EventHandler OnUpdate;
		public event RenderEventHandler OnRender;
		public event EventHandler OnDispose;

		const double FrameInterval = 30.0;

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

		struct VertexData
		{
			public byte R, G, B, A;
			public Vector3 Position;

			public static int SizeInBytes = 16;
		}

		const int MaxVertices = 1000000;
		VertexData[] VBO = new VertexData[MaxVertices];

		bool IsVbo1 = true;
		VertexData[] VBO1 = new VertexData[MaxVertices];
		VertexData[] VBO2 = new VertexData[MaxVertices];

		uint NumVertices = MaxVertices;
		uint VboHandle;

		bool BuffersCreated = false;
		bool BuffersDirty = true;

		System.Timers.Timer FrameTimer;

		public static bool Suspend { get; set; }

		public RenderManager_OGL()
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

		void TimerElapsed(object sender, EventArgs e)
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
			if (Suspend)
			{
				return;
			}

			if (OnUpdate != null)
			{
				EventArgs e = new EventArgs();
				OnUpdate(this, e);
			}
		}

		public void Render()
		{
			if (Suspend)
			{
				return;
			}

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
	}
}
