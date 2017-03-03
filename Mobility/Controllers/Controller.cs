//Class: Controller
//Author: Ovidiu Dolha
//Description:   -
//				 A generic Controller class that handles actions, by raising events


using System;
using System.Collections.Specialized;


namespace Direct3DAid.Mobility.Controllers
{
	public class Controller 
	{ 
		//A collection of strings representing the actions of this controller
		private StringCollection _ControlActions; 
		public StringCollection ControlActions 
		{ 
			get 
			{ 
				return this._ControlActions; 
			} 
			set 
			{ 
				this._ControlActions = value; 
			} 
		} 



	
		//EVENTS AND DELEGATES

		//Is called when an action is performed by this controller
		public delegate void ActionPerformedHandler(int index, string name, double val);
		public event ActionPerformedHandler ActionPerformed; 




		//Class Constructor 1
		public Controller() 
		{ 
			this._ControlActions = new StringCollection(); 
		} 

		//Class Constructor 2
		public Controller(StringCollection controlActions) 
		{ 
			if (controlActions == null) throw new ExceptionMobility("The controlActions must be a valid collection. It cannot be a null reference. If you don't want any actions in this Controller, use the overrided constructor with no parameters");
			this._ControlActions = controlActions; 
		} 






		//PerformAction - Variant 1
		//Performs an action, given it's index in ControlActions array
		public void PerformAction(int index, long val) 
		{ 
			if (index > this.ControlActions.Count - 1)
				throw new ExceptionMobility("Action with index (" + index.ToString() + ") is not found within controller (index is out of boundaries)"); 

			if (this.ActionPerformed != null) this.ActionPerformed(index, this.ControlActions[index], val);
		} 


		//PerformAction - Variant 2
		//Performs an action, given it's name as a string
		public void PerformAction(string name, long val) 
		{ 
			if (!this.ControlActions.Contains(name))
				throw new ExceptionMobility("Action with name <<" + name + ">> is not found within controller"); 

			this.PerformAction(this.ControlActions.IndexOf(name),val);
		} 


	}
}