using CoordinatePlaneLibrary.Styles;
using System;
using System.Drawing;

namespace CoordinatePlaneLibrary
{
	public class CoordinateVector : ICoordinatePlaneEntity
	{
		public readonly CoordinatePoint From, To;
		public string Name { get; private set; }
		public CoordinateVectorStyle Style { get; private set; }

		public CoordinateVector(CoordinatePoint from, CoordinatePoint to)
		{
			From = from;
			To = to;
			Style = new CoordinateVectorStyle();
		}
		public CoordinateVector(CoordinatePoint from, CoordinatePoint to, CoordinateVectorStyle style)
		{
			From = from;
			To = to;
			Style = style;
		}

		public CoordinateVector WithName(string name)
		{
			Name = name;
			return this;
		}

		public CoordinateVector SetStyle(CoordinateVectorStyle style) 
		{
			if (style != null)
				Style = style;
			return this;
		}

		public void Draw(CoordinatePlane cp, Graphics g)
		{
			var fromScaled = new PointF(cp.GetScaledX(From.X), cp.GetScaledY(From.Y));
			var toScaled = new PointF(cp.GetScaledX(To.X), cp.GetScaledY(To.Y));

			Style.DrawVector(fromScaled.X, fromScaled.Y, toScaled.X, toScaled.Y, g);

			if (Style.DrawFirstPoint)
				From.Draw(cp, g);

			if (Name == "" || !Style.DrawName) return;
			var nameSize = System.Windows.Forms.TextRenderer.MeasureText(Name, Style.Font);
			var strform = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

			var x = Style.PositionOfNameType == CornerPositionType.LeftBottom ||
				Style.PositionOfNameType == CornerPositionType.LeftTop ? toScaled.X - nameSize.Width : toScaled.X;
			var y = Style.PositionOfNameType == CornerPositionType.LeftTop ||
				Style.PositionOfNameType == CornerPositionType.RightTop ? toScaled.Y - nameSize.Height : toScaled.Y;
			g.DrawString(Name, Style.Font, Style.TextBrush,
				new RectangleF(x, y, nameSize.Width, nameSize.Height), strform);
		}

		public float GetMinX() => Math.Min(From.X, To.X);
		public float GetMaxX() => Math.Max(From.X, To.X);
		public float GetMinY() => Math.Min(From.Y, To.Y);
		public float GetMaxY() => Math.Max(From.Y, To.Y);
	}
}
