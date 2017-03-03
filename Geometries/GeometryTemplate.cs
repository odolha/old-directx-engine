//Class: (ABSTRACT) GeometryTemplate <- D3DBase
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//				 A template for a geometry in general (like Indexed, Meshed, etc)


using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Direct3DAid.Geometries
{
	public abstract class GeometryTemplate : D3DBase,
		IRenderable
	{
		//
		//Device inherited
		//


		//Class Constructor
		public GeometryTemplate():
			base()
		{
		}



		public virtual void Render()
		{
		}
	}
}
