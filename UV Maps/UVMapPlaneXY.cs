//Class: UVMapPlaneXY <- UVMapPlane
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//				 A XY plane texture map that can fit texture coordinates from a given plane by perpendicular projection


using System;
using System.Drawing;
using Microsoft.DirectX;



namespace Direct3DAid
{
	public class UVMapPlaneXY : UVMapPlane
	{
		//Class Constructor
		public UVMapPlaneXY(PointF origin, SizeF size, SizeF scale):
			base (origin, size, scale)
		{
		}




		//This is a plane Fit. It overrides the null fit from the parent UVMap
		public override TextureCoordinates Fit(Vector3 position)
		{
			TextureCoordinates tc = new TextureCoordinates(0,0);
			tc.u=position.X*this.Scale.Width/this.Size.Width-this.Origin.X;
			tc.v=position.Y*this.Scale.Height/this.Size.Height-this.Origin.Y;

			return tc;
		}

	}
}