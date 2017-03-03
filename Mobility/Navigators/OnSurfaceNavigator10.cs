//Class: OnSurface
//Author: Ovidiu Dolha
//Description:   -
//				 OnSurface Navigator v1.0 (made for a XZ surface)


using System;
using Microsoft.DirectX;
using Direct3DAid.Surfaces;
using Direct3DAid.SpaceObjects;
using Direct3DAid.Mobility.Controllers;


namespace Direct3DAid.Mobility.Navigators
{
	public class OnSurfaceNavigator10 : Navigator,
		ISpaceObject
	{ 
		//Info about the navigator
		public Version VersionNumber 
		{ 
			get 
			{ 
				return new Version(1, 0); 
			} 
		} 
		public string Author 
		{ 
			get 
			{ 
				return "Ovidiu Dolha"; 
			} 
		} 

		public string Comments 
		{ 
			get 
			{ 
				return "A merely wroking navigator, used only for test purposes. It has no real performance or integrity."; 
			} 
		}


		//Navigator fields
		//positioning
		protected Vector3 posF; 
		protected float velF; 
		protected float accF; 
		//rotation
		protected Vector3 rot; 
		protected Vector3 rotvel; 
		protected Vector3 rotacc; 

		



		//Represents how much will the velocity of the navigator yeld to a null value; 1-no affection
		private float _PositionSlowFactor; 
		public float PositionSlowFactor 
		{ 
			get 
			{ 
				return this._PositionSlowFactor; 
			} 
			set 
			{ 
				if (value != 0) 
				{ 
					this._PositionSlowFactor = value; 
				} 
			} 
		}

		//Represents how much will the rotation velocity of the navigator yeld to a null value; 1-no affection
		private float _RotationSlowFactor; 
		public float RotationSlowFactor 
		{ 
			get 
			{ 
				return this._RotationSlowFactor; 
			} 
			set 
			{ 
				if (value != 0) 
				{ 
					this._RotationSlowFactor = value; 
				} 
			} 
		} 

		
		
		
		//The folowing properties are not the values themselves, but values that affect vel, acc, rotvel and rotacc when certain actions happen
		
		private float _Velocity; 
		public float Velocity 
		{ 
			get 
			{ 
				return this._Velocity; 
			} 
			set 
			{ 
				this._Velocity = value; 
			} 
		} 

		private float _Acceleration; 
		public float Acceleration 
		{ 
			get 
			{ 
				return this._Acceleration; 
			} 
			set 
			{ 
				this._Acceleration = value; 
			} 
		} 

		private float _RotationVelocity; 
		public float RotationVelocity 
		{ 
			get 
			{ 
				return this._RotationVelocity; 
			} 
			set 
			{ 
				this._RotationVelocity = value; 
			} 
		} 

		private float _RotationAcceleration; 
		public float RotationAcceleration 
		{ 
			get 
			{ 
				return this._RotationAcceleration; 
			} 
			set 
			{ 
				this._RotationAcceleration = value; 
			} 
		} 

		private float _MaxVelocity; 
		public float MaxVelocity 
		{ 
			get 
			{ 
				return this._MaxVelocity; 
			} 
			set 
			{ 
				this._MaxVelocity = value; 
			} 
		} 

		private float _MaxAcceleration; 
		public float MaxAcceleration 
		{ 
			get 
			{ 
				return this._MaxAcceleration; 
			} 
			set 
			{ 
				this._MaxAcceleration = value; 
			} 
		} 
		
		private float _MaxRotationVelocity; 
		public float MaxRotationVelocity 
		{ 
			get 
			{ 
				return this._MaxRotationVelocity; 
			} 
			set 
			{ 
				this._MaxRotationVelocity = value; 
			} 
		} 

		private float _MaxRotationAcceleration; 
		public float MaxRotationAcceleration 
		{ 
			get 
			{ 
				return this._MaxRotationAcceleration; 
			} 
			set 
			{ 
				this._MaxRotationAcceleration = value; 
			} 
		} 

	
		//Surface attached
		private Surface _Surface;
		public Surface Surface
		{
			get
			{
				return this._Surface;
			}
			set
			{
				this._Surface = value;
			}
		}

		//Distance from ground (the height of the navigator)
		private float _DistanceFromGround;
		public float DistanceFromGround
		{
			get
			{
				return this._DistanceFromGround;
			}
			set
			{
				this._DistanceFromGround = value;
			}
		}








		//Class Constructor
		public OnSurfaceNavigator10(SpaceObject spaceObject, Controller controller, Surface surfaceAttached, float distanceFromGround):
			base(spaceObject, controller)
		{ 
			this.Controller.ActionPerformed += new Controller.ActionPerformedHandler(this.OnAction); 
			
			//default values
			this.Velocity = 0F; 
			this.Acceleration = 0.1F; 
			this.RotationAcceleration = 0F; 
			this.RotationVelocity = 0.01F; 
			this.PositionSlowFactor = 1.4F; 
			this.RotationSlowFactor = 1.2F; 
			
			this.MaxVelocity = 1.2F; 
			this.MaxAcceleration = 0.3F; 
			this.MaxRotationVelocity = 0.12F; 
			this.MaxRotationAcceleration = 0.02F; 
			
			
			this.posF = new Vector3(1,1,4); 
			this.rot = new Vector3(0, 0, 0); 
			this.rotvel = new Vector3(0, 0, 0); 
			this.rotacc = new Vector3(0, 0, 0); 


			this._Surface = surfaceAttached;

			this._DistanceFromGround = distanceFromGround;
		} 







		//Processes navigation
		public void Process() 
		{ 
			//transform the vectors that affect the position
			/*
			Vector3 rotaccT = this.Transformer.Transform(this.rotacc);
			Vector3 rotvelT = this.Transformer.Transform(this.rotvel);
			Vector3 posFT = this.Transformer.Transform(this.posF);
			Vector3 rotT = this.Transformer.Transform(this.rot);
			*/
			
			Vector3 rotaccT = this.rotacc;
			Vector3 rotvelT = this.rotvel;
			Vector3 posFT = this.posF;
			Vector3 rotT = this.rot;


			this.accF *= 1 / this.PositionSlowFactor; 
			this.rotacc.Multiply(1 / this.RotationSlowFactor);

			if (Math.Abs(this.accF) > this.MaxAcceleration) 
				this.accF = this.MaxAcceleration * Math.Sign(this.accF); 
			if (Math.Abs(rotaccT.X) > this.MaxRotationAcceleration) 
				rotaccT.X = this.MaxRotationAcceleration * Math.Sign(rotaccT.X); 
			if (Math.Abs(rotaccT.Y) > this.MaxRotationAcceleration) 
				rotaccT.Y = this.MaxRotationAcceleration * Math.Sign(rotaccT.Y); 
			if (Math.Abs(rotaccT.Z) > this.MaxRotationAcceleration) 
				rotaccT.Z = this.MaxRotationAcceleration * Math.Sign(rotaccT.Z); 
			
			this.velF += this.accF; 
			
			rotvelT.Add(rotaccT); 
			this.velF *= 1 / this.PositionSlowFactor; 
			rotvelT.Multiply(1 / this.RotationSlowFactor); 

			if (Math.Abs(this.velF) > this.MaxVelocity) 
				this.velF = this.MaxVelocity * Math.Sign(this.velF); 
			if (Math.Abs(rotvelT.X) > this.MaxRotationVelocity) 
				rotvelT.X = this.MaxRotationVelocity * Math.Sign(rotvelT.X); 
			if (Math.Abs(rotvelT.Y) > this.MaxRotationVelocity) 
				rotvelT.Y = this.MaxRotationVelocity * Math.Sign(rotvelT.Y); 
			if (Math.Abs(rotvelT.Z) > this.MaxRotationVelocity) 
				rotvelT.Z = this.MaxRotationVelocity * Math.Sign(rotvelT.Z); 

			Vector3 dir; 
			dir = Misc.GetMatrixDirectionZ(this.SpaceObject.SelfMatrix); 
			//transform the direction also, because it is relative to the coordinate system
			Vector3 dirT = dir;
			dirT.Multiply(this.velF); 
			posFT.Add(dirT); 

			rotT.Add(rotvelT); 

			//works as free spirit if no surface is attached
			if (this.Surface != null) posFT.Y = this.DistanceFromGround + this.Surface.GetPositionHeight(posFT.X,posFT.Z, ShiftModes.AutoDetect);


		
			posFT.Multiply(-1F);
			this.SpaceObject.SelfMatrix = Matrix.Multiply(Matrix.Translation(posFT), Matrix.Multiply(Matrix.RotationZ(rotT.Z), Matrix.Multiply(Matrix.RotationY(rotT.Y), Matrix.RotationX(rotT.X)))); 
			posFT.Multiply(-1F);

		

			//Transform information back to the navigator's default coordinate system (the transformations are only for export values)
			/*
			this.rotacc = this.Transformer.TransformInv(rotaccT);
			this.rotvel = this.Transformer.TransformInv(rotvelT);
			this.posF = this.Transformer.TransformInv(posFT);
			this.rot = this.Transformer.TransformInv(rotT);
			*/

			this.rotacc = rotaccT;
			this.rotvel = rotvelT;
			this.posF = posFT;
			this.rot = rotT;
		} 






		//This method is called when an action occurs from the controller, and the navigaor interprets it
		private void OnAction(int index, string name, double val) 
		{ 
			if (index == 0) 
			{ 
				this.accF = this.Acceleration; 
				this.velF = this.Velocity; 
			} 
			else if (index == 1) 
			{ 
				this.accF = -this.Acceleration / 2; 
				this.velF = -this.Velocity / 2; 
			} 
			else if (index == 6) 
			{ 
				if (val > 0) 
				{ 
					this.rotvel.Z = (float)(-this.RotationVelocity * val); 
					this.rotacc.Z = (float)(-this.RotationAcceleration * val); 
				} 
			} 
			else if (index == 7) 
			{ 
				if (val > 0) 
				{ 
					this.rotvel.Z = (float)(+this.RotationVelocity * val); 
					this.rotacc.Z = (float)(+this.RotationAcceleration * val); 
				} 
			} 
			else if (index == 8) 
			{ 
				if (val < 0) 
				{ 
					this.rotvel.Y = (float)(-this.RotationVelocity * val); 
					this.rotacc.Y = (float)(-this.RotationAcceleration * val); 
				} 
			} 
			else if (index == 9) 
			{ 
				if (val > 0) 
				{ 
					this.rotvel.Y = (float)(-this.RotationVelocity * val); 
					this.rotacc.Y = (float)(-this.RotationAcceleration * val); 
				} 
			} 
			else if (index == 10) 
			{ 
				if (val < 0) 
				{ 
					this.rotvel.X = (float)(-this.RotationVelocity * val); 
					this.rotacc.X = (float)(-this.RotationAcceleration * val); 
				} 
			} 
			else if (index == 11) 
			{ 
				if (val > 0) 
				{ 
					this.rotvel.X = (float)(-this.RotationVelocity * val); 
					this.rotacc.X = (float)(-this.RotationAcceleration * val); 
				} 
			} 
		} 






		//Returns a default controller suitable for this navigator
		public static new Controller GetDefaultController() 
		{ 
			Controller c = new Controller(); 

			c.ControlActions.Add("Foreward"); //0
			c.ControlActions.Add("Backward"); //1
			c.ControlActions.Add("Left"); //2
			c.ControlActions.Add("Right"); //3
			c.ControlActions.Add("Up"); //4
			c.ControlActions.Add("Down"); //5
			c.ControlActions.Add("RollLeft"); //6
			c.ControlActions.Add("RollRight"); //7
			c.ControlActions.Add("TurnLeft"); //8
			c.ControlActions.Add("TurnRight"); //9
			c.ControlActions.Add("TurnUp"); //10
			c.ControlActions.Add("TurnDown"); //11
			
			return c; 
		} 
	}
}
