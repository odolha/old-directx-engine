//Structure Collection: FVFs
//Author: Ovidiu Dolha
//Description:	 Using DirectX 9.0c
//				 This strucutre collection provides most used FVFs


using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Direct3DAid
{
	//A custom vertex format (uses Position and Color)
	public struct FVF_PosCol :
		IVertex
	{ 
		//FVF fields
		public float X,Y,Z;
		public int Color; 



		//Mutator implementations
		public 	Vector3 Pos
		{ 
			get
			{
				return new Vector3(this.X,this.Y, this.Z);
			}
			set
			{
				this.X=value.X;
				this.Y=value.Y;
				this.Z=value.Z;
			}
		} 

		public 	Vector3 Nor
		{ 
			get
			{
				return Vector3.Empty;
			}
			set
			{
			}
		} 

		public int Col
		{
			get
			{
				return this.Color;
			}
			set
			{
				this.Color=value;
			}
		}

		public TextureCoordinates Tex
		{
			get
			{
				return TextureCoordinates.Empty;
			}
			set
			{
			}
		}



		public const VertexFormats Format = VertexFormats.Position | VertexFormats.Diffuse;
	}






	//A custom vertex format (uses Position, Color and Texture)
	public struct FVF_PosColTex :
		IVertex
	{ 
		//FVF fields
		public float X,Y,Z;
		public int Color; 
		public float Tu,Tv; 



		//Mutator implementations
		public 	Vector3 Pos
		{ 
			get
			{
				return new Vector3(this.X,this.Y, this.Z);
			}
			set
			{
				this.X=value.X;
				this.Y=value.Y;
				this.Z=value.Z;
			}
		} 

		public 	Vector3 Nor
		{ 
			get
			{
				return Vector3.Empty;
			}
			set
			{
			}
		} 

		public int Col
		{
			get
			{
				return this.Color;
			}
			set
			{
				this.Color=value;
			}
		}

		public TextureCoordinates Tex
		{
			get
			{
				return new TextureCoordinates(this.Tu,this.Tv);
			}
			set
			{
				this.Tu=value.u;
				this.Tv=value.v;
			}
		}

		public const VertexFormats Format = VertexFormats.Position | VertexFormats.Diffuse | VertexFormats.Texture1;
	}










	//A custom vertex format (uses Position, Normal, Color and Texture)
	public struct FVF_PosNorColTex :
		IVertex
	{ 
		//FVF fields
		public float X,Y,Z;
		public float Nx,Ny,Nz; 
		public int Color; 
		public float Tu,Tv; 



		//Mutator implementations
		public 	Vector3 Pos
		{ 
			get
			{
				return new Vector3(this.X,this.Y, this.Z);
			}
			set
			{
				this.X=value.X;
				this.Y=value.Y;
				this.Z=value.Z;
			}
		} 

		public 	Vector3 Nor
		{ 
			get
			{
				return new Vector3(this.Nx,this.Ny, this.Nz);
			}
			set
			{
				this.Nx=value.X;
				this.Ny=value.Y;
				this.Nz=value.Z;
			}
		} 

		public int Col
		{
			get
			{
				return this.Color;
			}
			set
			{
				this.Color=value;
			}
		}

		public TextureCoordinates Tex
		{
			get
			{
				return new TextureCoordinates(this.Tu,this.Tv);
			}
			set
			{
				this.Tu=value.u;
				this.Tv=value.v;
			}
		}



		public const VertexFormats Format = VertexFormats.Position | VertexFormats.Normal | VertexFormats.Diffuse | VertexFormats.Texture1;
	}


}