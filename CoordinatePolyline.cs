using System.Linq;
using System.Drawing;
using CoordinatePlaneLibrary.Styles;

namespace CoordinatePlaneLibrary
{
	public class CoordinatePolyline : ICoordinatePlaneEntity
	{
		public readonly CoordinatePoint[] Points;
		public CoordinateLineStyle Style { get; private set; }

		public CoordinatePolyline(CoordinatePoint[] points)
		{
			Points = points;
			Style = new CoordinateLineStyle();
		}
		public CoordinatePolyline(CoordinatePoint[] points, CoordinateLineStyle style)
		{
			Points = points;
			Style = style;
		}

		public CoordinatePolyline SetStyle(CoordinateLineStyle style)
		{
			if (style != null)
				Style = style;
			return this;
		}

		public void Draw(CoordinatePlane cp, Graphics g)
		{
			if (Style.DrawPoints) Points.ToList().ForEach(p => p.Draw(cp, g));
			g.DrawLines(Style.Pen, Points.Select(p => new PointF(cp.GetScaledX(p.X), cp.GetScaledY(p.Y))).ToArray());
		}

		public float GetMinX() => Points.Min(p => p.X);
		public float GetMaxX() => Points.Max(p => p.X);
		public float GetMinY() => Points.Min(p => p.Y);
		public float GetMaxY() => Points.Max(p => p.Y);
	}
}