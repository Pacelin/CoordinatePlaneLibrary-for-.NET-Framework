using System.Drawing;

namespace CoordinatePlaneLibrary.Styles
{
	public class CoordinateMarkersStyle
	{
		public Font Font { get; private set; }
		public Color Color { get; private set; }
		public Color TextColor { get; private set; }
		public float LineWidth { get; private set; }
		public float LineSize { get; private set; }
		public bool IntersectOx { get; private set; }
		public bool IntersectOy { get; private set; }
		public bool DrawTextOx { get; private set; }
		public bool DrawTextOy { get; private set; }
		public MarkerXPosition MarkerXPosition { get; private set; }
		public MarkerYPosition MarkerYPosition { get; private set; }
		public Pen Pen => new Pen(Color, LineWidth);
		public Brush TextBrush => new SolidBrush(TextColor);

		public CoordinateMarkersStyle()
		{
			SetColor(Color.Black);
			SetTextColor(Color.Black);
			SetLineWidth(2);
			SetLineSize(5);
			SetFont(new Font("Times New Roman", 14));
			SetMarkerXPosition(MarkerXPosition.TopPlusBottomMinus);
			SetMarkerYPosition(MarkerYPosition.RightPlusLeftMinus);
			EnableIntersectOx();
			EnableIntersectOy();
			EnableDrawingText();
		}

		public CoordinateMarkersStyle SetColor(Color color)
		{
			Color = color;
			return this;
		}
		public CoordinateMarkersStyle SetTextColor(Color color)
		{
			TextColor = color;
			return this;
		}
		public CoordinateMarkersStyle SetLineWidth(float lineWidth)
		{
			LineWidth = lineWidth;
			return this;
		}
		public CoordinateMarkersStyle SetLineSize(float lineSize)
		{
			LineSize = lineSize;
			return this;
		}
		public CoordinateMarkersStyle SetFont(Font font)
		{
			Font = font;
			return this;
		}
		public CoordinateMarkersStyle SetMarkerXPosition(MarkerXPosition pos)
		{
			MarkerXPosition = pos;
			return this;
		}
		public CoordinateMarkersStyle SetMarkerYPosition(MarkerYPosition pos)
		{
			MarkerYPosition = pos;
			return this;
		}
		public CoordinateMarkersStyle EnableIntersectOx()
		{
			IntersectOx = true;
			return this;
		}
		public CoordinateMarkersStyle DisableIntersectOx()
		{
			IntersectOx = false;
			return this;
		}
		public CoordinateMarkersStyle EnableIntersectOy()
		{
			IntersectOy = true;
			return this;
		}
		public CoordinateMarkersStyle DisableIntersectOy()
		{
			IntersectOy = false;
			return this;
		}
		public CoordinateMarkersStyle EnableDrawingText() =>
			EnableDrawingTextOx().EnableDrawingTextOy();
		public CoordinateMarkersStyle DisableDrawingText() =>
			DisableDrawingTextOx().DisableDrawingTextOy();
		public CoordinateMarkersStyle EnableDrawingTextOx()
		{
			DrawTextOx = true;
			return this;
		}
		public CoordinateMarkersStyle DisableDrawingTextOx()
		{
			DrawTextOx = false;
			return this;
		}
		public CoordinateMarkersStyle EnableDrawingTextOy()
		{
			DrawTextOy = true;
			return this;
		}
		public CoordinateMarkersStyle DisableDrawingTextOy()
		{
			DrawTextOy = false;
			return this;
		}
	}
}
