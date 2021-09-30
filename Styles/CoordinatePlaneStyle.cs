using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CoordinatePlaneLibrary.Styles
{
	public class CoordinatePlaneStyle
	{
		public CoordinatePlaneLegendStyle LegendStyle
		{
			get => cp.Legend.Style;
			set { if (value != null) cp.Legend.Style = value; }
		}
		public LegendType LegendType { get; private set; }

		public CoordinateIntervalMarkersStyle IntervalMarkersStyle { get; set; }

		public CoordinatePointStyle DefaultPointStyle { get; set; }
		public CoordinatePointStyle DefaultNamedPointStyle { get; set; }
		public CoordinateLineStyle DefaultLineStyle { get; set; }
		public CoordinateLineStyle DefaultPolylineStyle { get; set; }
		public CoordinateMarkersStyle DefaultMarkerStyle { get; set; }
		public CoordinateVectorStyle DefaultVectorStyle { get; set; }
		public CoordinateVectorStyle DefaultNamedVectorStyle { get; set; }

		public CoordinateVectorStyle OxOyStyle { get; set; }
		public CoordinatePointStyle ZeroPointStyle { get; set; }
		
		public Image BackgroundImage { get; private set; }

		public string OxName { get; private set; }
		public string OyName { get; private set; }

		public bool IntervalMarkersXEnabled { get; private set; }
		public bool IntervalMarkersYEnabled { get; private set; }

		public Font CaptionFont { get; private set; }
		public Color CaptionColor { get; private set; }
		public Brush CaptionBrush => new SolidBrush(CaptionColor);

		public float OffsetElements { get; private set; }

		private CoordinatePlane cp;

		public CoordinatePlaneStyle(CoordinatePlane cp)
		{
			this.cp = cp;
			IntervalMarkersStyle = new CoordinateIntervalMarkersStyle();
			DefaultLineStyle = new CoordinateLineStyle();
			DefaultPolylineStyle = new CoordinateLineStyle();
			DefaultMarkerStyle = new CoordinateMarkersStyle();
			DefaultPointStyle = new CoordinatePointStyle().DisableDrawingName();
			DefaultVectorStyle = new CoordinateVectorStyle().DisableDrawingName();

			DefaultNamedPointStyle = new CoordinatePointStyle().EnableDrawingName();
			DefaultNamedVectorStyle = new CoordinateVectorStyle().EnableDrawingName();

			OxOyStyle = new CoordinateVectorStyle().EnableDrawingName();
			ZeroPointStyle = new CoordinatePointStyle().EnableDrawingName().SetSize(7);
			OxName = "X";
			OyName = "Y";

			CaptionFont = new Font("Times New Roman", 14, FontStyle.Bold);
			CaptionColor = Color.Black;
			OffsetElements = 20;

			SetBackgroundFillColor(Color.White);
		}

		public CoordinatePlaneStyle SetAllTextColor(Color color)
		{
			IntervalMarkersStyle.MarkersStyle.SetTextColor(color);
			DefaultMarkerStyle.SetTextColor(color);
			DefaultNamedPointStyle.SetTextColor(color);
			DefaultNamedVectorStyle.SetTextColor(color);
			SetCaptionColor(color);
			OxOyStyle.SetTextColor(color);
			ZeroPointStyle.SetTextColor(color);
			LegendStyle.SetTextColor(color);
			return this;
		}
		public CoordinatePlaneStyle SetAllColor(Color color)
		{
			IntervalMarkersStyle.LinesXStyle.SetColor(Color.FromArgb(40, color));
			IntervalMarkersStyle.LinesYStyle.SetColor(Color.FromArgb(40, color));
			IntervalMarkersStyle.MarkersStyle.SetColor(color);
			DefaultLineStyle.SetColor(color);
			DefaultMarkerStyle.SetColor(color);
			DefaultNamedPointStyle.SetColor(color);
			DefaultNamedVectorStyle.SetColor(color).SetArrowColor(color);
			DefaultPointStyle.SetColor(color);
			DefaultPolylineStyle.SetColor(color);
			DefaultVectorStyle.SetColor(color);
			OxOyStyle.SetColor(color).SetArrowColor(color);
			ZeroPointStyle.SetColor(color);
			return this;
		}

		public CoordinatePlaneStyle EnableLegendTop()
		{
			LegendType = LegendType.Top;
			return this;
		}
		public CoordinatePlaneStyle EnableLegendBottom()
		{
			LegendType = LegendType.Bottom;
			return this;
		}
		public CoordinatePlaneStyle EnableLegendLeft()
		{
			LegendType = LegendType.Left;
			return this;
		}
		public CoordinatePlaneStyle EnableLegendRight()
		{
			LegendType = LegendType.Right;
			return this;
		}
		public CoordinatePlaneStyle DisableLegend()
		{
			LegendType = LegendType.Right;
			return this;
		}

		public CoordinatePlaneStyle SetBackgroundFillColor(Color color)
		{
			BackgroundImage = new Bitmap(1920, 1080);
			var g = Graphics.FromImage(BackgroundImage);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.FillRectangle(new SolidBrush(color), 0, 0, 1920, 1080);
			g.Dispose();
			return this;
		}

		public CoordinatePlaneStyle SetOxOyName(string nameX, string nameY) =>
			SetOxName(nameX).SetOyName(nameY);
		public CoordinatePlaneStyle SetOxName(string name)
		{
			OxName = name;
			return this;
		}
		public CoordinatePlaneStyle SetOyName(string name)
		{
			OyName = name;
			return this;
		}
		public CoordinatePlaneStyle SetOffset(float offset)
		{
			OffsetElements = offset;
			return this;
		}
		public CoordinatePlaneStyle SetCaptionColor(Color color)
		{
			CaptionColor = color;
			return this;
		}
		public CoordinatePlaneStyle SetCaptionFont(Font font)
		{
			CaptionFont = font;
			return this;
		}
		public CoordinatePlaneStyle EnableIntervalMarkers() => EnableIntervalMarkersX().EnableIntervalMarkersY();
		public CoordinatePlaneStyle DisableIntervalMarkers() => DisableIntervalMarkersX().DisableIntervalMarkersY();
		public CoordinatePlaneStyle EnableIntervalMarkersX()
		{
			IntervalMarkersXEnabled = true;
			return this;
		}
		public CoordinatePlaneStyle DisableIntervalMarkersX() 
		{
			IntervalMarkersXEnabled = false;
			return this;
		}
		public CoordinatePlaneStyle EnableIntervalMarkersY() 
		{
			IntervalMarkersYEnabled = true;
			return this;
		}
		public CoordinatePlaneStyle DisableIntervalMarkersY()
		{
			IntervalMarkersYEnabled = false;
			return this;
		}

		public Bitmap GetBackground(int width, int height) => new Bitmap(BackgroundImage, width, height);

		public CoordinatePoint GetZeroPoint() =>
			new CoordinatePoint(0, 0, ZeroPointStyle).WithName("0");
		public CoordinateVector GetOx(float xFrom, float xTo) =>
			new CoordinateVector(GetDefaultStyledPoint(xFrom, 0), GetDefaultStyledPoint(xTo, 0), OxOyStyle).WithName(OxName);
		public CoordinateVector GetOy(float yFrom, float yTo) =>
			new CoordinateVector(GetDefaultStyledPoint(0, yFrom), GetDefaultStyledPoint(0, yTo), OxOyStyle).WithName(OyName);

		public CoordinatePoint GetDefaultStyledPoint(float x, float y) =>
			new CoordinatePoint(x, y, DefaultPointStyle);
		public CoordinatePoint GetDefaultStyledPoint(float x, float y, string name) =>
			new CoordinatePoint(x, y, DefaultNamedPointStyle).WithName(name);
		public CoordinateLine GetDefaultStyledLine(float x1, float y1, float x2, float y2) =>
			new CoordinateLine(GetDefaultStyledPoint(x1, y1), GetDefaultStyledPoint(x2, y2), DefaultLineStyle);
		public CoordinateLine GetDefaultStyledLine(CoordinatePoint p1, CoordinatePoint p2) =>
			new CoordinateLine(p1, p2, DefaultLineStyle);
		public CoordinatePolyline GetDefaultStyledPolyline(CoordinatePoint[] points) =>
			new CoordinatePolyline(points, DefaultPolylineStyle);
		public CoordinatePolyline GetDefaultStyledPolyline(float[] xs, float[] ys) =>
			new CoordinatePolyline(xs.Select((x, i) => GetDefaultStyledPoint(x, ys[i])).ToArray(), DefaultPolylineStyle);
		public CoordinatePolyline GetDefaultStyledPolyline(PointF[] points) =>
			new CoordinatePolyline(points.Select(p => GetDefaultStyledPoint(p.X, p.Y)).ToArray(), DefaultPolylineStyle);
		public CoordinateVector GetDefaultStyledVector(float xFrom, float yFrom, float xTo, float yTo, string name) =>
			new CoordinateVector(GetDefaultStyledPoint(xFrom, yFrom), GetDefaultStyledPoint(xTo, yTo), DefaultNamedVectorStyle).WithName(name);
		public CoordinateVector GetDefaultStyledVector(float xFrom, float yFrom, float xTo, float yTo) =>
			new CoordinateVector(GetDefaultStyledPoint(xFrom, yFrom), GetDefaultStyledPoint(xTo, yTo), DefaultVectorStyle);
		public CoordinateVector GetDefaultStyledVector(CoordinatePoint from, CoordinatePoint to) =>
			new CoordinateVector(from, to, DefaultVectorStyle);
		public CoordinateVector GetDefaultStyledVector(CoordinatePoint from, CoordinatePoint to, string name) =>
			new CoordinateVector(from, to, DefaultNamedVectorStyle).WithName(name);
		public CoordinateMarkerX GetDefaultStyledMarkerX(float x) =>
			new CoordinateMarkerX(x, DefaultMarkerStyle);
		public CoordinateMarkerY GetDefaultStyledMarkerY(float y) =>
			new CoordinateMarkerY(y, DefaultMarkerStyle);
		public CoordinateFunction GetDefaultStyledFunction(System.Func<float, float> func, float minX, float maxX, float minY, float maxY) =>
			new CoordinateFunction(func, minX, maxX, minY, maxY, DefaultLineStyle, DefaultPointStyle);

		public List<ICoordinatePlaneEntity> GetIntervalMarkers(float fromX, float toX, float fromY, float toY)
		{
			var res = new List<ICoordinatePlaneEntity>();
			if (IntervalMarkersXEnabled && IntervalMarkersStyle.StepX > 0)
			{
				for (var x = -IntervalMarkersStyle.StepX; x > fromX; x -= IntervalMarkersStyle.StepX)
				{
					if (IntervalMarkersStyle.MarkersXEnabled)
						res.Add(new CoordinateMarkerX(x, IntervalMarkersStyle.MarkersStyle));
					if (IntervalMarkersStyle.LinesXEnabled)
						res.Add(new CoordinateLine(GetDefaultStyledPoint(x, fromY), GetDefaultStyledPoint(x, toY),
							IntervalMarkersStyle.LinesXStyle));
				}
				for (var x = IntervalMarkersStyle.StepX; x < toX; x += IntervalMarkersStyle.StepX)
				{
					if (IntervalMarkersStyle.MarkersXEnabled)
						res.Add(new CoordinateMarkerX(x, IntervalMarkersStyle.MarkersStyle));
					if (IntervalMarkersStyle.LinesXEnabled)
						res.Add(new CoordinateLine(GetDefaultStyledPoint(x, fromY), GetDefaultStyledPoint(x, toY),
							IntervalMarkersStyle.LinesXStyle));
				}
			}
			
			if (IntervalMarkersYEnabled && IntervalMarkersStyle.StepY > 0)
			{
				for (var y = -IntervalMarkersStyle.StepY; y > fromY; y -= IntervalMarkersStyle.StepY)
				{
					if (IntervalMarkersStyle.MarkersYEnabled)
						res.Add(new CoordinateMarkerY(y, IntervalMarkersStyle.MarkersStyle));
					if (IntervalMarkersStyle.LinesYEnabled)
						res.Add(new CoordinateLine(GetDefaultStyledPoint(fromX, y), GetDefaultStyledPoint(toX, y),
							IntervalMarkersStyle.LinesYStyle));
				}
				for (var y = IntervalMarkersStyle.StepY; y < toY; y += IntervalMarkersStyle.StepY)
				{
					if (IntervalMarkersStyle.MarkersYEnabled)
						res.Add(new CoordinateMarkerY(y, IntervalMarkersStyle.MarkersStyle));
					if (IntervalMarkersStyle.LinesYEnabled)
						res.Add(new CoordinateLine(GetDefaultStyledPoint(fromX, y), GetDefaultStyledPoint(toX, y),
							IntervalMarkersStyle.LinesYStyle));
				}
			}
			return res;
		}
	}
}