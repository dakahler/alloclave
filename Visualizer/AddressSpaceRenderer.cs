using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloclave
{
	abstract class AddressSpaceRenderer
	{
		protected List<VisualMemoryBlock> _Blocks;
		public virtual List<VisualMemoryBlock> Blocks
		{
			set
			{
				_Blocks = value;
			}
		}

		protected VisualMemoryBlock _SelectedBlock;
		public virtual VisualMemoryBlock SelectedBlock
		{
			set
			{
				_SelectedBlock = value;
			}
		}

		protected Matrix _ViewMatrix = new Matrix();
		public virtual Matrix ViewMatrix
		{
			get
			{
				return _ViewMatrix;
			}
			set
			{
				_ViewMatrix = value;
			}
		}

		protected Size _Size = new Size(1, 1);
		public virtual Size Size
		{
			set
			{
				_Size = value;
			}
		}

		protected Size _WorldSize = new Size(1, 1);
		public virtual Size WorldSize
		{
			set
			{
				_WorldSize = value;
			}
		}

		protected Point _CurrentMouseLocation;
		public virtual Point CurrentMouseLocation
		{
			set
			{
				_CurrentMouseLocation = value;

			}
		}

		public Point GetLocalMouseLocation()
		{
			Point[] points = { _CurrentMouseLocation };

			Matrix invertedTransform = _ViewMatrix.Clone();
			invertedTransform.Invert();
			invertedTransform.TransformPoints(points);

			return points[0];
		}

		// TODO: Better name?
		protected abstract void Render();

		public abstract void Update();

		protected abstract void Redraw();

		public abstract void Blit(IntPtr deviceContext);
	}
}
