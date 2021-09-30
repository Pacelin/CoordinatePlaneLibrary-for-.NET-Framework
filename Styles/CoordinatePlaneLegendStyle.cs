using System.Drawing;

namespace CoordinatePlaneLibrary.Styles
{
	public class CoordinatePlaneLegendStyle
	{
		public Font Font { get; set; }
		public Color TextColor { get; set; }
		public float LineSize { get; set; }
		public Brush TextBrush => new SolidBrush(TextColor);
		
		public Image BackgroundImage { get; set; }

		public CoordinatePlaneLegendStyle()
		{
			SetFont(new Font("Times New Roman", 14));
			SetTextColor(Color.Black);
			SetLineSize(30);

			BackgroundImage = null;
		}

		public Bitmap GetBackground(float width, float height) => 
			new Bitmap(BackgroundImage, (int) width, (int) height);

		public CoordinatePlaneLegendStyle SetTextColor(Color color)
		{
			TextColor = color;
			return this;
		}
		public CoordinatePlaneLegendStyle SetFont(Font font)
		{
			Font = font;
			return this;
		}
		public CoordinatePlaneLegendStyle SetLineSize(float size)
		{
			LineSize = size;
			return this;
		}
		public CoordinatePlaneLegendStyle SetLegendBackground(Bitmap bg)
		{
			BackgroundImage = bg;
			return this;
		}
	}
}
