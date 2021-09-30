using CoordinatePlaneLibrary.Styles;
using System.Drawing;

namespace CoordinatePlaneLibrary.Legend
{
	public class CoordinatePlaneLegendVector : ICoordinatePlaneLegendItem
	{
		public CoordinatePlaneLegendStyle Style { get; set; }
		public CoordinateVectorStyle VectorStyle { get; set; }

		private string name;

		public CoordinatePlaneLegendVector(string name, CoordinateVectorStyle vectorStyle, CoordinatePlaneLegendStyle legendStyle)
		{
			this.name = name;
			VectorStyle = vectorStyle;
			Style = legendStyle;
		}

		public void Draw(float x, float y, Graphics g)
		{
			var size = GetSize(out var textSize);
			g.DrawString(name, Style.Font, Style.TextBrush, x + 10, y + size.Height / 2,
				new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
			VectorStyle.DrawVector(
				x + 10 + textSize.Width + 5, y,
				x + 10 + textSize.Width + 5 + Style.LineSize, y, g);
		}

		public SizeF GetSize(out Size textSize)
		{
			textSize = System.Windows.Forms.TextRenderer.MeasureText(name, Style.Font);
			return new SizeF(10 + textSize.Width + 5 + Style.LineSize + 10,
				System.Math.Max(VectorStyle.LineWidth, textSize.Height) + 20);
		}
		public SizeF GetSize() => GetSize(out var t);
		public string GetName() => name;
	}
}
