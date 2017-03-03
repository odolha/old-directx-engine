//GLOBAL ENUMS ARE DESCRIBED HERE
//~TODO: might move each enum into its rightfull class


namespace Direct3DAid.Surfaces
{
	//hatching of a surface tile
	public enum HatchingMode
	{
		NWSE=0,
		NESW=1
	};
	
	//shift mode of points in a surface
	public enum ShiftModes
	{
		AutoDetect = -1,
		None = 0,
		Horizontal = 1,
		Vertical = 2,
		Both = 3
	}



	//information of modifications for a ControlPoint
	public enum ControlPointModifications
	{
		None = 0,
		Height = 1,
		Color = 2,
		Division = 4,
		All = 7
	}


	//information of modifications for a Technique
	public enum TechniqueModifications
	{
		None = 0,
		TileSize = 1,
		TileHatching = 2,
		PointShift = 4,
		All = 7
	}


}
	
	
	
namespace Direct3DAid.Mobility
{
	//3D axis flags
	public enum Axis3 
	{ 
		X = 1, 
		Y = 2, 
		Z = 4 
	} 


}


