using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

		protected Vector _Offset = new Vector();
		public virtual Vector Offset
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

		protected Vector _CurrentMouseLocation;
		public virtual Vector CurrentMouseLocation
		{
			set
			{
				_CurrentMouseLocation = value;

			}
		}

		public Vector GetLocalMouseLocation()
		{
			return GetLocalMouseLocation(_CurrentMouseLocation);
		}

		public Vector GetLocalMouseLocation(Vector worldLocation)
		{
			Vector finalPoint = worldLocation;
			finalPoint = finalPoint - Offset;
			finalPoint /= Scale;

			return finalPoint;
		}

		// TODO: Better name?
		protected abstract void Render();
	}
}
