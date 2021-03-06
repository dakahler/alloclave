﻿using System;
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
		RenderManager_OGL RenderManager;

		bool GlControlLoaded;

		public AddressSpaceRenderer_OGL(AddressSpace parent, RenderManager_OGL renderManager)
		{
			Parent = parent;
			RenderManager = renderManager;

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

		protected override void Dispose(bool disposing)
		{
			RenderManager.OnRender -= OnRender;
			RenderManager.OnDispose -= OnDispose;

			if (glControl != null && !glControl.Disposing)
			{
				lock (glControl)
				{
					glControl.Dispose();
					glControl = null;
				}
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

				RenderManager.Bind();

				glControl.BringToFront();
				GlControlLoaded = true;
				GL.ClearColor(0.4f, 0.4f, 0.4f, 0);
				glControl.VSync = true;
			}

			SetupViewport();

			RenderManager.OnRender += OnRender;
			RenderManager.OnDispose += OnDispose;
		}

		void OnDispose(object sender, EventArgs e)
		{
			Dispose(true);
		}

		void OnRender(object sender, RenderManager_OGL.RenderEventArgs e)
		{
            try
            {
                if (!GlControlLoaded)
                {
                    return;
                }

                if (e.IsPreRender)
                {
                    Monitor.Enter(glControl);

                    if (glControl.Context == null)
                    {
                        Monitor.Exit(glControl);
                        return;
                    }

                    if (!glControl.Context.IsCurrent)
                    {
                        glControl.MakeCurrent();
                    }

                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Modelview);

                    GL.PushMatrix();
                    GL.Translate(Offset.X, Offset.Y, 0);
                    GL.Scale(Scale, Scale, Scale);
                }
                else
                {
                    // TODO: These should modify the VBO instead of using immediate mode
                    MemoryBlock selectedBlock = SelectedBlock;
                    if (selectedBlock != null)
                    {
                        GL.Begin(BeginMode.Triangles);
                        GL.Color3(Color.FromArgb(40, 40, 40));
                        foreach (Triangle triangle in selectedBlock.Triangles)
                        {
                            foreach (Vector vertex in triangle.Vertices)
                            {
                                GL.Vertex3(vertex.X * Width, vertex.Y, 1);
                            }
                        }
                        GL.End();
                    }

                    MemoryBlock hoverBlock = HoverBlock;
                    if (hoverBlock != null)
                    {
                        GL.Begin(BeginMode.Triangles);
                        GL.Color3(Color.FromArgb(40, 40, 40));
                        foreach (Triangle triangle in hoverBlock.Triangles)
                        {
                            foreach (Vector vertex in triangle.Vertices)
                            {
                                GL.Vertex3(vertex.X * Width, vertex.Y, 1);
                            }
                        }
                        GL.End();
                    }

                    GL.PopMatrix();

                    glControl.SwapBuffers();
                    glControl.Context.MakeCurrent(null);
                    Monitor.Exit(glControl);
                }
            }
            catch (ObjectDisposedException)
            {

            }
            catch (InvalidOperationException)
            {

            }
		}

		void glControl_Resize(object sender, EventArgs e)
		{
			SetupViewport();
		}

		void SetupViewport()
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

		internal override void Rebuilt(History history)
		{
            if (glControl != null)
            {
                base.Rebuilt(history);
                RenderManager.Rebuild(history.Snapshot, (int)Width);
            }
		}

		protected override float Width
		{
			get
			{
				return glControl.Width;
			}
		}
	}
}
