//Class: (ABSTRACT) UVMapPlane <- UVMap
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//				 A generic plane texture map that provides elements for actual plane fit maps


using System;
using System.Drawing;
using Microsoft.DirectX;



namespace Direct3DAid
{
	public abstract class UVMapPlane : UVMap
	{
		//The origin of this plane (projected to an object it will result texture coordinates of 0,0) 
		private PointF _Origin;
		public PointF Origin
		{
			get
			{
				return this._Origin;
			}
			set
			{
				this._Origin=value;
			}
		}


		//The size of this plane
		private SizeF _Size;
		public SizeF Size
		{
			get
			{
				return this._Size;
			}
			set
			{
				if (value.Width==0 || value.Height==0) throw new ExceptionUVMap("Size cannot be 0 on any direction");
				else this._Size=value;
			}
		}

	
		//The scale of the texture map (1 is normal, if smaller the texture is enlarged, if larger the texture is smaller)
		private SizeF _Scale;
		public SizeF Scale
		{
			get
			{
				return this._Scale;
			}
			set
			{
				if (value.Width==0 || value.Height==0) throw new ExceptionUVMap("Scale cannot be 0 on any direction");
				else this._Scale=value;
			}
		}









		//Class Constructor
		public UVMapPlane(PointF origin, SizeF size, SizeF scale)
		{
			this._Origin=origin;
			this._Size=size;
			this._Scale=scale;
		}





		//This is a plane Fit. It overrides the null fit from the parent UVMap //doesn't do anything and is abstrac so the inherited classes would write this function
		public abstract override TextureCoordinates Fit(Vector3 position);
	}
}