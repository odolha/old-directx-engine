//Class: ButtonInputAssignement <- InputAssignement
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//				 Class that stores one button assignement


using System;


namespace Direct3DAid.Mobility.Controllers
{
	public class ButtonInputAssignement : InputAssignement 
	{ 
		//Represents an array of keycodes and keychecks that form the input action (i.e. more keys mean that the action will occur when all of them are pressed)
		private KeyState[] _KeyCodes; 
		public KeyState[] KeyCodes 
		{ 
			get 
			{ 
				return this._KeyCodes; 
			} 
			set 
			{ 
				this._KeyCodes = value; 
			} 
		} 

		
		//Represents the number of calls that are going to be made on the "Process" procedure before this input assignement is tested
		private int _WaitingTime; 
		public int WaitingTime 
		{ 
			get 
			{ 
				return this._WaitingTime; 
			} 
			set 
			{ 
				this._WaitingTime = value; 
			} 
		} 
		

		//Represents the number of calls that are made on the "Process" procedure before this input action is tested again (after the first time this input action was tested and found true)
		private int _RepeatingTime; 
		public int RepeatingTime 
		{ 
			get 
			{ 
				return this._RepeatingTime; 
			} 
			set 
			{ 
				this._RepeatingTime = value; 
			} 
		} 






		//State Information
		private bool _Pressed = false; 
		public bool Pressed 
		{ 
			get 
			{ 
				return this._Pressed; 
			} 
			set
			{
				this._Pressed = value;
			}
		} 

		//State Information
		private int _TimeSincePressed = 0; 
		public int TimeSincePressed 
		{ 
			get 
			{ 
				return this._TimeSincePressed; 
			} 
			set
			{
				this._TimeSincePressed = value;
			}
		} 

		//State Information
		private int _TimeSinceActioned = 0; 
		public int TimeSinceActioned 
		{ 
			get 
			{ 
				return this._TimeSinceActioned; 
			} 
			set
			{
				this._TimeSinceActioned = value;
			}
		} 






		
		
		//Class Constructor 1
		public ButtonInputAssignement(int controlActionIndex, byte key, int waitingTime, int repeatingTime) 
		{ 
			this.ControlActionIndex = controlActionIndex; 
			this._KeyCodes = new KeyState[]{new KeyState(key)}; 
			this._WaitingTime = waitingTime; 
			this._RepeatingTime = repeatingTime; 
		} 


		//Class Constructor 2
		public ButtonInputAssignement(int controlActionIndex, byte key1, byte key2, int waitingTime, int repeatingTime) 
		{ 
			this.ControlActionIndex = controlActionIndex; 
			this._KeyCodes = new KeyState[]{new KeyState(key1), new KeyState(key2)}; 
			this._WaitingTime = waitingTime; 
			this._RepeatingTime = repeatingTime; 
		} 


		//Class Constructor 3
		public ButtonInputAssignement(int controlActionIndex, KeyState key, int waitingTime, int repeatingTime) 
		{ 
			this.ControlActionIndex = controlActionIndex; 
			this._KeyCodes = new KeyState[]{key}; 
			this._WaitingTime = waitingTime; 
			this._RepeatingTime = repeatingTime; 
		} 


		//Class Constructor 4
		public ButtonInputAssignement(int controlActionIndex, KeyState[] keys, int waitingTime, int repeatingTime) 
		{ 
			this.ControlActionIndex = controlActionIndex; 
			this._KeyCodes = keys; 
			this._WaitingTime = waitingTime; 
			this._RepeatingTime = repeatingTime; 
		} 

		 
	} 
}
