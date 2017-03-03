//Class: VertexN
//Author: Ovidiu Dolha
//Description:   -
//				 A simple class that holds a value of a current vertex index (in the VB) and several pointers to other vertices that are neighbour to this (in the IB)


using System;


namespace Direct3DAid.Geometries
{
	public class VertexN
	{
		//Index of this curent veretex (in the VB array)
		private int _IndexValue;
		public int IndexValue
		{
			get
			{
				return this._IndexValue;
			}
			set
			{
				this._IndexValue=value;
			}
		}


		//Neighbours of this vertex;
		private System.Collections.ArrayList _Neighbours;
		public System.Collections.ArrayList Neighbours
		{
			get
			{
				return this._Neighbours;
			}
			set
			{
				this._Neighbours=value;
			}
		}




	

		//Class Constructor 1
		public VertexN()
		{
			this._IndexValue = -1;
			this._Neighbours = new System.Collections.ArrayList();
		}

		//Class Constructor 2
		public VertexN(int indexValue)
		{
			this._IndexValue = indexValue;
			this._Neighbours = new System.Collections.ArrayList();
		}
	}
}
