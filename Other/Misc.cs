// MISCELANIOUS FUNCTIONS ARE DESCRIBED HERE


using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Direct3DAid.Geometries;
using Direct3DAid.Geometries.VerticesModifications;
using Direct3DAid.Surfaces;



namespace Direct3DAid
{
	public class Misc
	{
		//gets a random color that is near a given color, and within a random limit, given a random object
		public static Color GetRandomColor(Color originColor, int randomLimit, Random random)
		{
			Random r = random;

			int colR, colG, colB, v;

			v = r.Next(-randomLimit, randomLimit);
			if (v < 0) v = 0;
			if (v > 255) v = 255;
			colR = originColor.R + v;

			v = r.Next(-randomLimit, randomLimit);
			if (v < 0) v = 0;
			if (v > 255) v = 255;
			colG = originColor.R + v;

			v = r.Next(-randomLimit, randomLimit);
			if (v < 0) v = 0;
			if (v > 255) v = 255;
			colB = originColor.R + v;

			return Color.FromArgb(colR, colG, colB);
		}

		//gets a random color that is near a given color, and within a random limit
		public static Color GetRandomColor(Color originColor, int randomLimit)
		{
			Random r = new Random();

			return Misc.GetRandomColor(originColor, randomLimit, r);
		}



		//Gets a default keyboard device
		public static Microsoft.DirectX.DirectInput.Device GetDefaultKeyboardDevice(System.Windows.Forms.Control control, int bufferSize) 
		{ 
			Microsoft.DirectX.DirectInput.Device dev; 
			dev = new Microsoft.DirectX.DirectInput.Device (Microsoft.DirectX.DirectInput.SystemGuid.Keyboard); 

			dev.SetCooperativeLevel(control, Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Foreground | Microsoft.DirectX.DirectInput.CooperativeLevelFlags.NonExclusive); 
			dev.SetDataFormat(Microsoft.DirectX.DirectInput.DeviceDataFormat.Keyboard); 
			dev.Properties.BufferSize = bufferSize; 

			try 
			{ 
				dev.Acquire(); 
			} 
			catch (Exception e)
			{ 
				throw new ExceptionDirectXAid("Could not aquire default keyboard device. Original exception: <<" + e.Message + ">>"); 
			} 
			return dev; 
		} 

		//Gets a default mouse device
		public static Microsoft.DirectX.DirectInput.Device GetDefaultMouseDevice(System.Windows.Forms.Control control, int bufferSize) 
		{ 
			Microsoft.DirectX.DirectInput.Device dev; 
			dev = new Microsoft.DirectX.DirectInput.Device(Microsoft.DirectX.DirectInput.SystemGuid.Mouse); 

			dev.SetCooperativeLevel(control, Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Foreground | Microsoft.DirectX.DirectInput.CooperativeLevelFlags.NonExclusive); 
			dev.SetDataFormat(Microsoft.DirectX.DirectInput.DeviceDataFormat.Mouse); 
			dev.Properties.BufferSize = bufferSize; 

			try 
			{ 
				dev.Acquire(); 
			} 
			catch (Exception e)
			{ 
				throw new ExceptionDirectXAid(" Could not aquire default mouse device. Original exception: <<" + e.Message + ">>"); 
			} 
			return dev; 
		}











		//Returns whether a Point/Vector is inside a domain of boundaries or not
		//1. integer-Point within integer-Size (inclusive 0-s and margins) / Offset adds to the width/height of the bounds
		// - coordinates in Point and Size types
		public static bool WithinBounds(Point point, Size bounds, int widthOffset, int heightOffset)
		{
			if ((point.X >= 0) && (point.Y >= 0) && (point.X <= (bounds.Width + widthOffset)) && (point.Y <= (bounds.Height + heightOffset)))
				return true;
			else 
				return false;
		}
		// - coordinates in primitive types (int)
		public static bool WithinBounds(int pointX, int pointY, int boundsWidth, int boundsHeight, int widthOffset, int heightOffset)
		{
			if ((pointX >= 0) && (pointY >= 0) && (pointX <= (boundsWidth + widthOffset)) && (pointY <= (boundsHeight + heightOffset)))
				return true;
			else 
				return false;
		}
        





		//Retrieves the secvential index k of an element in an array Width*Height, given the coordinates (x,y)
		public static int GetIndexFromCoordinates(int arrayWidth, Point coords)
		{
			return (coords.Y*arrayWidth + coords.X);
		}

		//Retrieves the coordinates (x,y) of an element in an array Width*Height, given the secvential index k
		public static Point GetCoordinatesFromIndex(int arrayWidth, int index)
		{
			return new Point((int)(index%arrayWidth),(int)(index/arrayWidth));
		}





		//Takes an array of objects and makes elements unique
		public static System.Collections.ArrayList MinimalizeArrayList(System.Collections.ArrayList array)
		{
			System.Collections.ArrayList newArray;
			newArray = new System.Collections.ArrayList();
        
			for (int i=0; i<=array.Count-1; i++)
			{
				if (!newArray.Contains(array[i])) newArray.Add(array[i]);
			}
   
			return newArray;
		}



		//determins if an integer value is contained in an ArrayList
		public static bool ArrayListContainsInt(System.Collections.ArrayList array, int val)
		{
			for (int i=0; i<=array.Count-1; i++)
			{
				if (array[i].Equals(val)) return true;
			}
			return false;
		}

	



		//Returns the number of vertices from a VB object
		public static int GetNoOfVertices(VertexBuffer vB)
		{
			System.Array verts;
			verts = vB.Lock(0,0);
			int n = verts.Length;
			vB.Unlock();
			return n;
		}

		//Returns the number of indices from an IB object
		public static int GetNoOfIndices(IndexBuffer iB)
		{
			short[] indices;
			indices = (short[])iB.Lock(0,0);
			int n = indices.Length;
			iB.Unlock();
			return n;
		}





		//Returns origin position from a matrix
		public static Vector3 GetMatrixPosition(Matrix mat)
		{
			return new Vector3(mat.M41, mat.M42, mat.M43); 
		}

		//Returns X direction from a matrix
		public static Vector3 GetMatrixDirectionX(Matrix mat)
		{
			return new Vector3(mat.M11, mat.M21, mat.M31); 
		}

		//Returns Y direction from a matrix
		public static Vector3 GetMatrixDirectionY(Matrix mat)
		{
			return new Vector3(mat.M12, mat.M22, mat.M32); 
		}

		//Returns Z direction from a matrix
		public static Vector3 GetMatrixDirectionZ(Matrix mat)
		{
			return new Vector3(mat.M13, mat.M23, mat.M33); 
		}

	



		//Returns the normal of a triangle (points out if the tri is declared CW)
		//(scale = 1.0F)
		public static Vector3 GetTriangleNormal(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			return Misc.GetTriangleNormal(p1,p2,p3,1.0F);
		}

		//Returns the normal of a triangle (points out if the tri is declared CW)
		//(can be scaled)
		public static Vector3 GetTriangleNormal(Vector3 p1, Vector3 p2, Vector3 p3, float scale)
		{
			Plane p;
			p = Plane.FromPoints(p1,p2,p3);
        
			return new Vector3(p.A*scale,p.B*scale,p.C*scale);
		}








		//Takes one vertex modification and returns an array of a specified size that contains modifications cloned from the one given, but each with a different index (applied for a number of vertices given)
		//TODO: NOT TESTED!
		public static VertexModification[] MultiplyModification (VertexModification baseMod, int numVerts)
		{
			VertexModification[] vm;
			vm = new VertexModification[numVerts];

			for (int i=0; i<=numVerts-1; i++)
			{
				vm[i] = (VertexModification)baseMod.Clone();
				vm[i].Index=i;
			}

			return vm;
		}
    
	}
}