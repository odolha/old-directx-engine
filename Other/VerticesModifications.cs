//SUB-NameSpace, Class Collection: VerticesModifications
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//               This namespace holds classes that are used as information about modifications that are to be aplied to a Vertex Buffer


using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Direct3DAid.Geometries.VerticesModifications
{
	//Base Class for any modification
	public abstract class VertexModification:
		ICloneable 
	{
		//Index of the vertex for which this modification is apllied
		private int _Index;
		public int Index
		{
			get
			{
				return this._Index;
			}
			set
			{
				this._Index=value;
			}
		}


		//Abstract Constructor
		public VertexModification(int index)
		{
			this._Index = index;
		}



		//Base Clone method
		public abstract object Clone();
	}






	//Class that stores vector3 modification (position in space, normal in space)
	public class Vector3Modification : VertexModification
	{
		private Vector3 _NewVector;
		public Vector3 NewVector
		{
			get
			{
				return this._NewVector;
			}
			set
			{
				this._NewVector=value;
			}
		}

		

		//Constructor
		public Vector3Modification(int index, Vector3 newVector):
			base(index)
		{
			this._NewVector=newVector;
		}


		
		//Returns clone
		public override object Clone()
		{
			return new Vector3Modification(this.Index,this._NewVector);
		}
	}










	//Class that stores TextureCoordinates modification (vertex texture coordinates - tu and tv)
	public class TextureCoordinatesModification : VertexModification
	{
		private TextureCoordinates _NewTextureCoord;
		public TextureCoordinates NewTextureCoord
		{
			get
			{
				return this._NewTextureCoord;
			}
			set
			{
				this._NewTextureCoord=value;
			}
		}

		

		//Constructor
		public TextureCoordinatesModification(int index, TextureCoordinates newTextureCoord):
			base(index)
		{
			this._NewTextureCoord=newTextureCoord;
		}

	

		//Returns clone
		public override object Clone()
		{
			return new TextureCoordinatesModification (this.Index,this._NewTextureCoord);
		}
	}





    




	//Class that stores Color modification (vertex color)
	public class ColorModification : VertexModification
	{
		private int _NewColor;
		public int NewColor
		{
			get
			{
				return this._NewColor;
			}
			set
			{
				this._NewColor=value;
			}
		}


		//If this is active then only the alpha channle is considered when applied
		private bool _AlphaOnly;
		public bool AlphaOnly
		{
			get
			{
				return this._AlphaOnly;
			}
			set
			{
				this._AlphaOnly = value;
			}
		}

		

		//Constructor 1
		public ColorModification(int index, int newColor):
			base(index)
		{
			this._NewColor=newColor;
			this._AlphaOnly=false;
		}

		//Constructor 2
		public ColorModification(int index, int newColor, bool alphaOnly):
			base(index)
		{
			this._NewColor=newColor;
			this._AlphaOnly=alphaOnly;
		}




		//Returns clone
		public override object Clone()
		{
			return new ColorModification(this.Index,this._NewColor,this._AlphaOnly);
		}
	}


}
