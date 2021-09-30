namespace CoordinatePlaneLibrary.Legend
{
	public interface ICoordinatePlaneLegendItem
	{
		void Draw(float x, float y, System.Drawing.Graphics g);
		System.Drawing.SizeF GetSize(out System.Drawing.Size textSize);
		System.Drawing.SizeF GetSize();
		string GetName();
	}
}
