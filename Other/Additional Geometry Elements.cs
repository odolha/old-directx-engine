//Class Colection: Additional Geometry Elements
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//               Some geometry classes that were not included in DX nor in .NET framework (like line)


using System;
using System.Drawing;
using Microsoft.DirectX; 
using Microsoft.DirectX.Direct3D; 


namespace Direct3DAid
{
	//Class that holds information about a 2D line
	public class Line2D
	{
		//Origin of the line
		private float _X0;
		public float X0
		{
			get
			{
				return this._X0;
			}
			set
			{
				this._X0 = value;
			}
		}
		private float _Y0;
		public float Y0
		{
			get
			{
				return this._Y0;
			}
			set
			{
				this._Y0 = value;
			}
		}


		//Directory vector of the line
		private float _VX;
		public float VX
		{
			get
			{
				return this._VX;
			}
			set
			{
				this._VX = value;
			}
		}
		private float _VY;
		public float VY
		{
			get
			{
				return this._VY;
			}
			set
			{
				this._VY = value;
			}
		}

        

		//Class Constructor 1 (given the origin and the directory vector)
		public Line2D(float x0, float y0, float vX, float vY)
		{
            this._X0=x0;
			this._Y0=y0;
			this._VX=vX;
			this._VY=vY;
		}

		
		
		
		

		//Returns a Line2D given 2 points
		public static Line2D FromTwoPoints(float x1, float y1, float x2, float y2)
		{
			return new Line2D(x1,y1,x2-x1,y2-y1);
		}



		
		//Retrieves the point of intersection between two lines (in 2D), returns infinite if they don't intersect
		public static PointF Intersection(Line2D l1, Line2D l2)
		{
			float t = (l2.VY*(l2.X0-l1.X0)-l2.VX*(l2.Y0-l1.Y0))/(l1.VX*l2.VY-l2.VX*l1.VY);
            return new PointF(l1.X0+l1.VX*t,l1.Y0+l1.VY*t);
		}
	}
}