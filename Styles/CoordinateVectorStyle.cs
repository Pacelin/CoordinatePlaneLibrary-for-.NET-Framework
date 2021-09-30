using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CoordinatePlaneLibrary.Styles
{
	public class CoordinateVectorStyle
	{
		public Font Font { get; private set; }
		public Color TextColor { get; private set; }
		public bool DrawName { get; private set; }
		public CornerPositionType PositionOfNameType { get; private set; }
		public Color Color { get; private set; }
		public Color ArrowColor { get; private set; }
		public float ArrowSize { get; private set; }
		public float LineWidth { get; private set; }
		public bool DrawFirstPoint { get; private set; }
		public DashStyle LineType { get; private set; }
		public LineCap LineStartCap { get; private set; }
		public DashCap DashCap { get; private set; }
		public LineCap LineEndCap { get; private set; }
		public Pen Pen => new Pen(Color, LineWidth) { DashStyle = LineType, DashCap = DashCap, StartCap = LineStartCap, EndCap = LineEndCap };
		public Pen ArrowPen => new Pen(ArrowColor, LineWidth) { StartCap = LineStartCap, EndCap = LineEndCap };
		public Brush TextBrush => new SolidBrush(TextColor);

		public CoordinateVectorStyle()
		{
			SetColor(Color.Black);
			SetLineWidth(2);
			DisableDrawingFirstPoint();
			EnableDrawingName();
			SetFont(new Font("Times New Roman", 14));
			SetTextColor(Color.Black);
			SetArrowColor(Color.Black);
			SetNamePosition(CornerPositionType.RightTop);
			SetLineType(DashStyle.Solid);
			SetLineCap(LineCap.Flat, LineCap.Flat);
			SetDashCap(DashCap.Flat);
			SetArrowSize(10);
		}

		public void DrawVector(float xFrom, float yFrom, float xTo, float yTo, Graphics g)
		{
			g.DrawLine(Pen, xFrom, yFrom, xTo, yTo);

			var v0 = new PointF(xTo - xFrom, yTo - yFrom);
			var c = (float)Math.Sqrt(Math.Pow(v0.X, 2) + Math.Pow(v0.Y, 2));
			var normalized = new PointF(v0.Y / c, v0.X / c);
			var a1 = new PointF((float)Math.Sin(Math.Asin(normalized.Y) + Math.PI / 8), (float)Math.Cos(Math.Acos(normalized.X) + Math.PI / 8));
			var a2 = new PointF((float)Math.Sin(Math.Asin(normalized.Y) - Math.PI / 8), (float)Math.Cos(Math.Acos(normalized.X) - Math.PI / 8));
			g.DrawLines(ArrowPen,
				new[]
				{
					new PointF(xTo - a1.X * ArrowSize, yTo - a1.Y * ArrowSize),
					new PointF(xTo, yTo),
					new PointF(xTo - a2.X * ArrowSize, yTo - a2.Y * ArrowSize)
				});
		}
		public CoordinateVectorStyle SetNamePosition(CornerPositionType pos)
		{
			PositionOfNameType = pos;
			return this;
		}
		public CoordinateVectorStyle SetFont(Font font)
		{
			Font = font;
			return this;
		}
		public CoordinateVectorStyle SetTextColor(Color color)
		{
			TextColor = color;
			return this;
		}
		public CoordinateVectorStyle SetArrowColor(Color color)
		{
			ArrowColor = color;
			return this;
		}
		public CoordinateVectorStyle SetLineCap(LineCap start, LineCap end)
		{
			LineStartCap = start;
			LineEndCap = end;
			return this;
		}
		public CoordinateVectorStyle SetDashCap(DashCap cap)
		{
			DashCap = cap;
			return this;
		}
		public CoordinateVectorStyle SetLineType(DashStyle type)
		{
			LineType = type;
			return this;
		}
		public CoordinateVectorStyle SetColor(Color color)
		{
			Color = color;
			return this;
		}
		public CoordinateVectorStyle SetLineWidth(float lineWidth)
		{
			LineWidth = lineWidth;
			return this;
		}
		public CoordinateVectorStyle SetArrowSize(float arrowSize)
		{
			ArrowSize = arrowSize;
			return this;
		}
		public CoordinateVectorStyle EnableDrawingFirstPoint()
		{
			DrawFirstPoint = true;
			return this;
		}
		public CoordinateVectorStyle DisableDrawingFirstPoint()
		{
			DrawFirstPoint = false;
			return this;
		}
		public CoordinateVectorStyle EnableDrawingName()
		{
			DrawName = true;
			return this;
		}
		public CoordinateVectorStyle DisableDrawingName()
		{
			DrawName = false;
			return this;
		}
	}
}