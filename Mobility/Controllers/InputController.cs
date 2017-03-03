//Class: Controller
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//				 A DirectInput controller for keyboard and mouse


using System;
using System.Collections.Specialized;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;


namespace Direct3DAid.Mobility.Controllers
{
	public class InputController : Controller 
	{
		//Device to use for keyboard
		private Device _DeviceKeyboard; 
		public Device DeviceKeyboard 
		{ 
			get 
			{ 
				return this._DeviceKeyboard; 
			} 
			set 
			{ 
				this._DeviceKeyboard = value; 
			} 
		} 


		//Device to use for mouse
		private Device _DeviceMouse; 
		public Device DeviceMouse 
		{ 
			get 
			{ 
				return this._DeviceMouse; 
			} 
			set 
			{ 
				this._DeviceMouse = value; 
			} 
		} 



		//Assignements Map
		private AssignementsMap _Assignements; 
		public AssignementsMap Assignements 
		{ 
			get 
			{ 
				return this._Assignements; 
			} 
			set 
			{ 
				this._Assignements = value; 
			} 
		} 







		//Class Constructor
		public InputController(Device deviceKeyboard, Device deviceMouse, StringCollection controlActions, AssignementsMap assignements):
			base(controlActions)
		{ 
			this.DeviceKeyboard = deviceKeyboard; 
			this.DeviceMouse = deviceMouse; 
			this.Assignements = assignements; 
		} 






		//Core processing method. It processes keyboard and mouse actions from the user and converts them into controller actions
		//This function should be called runtime-like
		public ProcessingResult Process() 
		{ 
			ProcessingResult r = ProcessingResult.Nothing; 
			
			if (!(ProcessKeyboard())) r = r | ProcessingResult.KeyboardProcessingFailed; 
			if (!(ProcessMouse())) r = r | ProcessingResult.MouseProcessingFailed; 

			if (r == ProcessingResult.Nothing) r = ProcessingResult.Success; 
			return r; 
		} 






		//Processes keyboard
		//Returns false if it didn't succede because the device is not set or it is not aquired
		protected bool ProcessKeyboard() 
		{ 
			//processing keyboard
			if (!((this.DeviceKeyboard == null))) 
			{ 
				try 
				{ 
					this.DeviceKeyboard.Poll(); 

					ButtonInputAssignement ib; 
					int k; //temp counter

					
					if (this.DeviceKeyboard.Properties.BufferSize == 0) //if it's 0 then it's probably not intended to use the buffer
					{ 
						KeyboardState ks; 
						ks = this.DeviceKeyboard.GetCurrentKeyboardState(); 

						foreach (InputAssignement i in this.Assignements.KeyboardAssignements) 
						{ 
							//only button inputs are available in keyboard check
							if (i is ButtonInputAssignement) 
							{ 
								ib = (ButtonInputAssignement)(i); 

								bool b = true; //if all keys are pressed or not
								foreach (KeyState j in ib.KeyCodes) 
								{ 
									//we need to check both for keys that need to be pressed and for those who need to be up, that's why we use xor
									if ((ks[(Key)j.Code] ^ j.Check)) 
									{
										//one key is not pressed, action will not occure
										b = false; 
									}
								} 

								if (b) 
								{ 
									ib.Pressed = true; 
									ib.TimeSincePressed += 1;
 
									if (ib.TimeSincePressed > ib.WaitingTime) 
									{ 
                                        //action is allowed
										ib.TimeSinceActioned += 1; 

										if (ib.TimeSinceActioned > ib.RepeatingTime) 
										{ 
											//action occures this time
											this.PerformAction(ib.ControlActionIndex,1); 

											ib.TimeSinceActioned = 0; 
										} 
									} 
								} 
								else 
								{ 
									ib.Pressed = false; 
									ib.TimeSinceActioned = 0; 
									ib.TimeSincePressed = 0; 
								} 
							} 
						} //NEXT i
					} 
					else if (this.DeviceKeyboard.Properties.BufferSize > 0) //buffered input
					{ 
						BufferedDataCollection bdc; 
						bdc = this.DeviceKeyboard.GetBufferedData(); 

						foreach (InputAssignement i in this.Assignements.KeyboardAssignements) 
						{ 
							//only button inputs are available in keyboard check
							if (i is ButtonInputAssignement) 
							{ 
								ib = ((ButtonInputAssignement)(i)); 

								bool[] b = new bool[ib.KeyCodes.GetLength(0)]; //if keys are pressed or not

								k = 0;
								foreach (KeyState j in ib.KeyCodes) 
								{ 
									if (ib.WaitingTime > -1) b[k] = ib.Pressed;
														else b[k] = false; 

									if (bdc == null)
									{
                                        //no buffer data to process
									}
									else 
									{ 
										//each data in the buffer must be examined, loop through buffer
										foreach (BufferedData d in bdc) 
										{ 
											if (d.Offset == j.Code) //if the data resembles this key
											{ 
												//check if the key was pressed now (or if the key was not pressed now, depending on key.check), the buffer high bit is 1
												if (!(((d.Data & 128) != 0) ^ j.Check)) b[k] = true; 
																				   else b[k] = false; 
											} 
										} 
									}

									k += 1; 
								} //NEXT j

								bool perf = true; //perf will remember if this action should be performed
								for (k=0; k<=b.Length-1; k++) 
								{ 
									if (!(b[k])) 
									{ 
										//action will not happen as at least one necesary key is not pressed
										perf = false; 
										break;
									} 
								} 

								if (perf) 
								{ 
                                    //action could happen
									ib.Pressed = true; 
									ib.TimeSincePressed += 1; 

									if (ib.TimeSincePressed > ib.WaitingTime) 
									{
										//action is allowed
										ib.TimeSinceActioned += 1; 

										if (ib.TimeSinceActioned > ib.RepeatingTime) 
										{ 
											//action occures this time
											this.PerformAction(ib.ControlActionIndex,1); 

											ib.TimeSinceActioned = 0; 
										} 
									} 
								}
								else 
								{   
									//action will not happen
									ib.Pressed = false; 
									ib.TimeSinceActioned = 0; 
									ib.TimeSincePressed = 0; 
								} 
							} 
						} 
					} 
				} 
				catch 
				{ 
                    //the control is probably not aquired
					try 
					{ 
						this.DeviceKeyboard.Acquire(); 
					} 
					catch 
					{ 
						//the device could not be reaquired
					} 
					return false; 
				} 
			} 
			else 
			{
				//no device present
				return false; 
			}

			return true; 
		} 








		//Processes mouse
		//Returns false if it didn't succede because the device is not set or it is not aquired
		protected bool ProcessMouse() 
		{ 
			//processing mouse
			if (!((this.DeviceMouse == null))) 
			{ 
				try 
				{ 
					this.DeviceMouse.Poll(); 

					ButtonInputAssignement ib; 
					AxisInputAssignement ia; 
					int k; //temp counter

					if (this.DeviceMouse.Properties.BufferSize == 0) //if it's 0 then it's probably not intended to use the buffer
					{ 
						MouseState ms; 
						ms = this.DeviceMouse.CurrentMouseState; 
						byte[] mousebuttons; 
						mousebuttons = ms.GetMouseButtons(); 

						foreach (InputAssignement i in this.Assignements.MouseAssignements) 
						{ 
							//buttons check first
							if (i is ButtonInputAssignement) 
							{ 
								ib = ((ButtonInputAssignement)(i)); 

								bool b = true; //if all keys are pressed or not
								foreach (KeyState jb in ib.KeyCodes) 
								{ 
									if (((mousebuttons[jb.Code] & 128) != 0) ^ jb.Check)
									{ 
										//one button is not pressed (or pressed, depending on check type), action will not occure
										b = false; 
									} 
								} 

								if (b) 
								{ 
									ib.Pressed = true; 
									ib.TimeSincePressed += 1; 

									if (ib.TimeSincePressed > ib.WaitingTime) 
									{ 
										//action is allowed
										ib.TimeSinceActioned += 1; 

										if (ib.TimeSinceActioned > ib.RepeatingTime) 
										{ 
											//action occures this time
											this.PerformAction(ib.ControlActionIndex,1); 

											ib.TimeSinceActioned = 0; 
										} 
									} 
								} 
								else 
								{
									//action doesn't occur
									ib.Pressed = false; 
									ib.TimeSinceActioned = 0; 
									ib.TimeSincePressed = 0; 
								} 
							} 
							else if (i is AxisInputAssignement) 
							{ 
								//axis check
								ia = (AxisInputAssignement)(i); 

								if (ia.PersistentCaller) 
								{ 
									//call no matter what if is persistent
									this.PerformAction(ia.ControlActionIndex, ms.X); 
									this.PerformAction(ia.ControlActionIndex, ms.Y); 
									this.PerformAction(ia.ControlActionIndex, ms.Z); 
								} 
								else 
								{ 
									//call only if axis is assigned and changes have happened recently - for each axis
									if (ia.Axis == Axis3.X) 
									{ 
										if (ms.X != 0) this.PerformAction(ia.ControlActionIndex, ms.X); 
									} 
									if (ia.Axis == Axis3.Y) 
									{ 
										if (ms.Y != 0) this.PerformAction(ia.ControlActionIndex, ms.Y); 
									} 
									if (ia.Axis == Axis3.Z) 
									{ 
										if (ms.Z != 0) this.PerformAction(ia.ControlActionIndex, ms.Z); 
									} 
								}
							} 
						} 
					} //NEXT i
					else if (this.DeviceMouse.Properties.BufferSize == 0) //buffered input
					{ 
						BufferedDataCollection bdc; 
						bdc = this.DeviceMouse.GetBufferedData(); 

						foreach (InputAssignement i in this.Assignements.MouseAssignements) 
						{ 
							//buttons check first
							if (i is ButtonInputAssignement) 
							{ 
								ib = (ButtonInputAssignement)(i); 

								bool[] b = new bool[ib.KeyCodes.Length]; //if keys are pressed or not
								
								k = 0; 
								foreach (KeyState jb in ib.KeyCodes) 
								{ 
									if (ib.WaitingTime > -1) b[k] = ib.Pressed; 
														else b[k] = false; 
									
									if (bdc == null) 
									{
										//no buffer data to process
									} 
									else 
									{ 
										//each data in the buffer must be examined, loop through buffer
										foreach (BufferedData d in bdc) 
										{ 
											if (d.Offset == (int)jb.ToDirectInputValue()) //if the data resembles this button key
											{ 
												//check if the key was pressed now, the buffer high bit is 1
												if ((d.Data & 128) != 0) b[k] = true; 
																	else b[k] = false; 
											}
										} 
									} 
									k += 1; 
								} //NEXT jb
								
								bool perf = true; //perf will remember if this action should be performed
								for (k=0; k<=b.Length-1; k++) 
								{ 
									if (!(b[k])) 
									{ 
										perf = false; 
										break;
									} 
								} 
								
								if (perf) 
								{ 
									//action could happen
									ib.Pressed = true; 
									ib.TimeSincePressed += 1; 

									if (ib.TimeSincePressed > ib.WaitingTime) 
									{ 
										//action is allowed
										ib.TimeSinceActioned += 1; 

										if (ib.TimeSinceActioned > ib.RepeatingTime) 
										{ 
											//action occures this time
											this.PerformAction(ib.ControlActionIndex,1); 

											ib.TimeSinceActioned = 0; 
										} 
									} 
								} 
								else 
								{ 
									//action will not happen
									ib.Pressed = false; 
									ib.TimeSinceActioned = 0; 
									ib.TimeSincePressed = 0; 
								} 
							} 
							else if (i is AxisInputAssignement) 
							{ 
								//axis check
								ia = ((AxisInputAssignement)(i)); 

								if (bdc == null) 
								{ 
									//no buffer data to process
								} 
								else 
								{ 
									//each data in the buffer must be examined, loop through buffer
									foreach (BufferedData d in bdc) 
									{ 
										if (ia.Axis == Axis3.X) 
										{ 
											if (d.Offset == (int)AxisInputAssignement.DirectInputValueOf(Axis3.X)) 
												this.PerformAction(ia.ControlActionIndex, d.Data); 
										} 
										if (ia.Axis == Axis3.Y) 
										{ 
											if (d.Offset == (int)AxisInputAssignement.DirectInputValueOf(Axis3.Y)) 
												this.PerformAction(ia.ControlActionIndex, d.Data); 
										} 
										if (ia.Axis == Axis3.Z) 
										{ 
											if (d.Offset == (int)AxisInputAssignement.DirectInputValueOf(Axis3.Z)) 
												this.PerformAction(ia.ControlActionIndex, d.Data); 
										} 
									} 
								} 
								//TODO: !! persistent caller?
							} 
						} //NEXT i
					} 
				} 
				catch 
				{ 
                    //the control is probably not aquired
					try 
					{ 
						this.DeviceMouse.Acquire(); 
					} 
					catch 
					{ 
						//the device could not be reaquired
					} 

					return false; 
				} 
			} 
			else 
			{ 
				//not device present
				return false; 
			} 

			return true; 
		} 
	}







	//ENUMS

	//Result from processing an Input Controller
	public enum ProcessingResult 
	{ 
		Success = -1, 
		Nothing = 0, 
		KeyboardProcessingFailed = 1, 
		MouseProcessingFailed = 2 
	} 

}
