//Class: (ABSTRACT) D3DBase
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//				 This is the base class from which all classes that use D3D in a relevant way derive


using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Direct3DAid
{
	public abstract class D3DBase :
		IUsingD3DDevice 
	{
		//Device to use (for the classes to follow)
		private Device _Device; 
		public Device Device
		{ 
			get 
			{ 
				return this._Device; 
			} 
			set 
			{ 
				this._Device = value; 
			} 
		} 


	

		//Class Constructor 1
		public D3DBase()
		{
			this._Device=null;
		}

		//Class Constructor 2
		public D3DBase(Device device)
		{
			this._Device=device;
		}
	}
}