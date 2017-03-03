//Class: (ABSTRACT) InputAssignement
//Author: Ovidiu Dolha
//Description:   -
//				 Class that stores one assignement (any kind)


using System;


namespace Direct3DAid.Mobility.Controllers
{
	public abstract class InputAssignement 
	{ 
		//Represents the index of the control action within this controller that is associeted with this input assignement
		private int _ControlActionIndex; 
		public int ControlActionIndex 
		{ 
			get 
			{ 
				return this._ControlActionIndex; 
			} 
			set 
			{ 
				this._ControlActionIndex = value; 
			} 
		} 
	} 
}
