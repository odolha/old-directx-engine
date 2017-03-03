//Class: AxisInputAssignement <- InputAssignement
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//				 Class that stores one axis assignement


using System;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;


namespace Direct3DAid.Mobility.Controllers
{
	public class AxisInputAssignement : InputAssignement 
	{
		//Axis that forms the action
		private Axis3 _Axis; 
		public Axis3 Axis 
		{ 
			get 
			{ 
				return this._Axis; 
			} 
			set 
			{ 
				this._Axis = value; 
			} 
		} 


		//When persistent caller is on, this input assignement makes the host controller call 'ActionPerformed' every frame, even when no need (i.e. axis result is null); only works with unbuffered input
		private bool _PersistentCaller; 
		public bool PersistentCaller 
		{ 
			get 
			{ 
				return this._PersistentCaller; 
			} 
			set 
			{ 
				this._PersistentCaller = value; 
			} 
		} 

        




        //Class Constructor
		public AxisInputAssignement(int controlActionIndex, Axis3 axis, bool persistentCallerOn) 
		{ 
			this.ControlActionIndex = controlActionIndex; 
			this._Axis = axis; 
			this._PersistentCaller = persistentCallerOn; 
		} 

		
		
		
		
		//Returns a direct input value from an input axis3 struc
		public static MouseOffset DirectInputValueOf(Axis3 axis) 
		{ 
			if (axis == Axis3.X) return MouseOffset.X; 
			else if (axis == Axis3.Y) return MouseOffset.Y; 
			else if (axis == Axis3.Z) return MouseOffset.Z; 
			else return (MouseOffset)(-1);
		} 

	
	} 
}
