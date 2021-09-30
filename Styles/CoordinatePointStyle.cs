using System.Drawing;

namespace CoordinatePlaneLibrary.Styles
{
	public class CoordinatePointStyle
	{
		public float Size { get; private set; }
		public Color Color { get; private set; }
		public Color TextColor { get; private set; }
		public Font Font { get; private set; }
		public CornerPositionType PositionOfNameType { get; private set; }
		public bool DrawName { get; private set; }
		public PointStyle PointStyle { get; private set; }
		public float LineWidth { get; private set; }
		public Brush Brush => new SolidBrush(Color);
		public Brush TextBrush => new SolidBrush(TextColor);
		public Pen Pen => new Pen(Color, LineWidth);

		public CoordinatePointStyle()
		{
			SetSize(5);
			SetColor(Color.Black);
			SetTextColor(Color.Black);
			SetFont(new Font("Times New Roman", 14));
			SetNamePosition(CornerPositionType.RightTop);
			DisableDrawingName();
			SetStyle(PointStyle.Point);
			SetLineWidth(2);
		}

		public void DrawPoint(float x, float y, Graphics g)
		{
			switch (PointStyle)
			{
				case PointStyle.Point:
					g.FillEllipse(Brush,
						x - Size / 2, y - Size / 2,
						Size, Size);
					break;
				case PointStyle.X:
					g.DrawLine(Pen,
						x - Size / 2, y - Size / 2,
						x + Size / 2, y + Size / 2);
					g.DrawLine(Pen,
						x - Size / 2, y + Size / 2,
						x + Size / 2, y - Size / 2);
					break;
				case PointStyle.O:
					g.DrawEllipse(Pen,
						x - Size / 2, y - Size / 2,
						Size, Size);
					break;
			}
		}

		public CoordinatePointStyle SetNamePosition(CornerPositionType pos)
		{
			PositionOfNameType = pos;
			return this;
		}
		public CoordinatePointStyle SetFont(Font font)
		{
			Font = font;
			return this;
		}
		public CoordinatePointStyle SetColor(Color color)
		{
			Color = color;
			return this;
		}
		public CoordinatePointStyle SetTextColor(Color color)
		{
			TextColor = color;
			return this;
		}
		public CoordinatePointStyle SetSize(float size)
		{
			Size = size;
			return this;
		}
		public CoordinatePointStyle SetLineWidth(float lineWidth)
		{
			LineWidth = lineWidth;
			return this;
		}
		public CoordinatePointStyle SetStyle(PointStyle style)
		{
			PointStyle = style;
			return this;
		}
		public CoordinatePointStyle EnableDrawingName()
		{
			DrawName = true;
			return this;
		}
		public CoordinatePointStyle DisableDrawingName()
		{
			DrawName = false;
			return this;
		}

		public CoordinatePointStyle Clone() =>
			new CoordinatePointStyle() { DrawName = this.DrawName }
			.SetSize(Size)
			.SetColor(Color)
			.SetTextColor(TextColor)
			.SetFont((Font) Font.Clone())
			.SetNamePosition(PositionOfNameType)
			.SetStyle(PointStyle)
			.SetLineWidth(LineWidth);
	}
}