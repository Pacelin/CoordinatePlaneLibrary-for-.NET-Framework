using CoordinatePlaneLibrary.Styles;
using System.Drawing;

namespace CoordinatePlaneLibrary
{
	public class CoordinateEllipse : ICoordinatePlaneEntity
	{
		public readonly float X, Y;
		public readonly float W, H;
		public CoordinateLineStyle Style { get; private set; }

		public CoordinateEllipse(float x, float y, float r)
		{
			X = x;
			Y = y;
			W = H = r * 2;
			Style = new CoordinateLineStyle();
		}
		public CoordinateEllipse(float x, float y, float w, float h)
		{
			X = x;
			Y = y;
			W = w;
			H = h;
			Style = new CoordinateLineStyle();
		}
		public CoordinateEllipse(float x, float y, float r, CoordinateLineStyle style)
		{
			X = x;
			Y = y;
			W = H = r * 2;
			Style = style;
		}
		public CoordinateEllipse(float x, float y, float w, float h, CoordinateLineStyle style)
		{
			X = x;
			Y = y;
			W = w;
			H = h;
			Style = style;
		}

		public CoordinateEllipse SetStyle(CoordinateLineStyle style)
		{
			if (style != null)
				Style = style;
			return this;
		}

		public void Draw(CoordinatePlane cp, Graphics g)
		{
			var x = cp.GetScaledX(X);
			var y = cp.GetScaledY(Y);
			var size = cp.GetScaledSize(W, H);
			g.DrawEllipse(Style.Pen, x - size.Width / 2, y - size.Height / 2, size.Width, size.Height);
		}

		public float GetMinX() => X - W / 2;
		public float GetMaxX() => X + W / 2;
		public float GetMinY() => Y - H / 2;
		public float GetMaxY() => Y + H / 2;
	}
}
