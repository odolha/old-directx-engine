// INTERFACES ARE DESCRIBED HERE


using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Direct3DAid.SpaceObjects;


namespace Direct3DAid
{
	//The base interface for base classes that their children will use a DX Device
	public interface IUsingD3DDevice
	{
		Device Device 
		{ 
			get;
			set;
		} 
	}


	public interface ISpaceObject
	{
		SpaceObject SpaceObject
		{
			get;
			set;
		}
	}






	//Interface for any renderable object with 0 arguments
	public interface IRenderable
	{
		void Render();
	}






	public interface IVertex
	{
		Vector3 Pos
		{ 
			get;
			set;
		} 

		Vector3 Nor
		{ 
			get;
			set;
		} 

		int Col
		{
			get;
			set;
		}

		TextureCoordinates Tex
		{
			get;
			set;
		}

	}
}