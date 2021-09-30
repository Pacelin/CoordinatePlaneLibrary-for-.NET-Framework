using System;
using System.Drawing;
using CoordinatePlaneLibrary.Styles;

namespace CoordinatePlaneLibrary
{
	public class CoordinateLine : ICoordinatePlaneEntity
	{
		public readonly CoordinatePoint P1, P2;
		public CoordinateLineStyle Style { get; private set; }

		public CoordinateLine(CoordinatePoint p1, CoordinatePoint p2)
		{
			P1 = p1;
			P2 = p2;
			Style = new CoordinateLineStyle();
		}
		public CoordinateLine(CoordinatePoint p1, CoordinatePoint p2, CoordinateLineStyle style)
		{
			P1 = p1;
			P2 = p2;
			Style = style;
		}

		public CoordinateLine SetStyle(CoordinateLineStyle style)
		{
			if (style != null)
				Style = style;
			return this;
		}

		public void Draw(CoordinatePlane cp, Graphics g)
		{
			Style.DrawLine(
				cp.GetScaledX(P1.X), cp.GetScaledY(P1.Y),
				cp.GetScaledX(P2.X), cp.GetScaledY(P2.Y), g);

			if (Style.DrawPoints) 
			{
				P1.Draw(cp, g);
				P2.Draw(cp, g);
			}
		}

		public float GetMinX() => Math.Min(P1.X, P2.X);
		public float GetMaxX() => Math.Max(P1.X, P2.X);
		public float GetMinY() => Math.Min(P1.Y, P2.Y);
		public float GetMaxY() => Math.Max(P1.Y, P2.Y);

	}
}
