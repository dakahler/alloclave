using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Alloclave
{
	class VisualMemoryBox
	{
		public readonly Rectangle DefaultBox = new Rectangle(0, 0, 20, 20);
		public Matrix Transform = new Matrix();
	}
}
