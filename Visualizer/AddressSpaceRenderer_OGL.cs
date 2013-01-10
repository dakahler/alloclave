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

namespace Alloclave
{
	class AddressSpaceRenderer_OGL : AddressSpaceRenderer
	{
		AddressSpace Parent;
		GLControl glControl;
		bool GlControlLoaded;

		private Vector3[] Vertices;

		const int MaxVertices = 1000000;
		VertexC4ubV3f[] VBO = new VertexC4ubV3f[MaxVertices];
		uint vboCount;
		uint VBOHandle;

		struct VertexC4ubV3f
		{
			public byte R, G, B, A;
			public Vector3 Position;

			public static int SizeInBytes = 16;
		}

		public AddressSpaceRenderer_OGL(AddressSpace parent)
		{
			Parent = parent;
			glControl = new GLControl();
			glControl.Parent = parent;
			glControl.Dock = DockStyle.Fill;
			glControl.Load += glControl_Load;
			glControl.Paint += glControl_Paint;
			glControl.Resize += glControl_Resize;

			glControl.MouseDown += new MouseEventHandler(Parent.AddressSpace_MouseDown);
			glControl.MouseUp += new MouseEventHandler(Parent.AddressSpace_MouseUp);
			glControl.MouseMove += new MouseEventHandler(Parent.AddressSpace_MouseMove);
			glControl.MouseWheel += new MouseEventHandler(Parent.AddressSpace_MouseWheel);
			glControl.MouseLeave += new EventHandler(Parent.AddressSpace_MouseLeave);
		}

		void glControl_Load(object sender, EventArgs e)
		{
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

			GL.GenBuffers(1, out VBOHandle);

			// Since there's only 1 VBO in the app, might aswell setup here.
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBOHandle);
			GL.ColorPointer(4, ColorPointerType.UnsignedByte, VertexC4ubV3f.SizeInBytes, (IntPtr)0);
			GL.VertexPointer(3, VertexPointerType.Float, VertexC4ubV3f.SizeInBytes, (IntPtr)(4 * sizeof(byte)));


			glControl.BringToFront();
			GlControlLoaded = true;
			GL.ClearColor(255, 255, 255, 0);
			glControl.VSync = true;
			SetupViewport();
		}

		void glControl_Resize(object sender, EventArgs e)
		{
			SetupViewport();
			glControl.Invalidate();
		}

		private void SetupViewport()
		{
			int w = glControl.Width;
			int h = glControl.Height;
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, w, h, 0, -10, 10); // Bottom-left corner pixel has coordinate (0, 0)
			GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
		}

		~AddressSpaceRenderer_OGL()
		{
			GL.DeleteBuffers(1, ref VBOHandle);
		}

		void glControl_Paint(object sender, PaintEventArgs e)
		{
			if (!GlControlLoaded || _Blocks == null || Vertices == null)
			{
				return;
			}

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);

			GL.PushMatrix();
			GL.Translate(Offset.X, Offset.Y, 0);
			GL.Scale(Scale, Scale, Scale);

			// Tell OpenGL to discard old VBO when done drawing it and reserve memory _now_ for a new buffer.
			// without this, GL would wait until draw operations on old VBO are complete before writing to it
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexC4ubV3f.SizeInBytes * MaxVertices), IntPtr.Zero, BufferUsageHint.DynamicDraw);
			// Fill newly allocated buffer
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexC4ubV3f.SizeInBytes * MaxVertices), VBO, BufferUsageHint.DynamicDraw);
			// Only draw particles that are alive
			GL.DrawArrays(BeginMode.Triangles, 0, (int)vboCount);

			if (_SelectedBlock != null)
			{
				GL.Color3(Color.Black);
				GL.Begin(BeginMode.Triangles);
				foreach (Triangle triangle in _SelectedBlock.Triangles)
				{
					foreach (Point vertex in triangle.Vertices)
					{
						GL.Vertex3(vertex.X, vertex.Y, 1);
					}
				}
				GL.End();
			}

			// TODO: Alpha blending
			if (_HoverBlock != null)
			{
				GL.Color4(0, 0, 0, 128);
				GL.Begin(BeginMode.Triangles);
				foreach (Triangle triangle in _HoverBlock.Triangles)
				{
					foreach (Point vertex in triangle.Vertices)
					{
						GL.Vertex3(vertex.X, vertex.Y, 1);
					}
				}
				GL.End();
			}

			GL.PopMatrix();

			glControl.SwapBuffers();
		}

		protected override void Render()
		{
			
		}

		private void RebuildVertices()
		{
			// TODO: Vertex incremental rebuilding
			vboCount = 0;
			List<Vector3> vertexList = new List<Vector3>();
			foreach (var block in _Blocks)
			{
				foreach (Triangle triangle in block.Value.Triangles)
				{
					foreach (Point vertex in triangle.Vertices)
					{
						VBO[vboCount].R = block.Value._Color.R;
						VBO[vboCount].G = block.Value._Color.G;
						VBO[vboCount].B = block.Value._Color.B;
						VBO[vboCount].A = block.Value._Color.A;
						VBO[vboCount].Position = new Vector3(vertex.X, vertex.Y, 0);
						vboCount++;
					}
				}
			}

			Vertices = vertexList.ToArray();
		}

		public override SortedList<UInt64, VisualMemoryBlock> Blocks
		{
			set
			{
				base.Blocks = value;
				RebuildVertices();
			}
		}

		public override VisualMemoryBlock SelectedBlock
		{
			set
			{
				base.SelectedBlock = value;
			}
		}

		public override Size Size
		{
			set
			{
				base.Size = value;
			}
		}

		public override Size WorldSize
		{
			set
			{
				base.WorldSize = value;
			}
		}

		public override Point CurrentMouseLocation
		{
			set
			{
				base.CurrentMouseLocation = value;
			}
		}

		public override Point Offset
		{
			get
			{
				return base.Offset;
			}
			set
			{
				base.Offset = value;
				glControl.Invalidate();
			}
		}

		public override float Scale
		{
			get
			{
				return base.Scale;
			}
			set
			{
				base.Scale = value;
				glControl.Invalidate();
			}
		}

		public override void Update()
		{
			
		}

		protected override void Redraw()
		{
			
		}

		public override void Blit(IntPtr deviceContext)
		{
			
		}

		public override Bitmap GetMainBitmap()
		{
			return null;
		}
	}
}
