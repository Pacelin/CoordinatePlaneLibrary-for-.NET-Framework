using System.Drawing;
using System.Drawing.Drawing2D;

namespace CoordinatePlaneLibrary.Styles
{
	public class CoordinateLineStyle
	{
		public Color Color { get; private set; }
		public float LineWidth { get; private set; }
		public bool DrawPoints { get; private set; }
		public DashStyle LineType { get; private set; }
		public LineCap LineStartCap { get; private set; }
		public DashCap DashCap { get; private set; }
		public LineCap LineEndCap { get; private set; }
		public Pen Pen => new Pen(Color, LineWidth) { DashStyle = LineType, DashCap = DashCap, StartCap = LineStartCap, EndCap = LineEndCap };

		public CoordinateLineStyle()
		{
			SetColor(Color.Black);
			SetLineWidth(2);
			DisableDrawingPoints();
			SetLineType(DashStyle.Solid);
			SetLineCap(LineCap.Flat, LineCap.Flat);
			SetDashCap(DashCap.Flat);
		}
		public void DrawLine(float x1, float y1, float x2, float y2, Graphics g) =>
			g.DrawLine(Pen, x1, y1,	x2, y2);

		public CoordinateLineStyle SetLineCap(LineCap start, LineCap end)
		{
			LineStartCap = start;
			LineEndCap = end;
			return this;
		}
		public CoordinateLineStyle SetDashCap(DashCap cap)
		{
			DashCap = cap;
			return this;
		}
		public CoordinateLineStyle SetLineType(DashStyle type)
		{
			LineType = type;
			return this;
		}
		public CoordinateLineStyle SetColor(Color color)
		{
			Color = color;
			return this;
		}
		public CoordinateLineStyle SetLineWidth(float lineWidth)
		{
			LineWidth = lineWidth;
			return this;
		}
		public CoordinateLineStyle EnableDrawingPoints()
		{
			DrawPoints = true;
			return this;
		}
		public CoordinateLineStyle DisableDrawingPoints()
		{
			DrawPoints = false;
			return this;
		}

		public CoordinateLineStyle Clone() =>
			new CoordinateLineStyle()
			.SetColor(Color)
			.SetDashCap(DashCap)
			.SetLineCap(LineStartCap, LineEndCap)
			.SetLineType(LineType)
			.SetLineWidth(LineWidth);
	}
}