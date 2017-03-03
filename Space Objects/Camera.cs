//Class: Camera <- SpaceObject
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//               Represents a parametrizable camera in space. It handles the Device.View matrix of the this.Device


using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Direct3DAid.SpaceObjects
{
	public class Camera : SpaceObject
	{ 
		//Camera parameters

		//Field of view Y
		private float _FieldOfViewY; 
		public float FieldOfViewY 
		{ 
			get 
			{ 
				return this._FieldOfViewY; 
			} 
			set 
			{ 
				this._FieldOfViewY = value; 
				this.UpdateProjection(); 
			} 
		} 

		//The field of view x is calculated from the fovy and aspect ratio
		public float FieldOfViewX 
		{ 
			get 
			{ 
				return (this._FieldOfViewY * this._AspectRatio);
			} 
		} 

		//The aspect ratio of the camera
		private float _AspectRatio; 
		public float AspectRatio 
		{ 
			get 
			{ 
				return this._AspectRatio; 
			} 
			set 
			{ 
				this._AspectRatio = value; 
				this.UpdateProjection(); 
			} 
		} 
		
		//Near plane
		private float _ZNearPlane; 
		public float ZNearPlane 
		{ 
			get 
			{ 
				return this._ZNearPlane; 
			} 
			set 
			{ 
				this._ZNearPlane = value; 
				this.UpdateProjection(); 
			} 
		} 

		//Far plane
		private float _ZFarPlane; 
		public float ZFarPlane 
		{ 
			get 
			{ 
				return this._ZFarPlane; 
			} 
			set 
			{ 
				this._ZFarPlane = value; 
				this.UpdateProjection(); 
			} 
		} 




		//If the camera updates 
		protected bool _Active; 
		public bool Active 
		{ 
			get 
			{ 
				return this._Active; 
			} 
			set 
			{ 
				this._Active = value; 
				if (value == true) 
				{ 
					this.UpdateView(); 
					this.UpdateProjection(); 
				} 
			} 
		} 




		//Class Constructor
		public Camera(Device device, float fieldOfViewY, float aspectRatio, float zNearPlane, float zFarPlane):
			base(device)
		{ 
			this._Active = true; 

			this.MatrixModified += new MatrixModifiedHandler(this.UpdateView); 
			this.UpdateView(); 

			this._FieldOfViewY = fieldOfViewY; 
			this._AspectRatio = aspectRatio; 
			this._ZNearPlane = zNearPlane; 
			this._ZFarPlane = zFarPlane; 

			this.UpdateProjection(); 
		} 





		//Updates the view of this.Device
		public void UpdateView() 
		{ 
			if (this.Active) 
				this.Device.Transform.View = this.SelfMatrix; 
		} 


		//Updates the projection of this.Device
		public void UpdateProjection() 
		{ 
			if (this.Active) 
				this.Device.Transform.Projection = Matrix.PerspectiveFovLH(this.FieldOfViewY, this.AspectRatio, this.ZNearPlane, this.ZFarPlane); 
		} 
	}
}
