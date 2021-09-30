using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoordinatePlaneLibrary.Styles;

namespace CoordinatePlaneLibrary.Legend
{
	public class CoordinatePlaneLegend
	{
		public int ItemsCount => items.Count;
		public CoordinatePlaneLegendStyle Style { get; set; }
		private readonly List<ICoordinatePlaneLegendItem> items;

		public CoordinatePlaneLegend()
		{
			items = new List<ICoordinatePlaneLegendItem>();
			Style = new CoordinatePlaneLegendStyle();
		}

		public void Draw(float x, float y, float w, float h, int rowsCount, int columnsCount, Graphics g)
		{
			if (Style.BackgroundImage != null)
				g.DrawImage(Style.GetBackground(w, h), x, y);
			if (items.Count == 0 || (rowsCount == 0 && columnsCount == 0)) return;
			var listOfRows = new List<List<ICoordinatePlaneLegendItem>>();
			var center = new PointF(x + w / 2, y + h / 2);
			var vertical = rowsCount == 0;
			if (rowsCount == 0)
			{
				var curItem = 0;
				while (curItem < items.Count)
				{
					listOfRows.Add(new List<ICoordinatePlaneLegendItem>());
					for (var i = 0; i < columnsCount; i++)
					{
						listOfRows[listOfRows.Count - 1].Add(items[curItem]);
						curItem++;
						if (curItem >= items.Count) break;
					}
				}
			}
			else
			{
				var curItem = 0;
				columnsCount = (int)System.Math.Ceiling((double)items.Count / rowsCount);
				while (curItem < items.Count)
				{
					listOfRows.Add(new List<ICoordinatePlaneLegendItem>());
					for (var i = 0; i < columnsCount; i++)
					{
						listOfRows[listOfRows.Count - 1].Add(items[curItem]);
						curItem++;
						if (curItem >= items.Count) break;
					}
				}
			}

			var maxH = GetMinHeight();
			var totalH = maxH * listOfRows.Count;

			for (var i = 0; i < listOfRows.Count; i++)
			{
				var totalW = listOfRows[i].Sum(item => item.GetSize().Width);
				var curW = 0f;
				for (var j = 0; j < listOfRows[i].Count; j++)
				{
					var size = listOfRows[i][j].GetSize();
					listOfRows[i][j].Draw(
						center.X - totalW / 2 + curW,
						center.Y - totalH / 2 + i * maxH +
						(vertical ? 0 : size.Height / 2), g);
					curW += size.Width;
				}
			}
		}

		public void AddItem(CoordinatePointStyle style, string name)
		{
			if (items.Exists(item =>
			item.GetType() == typeof(CoordinatePlaneLegendPoint) &&
			style == ((CoordinatePlaneLegendPoint)item).PointStyle)) return;

			items.Add(new CoordinatePlaneLegendPoint(name, style, Style));
		}
		public void AddItem(CoordinateLineStyle style, string name)
		{
			if (items.Exists(item =>
			item.GetType() == typeof(CoordinatePlaneLegendLine) &&
			style == ((CoordinatePlaneLegendLine)item).LineStyle)) return;

			items.Add(new CoordinatePlaneLegendLine(name, style, Style));
		}
		public void AddItem(CoordinateVectorStyle style, string name)
		{
			if (items.Exists(item =>
			item.GetType() == typeof(CoordinatePlaneLegendVector) &&
			style == ((CoordinatePlaneLegendVector)item).VectorStyle)) return;

			items.Add(new CoordinatePlaneLegendVector(name, style, Style));
		}
		public void RemoveItem(string name)
		{
			if (items.Exists(item => item.GetName() == name))
				items.RemoveAll(item => item.GetName() == name);
		}

		public void Clear() => items.Clear();

		public float GetMinWidth() =>
			items.Select(item => item.GetSize()).Max(size => size.Width);		
		public float GetMinHeight() => 
			items.Select(item => item.GetSize()).Max(size => size.Height);
		public float GetMinWidth(out float height)
		{
			var sizes = items.Select(item => item.GetSize());
			height = sizes.Sum(s => s.Height);
			return sizes.Max(s => s.Width);
		}
		public float GetMinHeight(out float width)
		{
			var sizes = items.Select(item => item.GetSize());
			width = sizes.Sum(s => s.Width);
			return sizes.Max(s => s.Height);
		}
	}
}
