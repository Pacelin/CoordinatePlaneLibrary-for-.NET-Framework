using System.Drawing;
using CoordinatePlaneLibrary.Styles;

namespace CoordinatePlaneLibrary
{
	public class CoordinatePoint : ICoordinatePlaneEntity
	{
		public readonly float X, Y;
		public string Name { get; private set; }
		public CoordinatePointStyle Style { get; private set; }

		public CoordinatePoint(float x, float y)
		{
			X = x;
			Y = y;
			Style = new CoordinatePointStyle();
		}
		public CoordinatePoint(float x, float y, CoordinatePointStyle style)
		{
			X = x;
			Y = y;
			Style = style;
		}

		public CoordinatePoint WithName(string name)
		{
			Name = name;
			return this;
		}

		public CoordinatePoint SetStyle(CoordinatePointStyle style)
		{
			if (style != null)
				Style = style;
			return this;
		}

		public void Draw(CoordinatePlane cp, Graphics g)
		{
			var x = cp.GetScaledX(X);
			var y = cp.GetScaledY(Y);
			Style.DrawPoint(x, y, g);
			if (Name == "" || !Style.DrawName) return;
			var strform = new StringFormat()
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			};
			var right = 
				Style.PositionOfNameType == CornerPositionType.RightTop ||
				Style.PositionOfNameType == CornerPositionType.RightBottom;
			var bottom =
				Style.PositionOfNameType == CornerPositionType.LeftBottom ||
				Style.PositionOfNameType == CornerPositionType.RightBottom;

			g.DrawString(Name, Style.Font, Style.TextBrush,
				new RectangleF(x - (right ? 0 : 30), y - (bottom ? 0 : 30), 30, 30), strform);
		}

		public float GetMinX() => X;
		public float GetMaxX() => X;
		public float GetMinY() => Y;
		public float GetMaxY() => Y;
	}
}
