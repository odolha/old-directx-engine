//Class: SurfaceXY
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//               A class that would handle simple functionalities like: storing and editing a surface's points and various functions for using it as a walking surface;
//               Surface technique used: XY oriented, with Z as height, costumizable k*l triangle orientation and k*l shift controls (meaning that a k*l tile can be costumized.. the triangles in the tile and the shift of k*l points)


using System;
using System.Drawing;
using Microsoft.DirectX; 
using Microsoft.DirectX.Direct3D; 

using Direct3DAid.Geometries;


namespace Direct3DAid.Surfaces
{
	public class Surface
	{ 
		//Handles the size of the surface in vertices
		private Size _Size;
		public Size Size 
		{ 
			get 
			{ 
				return this._Size;
			} 
			set 
			{ 
				this._ControlPoint = new ControlPoint[value.Width, value.Height]; 
				this._Size=value;
			} 
		} 



		//The internal array of the surface. Contains point information about each vertex in the surface
		//Information is hold from 0 to Size-1 on each dimension
		//This property is handled with methods, because it is a property for a certain point in the array. You cannot obtain the array, only one point from within. And by modifying it some events are called. These events can be used to determine when an object that uses a Surface should update
		private ControlPoint[,] _ControlPoint; 
		public ControlPoint GetControlPoint(int x, int y)
		{
			return this._ControlPoint[x,y];
		}
		public void SetControlPoint(int x, int y, ControlPoint val)
		{
			ControlPoint old = this.GetControlPoint(x,y);
			this._ControlPoint[x,y] = val;
			if (this.ControlPointModified != null) this.ControlPointModified(x,y,ControlPointModifications.All,old);
		}
		public void SetControlPoint(int x, int y, float height, int color, uint division)
		{
			ControlPoint old = this.GetControlPoint(x,y);
			this._ControlPoint[x,y].Height = height;
			this._ControlPoint[x,y].Color = color;
			this._ControlPoint[x,y].Division = division;
			if (this.ControlPointModified != null) this.ControlPointModified(x,y,ControlPointModifications.All,old);
		}
		public void SetControlPointHeight(int x, int y, float height)
		{
			ControlPoint old = this.GetControlPoint(x,y);
			this._ControlPoint[x,y].Height = height;
			if (this.ControlPointModified != null) this.ControlPointModified(x,y,ControlPointModifications.Height,old);
		}
		public void SetControlPointColor(int x, int y, int color)
		{
			ControlPoint old = this.GetControlPoint(x,y);
			this._ControlPoint[x,y].Color = color;
			if (this.ControlPointModified != null) this.ControlPointModified(x,y,ControlPointModifications.Color,old);
		}
		public void SetControlPointDivision(int x, int y, uint division)
		{
			ControlPoint old = this.GetControlPoint(x,y);
			this._ControlPoint[x,y].Division = division;
			if (this.ControlPointModified != null) this.ControlPointModified(x,y,ControlPointModifications.Division,old);
		}




		//Stores the technique used for when the surface geometry is updated
		private Technique _SurfaceTechnique;
		public Technique SurfaceTechnique
		{
			get
			{
				return this._SurfaceTechnique;
			}
			set
			{
				this._SurfaceTechnique = value;
				if (this.TechniqueModified != null) this.TechniqueModified(TechniqueModifications.All);
			}
		}






		//EVENTS AND DELEGATES

		//Alerts when the techinque of this surface is modified internally. It provides info about what aspects were modified
		public delegate void TechniqueModifiedHandler(TechniqueModifications modifications);
		public event TechniqueModifiedHandler TechniqueModified;
	
		//Alerts when a control point of this surface is modified internally. It provides info about the point that was modified, what characteristics were modified, and an old copy of this point
		public delegate void ControlPointModifiedHandler(int x, int y, ControlPointModifications modifications, ControlPoint oldControlPoint);
		public event ControlPointModifiedHandler ControlPointModified;








		//Class Constructor 1
		public Surface(Size size, Technique surfaceTechnique)
		{ 
			this._Size = size;
			this._ControlPoint = new ControlPoint[size.Width, size.Height]; 
			this._SurfaceTechnique = surfaceTechnique;

			this.SurfaceTechnique.Modified += new Technique.ModifiedHandler(this.OnSurfaceTechniqueModified);
		} 

		//Class Constructor 2
		public Surface(Size size, ControlPoint initValue, Technique surfaceTechnique)
		{ 
			this._Size = size;
			this._ControlPoint = new ControlPoint[Size.Width, Size.Height]; 
		
			int i,j; 
			for (j=0; j<=this.Size.Height-1; j++) 
			{ 
				for (i=0; i<=this.Size.Width-1; i++) 
				{ 
					this._ControlPoint[i,j] = (ControlPoint)initValue.Clone(); 
				} 
			} 
			this._SurfaceTechnique = surfaceTechnique;

			this.SurfaceTechnique.Modified += new Technique.ModifiedHandler(this.OnSurfaceTechniqueModified);
		} 







		//Returns the real coordinate x of a control point (i,j); 0<=(i,j)<=Size-1; (i corespondednt of x), (j corespondednt of y)
		//applyShift makes the value be more accurate, by aplying the shift information on point
		public float GetPointRealCoordinateX(int i, int j, bool applyShift)
		{
			float x;
			x = i*this.SurfaceTechnique.TileSize.Width;

			if (applyShift == true)
			{
				//calculate the point's real value with shift applied
				float pShiftX; //this point's shift info
				pShiftX = this.SurfaceTechnique.GetPointShift(i%this.SurfaceTechnique.GetPointShiftWidth(),j%this.SurfaceTechnique.GetPointShiftHeight()).X; //gets the info for (i,j) from the technique k,l template

				x = x + pShiftX;
			}

			return x;
		}

		//Returns the real coordinate y of a control point (i,j); 0<=(i,j)<=Size-1; (i corespondednt of x), (j corespondednt of y)
		//applyShift makes the value be more accurate, by aplying the shift information on point
		public float GetPointRealCoordinateY(int i, int j, bool applyShift)
		{
			float y;
			y = i*this.SurfaceTechnique.TileSize.Height;

			if (applyShift == true)
			{
				//calculate the point's real value with shift applied
				float pShiftY; //this point's shift info
				pShiftY = this.SurfaceTechnique.GetPointShift(i%this.SurfaceTechnique.GetPointShiftWidth(),j%this.SurfaceTechnique.GetPointShiftHeight()).Y; //gets the info for (i,j) from the technique k,l template

				y = y + pShiftY;
			}

			return y;
		}


		//Returns the real coordinates (x,y) of a control point (i,j); 0<=(i,j)<=Size-1; (i corespondednt of x), (j corespondednt of y)
		//applyShift makes the values be more accurate, by aplying the shift information on point;
		public Vector2 GetPointRealCoordinatesXY(int i, int j, bool applyShift)
		{
			float x,y;
			x = i*this.SurfaceTechnique.TileSize.Width;
			y = j*this.SurfaceTechnique.TileSize.Height;

			if (applyShift == true)
			{
				//calculate the point's real value with shift applied
				PointF pShift; //this point's shift info
				pShift = this.SurfaceTechnique.GetPointShift(i%this.SurfaceTechnique.GetPointShiftWidth(),j%this.SurfaceTechnique.GetPointShiftHeight()); //gets the info for (i,j) from the technique k,l template

				x = x + pShift.X;
				y = y + pShift.Y;
			}

			return new Vector2(x,y);
		}
		

		//Returns the real coordinates (x,y, z=height) of a control point (i,j); 0<=(i,j)<=Size-1; (i corespondednt of x), (j corespondednt of y)
		//applyShift makes the values be more accurate, by aplying the shift information on point;
		public Vector3 GetPointRealCoordinatesXYHeight(int i, int j, bool applyShift)
		{
			Vector2 v = this.GetPointRealCoordinatesXY(i,j,applyShift);
			return new Vector3(v.X,v.Y,this.GetControlPoint(i,j).Height);
		}




		
		//Returns the Hatching Mode of a tile (ti,tj); 0<=(i,j)<=Size-2; (ti corespondednt of x), (tj corespondednt of y)
		public HatchingMode GetTileHatching(int ti, int tj)
		{
			return this.SurfaceTechnique.GetTileHatching(ti%this.SurfaceTechnique.GetTileHatchingWidth(),tj%this.SurfaceTechnique.GetTileHatchingHeight());
		}










		//Returns the tile point that contains a certain position (x,y) on the surface
		//Using Surface's technique for information
		public Point GetPositionTile(float x, float y, ShiftModes shiftApplied)
		{
			Point tile = new Point(0,0); //holds the result

			ShiftModes shiftm = shiftApplied;

			//detect shifting from technique
			if (shiftApplied == ShiftModes.AutoDetect)
			{
				shiftm = this.SurfaceTechnique.DetectShiftMode();
				if (shiftm == ShiftModes.Both) throw new ExceptionSurface("A horizontal and a vertical shift were both found in this surface's technique. Function GetPositionHeight cannot compute surfaces with a technique that uses shifts both horisontal and vertical! Please use only one kind of shift.");
			}


			//different algortitms for different shift modes, although somehow analogues
			if (shiftm == ShiftModes.None)
			{
				//NO SHIFT mode (fast computing)

				tile.X = (int)Math.Floor(x/this.SurfaceTechnique.TileSize.Width); //the column this point is in
				tile.Y = (int)Math.Floor(y/this.SurfaceTechnique.TileSize.Height); //the row this point is in
			}
			else if (shiftm == ShiftModes.Horizontal)
			{
				//HORIZONTAL SHIFT mode

				tile.Y = (int)Math.Floor(y/this.SurfaceTechnique.TileSize.Height); //the row this point is in
				
				//lower/upper points line (projection on lower/upper points line of this tile line):
				int tileXLower,tileXUpper; //column of the tile in which the lower/upper projection of the point is located
				tileXLower = this.GetTileForProjectedX(tile.Y,x);
				tileXUpper = this.GetTileForProjectedX(tile.Y+1,x);
                
				if (tileXLower == tileXUpper) //it is clearly in this tile
				{
					tile.X = tileXLower;
				}
				else //could be in either of these two or between them
				{
					Vector2 pLower, pUpper; //keep the positions of each couple of points that separate each two tiles in which the point might be
					bool found = false; //changes to true when the tile is found

					for (int i=Math.Min(tileXLower,tileXUpper)+1; i<=Math.Max(tileXLower,tileXUpper); i++) //verify (agains the point) each line in between two consecutive tiles from the lowest possible tile to the highest one to see where the point is
					{
						pLower = this.GetPointRealCoordinatesXY(i,tile.Y,true);
						pUpper = this.GetPointRealCoordinatesXY(i,tile.Y+1,true); 

						PointF pInter = Line2D.Intersection(Line2D.FromTwoPoints(pUpper.X, pUpper.Y, pLower.X, pLower.Y), new Line2D(x,y,0,1)); //gets the intersection between the determination line (the line between these two tiles) and the projection line of the point on OX

						//the two cases when the point is to the left of the determination line (the line that separates the two tiles) which means the point belongs to the left tile -> i-1;
						if (pUpper.X > pLower.X) if (y > pInter.Y)
												 {
													 tile.X = i-1;
													 found = true;
													 break;
												 }
						if (pUpper.X < pLower.X) if (y < pInter.Y)
												 {
													 tile.X = i-1;
													 found = true;
													 break;
												 }
					}
					
					if (!found) tile.X = Math.Max(tileXLower,tileXUpper); //is no tile is found in the left of each line that separates two lines (the determination lines) then the point is surely in the mostright tile from the series of tiles found
				}
			}
			else if (shiftm == ShiftModes.Vertical)
			{
				//VERTICAL SHIFT mode

				// !!! NEEDS DONE FOR VERTICAL SHIFT AS WELL !!!
			}

			return tile;
		}
		//END of main method (GetPositionTile)
		
		//
		//
		//

		//Submethod: Method used by the method "GetPositionTile"
		//This method returns the tile position for a projection point. the line is the line of the points on which the projection is made; x means the x coordinate of the point that is computer (x is really the projection value on any line)
		private int GetTileForProjectedX(int line, float x)
		{
			int virtualTileX = (int)Math.Floor(x/this.SurfaceTechnique.TileSize.Width); //the virtual column this point is in (virtual means that it is the column of the tile it is in if no shifts would be applied)

			int i; //keps the curent point
			short s = 0; //remembers the side where the tile was found (0-exacly the same as the virtual(unshifted) one; -x - x tiles left; +x - x tiles right)

			i = virtualTileX; //check on left side of the point's projection
			while (this.GetPointRealCoordinateX(i,line,true) > x) 
			{
				i--;
				s--;
			}
			i = virtualTileX+1; //check on right side of the point's projection
			while (this.GetPointRealCoordinateX(i,line,true) < x)
			{
				i++;
				s++;
			}

            return virtualTileX + s;
		}






		//Returns the triangle in which a given point (ray) (x,y) is located. It returns the real coordinates of points that form the triangle in which the ray (x,y) goes trough the surface
		//Using Surface's technique for information
		public Vector3[] GetPositionTriangle(float x, float y, ShiftModes shiftApplied)
		{
			ShiftModes shiftm = shiftApplied;

			//detect shifting from technique
			if (shiftApplied == ShiftModes.AutoDetect)
			{
				shiftm = this.SurfaceTechnique.DetectShiftMode();
				if (shiftm == ShiftModes.Both) throw new ExceptionSurface("A horizontal and a vertical shift were both found in this surface's technique. Function GetPositionHeight cannot compute surfaces with a technique that uses shifts both horisontal and vertical! Please use only one kind of shift.");
			}
			
			//get the tile where the position is located and check if the tile obtained is within surface limits. if not, throw exception
			Point tile = this.GetPositionTile(x,y,shiftApplied); 
			if (!Misc.WithinBounds(tile,this.Size,-2,-2)) throw new ExceptionSurface("The tile that contains the point (" + x + "," + y + ") is not within surface limits!"); //okay, uses offsets of -2 because: for once the tile number is one less than the points in a surface, which is returned in this.size. and another thing is that the function verifies in interval [0,size] but it must not include size in our case, because from 0 to size-1 is the boundary that includes all good points

			HatchingMode tileHatch = this.GetTileHatching(tile.X,tile.Y);

			//determine the triangle from this tile that contains this point
			//same algorithm for any shift mode works
			Vector3 p1,p2; //points that determin the hatching line on this tile
			Vector3 A,B,C; //points of the triangle that contains this point
			if (tileHatch == HatchingMode.NWSE)
			{
				p1 = this.GetPointRealCoordinatesXYHeight(tile.X,tile.Y+1,true);
				p2 = this.GetPointRealCoordinatesXYHeight(tile.X+1,tile.Y,true);

				PointF pInter = Line2D.Intersection(Line2D.FromTwoPoints(p1.X,p1.Y,p2.X,p2.Y),new Line2D(x,y,0,1)); //determine the intersection point between a vertical line that goes through the point and the one that determines the hatching. The result can help determine which triangle exacly this point belongs to

				bool leftTriangle = true; // consider it is the left triangle first, for faster computing

				//lines were parallel and the point is on the right triangle
				if (float.IsInfinity(pInter.Y)) if (x > p1.X) leftTriangle = false;

				//lines not parallel, but the point is on the right triangle
				if (y > pInter.Y) leftTriangle = false;

				//set the triangle (obs: one point was not calculated)
				if (leftTriangle)
				{
					A = p1;
					B = p2;
					C = this.GetPointRealCoordinatesXYHeight(tile.X,tile.Y,true);
				}
				else //right triangle
				{
					A = p1;
					B = this.GetPointRealCoordinatesXYHeight(tile.X+1,tile.Y+1,true);
					C = p2;
				}
			}
			else //NESW
			{
				p1 = this.GetPointRealCoordinatesXYHeight(tile.X,tile.Y,true);
				p2 = this.GetPointRealCoordinatesXYHeight(tile.X+1,tile.Y+1,true);

				PointF pInter = Line2D.Intersection(Line2D.FromTwoPoints(p1.X,p1.Y,p2.X,p2.Y),new Line2D(x,y,0,1)); //determine the intersection point between a vertical line that goes through the point and the one that determines the hatching. The result can help determine which triangle exacly this point belongs to

				bool leftTriangle = true; // consider it is the left triangle first, for faster computing

				//lines were parallel and the point is on the right triangle
				if (float.IsInfinity(pInter.Y)) if (x > p1.X) leftTriangle = false;

				//lines not parallel, but the point is on the right triangle
				if (y < pInter.Y) leftTriangle = false;

				//set the triangle (obs: one point was not calculated)
				if (leftTriangle)
				{
					A = p1;
					B = this.GetPointRealCoordinatesXYHeight(tile.X,tile.Y+1,true);
					C = p2;
				}
				else //right triangle
				{
					A = p1;
					B = p2;
					C = this.GetPointRealCoordinatesXYHeight(tile.X+1,tile.Y,true);
				}
			}

            //returns the triangle in the form of an 3 elements array of Vector3 object. (The triangle is given clockwise, relative to the surface)
            Vector3[] triangle = new Vector3[3];
			triangle[0]=A; triangle[1]=B; triangle[2]=C;

			return triangle;
		}




		//Returns the exact height for a real position (x,y) on the surface
		//Using Surface's technique for information
		public float GetPositionHeight(float x, float y, ShiftModes shiftApplied)
		{
            Vector3[] tri;
			tri = this.GetPositionTriangle(x,y,shiftApplied); //gets the triangle that the point/ray (x,y) intersects with the surface

			//intersect the plane obtained from the triangle with the ray (x,y) and return the height (z)
			return Plane.IntersectLine(Plane.FromPoints(tri[0],tri[1],tri[2]),new Vector3(x,y,0),new Vector3(x,y,1)).Z;
		}









		//Sets the control points' height from a height map (only works if the size of the height map equals or is larger than the one stored in this surface)
		//heightScale means the amount of height a color division means (the color value found at a certain location is multiplied by this and thus results the height set for that location in the surface)
		//heightOffset means the amount of height added to each point no matter what
		public void SetHeightsFromHeightMap(string heightMapPath, float heightScale, float heightOffset)
		{
            Bitmap heightMap = (Bitmap)Image.FromFile(heightMapPath);
            
			if ((this.Size.Width > heightMap.Size.Width) || (this.Size.Height > heightMap.Size.Height)) throw new ExceptionSurface("Cannot load a height map over a surface that has a differet size than the surface does");
			for (int i=0; i<=this.Size.Width-1; i++)
			{
				for (int j=0; j<=this.Size.Height-1; j++)
				{
                    this.SetControlPointHeight(i,j,heightOffset + heightScale*heightMap.GetPixel(i,j).R);
				}
			}
		}



		//Sets the division information for each point according the it's height.
		//The levels represent the height limit for each division (the number of levels is always less than the divisions you intend to use)
		public void SetDivisionsAccordingToHeight(float[] levels)
		{
			for (int i=0; i<=this.Size.Width-1; i++)
			{
				for (int j=0; j<=this.Size.Height-1; j++)
				{
					uint div=0;
					for (uint k=0; k<=levels.Length-1; k++)
					{
						if (this.GetControlPoint(i,j).Height > levels[k]) div=k+1;
					}

					this.SetControlPointDivision(i,j,div);
				}
			}
		}










		
		//EVENTS ATTACHED METHODS
		
		
		//when the SurfaceTechnique is modified
		public void OnSurfaceTechniqueModified(TechniqueModifications modifications)
		{
			//raise the SurfaceXY event that tells that its technique is modified
            if (this.TechniqueModified != null) this.TechniqueModified(modifications);
		}
	
	}
}