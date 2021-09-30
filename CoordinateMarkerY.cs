using CoordinatePlaneLibrary.Styles;
using System.Drawing;

namespace CoordinatePlaneLibrary
{
	public class CoordinateMarkerY : ICoordinatePlaneEntity
	{
		public readonly float Y;
		public CoordinateMarkersStyle Style;

		public CoordinateMarkerY(float y, CoordinateMarkersStyle style)
		{
			Y = y;
			Style = style;
		}
		public CoordinateMarkerY(float y)
		{
			Y = y;
			Style = new CoordinateMarkersStyle();
		}

		public CoordinateMarkerY SetStyle(CoordinateMarkersStyle style)
		{
			if (style != null)
				Style = style;
			return this;
		}

		public void Draw(CoordinatePlane cp, Graphics g)
		{
			var x = cp.GetScaledX(0);
			var y = cp.GetScaledY(Y);
			var left = (Y > 0 && Style.MarkerYPosition == MarkerYPosition.LeftPlusRightMinus) ||
				(Y < 0 && Style.MarkerYPosition == MarkerYPosition.RightPlusLeftMinus) ||
				Style.MarkerYPosition == MarkerYPosition.Left;

			if (Style.IntersectOy) 
				g.DrawLine(Style.Pen, x - Style.LineSize, y, x + Style.LineSize, y);
			else
			{
				if (left)
					g.DrawLine(Style.Pen, x - Style.LineSize, y, x, y);
				else
					g.DrawLine(Style.Pen, x, y, x + Style.LineSize, y);
			}
			if (!Style.DrawTextOy) return;
			var strform = new StringFormat() { LineAlignment = StringAlignment.Center };
			strform.Alignment = left ? StringAlignment.Far : StringAlignment.Near;
			g.DrawString(Y.ToString("0.##"), Style.Font, Style.TextBrush, 
				left ? x - Style.LineSize - 5 : x + Style.LineSize + 5, y, strform);
		}

		public float GetMinX() => 0;
		public float GetMaxX() => 0;
		public float GetMinY() => Y;
		public float GetMaxY() => Y;
	}
}