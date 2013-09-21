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
using OpenTK.Graphics.OpenGL;
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
		bool ShowText = true;

		OpenTK.Graphics.TextPrinter textPrinter = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.High);

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

		~AddressSpaceRenderer_OGL()
		{
			if (glControl != null && !glControl.Disposing)
			{
				glControl.Dispose();
				glControl = null;
			}
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
				//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.DstAlpha);
				//GL.AlphaFunc(AlphaFunction.Greater, 0);
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

				// Setup VBO state
				GL.EnableClientState(ArrayCap.ColorArray);
				GL.EnableClientState(ArrayCap.VertexArray);

				RenderManager_OGL.Instance.Bind();

				glControl.BringToFront();
				GlControlLoaded = true;
				GL.ClearColor(0.4f, 0.4f, 0.4f, 0);
				glControl.VSync = true;
			}

			SetupViewport();

			RenderManager_OGL.Instance.OnRender += OnRender;
			RenderManager_OGL.Instance.OnDispose += OnDispose;
		}

		void OnDispose(object sender, EventArgs e)
		{
			glControl.Dispose();
			glControl = null;
		}

		void OnRender(object sender, RenderManager_OGL.RenderEventArgs e)
		{
			if (!GlControlLoaded)
			{
				return;
			}

			if (e.IsPreRender)
			{
				Monitor.Enter(glControl);
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
				VisualMemoryBlock selectedBlock = _SelectedBlock;
				if (selectedBlock != null)
				{
					GL.Begin(BeginMode.Triangles);
					GL.Color3(Color.FromArgb(40, 40, 40));
					foreach (Triangle triangle in selectedBlock.Triangles)
					{
						foreach (Vector vertex in triangle.Vertices)
						{
							double yVertex = vertex.Y;
							if (History.Instance.Snapshot.HeapOffsets.ContainsKey(selectedBlock.Allocation.HeapId))
							{
								yVertex -= (double)History.Instance.Snapshot.HeapOffsets[selectedBlock.Allocation.HeapId];
							}

							GL.Vertex3(vertex.X, yVertex, 1);
						}
					}
					GL.End();
				}

				VisualMemoryBlock hoverBlock = _HoverBlock;
				if (hoverBlock != null)
				{
					GL.Begin(BeginMode.Triangles);
					GL.Color3(Color.FromArgb(40, 40, 40));
					foreach (Triangle triangle in hoverBlock.Triangles)
					{
						foreach (Vector vertex in triangle.Vertices)
						{
							double yVertex = vertex.Y;
							if (History.Instance.Snapshot.HeapOffsets.ContainsKey(hoverBlock.Allocation.HeapId))
							{
								yVertex -= (double)History.Instance.Snapshot.HeapOffsets[hoverBlock.Allocation.HeapId];
							}

							GL.Vertex3(vertex.X, yVertex, 1);
						}
					}
					GL.End();
				}

				GL.PopMatrix();

				if (History.Instance.Snapshot.Count == 0 && ShowText)
				{
					GL.PushMatrix();
					String waitingText = "Waiting For Data...";
					Font font = new Font("Arial", 30);
					OpenTK.Graphics.TextExtents extents = textPrinter.Measure(waitingText, font);

					GL.Translate((glControl.Width / 2) - (extents.BoundingBox.Width / 2),
						(glControl.Height / 2) - (extents.BoundingBox.Height / 2), 0);

					textPrinter.Print(waitingText, font, Color.FromArgb(200, 200, 200));
					GL.PopMatrix();
				}
				else
				{
					ShowText = false;
				}

				glControl.SwapBuffers();
				glControl.Context.MakeCurrent(null);
				Monitor.Exit(glControl);
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
	}
}
