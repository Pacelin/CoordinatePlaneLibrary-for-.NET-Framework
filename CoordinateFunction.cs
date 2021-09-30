using CoordinatePlaneLibrary.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CoordinatePlaneLibrary
{
	public class CoordinateFunction : ICoordinatePlaneEntity
	{
		public readonly Func<float, float> Function;

		public CoordinatePoint[][] Lines { get; private set; }
		public CoordinateLineStyle Style { get; private set; }
		public CoordinatePointStyle PointsStyle { get; private set; }

		private float minX = 1, maxX = -1, minY, maxY;

		public CoordinateFunction(Func<float, float> func, float minX, float maxX, float minY, float maxY)
		{
			Function = func;
			Style = new CoordinateLineStyle();
			PointsStyle = new CoordinatePointStyle();
			this.minX = minX;
			this.maxX = maxX;
			this.minY = minY;
			this.maxY = maxY;
			Lines = CalculateLines(func, minX, maxX, minY, maxY);
		}
		public CoordinateFunction(Func<float, float> func, float minX, float maxX, float minY, float maxY, CoordinateLineStyle style)
		{
			Function = func;
			Style = style;
			PointsStyle = new CoordinatePointStyle();
			this.minX = minX;
			this.maxX = maxX;
			this.minY = minY;
			this.maxY = maxY;
			Lines = CalculateLines(func, minX, maxX, minY, maxY);
		}
		public CoordinateFunction(Func<float, float> func, float minX, float maxX, float minY, float maxY, CoordinateLineStyle style, CoordinatePointStyle pointsStyle)
		{
			Function = func;
			Style = style;
			PointsStyle = pointsStyle;
			this.minX = minX;
			this.maxX = maxX;
			this.minY = minY;
			this.maxY = maxY;
			Lines = CalculateLines(func, minX, maxX, minY, maxY);
		}
		public CoordinateFunction(Func<float, float> func, CoordinateLineStyle style, CoordinatePointStyle pointsStyle)
		{
			Function = func;
			Style = style;
			PointsStyle = pointsStyle;
		}

		public CoordinateFunction SetStyle(CoordinateLineStyle style)
		{
			if (style != null)
				Style = style;
			return this;
		}

		public void Draw(CoordinatePlane cp, Graphics g)
		{
			if (minX > maxX)
				Lines = CalculateLines(Function, cp.ScaledBounds.Left, cp.ScaledBounds.Right, cp.ScaledBounds.Bottom, cp.ScaledBounds.Top);
			foreach (var line in Lines)
			{
				g.DrawLines(Style.Pen, line.Select(p => new PointF(cp.GetScaledX(p.X), cp.GetScaledY(p.Y))).ToArray());
				if (Style.DrawPoints) line.ToList().ForEach(p => p.Draw(cp, g));
			}
		}
		public float GetMinX() => minX;
		public float GetMaxX() => maxX;
		public float GetMinY() => minY;
		public float GetMaxY() => maxY;

		private CoordinatePoint[][] CalculateLines(Func<float, float> func, float minX, float maxX, float minY, float maxY)
		{
			var lines = new List<List<CoordinatePoint>>();
			lines.Add(new List<CoordinatePoint>());
			for (var x = minX; x <= maxX; x += 0.01f)
			{
				try
				{
					var y = func(x);
					if (maxX > y || y > minX)
					{
						lines.Add(new List<CoordinatePoint>());
						continue;
					}
					lines.Last().Add(new CoordinatePoint(x, y, PointsStyle));
				}
				catch
				{
					lines.Add(new List<CoordinatePoint>());
				}
			}
			return lines.Select(line => line.ToArray()).ToArray();
		}
	}
}
