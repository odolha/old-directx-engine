//Class: AssignementsMap
//Author: Ovidiu Dolha
//Description:   -
//				 'Class that stores all assignements


using System;


namespace Direct3DAid.Mobility.Controllers
{
	public class AssignementsMap
	{ 
		//Collection of keyboard assignements
		private InputAssignementCollection _KeyboardAssignements; 
		public InputAssignementCollection KeyboardAssignements 
		{ 
			get 
			{ 
				return this._KeyboardAssignements; 
			} 
			set 
			{ 
				this._KeyboardAssignements = value; 
			} 
		} 


		//Collection of mouse assignements
		private InputAssignementCollection _MouseAssignements; 
		public InputAssignementCollection MouseAssignements 
		{ 
			get 
			{ 
				return this._MouseAssignements; 
			} 
			set 
			{ 
				this._MouseAssignements = value; 
			} 
		} 



		
		//Class Constructor
		public AssignementsMap() 
		{ 
			this.KeyboardAssignements = new InputAssignementCollection(); 
			this.MouseAssignements = new InputAssignementCollection(); 
		} 
	} 
}
