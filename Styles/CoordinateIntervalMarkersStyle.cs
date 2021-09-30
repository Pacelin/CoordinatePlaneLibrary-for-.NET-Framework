using System.Drawing;

namespace CoordinatePlaneLibrary.Styles
{
	public class CoordinateIntervalMarkersStyle
	{
		public CoordinateMarkersStyle MarkersStyle { get; set; }
		public CoordinateLineStyle LinesXStyle { get; set; }
		public CoordinateLineStyle LinesYStyle { get; set; }
		public float StepX { get; private set; }
		public float StepY { get; private set; }
		public bool LinesXEnabled { get; private set; }
		public bool LinesYEnabled { get; private set; }
		public bool MarkersXEnabled { get; private set; }
		public bool MarkersYEnabled { get; private set; }


		public CoordinateIntervalMarkersStyle()
		{
			MarkersStyle = new CoordinateMarkersStyle()
				.SetMarkerXPosition(MarkerXPosition.TopPlusBottomMinus)
				.SetMarkerYPosition(MarkerYPosition.RightPlusLeftMinus)
				.EnableDrawingText();
			LinesXStyle = new CoordinateLineStyle()
				.SetLineWidth(1)
				.SetColor(Color.FromArgb(40, 0, 0, 0));
			LinesYStyle = new CoordinateLineStyle()
				.SetLineWidth(1)
				.SetColor(Color.FromArgb(40, 0, 0, 0));
			SetStep(1);
			EnableMarkers();
			DisableLines();
		}

		public CoordinateIntervalMarkersStyle SetMarkersStyle(CoordinateMarkersStyle style)
		{
			if (style != null)
				MarkersStyle = style;
			return this;
		}
		public CoordinateIntervalMarkersStyle SetLinesXStyle(CoordinateLineStyle style)
		{
			if (style != null)
				LinesXStyle = style;
			return this;
		}
		public CoordinateIntervalMarkersStyle SetLinesYStyle(CoordinateLineStyle style)
		{
			if (style != null)
				LinesYStyle = style;
			return this;
		}

		public CoordinateIntervalMarkersStyle SetStep(float step) =>
			SetStepX(step).SetStepY(step);
		public CoordinateIntervalMarkersStyle SetStepX(float step)
		{
			StepX = step;
			return this;
		}
		public CoordinateIntervalMarkersStyle SetStepY(float step)
		{
			StepY = step;
			return this;
		}
		public CoordinateIntervalMarkersStyle EnableLines() =>
			EnableLinesX().EnableLinesY();
		public CoordinateIntervalMarkersStyle EnableLinesX()
		{
			LinesXEnabled = true;
			return this;
		}
		public CoordinateIntervalMarkersStyle EnableLinesY()
		{
			LinesYEnabled = true;
			return this;
		}
		public CoordinateIntervalMarkersStyle DisableLines() =>
			DisableLinesX().DisableLinesY();
		public CoordinateIntervalMarkersStyle DisableLinesX()
		{
			LinesXEnabled = false;
			return this;
		}
		public CoordinateIntervalMarkersStyle DisableLinesY()
		{
			LinesYEnabled = false;
			return this;
		}
		public CoordinateIntervalMarkersStyle EnableMarkers() =>
			EnableMarkersX().EnableMarkersY();
		public CoordinateIntervalMarkersStyle EnableMarkersX()
		{
			MarkersXEnabled = true;
			return this;
		}
		public CoordinateIntervalMarkersStyle EnableMarkersY()
		{
			MarkersYEnabled = true;
			return this;
		}
		public CoordinateIntervalMarkersStyle DisableMarkers() =>
			DisableMarkersX().DisableMarkersY();
		public CoordinateIntervalMarkersStyle DisableMarkersX()
		{
			MarkersXEnabled = false;
			return this;
		}
		public CoordinateIntervalMarkersStyle DisableMarkersY()
		{
			MarkersYEnabled = false;
			return this;
		}
	}
}
