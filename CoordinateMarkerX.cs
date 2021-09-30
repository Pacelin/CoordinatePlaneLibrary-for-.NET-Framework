using CoordinatePlaneLibrary.Styles;
using System.Drawing;

namespace CoordinatePlaneLibrary
{
	public class CoordinateMarkerX : ICoordinatePlaneEntity
	{
		public readonly float X;
		public CoordinateMarkersStyle Style;

		public CoordinateMarkerX(float x)
		{
			X = x;
			Style = new CoordinateMarkersStyle();
		}
		public CoordinateMarkerX(float x, CoordinateMarkersStyle style)
		{
			X = x;
			Style = style;
		}

		public CoordinateMarkerX SetStyle(CoordinateMarkersStyle style)
		{
			if (style != null)
				Style = style;
			return this;
		}

		public void Draw(CoordinatePlane cp, Graphics g)
		{
			var x = cp.GetScaledX(X);
			var y = cp.GetScaledY(0);
			var top = (X > 0 && Style.MarkerXPosition == MarkerXPosition.TopPlusBottomMinus) ||
				(X < 0 && Style.MarkerXPosition == MarkerXPosition.BottomPlusTopMinus) ||
				Style.MarkerXPosition == MarkerXPosition.Top;

			if (Style.IntersectOx)
				g.DrawLine(Style.Pen, x, y - Style.LineSize, x, y + Style.LineSize);
			else
			{
				if (top)
					g.DrawLine(Style.Pen, x, y - Style.LineSize, x, y);
				else
					g.DrawLine(Style.Pen, x, y, x, y + Style.LineSize);
			}
			
			if (!Style.DrawTextOx) return;
			
			var strform = new StringFormat() { Alignment = StringAlignment.Center };
			strform.LineAlignment = top ? StringAlignment.Far : StringAlignment.Near;

			g.DrawString(X.ToString("0.##"), Style.Font, Style.TextBrush,
				x, top ? y - Style.LineSize - 5 : y + Style.LineSize + 5, strform);
		}

		public float GetMinX() => X;
		public float GetMaxX() => X;
		public float GetMinY() => 0;
		public float GetMaxY() => 0;
	}
}
