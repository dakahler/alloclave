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
	abstract class AddressSpaceRenderer : IDisposable
	{
		public virtual MemoryBlock SelectedBlock
		{
			get;
			set;
		}

		public virtual MemoryBlock HoverBlock
		{
			get;
			set;
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

		protected abstract float Width
		{
			get;
		}

		public virtual Vector CurrentMouseLocation
		{
			private get;
			set;
		}

		public Vector GetLocalMouseLocation()
		{
			return GetLocalMouseLocation(CurrentMouseLocation);
		}

		public Vector GetLocalMouseLocation(Vector worldLocation)
		{
			Vector finalPoint = worldLocation;
			finalPoint.X /= Width;
			finalPoint = finalPoint - Offset;
			finalPoint /= Scale;

			return finalPoint;
		}

		internal virtual void Rebuilt(History history)
		{
			// TODO: Too circular how history calls this and then it asks history for stuff
			if (!history.Snapshot.Contains(SelectedBlock))
			{
				SelectedBlock = null;
			}

			if (!history.Snapshot.Contains(HoverBlock))
			{
				HoverBlock = null;
			}
		}

		// Implement IDisposable. 
		// Do not make this method virtual. 
		// A derived class should not be able to override this method. 
		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method. 
			// Therefore, you should call GC.SupressFinalize to 
			// take this object off the finalization queue 
			// and prevent finalization code for this object 
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		protected abstract void Dispose(bool disposing);
	}
}
