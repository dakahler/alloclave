using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alloclave
{
	class AddressSpaceScroller_OGL : AddressSpaceScroller
	{
		GLControl glControl;
		History History;

		public AddressSpaceScroller_OGL(History history, int parentWidth)
			: base(parentWidth)
		{
			History = history;

			glControl = new GLControl();
			glControl.Parent = this;
			glControl.Dock = DockStyle.Fill;
			glControl.Load += glControl_Load;
			glControl.Resize += glControl_Resize;

			glControl.MouseDown += new MouseEventHandler(AddressSpaceScroller_MouseDown);
			glControl.MouseUp += new MouseEventHandler(AddressSpaceScroller_MouseUp);
			glControl.MouseMove += new MouseEventHandler(AddressSpaceScroller_MouseMove);

			this.Controls.Add(glControl);
		}

		~AddressSpaceScroller_OGL()
		{
			
		}

		protected override void Dispose(bool disposing)
		{
			RenderManager_OGL.Instance.OnRender -= OnRender;
			RenderManager_OGL.Instance.OnDispose -= OnDispose;


			if (glControl != null && !glControl.Disposing)
			{
				lock (glControl)
				{
					glControl.Dispose();
					glControl = null;
				}
			}

			base.Dispose(disposing);
		}

		void glControl_Load(object sender, EventArgs e)
		{
			lock (glControl)
			{
				glControl.MakeCurrent();

				GL.Enable(EnableCap.DepthTest);

				// Setup parameters for Points
				GL.PointSize(5f);
				GL.Enable(EnableCap.PointSmooth);
				GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);

				GL.Enable(EnableCap.AlphaTest);
				GL.Enable(EnableCap.Blend);

				// Setup VBO state
				GL.EnableClientState(ArrayCap.ColorArray);
				GL.EnableClientState(ArrayCap.VertexArray);

				RenderManager_OGL.Instance.Bind();


				glControl.BringToFront();
				GL.ClearColor(0.4f, 0.4f, 0.4f, 0);
				glControl.VSync = true;
			}

			SetupViewport();

			RenderManager_OGL.Instance.OnRender += OnRender;
			RenderManager_OGL.Instance.OnDispose += OnDispose;
		}

		void OnDispose(object sender, EventArgs e)
		{
			if (glControl != null && !glControl.Disposing)
			{
				glControl.Dispose();
				glControl = null;
			}
		}

		void OnRender(object sender, RenderManager_OGL.RenderEventArgs e)
		{
			try
			{
				if (e.IsPreRender)
				{
					Monitor.Enter(glControl);

					if (!glControl.Context.IsCurrent)
					{
						glControl.MakeCurrent();
					}

					Rectangle bounds = Bounds;

					UInt64 maxWidth = (UInt64)ParentWidth;
					UInt64 maxHeight = (UInt64)bounds.Bottom;

					float scaleX = (float)Width / (float)maxWidth;
					float scaleY = (float)Height / (float)maxHeight;


					GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

					GL.MatrixMode(MatrixMode.Modelview);

					GL.PushMatrix();
					GL.Scale(scaleX, scaleY, 1);
				}
				else
				{
					GL.PopMatrix();
					glControl.SwapBuffers();
					glControl.Context.MakeCurrent(null);
					Monitor.Exit(glControl);
				}
			}
			catch (ObjectDisposedException)
			{

			}
		}

		void glControl_Resize(object sender, EventArgs e)
		{
			SetupViewport();
		}

		private void SetupViewport()
		{
			lock (glControl)
			{
				glControl.MakeCurrent();

				int w = glControl.Width;
				int h = glControl.Height;
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Ortho(0, w, h, 0, -10, 10); // Bottom-left corner pixel has coordinate (0, 0)
				GL.Viewport(0, 0, w, h); // Use all of the glControl painting area

				glControl.Context.MakeCurrent(null);
			}
		}

		protected override Rectangle Bounds
		{
			get
			{
				return History.Snapshot.Bounds;
			}
		}
	}
}
