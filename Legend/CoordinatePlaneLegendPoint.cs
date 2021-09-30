using System.Drawing;
using CoordinatePlaneLibrary.Styles;

namespace CoordinatePlaneLibrary.Legend
{
	public class CoordinatePlaneLegendPoint : ICoordinatePlaneLegendItem
	{
		public CoordinatePlaneLegendStyle Style { get; set; }
		public CoordinatePointStyle PointStyle { get; set; }

		private string name;

		public CoordinatePlaneLegendPoint(string name, CoordinatePointStyle pointStyle, CoordinatePlaneLegendStyle legendStyle)
		{
			this.name = name;
			PointStyle = pointStyle;
			Style = legendStyle;
		}

		public void Draw(float x, float y, Graphics g)
		{
			var size = GetSize(out var textSize);
			g.DrawString(name, Style.Font, Style.TextBrush, x + 10, y, 
				new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
			PointStyle.DrawPoint(x + 10 + textSize.Width + 5 + PointStyle.Size / 2, y, g);
		}

		public SizeF GetSize(out Size textSize)
		{
			textSize = System.Windows.Forms.TextRenderer.MeasureText(name, Style.Font);
			return new SizeF(10 + textSize.Width + 5 + PointStyle.Size + 10,
				System.Math.Max(PointStyle.Size, textSize.Height) + 20);
		}
		public SizeF GetSize() => GetSize(out var t);
		public string GetName() => name;
	}
}
