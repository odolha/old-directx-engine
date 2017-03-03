//Class: Navigator
//Author: Ovidiu Dolha
//Description:   -
//				 'Class that represents the base for a navigator


using System;
using Direct3DAid.SpaceObjects;
using Direct3DAid.Mobility.Controllers;


namespace Direct3DAid.Mobility.Navigators
{
	public class Navigator 
	{ 
		//The space object attached to the navigator
		private SpaceObject _SpaceObject; 
		public SpaceObject SpaceObject 
		{ 
			get 
			{ 
				return this._SpaceObject; 
			} 
			set 
			{ 
				this._SpaceObject = value; 
				if (this.ObjectChanged != null) this.ObjectChanged(); 
			} 
		} 


		//The controller that handles this navigator
		private Controller _Controller; 
		public Controller Controller 
		{ 
			get 
			{ 
				return this._Controller; 
			} 
			set 
			{ 
				this._Controller = value; 
			} 
		} 

		
		


		//EVENTS AND DELEGATES

		protected delegate void ObjectChangedHandler();
		protected event ObjectChangedHandler ObjectChanged; 





		//Class Constructor
		public Navigator(SpaceObject spaceObject, Controller controller) 
		{ 
			this._SpaceObject = spaceObject; 
			this._Controller = controller; 
		} 





		//Returns a default controller for this navigator
		public static Controller GetDefaultController() 
		{
			//there is no default controller for this base navigator
			return null;
		} 
	}
}
