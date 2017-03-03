//Structure: KeyState
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//				 Structure to hold a key in action: the code and cheking method: whether to check if it's pressed or check if is not; in case of a mouse button, it stores 0...7 depending on the mouse button. larger numbers are ignored


using System;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;


namespace Direct3DAid.Mobility.Controllers
{
	public struct KeyState
	{ 
		//Code of this key (or mouse button)
		private byte _Code;
		public byte Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				this._Code = value;
			}
		}
		
		
		//When true, it means classes that use this key will check for a pressed key, when false they will check for a non-pressed one;
		private bool _Check;
		public bool Check
		{
			get
			{
				return this._Check;
			}
			set
			{
				this._Check = value;
			}
		}





		//Structure Constructor 1
		public KeyState(byte code) 
		{ 
			this._Code = code; 
			this._Check = true; 
		} 


		//Structure Constructor 2
		public KeyState(byte code, bool check) 
		{ 
			this._Code = code; 
			this._Check = check; 
		} 


		

		//Returns the direct input mouse code from the button number			
		public MouseOffset ToDirectInputValue() 
		{ 
			if (this.Code == 0) return MouseOffset.Button0;
			else if (this.Code == 1) return MouseOffset.Button1;
			else if (this.Code == 2) return MouseOffset.Button2;
			else if (this.Code == 3) return MouseOffset.Button3;
			else if (this.Code == 4) return MouseOffset.Button4;
			else if (this.Code == 5) return MouseOffset.Button5;
			else if (this.Code == 6) return MouseOffset.Button6;
			else if (this.Code == 7) return MouseOffset.Button7;
			else return (MouseOffset)(-1);
		} 
	}
}
