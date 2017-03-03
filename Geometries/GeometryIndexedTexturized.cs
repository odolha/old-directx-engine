//Class: GeometryIndexedSplatted <- GeometryIndexed
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//               Like an indexed geometry, but used for textured geometries and combination of texture and diffuse element (color)
//				 Only handles triangles (solid geometry)


using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Direct3DAid.Geometries
{
	public class GeometryIndexedTexturized : GeometryIndexed
	{
		//Texture to use for this geometry
		private Texture _Texture;
		public Texture Texture
		{
			get
			{
				return this._Texture;
			}
			set
			{
				this._Texture = value;
			}
		}




		
	

	
		//Class Constructor 1
		public GeometryIndexedTexturized(Device device, VertexBuffer vB, IndexBuffer iB, Type vertexType, VertexFormats vertexFormat, PrimitiveType primitivesType):
			base(device, vB, iB, vertexType, vertexFormat, primitivesType)
		{
		}


		//Class Constructor 2
		public GeometryIndexedTexturized(Device device, int numVertices, int numIndices, Type vertexType, VertexFormats vertexFormat, PrimitiveType primitivesType) :
			base(device, numVertices, numIndices, vertexType, vertexFormat, primitivesType)
		{ 
		}







		//Renders this texturized indexed geometry
		public override void Render()
		{
			if (this.Device == null) throw new ExceptionGeometry("Device not set. Cannot render geometry");  

			//Vital settings for the device and drawing
			this.Device.VertexFormat = this.VertexFormat;
			this.Device.SetStreamSource(0, this._VB, 0); 
			this.Device.Indices = this._IB; 

			//Set texture and blending with diffuse
			if (this.Texture == null) throw new ExceptionGeometry("Texture is not set. This texturizable geometry cannot be rendered without a texture");
			this.Device.SetTexture(0, this.Texture); 
			this.Device.TextureState[0].ColorOperation = TextureOperation.Modulate; 
			this.Device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor; 
			this.Device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse; 

			//can only use TriangleList for now, if other is used then announce the user
			if (step!=3) throw new ExceptionGeometry("Cannot handle other primitive type than TriangleList for now; CANNOT RENDER!");

			this.Device.DrawIndexedPrimitives(this.PrimitivesType, 0, 0, this.NumVertices, 0, this.NumIndices/step);  
		}
	}
}
