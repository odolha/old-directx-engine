//Structure: ControlPoint
//Author: Ovidiu Dolha
//Description:   -
//               A class containing information about one control point from a Surface (height, color, division number...)


using System;


namespace Direct3DAid.Surfaces
{
	public struct ControlPoint:
		ICloneable 
	{
		//Height of this point (on the surface)
		private float _Height;
		public float Height
		{
			get
			{
				return this._Height;
			}
			set
			{
				this._Height = value;
			}
		}


		//Color of this point (on a colored surface)
		private int _Color;
		public int Color
		{
			get
			{
				return this._Color;
			}
			set
			{
				this._Color = value;
			}
		}

    
		//To which division this point belong (for splatted surface with more than one division) - from 0 to numofdiv-1 (must obey this!)
		private uint _Division;
		public uint Division
		{
			get
			{
				return this._Division;
			}
			set
			{
				this._Division = value;
			}
		}

	


	
		//Structure Constructor 1
		public ControlPoint(float height)
		{
			this._Height=height;
			this._Color=0;
			this._Division=0;
		}

		//Structure Constructor 2
		public ControlPoint(float height, int color)
		{
			this._Height=height;
			this._Color=color;
			this._Division=0;
		}

		//Structure Constructor 3
		public ControlPoint(float height, uint division)
		{
			this._Height=height;
			this._Color=0;
			this._Division=division;
		}

		//Structure Constructor 4
		public ControlPoint(float height, int color, uint division)
		{
			this._Height=height;
			this._Color=color;
			this._Division=division;
		}





		//Clones object
		public object Clone()
		{
			return new ControlPoint(this.Height, this.Color, this.Division);
		}
	}
}