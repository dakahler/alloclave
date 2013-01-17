using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows;
using System.Threading;

namespace Alloclave
{
	class AddressSpaceRenderer_OGL : AddressSpaceRenderer
	{
		AddressSpace Parent;
		GLControl glControl;
		bool GlControlLoaded;

		private Mutex mutex = new Mutex();

		public AddressSpaceRenderer_OGL(AddressSpace parent)
		{
			Parent = parent;
			glControl = new GLControl();
			glControl.Parent = parent;
			glControl.Dock = DockStyle.Fill;
			glControl.Load += glControl_Load;
			glControl.Resize += glControl_Resize;

			glControl.MouseDown += new MouseEventHandler(Parent.AddressSpace_MouseDown);
			glControl.MouseUp += new MouseEventHandler(Parent.AddressSpace_MouseUp);
			glControl.MouseMove += new MouseEventHandler(Parent.AddressSpace_MouseMove);
			glControl.MouseWheel += new MouseEventHandler(Parent.AddressSpace_MouseWheel);
			glControl.MouseLeave += new EventHandler(Parent.AddressSpace_MouseLeave);
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

			// Setup VBO state
			GL.EnableClientState(EnableCap.ColorArray);
			GL.EnableClientState(EnableCap.VertexArray);

			RenderManager_OGL.Instance.Bind();

			glControl.BringToFront();
			GlControlLoaded = true;
			GL.ClearColor(255, 255, 255, 0);
			glControl.VSync = true;
			SetupViewport();

			RenderManager_OGL.Instance.OnRender += OnRender;
		}

		void OnRender(object sender, RenderManager_OGL.RenderEventArgs e)
		{
			if (!GlControlLoaded || MemoryBlockManager.Instance.Count == 0)
			{
				return;
			}

			if (e.IsPreRender)
			{
				mutex.WaitOne();
				glControl.MakeCurrent();

				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

				GL.MatrixMode(MatrixMode.Modelview);

				GL.PushMatrix();
				GL.Translate(Offset.X, Offset.Y, 0);
				GL.Scale(Scale, Scale, Scale);
			}
			else
			{
				// TODO: These should modify the VBO instead of using immediate mode
				if (_SelectedBlock != null)
				{
					GL.Begin(BeginMode.Triangles);
					GL.Color3(Color.Black);
					foreach (Triangle triangle in _SelectedBlock.Triangles)
					{
						foreach (Vector vertex in triangle.Vertices)
						{
							GL.Vertex3(vertex.X, vertex.Y, 1);
						}
					}
					GL.End();
				}

				if (_HoverBlock != null)
				{
					GL.Begin(BeginMode.Triangles);
					GL.Color4(Color.FromArgb(128, Color.Black));
					foreach (Triangle triangle in _HoverBlock.Triangles)
					{
						foreach (Vector vertex in triangle.Vertices)
						{
							GL.Vertex3(vertex.X, vertex.Y, 1);
						}
					}
					GL.End();
				}

				GL.PopMatrix();
				glControl.SwapBuffers();
				glControl.Context.MakeCurrent(null);
				mutex.ReleaseMutex();
			}
		}

		void glControl_Resize(object sender, EventArgs e)
		{
			SetupViewport();
		}

		private void SetupViewport()
		{
			mutex.WaitOne();
			glControl.MakeCurrent();

			int w = glControl.Width;
			int h = glControl.Height;
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, w, h, 0, -10, 10); // Bottom-left corner pixel has coordinate (0, 0)
			GL.Viewport(0, 0, w, h); // Use all of the glControl painting area

			glControl.Context.MakeCurrent(null);
			mutex.ReleaseMutex();
		}
	}
}
