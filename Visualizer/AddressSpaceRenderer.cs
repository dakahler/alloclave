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
		protected VisualMemoryBlock _SelectedBlock;
		public virtual VisualMemoryBlock SelectedBlock
		{
			get
			{
				return _SelectedBlock;
			}
			set
			{
				_SelectedBlock = value;
			}
		}

		protected VisualMemoryBlock _HoverBlock;
		public virtual VisualMemoryBlock HoverBlock
		{
			set
			{
				_HoverBlock = value;
			}
		}

		protected Point _Offset = new Point();
		public virtual Point Offset
		{
			get
			{
				return _Offset;
			}
			set
			{
				_Offset = value;
			}
		}

		protected float _Scale = 1.0f;
		public virtual float Scale
		{
			get
			{
				return _Scale;
			}
			set
			{
				_Scale = value;
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
			return GetLocalMouseLocation(_CurrentMouseLocation);
		}

		public Point GetLocalMouseLocation(Point worldLocation)
		{
			Point finalPoint = worldLocation;
			finalPoint = Point.Subtract(finalPoint, new Size(Offset));
			finalPoint = new Point((int)((float)finalPoint.X / Scale), (int)((float)finalPoint.Y / Scale));

			return finalPoint;
		}

		// TODO: Better name?
		protected abstract void Render();
	}
}
