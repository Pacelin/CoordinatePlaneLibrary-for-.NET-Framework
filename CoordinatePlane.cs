using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CoordinatePlaneLibrary.Legend;
using CoordinatePlaneLibrary.Styles;

namespace CoordinatePlaneLibrary
{
	public class CoordinatePlane : PictureBox
	{
		public string Caption { get; private set; }
		public RectangleF ScaledBounds { get; private set; }
		public RectangleF PixelBounds { get; private set; }
		public PointF ScalePoint { get; private set; }
		public CoordinatePlaneStyle Style { get; private set; }
		public CoordinatePlaneLegend Legend { get; private set; }

		private BindingList<ICoordinatePlaneEntity> entities = new BindingList<ICoordinatePlaneEntity>();

		private bool locked;

		public CoordinatePlane()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
			DoubleBuffered = true;
			PixelBounds = new RectangleF(30, 30, Width - 60, Height - 60);
			locked = false;
			Style = new CoordinatePlaneStyle(this);
			Legend = new CoordinatePlaneLegend();
			Caption = "";

			entities.ListChanged += Entities_ListChanged;
			base.Paint += OnPaint;
			base.SizeChanged += ControlSizeChanged;
		}

		#region Caption Settings
		public CoordinatePlane EnableCaption(string caption)
		{
			Caption = caption;
			return this;
		}
		public CoordinatePlane DiasbleCaption(string caption)
		{
			Caption = caption;
			return this;
		}
		#endregion

		#region Add Elements
		public CoordinatePlane AddPoint(CoordinatePoint p)
		{
			entities.Add(p);
			return this;
		}
		public CoordinatePlane AddPoints(CoordinatePoint[] points)
		{
			if (locked)
				foreach (var point in points) entities.Add(point);
			else
			{
				LockUpdate();
				foreach (var point in points) entities.Add(point);
				UnlockUpdate();
			}
			return this;
		}
		public CoordinatePlane AddLine(CoordinateLine l)
		{
			entities.Add(l);
			return this;
		}
		public CoordinatePlane AddPolyLine(CoordinatePolyline l)
		{
			entities.Add(l);
			return this;
		}
		public CoordinatePlane AddVector(CoordinateVector v)
		{
			entities.Add(v);
			return this;
		}
		public CoordinatePlane AddFunction(CoordinateFunction f)
		{
			entities.Add(f);
			return this;
		}

		#region Add Legend Items
		public CoordinatePlane AddLegendItem(CoordinateLineStyle style, string name)
		{
			Legend.AddItem(style, name);
			UpdatePlane();
			return this;
		}
		public CoordinatePlane AddLegendItem(CoordinatePointStyle style, string name)
		{
			Legend.AddItem(style, name);
			UpdatePlane();
			return this;
		}
		public CoordinatePlane AddLegendItem(CoordinateVectorStyle style, string name)
		{
			Legend.AddItem(style, name);
			UpdatePlane();
			return this;
		}
		public CoordinatePlane RemoveLegendItem(string name)
		{
			Legend.RemoveItem(name);
			UpdatePlane();
			return this;
		}
		public CoordinatePlane ClearLegend()
		{
			Legend.Clear();
			return this;
		}
		#endregion

		#region Add Default Elements
		public CoordinatePlane AddPoint(float x, float y)
		{
			entities.Add(Style.GetDefaultStyledPoint(x, y));
			return this;
		}
		public CoordinatePlane AddPoints(float[] xs, float[] ys)
		{
			var points = xs.Select((x, i) => Style.GetDefaultStyledPoint(x, ys[i]));
			if (locked)
			{
				foreach (var point in points) entities.Add(point);
			}
			else
			{
				LockUpdate();
				foreach (var point in points) entities.Add(point);
				UnlockUpdate();
			}
			return this;
		}
		public CoordinatePlane AddPoints(PointF[] points)
		{
			var ps = points.Select(p => Style.GetDefaultStyledPoint(p.X, p.Y));
			if (locked)
			{
				foreach (var point in ps) entities.Add(point);
			}
			else
			{
				LockUpdate();
				foreach (var point in ps) entities.Add(point);
				UnlockUpdate();
			}
			return this;
		}
		public CoordinatePlane AddLine(CoordinatePoint p1, CoordinatePoint p2)
		{
			entities.Add(Style.GetDefaultStyledLine(p1, p2));
			return this;
		}
		public CoordinatePlane AddLine(PointF p1, PointF p2)
		{
			entities.Add(Style.GetDefaultStyledLine(p1.X, p1.Y, p2.X, p2.Y));
			return this;
		}
		public CoordinatePlane AddLine(float x1, float y1, float x2, float y2)
		{
			entities.Add(Style.GetDefaultStyledLine(x1, y1, x2, y2));
			return this;
		}
		public CoordinatePlane AddPolyLine(float[] xs, float[] ys)
		{
			entities.Add(Style.GetDefaultStyledPolyline(xs, ys));
			return this;
		}
		public CoordinatePlane AddPolyLine(PointF[] points)
		{
			entities.Add(Style.GetDefaultStyledPolyline(points));
			return this;
		}
		public CoordinatePlane AddPolyLine(CoordinatePoint[] points)
		{
			entities.Add(Style.GetDefaultStyledPolyline(points));
			return this;
		}
		public CoordinatePlane AddVector(float xFrom, float yFrom, float xTo, float yTo)
		{
			entities.Add(Style.GetDefaultStyledVector(xFrom, yFrom, xTo, yTo));
			return this;
		}
		public CoordinatePlane AddVector(PointF from, PointF to)
		{
			entities.Add(Style.GetDefaultStyledVector(from.X, from.Y, to.X, to.Y));
			return this;
		}
		public CoordinatePlane AddVector(CoordinatePoint from, CoordinatePoint to)
		{
			entities.Add(Style.GetDefaultStyledVector(from, to));
			return this;
		}
		public CoordinatePlane AddFunction(Func<float, float> func, float minX, float maxX, float minY, float maxY)
		{
			entities.Add(Style.GetDefaultStyledFunction(func, minX, maxX, minY, maxY));
			return this;
		}
		public CoordinatePlane AddFunction(Func<float, float> func)
		{
			entities.Add(new CoordinateFunction(func, Style.DefaultLineStyle, Style.DefaultPointStyle));
			return this;
		}
		#endregion

		#region Add Styled Elements
		public CoordinatePlane AddPoint(float x, float y, CoordinatePointStyle style)
		{
			entities.Add(new CoordinatePoint(x, y, style));
			return this;
		}
		public CoordinatePlane AddPoints(float[] xs, float[] ys, CoordinatePointStyle style)
		{
			var points = xs.Select((x, i) => new CoordinatePoint(x, ys[i], style));
			if (locked)
			{
				foreach (var point in points) entities.Add(point);
			}
			else
			{
				LockUpdate();
				foreach (var point in points) entities.Add(point);
				UnlockUpdate();
			}
			return this;
		}
		public CoordinatePlane AddPoints(PointF[] points, CoordinatePointStyle style)
		{
			var ps = points.Select(p => new CoordinatePoint(p.X, p.Y, style));
			if (locked)
			{
				foreach (var point in ps) entities.Add(point);
			}
			else
			{
				LockUpdate();
				foreach (var point in ps) entities.Add(point);
				UnlockUpdate();
			}
			return this;
		}
		public CoordinatePlane AddLine(CoordinatePoint p1, CoordinatePoint p2, CoordinateLineStyle style)
		{
			entities.Add(new CoordinateLine(p1, p2, style));
			return this;
		}
		public CoordinatePlane AddLine(PointF p1, PointF p2, CoordinateLineStyle style)
		{
			entities.Add(new CoordinateLine(Style.GetDefaultStyledPoint(p1.X, p1.Y), Style.GetDefaultStyledPoint(p2.X, p2.Y), style));
			return this;
		}
		public CoordinatePlane AddLine(float x1, float y1, float x2, float y2, CoordinateLineStyle style)
		{
			entities.Add(new CoordinateLine(Style.GetDefaultStyledPoint(x1, y1), Style.GetDefaultStyledPoint(x2, y2), style));
			return this;
		}
		public CoordinatePlane AddPolyLine(float[] xs, float[] ys, CoordinateLineStyle style)
		{
			entities.Add(new CoordinatePolyline(xs.Select((x, i) => Style.GetDefaultStyledPoint(x, ys[i])).ToArray(), style));
			return this;
		}
		public CoordinatePlane AddPolyLine(PointF[] points, CoordinateLineStyle style)
		{
			entities.Add(new CoordinatePolyline(points.Select(p => Style.GetDefaultStyledPoint(p.X, p.Y)).ToArray(), style));
			return this;
		}
		public CoordinatePlane AddPolyLine(CoordinatePoint[] points, CoordinateLineStyle style)
		{
			entities.Add(new CoordinatePolyline(points, style));
			return this;
		}
		public CoordinatePlane AddVector(float xFrom, float yFrom, float xTo, float yTo, CoordinateVectorStyle style)
		{
			entities.Add(new CoordinateVector(Style.GetDefaultStyledPoint(xFrom, yFrom), Style.GetDefaultStyledPoint(xTo, yTo), style));
			return this;
		}
		public CoordinatePlane AddVector(PointF from, PointF to, CoordinateVectorStyle style)
		{
			entities.Add(new CoordinateVector(Style.GetDefaultStyledPoint(from.X, from.Y), Style.GetDefaultStyledPoint(to.X, to.Y), style));
			return this;
		}
		public CoordinatePlane AddVector(CoordinatePoint from, CoordinatePoint to, CoordinateVectorStyle style)
		{
			entities.Add(new CoordinateVector(from, to, style));
			return this;
		}
		public CoordinatePlane AddFunction(Func<float, float> func, float minX, float maxX, float minY, float maxY, CoordinateLineStyle style)
		{
			entities.Add(new CoordinateFunction(func, minX, maxX, minY, maxY, style, Style.DefaultPointStyle));
			return this;
		}
		public CoordinatePlane AddFunction(Func<float, float> func, float minX, float maxX, float minY, float maxY, CoordinateLineStyle style, CoordinatePointStyle pointsStyle)
		{
			entities.Add(new CoordinateFunction(func, minX, maxX, minY, maxY, style, pointsStyle));
			return this;
		}
		public CoordinatePlane AddFunction(Func<float, float> func, CoordinateLineStyle style)
		{
			entities.Add(new CoordinateFunction(func, style, Style.DefaultPointStyle));
			return this;
		}
		public CoordinatePlane AddFunction(Func<float, float> func, CoordinateLineStyle style, CoordinatePointStyle pointsStyle)
		{
			entities.Add(new CoordinateFunction(func, style, pointsStyle));
			return this;
		}
		public CoordinatePlane AddEllipse(float x, float y, float r, CoordinateLineStyle style)
		{
			entities.Add(new CoordinateEllipse(x, y, r, style));
			return this;
		}
		public CoordinatePlane AddEllipse(float x, float y, float w, float h, CoordinateLineStyle style)
		{
			entities.Add(new CoordinateEllipse(x, y, w, h, style));
			return this;
		}
		#endregion

		#region Markers Adding
		public CoordinatePlane AddMarkerX(CoordinateMarkerX m)
		{
			entities.Add(m);
			return this;
		}
		public CoordinatePlane AddMarkerX(float x)
		{
			entities.Add(Style.GetDefaultStyledMarkerX(x));
			return this;
		}
		public CoordinatePlane AddMarkerY(CoordinateMarkerY m)
		{
			entities.Add(m);
			return this;
		}
		public CoordinatePlane AddMarkerY(float y)
		{
			entities.Add(Style.GetDefaultStyledMarkerY(y));
			return this;
		}
		public CoordinatePlane AddMarkers(CoordinateMarkerX mx, CoordinateMarkerY my)
		{
			entities.Add(mx);
			entities.Add(my);
			return this;
		}
		public CoordinatePlane AddMarkers(PointF p)
		{
			entities.Add(Style.GetDefaultStyledMarkerX(p.X));
			entities.Add(Style.GetDefaultStyledMarkerY(p.Y));
			return this;
		}
		public CoordinatePlane AddMarkers(float x, float y)
		{
			entities.Add(Style.GetDefaultStyledMarkerX(x));
			entities.Add(Style.GetDefaultStyledMarkerY(y));
			return this;
		}
		public CoordinatePlane AddMarkers(CoordinatePoint p)
		{
			entities.Add(Style.GetDefaultStyledMarkerX(p.X));
			entities.Add(Style.GetDefaultStyledMarkerY(p.Y));
			return this;
		}
		#endregion

		public CoordinatePlane Clear()
		{
			entities.Clear();
			return this;
		}
		#endregion

		#region Update
		public void UpdatePlane()
		{
			if (entities.Count <= 0 || locked) return;
			Refresh();
		}
		public void LockUpdate() => locked = true;
		public void UnlockUpdate()
		{
			if (!locked) return;
			locked = false;
			UpdatePlane();
		}
		#endregion

		public float GetScaledX(float x) => PixelBounds.Left + ScalePoint.X * (x - ScaledBounds.Left);
		public float GetScaledY(float y) => PixelBounds.Top + ScalePoint.Y * (ScaledBounds.Bottom - y);
		public SizeF GetScaledSize(SizeF size) => GetScaledSize(size.Width, size.Height);
		public SizeF GetScaledSize(float w, float h) => new SizeF(
			w / ScaledBounds.Width * PixelBounds.Width,
			h / ScaledBounds.Height * PixelBounds.Height);

		public PointF GetScaledPoint(PointF point) => new PointF(
			PixelBounds.Left + ScalePoint.X * (point.X - ScaledBounds.Left),
			PixelBounds.Top + ScalePoint.Y * (ScaledBounds.Bottom - point.Y));

		#region Private Methods
		private void OnPaint(object sender, PaintEventArgs args)
		{
			if (entities.Count <= 0 || locked) return;

			var g = args.Graphics;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.DrawImage(Style.GetBackground(Width, Height), 0, 0);

			var minX = Math.Min(0, entities.Min(e => e.GetMinX()));
			var maxX = Math.Max(0, entities.Max(e => e.GetMaxX()));
			var minY = Math.Min(0, entities.Min(e => e.GetMinY()));
			var maxY = Math.Max(0, entities.Max(e => e.GetMaxY()));

			var width = maxX - minX;
			var height = maxY - minY;

			PixelBounds = new RectangleF(30, 30,
				Width - 60, Height - 60);

			if (Caption != "")
			{
				var textSize = TextRenderer.MeasureText(g, Caption, Style.CaptionFont, 
					new Size((int)PixelBounds.Width, (int)(PixelBounds.Height * 0.15f)),
					TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
				PixelBounds = new RectangleF(PixelBounds.X, PixelBounds.Y + Style.OffsetElements + textSize.Height, PixelBounds.Width, PixelBounds.Height - Style.OffsetElements - textSize.Height);
				g.DrawString(Caption, Style.CaptionFont, Style.CaptionBrush,
					new RectangleF(PixelBounds.X, PixelBounds.Y - Style.OffsetElements - textSize.Height, PixelBounds.Width, textSize.Height),
					new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			}
			if (Style.LegendType != LegendType.None && Legend.ItemsCount > 0)
			{
				var minH = Legend.GetMinHeight(out var w);
				var minW = Legend.GetMinWidth(out var h);
				var maxHorizontalSize = new SizeF(PixelBounds.Width, PixelBounds.Height * 0.15f);
				var maxVerticalSize = new SizeF(PixelBounds.Width * 0.2f, PixelBounds.Height);
				if (Style.LegendType == LegendType.Bottom || Style.LegendType == LegendType.Top)
				{
					var rowsCount = 1;
					while (w > maxHorizontalSize.Width)
					{
						w -= minW * (Legend.ItemsCount + Legend.ItemsCount % 2) / 2;
						rowsCount++;
					}
					if (Style.LegendType == LegendType.Bottom)
					{
						PixelBounds = new RectangleF(PixelBounds.X, PixelBounds.Y,
							PixelBounds.Width, PixelBounds.Height - rowsCount * minH - Style.OffsetElements);
						Legend.Draw(PixelBounds.X, PixelBounds.Y + PixelBounds.Height + Style.OffsetElements,
							PixelBounds.Width, rowsCount * minH, rowsCount, 0, g);
					}
					else
					{
						PixelBounds = new RectangleF(PixelBounds.X, PixelBounds.Y + rowsCount * minH + Style.OffsetElements,
							PixelBounds.Width, PixelBounds.Height - rowsCount * minH - Style.OffsetElements);
						Legend.Draw(PixelBounds.X, PixelBounds.Y - rowsCount * minH - Style.OffsetElements, PixelBounds.Width,
							rowsCount * minH, rowsCount, 0, g);
					}
				}
				else if (Style.LegendType == LegendType.Left || Style.LegendType == LegendType.Right)
				{
					var columnsCount = 1;
					while (h > maxVerticalSize.Height)
					{
						h -= minH * (Legend.ItemsCount + Legend.ItemsCount % 2) / 2;
						columnsCount++;
					}
					if (Style.LegendType == LegendType.Left)
					{
						PixelBounds = new RectangleF(PixelBounds.X + columnsCount * minW + Style.OffsetElements, PixelBounds.Y,
							PixelBounds.Width - columnsCount * minW - Style.OffsetElements, PixelBounds.Height);
						Legend.Draw(PixelBounds.X - columnsCount * minW - Style.OffsetElements, PixelBounds.Y, 
							columnsCount * minW, PixelBounds.Height, 0, columnsCount, g);
					}
					else
					{
						PixelBounds = new RectangleF(PixelBounds.X, PixelBounds.Y,
							PixelBounds.Width - columnsCount * minW - Style.OffsetElements, PixelBounds.Height);
						Legend.Draw(PixelBounds.X + PixelBounds.Width + Style.OffsetElements, PixelBounds.Y,
							columnsCount * minW, PixelBounds.Height, 0, columnsCount, g);
					}
				}
			}

			if (Style.OxName != "")
			{
				var oxNameSize = TextRenderer.MeasureText(Style.OxName, Style.OxOyStyle.Font);
				if (Width - PixelBounds.X - PixelBounds.Width - 60 < oxNameSize.Width)
					PixelBounds = new RectangleF(PixelBounds.X, PixelBounds.Y,
						PixelBounds.Width - oxNameSize.Width, PixelBounds.Height);
			}
			if (Style.OyName != "")
			{
				var oyNameSize = TextRenderer.MeasureText(Style.OyName, Style.OxOyStyle.Font);
				PixelBounds = new RectangleF(PixelBounds.X, PixelBounds.Y,
					PixelBounds.Width, PixelBounds.Height);
			}
			ScaledBounds = new RectangleF(minX - width / 10, minY - height / 10, width + width / 5, height + height / 5);
			ScalePoint = new PointF(PixelBounds.Width / ScaledBounds.Width, PixelBounds.Height / ScaledBounds.Height);

			Style.GetIntervalMarkers(ScaledBounds.Left, ScaledBounds.Right, ScaledBounds.Top, ScaledBounds.Bottom)
				.ForEach(e => e.Draw(this, g));

			Style.GetZeroPoint().Draw(this, g);
			Style.GetOx(ScaledBounds.Left, ScaledBounds.Right).Draw(this, g);
			Style.GetOy(ScaledBounds.Top, ScaledBounds.Bottom).Draw(this, g);
			
			entities.ToList().ForEach(e => e.Draw(this, g));
		}

		private void ControlSizeChanged(object sender, EventArgs e)
		{
			PixelBounds = new RectangleF(20, 20, Width - 40, Height - 40);
			UpdatePlane();
		}
		private void Entities_ListChanged(object sender, ListChangedEventArgs e) => UpdatePlane();
		#endregion
	}
}