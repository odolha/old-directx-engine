//Class: Technique
//Author: Ovidiu Dolha
//Description:   -
//               A class that would contain information about how is a Surface constructed (k*l templates that are tiled over the surface, for vertices position shift, and for tiles triangle orientation). It's used at the UpdateGeometry function in Surface class


using System;
using System.Drawing;


namespace Direct3DAid.Surfaces
{
	public class Technique
	{
		//Stores the size of a tile from the surface
		private SizeF _TileSize; 
		public SizeF TileSize 
		{ 
			get 
			{ 
				return this._TileSize; 
			} 
			set 
			{ 
				this._TileSize = value;
				if (this.Modified != null) this.Modified(TechniqueModifications.TileSize);
			} 
		} 


	
		//Hatching used on the i*j tile from the surface portion
		private HatchingMode[,] _TileHatching;
		public HatchingMode GetTileHatching(int x, int y)
		{
			return this._TileHatching[x,y];
		}
		public int GetTileHatchingWidth()
		{
			return this._TileHatching.GetLength(0);
		}
		public int GetTileHatchingHeight()
		{
			return this._TileHatching.GetLength(1);
		}
		public void SetTileHatching(int x, int y, HatchingMode val)
		{
			if (!Misc.WithinBounds(x,y,this._TileHatching.GetLength(0),this._TileHatching.GetLength(1),-1,-1))
				throw new ExceptionSurface("Point (" + x + "," + y + ") is not within TileHatching bounds!");
			this._TileHatching[x,y] = val;
			if (this.Modified != null) this.Modified(TechniqueModifications.TileHatching);
		}


	
		//Shift (horizontal, and vertical) used on the i*j point from the surface portion
		//It is unrecomended that your shifting displaces the point outside the size of a tile, and the results might be unexpected!
		private PointF[,] _PointShift;
		public PointF GetPointShift(int x, int y)
		{
			return this._PointShift[x,y];
		}
		public int GetPointShiftWidth()
		{
			return this._PointShift.GetLength(0);
		}
		public int GetPointShiftHeight()
		{
			return this._PointShift.GetLength(1);
		}
		public void SetPointShift(int x, int y, PointF val)
		{
			if (!Misc.WithinBounds(x,y,this._PointShift.GetLength(0),this._PointShift.GetLength(1),-1,-1))
				throw new ExceptionSurface("Point (" + x + "," + y + ") is not within PointShoft bounds!");
			this._PointShift[x,y] = val;
			if (this.Modified != null) this.Modified(TechniqueModifications.PointShift);
		}





		//EVENTS AND DELEGATES

		//Alerts when it is modified. It provides info about what aspects were modified
		public delegate void ModifiedHandler(TechniqueModifications modifications);
		public event ModifiedHandler Modified;






		//Class Constructor
		public Technique(SizeF tileSize, Point tiles, Point points)
		{
			this._TileSize = tileSize; 
			this._TileHatching = new HatchingMode[tiles.X,tiles.Y];
			this._PointShift = new PointF[points.X,points.Y];
		}





		//Detects shift used from technique
		public ShiftModes DetectShiftMode()
		{
			ShiftModes shiftm = ShiftModes.None;

			PointF p;

			for (int i=0; i<=this.GetPointShiftWidth()-1; i++)
			{
				for (int j=0; j<=this.GetPointShiftHeight()-1; j++)
				{
					p = this.GetPointShift(i,j);

					if (p.X != 0)
					{
						shiftm = shiftm | ShiftModes.Horizontal;
					}
					if (p.Y != 0)
					{
						shiftm = shiftm | ShiftModes.Vertical;
					}
				}
			}

			return shiftm;
		}





		//---
		//DEFAULT OBJECTS
		//---
		
		//Default Rectangled Simple Surface (no shifts)
		public static Technique DefaultRectangledSimple(SizeF tileSize, HatchingMode hatching)
		{
			Technique t;
			t = new Technique(tileSize,new Point(1,1),new Point(1,1));
			t.SetTileHatching(0,0,hatching);
			t.SetPointShift(0,0,new PointF(0,0));

			return t;
		}


		//Default Rectangled Alternated Surface (no shifts) / hatching00 represents the hatching of tile (0,0). the rest are alternated to this
		public static Technique DefaultRectangledAlternated(SizeF tileSize, HatchingMode hatchingOnTile00)
		{
			Technique t;
			t = new Technique(tileSize,new Point(2,2),new Point(1,1));
			t.SetTileHatching(0,0,hatchingOnTile00);
			t.SetTileHatching(0,1,(HatchingMode)(1-(int)hatchingOnTile00)); //1-x does bool negation
			t.SetTileHatching(1,0,(HatchingMode)(1-(int)hatchingOnTile00)); // --''--
			t.SetTileHatching(1,1,hatchingOnTile00);
            
			t.SetPointShift(0,0,new PointF(0,0));

			return t;
		}


		//Default Triangled Surface (shifts used to make the surface triangle-oriented)
		public static Technique DefaultTriangledSimple(SizeF tileSize, bool shiftOnRow0)
		{
			Technique t;
			t = new Technique(tileSize,new Point(1,2),new Point(1,2));
			if (shiftOnRow0==false)
			{
				t.SetTileHatching(0,0,HatchingMode.NWSE);
				t.SetTileHatching(0,1,HatchingMode.NESW);
				t.SetPointShift(0,0,new PointF(0,0));
				t.SetPointShift(0,1,new PointF(+tileSize.Width/2,0));
			}
			else
			{
				t.SetTileHatching(0,0,HatchingMode.NESW);
				t.SetTileHatching(0,1,HatchingMode.NWSE);
				t.SetPointShift(0,0,new PointF(+tileSize.Width/2,0));
				t.SetPointShift(0,1,new PointF(0,0));
			}

			return t;
		}

		//Default Equilateral Triangled Surface (shifts used to make the surface triangle-oriented)
		public static Technique DefaultTriangledEquilateral(float tileWidth, bool shiftOnRow0)
		{
			return DefaultTriangledSimple(new SizeF(tileWidth,(float)((Math.Sqrt(3)/2)*tileWidth)),shiftOnRow0);
		}




	}

}