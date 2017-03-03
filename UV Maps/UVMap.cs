//Class: (ABSTRACT) UVMap
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//				 An informative class that contains info about how to fit a texture to a geometry (it is a general texture fit class)



using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;



namespace Direct3DAid
{
	public abstract class UVMap
	{

		//Class Constructor
		public UVMap()
		{
		}




		//Function that returns a texture coordinate for a given point in space (according to it's Fit function)
		//this one is a null fit (returns null coordinates)
		public virtual TextureCoordinates Fit(Vector3 position)
		{
			return new TextureCoordinates(0,0);
		}



		//Fits a map over a whole VB
		public void FitOverVB(VertexBuffer vB)
		{
			//retrieve the vertice in an array (of objects) since we don't know what kind of vertex type we have
			System.Array verts;
			verts = vB.Lock(0,0);

			IVertex o; //temp general vertex obj used to change propreties


			//reset texture coord using this Texture Fit
			for (int i=0; i<=verts.Length-1; i++)
			{
				o = (IVertex)verts.GetValue(i);
				o.Tex=this.Fit(o.Pos);
				verts.SetValue(o,i);
			}

			vB.Unlock();
		}

	}

}
