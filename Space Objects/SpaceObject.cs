//Class: SpaceObject <- D3DBase
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//               A base class for any directX space object (could be virtual, like a camera, or material like a solid, in which case there is a Geometry template and a Render method provided)


using System;
using Microsoft.DirectX; 
using Microsoft.DirectX.Direct3D;

using Direct3DAid.Geometries;


namespace Direct3DAid.SpaceObjects
{
	public class SpaceObject : D3DBase,
		IRenderable
	{ 
		//
		//Device inherited
		//
	


		//The matrix that represents this space object in 3D space (is a relative matrix)
		protected Matrix _SelfMatrix; 
		public Matrix SelfMatrix 
		{ 
			get 
			{ 
				return _SelfMatrix; 
			} 
			set 
			{ 
				_SelfMatrix = value; 
				if (this.MatrixModified != null) this.MatrixModified();
			} 
		} 


		//Retrieves the matrix after all the hierarchy transformation are applied (is an absolute matrix)
		public Matrix TransformedMatrix
		{
			get
			{
				//is it has no parent then just return self matrix (for efficiency)
				if (this._Parent==null) return this._SelfMatrix;

				//using directly the fields from space object (for efficiency)
				Matrix m = this._SelfMatrix;
				SpaceObject s = this._Parent;
            
				while (s!=null)
				{
					m=Matrix.Multiply(s._SelfMatrix,m);
					s=s._Parent;
				}

				return m;
			}
		}





		//An object that represents it's parent in hierarchy. If there is no parent then the reference should be null
		private SpaceObject _Parent;
		public SpaceObject Parent
		{
			get
			{
				return this._Parent;
			}
			set
			{
				this._Parent=value;
			}
		}




		//Geometry used for render. OPTIONAL
		private GeometryTemplate _Geometry;
		public GeometryTemplate Geometry
		{
			get
			{
				return this._Geometry;
			}
			set
			{
				this._Geometry=value;
			}
		}





		//EVENTS AND DELEGATES

		//Alerts when the matrix that represents this space object is modified
		public delegate void MatrixModifiedHandler();
		public event MatrixModifiedHandler MatrixModified;





	
		//Class Constructor 1
		public SpaceObject():
			base()
		{ 
			this._SelfMatrix = Matrix.Identity; //a new space object is set to be in the origin
			this._Parent = null; //no parent is assigned by default
			this._Geometry = null; //no geometry by default
		} 

		//Class Constructor 2
		public SpaceObject(Device device):
			base(device)
		{ 
			this._SelfMatrix = Matrix.Identity; //a new space object is set to be in the origin
			this._Parent = null; //no parent is assigned by default
			this._Geometry = null; //no geometry by default
		} 






		//A function that renders this object (in it's space coordinates and rotation) if provided with a geometry template (a blueprint of the object to be drawn) and with a Direct3D Device
		public void Render()
		{
			if (this.Device == null) throw new ExceptionSpaceObject("Device not set. Cannot render this object"); 
			if (this.Geometry.Device == null) throw new ExceptionSpaceObject("Geometry is a null reference. There is no template from wich to render this object.");
			if (this.Device != this.Geometry.Device) throw new ExceptionSpaceObject("The device on this space object and the one from the geometry attached for the render are different!"); 

			this.Device.Transform.World = this.TransformedMatrix;
        
			this.Geometry.Render();
		}
	}

}
