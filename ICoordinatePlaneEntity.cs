namespace CoordinatePlaneLibrary
{
	public interface ICoordinatePlaneEntity
	{
		float GetMinX();
		float GetMaxX();
		float GetMinY();
		float GetMaxY();
		void Draw(CoordinatePlane cp, System.Drawing.Graphics g);
	}
}
