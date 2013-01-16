using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	class AddressSpaceScroller_OGL : AddressSpaceScroller
	{
		GLControl glControl;
		bool GlControlLoaded;

		public AddressSpaceScroller_OGL(int parentWidth)
			: base(parentWidth)
		{
			glControl = new GLControl();
			//glControl.Parent = parent;
			glControl.Dock = DockStyle.Fill;
			glControl.Load += glControl_Load;
			glControl.Paint += glControl_Paint;
			glControl.Resize += glControl_Resize;

			glControl.MouseDown += new MouseEventHandler(AddressSpaceScroller_MouseDown);
			glControl.MouseUp += new MouseEventHandler(AddressSpaceScroller_MouseUp);
			glControl.MouseMove += new MouseEventHandler(AddressSpaceScroller_MouseMove);

			this.Controls.Add(glControl);
		}

		void glControl_Load(object sender, EventArgs e)
		{
			glControl.MakeCurrent();

			GL.Enable(EnableCap.DepthTest);

			// Setup parameters for Points
			GL.PointSize(5f);
			GL.Enable(EnableCap.PointSmooth);
			GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);

			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.DstAlpha);
			//GL.AlphaFunc(AlphaFunction.Greater, 0);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			//// Setup VBO state
			GL.EnableClientState(EnableCap.ColorArray);
			GL.EnableClientState(EnableCap.VertexArray);

			//GL.GenBuffers(1, out VBOHandle);

			// Since there's only 1 VBO in the app, might aswell setup here.
			GL.BindBuffer(BufferTarget.ArrayBuffer, AddressSpaceRenderer_OGL.VBOHandle);
			GL.ColorPointer(4, ColorPointerType.UnsignedByte, AddressSpaceRenderer_OGL.VertexC4ubV3f.SizeInBytes, (IntPtr)0);
			GL.VertexPointer(3, VertexPointerType.Float, AddressSpaceRenderer_OGL.VertexC4ubV3f.SizeInBytes, (IntPtr)(4 * sizeof(byte)));


			glControl.BringToFront();
			GlControlLoaded = true;
			GL.ClearColor(255, 255, 255, 0);
			glControl.VSync = true;
			SetupViewport();

			const double interval = 10.0;
			System.Timers.Timer timer = new System.Timers.Timer(interval);
			timer.Elapsed += TimerElapsed;
			timer.Start();
		}

		void TimerElapsed(object sender, EventArgs e)
		{
			glControl.Invalidate();
		}

		void glControl_Resize(object sender, EventArgs e)
		{
			SetupViewport();
			glControl.Invalidate();
		}

		void glControl_Paint(object sender, PaintEventArgs e)
		{
			Render(e);
		}

		private void SetupViewport()
		{
			glControl.MakeCurrent();

			int w = glControl.Width;
			int h = glControl.Height;
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, w, h, 0, -10, 10); // Bottom-left corner pixel has coordinate (0, 0)
			GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
		}

		protected override void Render(PaintEventArgs e)
		{
			if (MemoryBlockManager.Instance.Count == 0)
			{
				return;
			}

			glControl.MakeCurrent();

			Rectangle bounds = MemoryBlockManager.Instance.Bounds;

			UInt64 maxWidth = (UInt64)ParentWidth;
			UInt64 maxHeight = (UInt64)bounds.Bottom;

			float scaleX = (float)Width / (float)maxWidth;
			float scaleY = (float)Height / (float)maxHeight;


			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);

			GL.PushMatrix();
			GL.Scale(scaleX, scaleY, 1);

			// Tell OpenGL to discard old VBO when done drawing it and reserve memory _now_ for a new buffer.
			// without this, GL would wait until draw operations on old VBO are complete before writing to it
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(AddressSpaceRenderer_OGL.VertexC4ubV3f.SizeInBytes * AddressSpaceRenderer_OGL.MaxVertices), IntPtr.Zero, BufferUsageHint.DynamicDraw);
			// Fill newly allocated buffer
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(AddressSpaceRenderer_OGL.VertexC4ubV3f.SizeInBytes * AddressSpaceRenderer_OGL.MaxVertices), AddressSpaceRenderer_OGL.VBO, BufferUsageHint.DynamicDraw);
			// Draw everything
			GL.DrawArrays(BeginMode.Triangles, 0, (int)AddressSpaceRenderer_OGL.vboCount);

			GL.PopMatrix();

			glControl.SwapBuffers();
		}
	}
}
