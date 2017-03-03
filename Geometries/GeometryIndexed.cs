//Class: GeometryIndexed <- GeometryTemplate
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//               A class that would handle all indexed geometry. It encapsulates a VB and an IB that define the geometry along with some other properties and methods


using System;
using System.Drawing;
using Microsoft.DirectX; 
using Microsoft.DirectX.Direct3D;


namespace Direct3DAid.Geometries
{
	public class GeometryIndexed : GeometryTemplate
	{ 
		protected int step = 0;

		//For methods that might take alot of time and that deal with loops. If this is on then the alerts (events) raised from those methods are raised inside long loops for each iteration, specifying information about the number of vertex/primitive/point etc that is currently processed
		public bool _FrequentAlerts = false;
		public bool FrequentAlerts
		{
			get
			{
				return this._FrequentAlerts;
			}
			set
			{
				this._FrequentAlerts = value;
			}
		}


		//
		//Device inherited
		//
    
	
		//Vertex Buffer used to hold vertice information
		//MOST OF THE TIME, THE USER SHOULD NOT CHANGE THIS OBJECT IN ANY WAY
		protected VertexBuffer _VB; 
		public VertexBuffer VB 
		{ 
			get 
			{ 
				return this._VB; 
			} 
			set
			{
				if (value!=null) //null value doesnt need updating
				{
					this._VB = value; 
					this.UpdateNoOfVertices();
					this.UpdateNeighbourList();  //the geometry is changed, so the neighbour list must be updated

					if (this.VBModified != null) this.VBModified();
				}
			}
		} 
	
		//Index Buffer used to hold triangle information
		//MOST OF THE TIME, THE USER SHOULD NOT CHANGE THIS OBJECT IN ANY WAY (if you must, then you should use UpdateNeighbours manually)
		protected IndexBuffer _IB; 
		public IndexBuffer IB 
		{ 
			get 
			{ 
				return this._IB; 
			} 
			set 
			{ 
				if (value!=null) //null value doesnt need updating
				{
					this._IB = value;
					this.UpdateNoOfIndices();
					this.UpdateNeighbourList(); //the geometry is changed, so the neighbour list must be updated

					if (this.IBModified != null) this.IBModified();
				}
			} 
		} 


		//sets both VB and IB at the same time (for performance use this)
		public void SetVBAndIB(VertexBuffer vB, IndexBuffer iB)
		{
			this._VB = vB; 
			this._IB = iB;

			this.UpdateNoOfVertices();
			this.UpdateNoOfIndices();
			this.UpdateNeighbourList(); //the geometry is changed, so the neighbour list must be updated

			if (this.VBModified != null) this.VBModified();
			if (this.IBModified != null) this.IBModified();
		}




	
	
	
		//The type of primitives this geometry uses for rendering
		private PrimitiveType _PrimitivesType; 
		public PrimitiveType PrimitivesType 
		{ 
			get 
			{ 
				return this._PrimitivesType; 
			} 
			set 
			{
				//can only use TriangleList or LineList for now
				if (value==PrimitiveType.LineList) step=2;
				if (value==PrimitiveType.TriangleList) step=3;

				this._PrimitivesType = value; 
			} 
		} 




		//Retrieves number of vertices
		private int _NumVertices;
		public int NumVertices 
		{ 
			get 
			{ 
				return this._NumVertices;
			} 
		} 

	
		//Retrieves number of indices
		private int _NumIndices;
		public int NumIndices 
		{ 
			get 
			{ 
				return this._NumIndices;
			} 
		} 

	


		//Type of vertex used
		private Type _VertexType; 
		public Type VertexType 
		{ 
			get 
			{ 
				return this._VertexType; 
			} 
			set 
			{ 
				this._VertexType = value; 
			} 
		} 

	
		//Format of vertex used (usualy matches the vertextype)
		private VertexFormats _VertexFormat; 
		public VertexFormats VertexFormat 
		{ 
			get 
			{ 
				return this._VertexFormat; 
			} 
			set 
			{ 
				this._VertexFormat = value; 
			} 
		} 


	


		//Vertex neighbour list (an array that holds vertex-neighbour information about each vertex)
		//This list is auto-update when needed, or can be manually updated
		private VertexN[] _VertexNeighbourList;
		private VertexN[] VertexNeighbourList
		{
			get
			{
				return this._VertexNeighbourList;
			}
		}
		public VertexN GetVertexNeighbour(int index)
		{
			return this.VertexNeighbourList[index];
		}



		//RECONSIDER THE USE OF THIS LATER
		//Primitive neighbour list (an array that holds primitive-neighbour information about each vertex)
		//This list is auto-update when needed, or can be manually updated
		//Is a list of arrays because one primitve contains many indices
		private VertexN[] _PrimitiveNeighbourList;
		private VertexN[] PrimitiveNeighbourList
		{
			get
			{
				return this._PrimitiveNeighbourList;
			}
		}
		public VertexN GetPrimitiveNeighbour(int index)
		{
			return this.PrimitiveNeighbourList[index];
		}





		//EVENTS AND DELEGATES (general)
		
		//Alerts when the VB is modified
		public delegate void VBModifiedHandler();
		public event VBModifiedHandler VBModified;
	
		//Alerts when the IB is modified
		public delegate void IBModifiedHandler();
		public event IBModifiedHandler IBModified;
	







		//Class Constructor 1
		public GeometryIndexed(Device device, VertexBuffer vB, IndexBuffer iB, Type vertexType, VertexFormats vertexFormat, PrimitiveType primitivesType) :
			base()
		{ 
			this.Device = device; 
			this._PrimitivesType = primitivesType; 
			this._VertexType = vertexType; 
			this._VertexFormat = vertexFormat; 
			this._VB = vB; 
			this._IB = iB;

			//can only use TriangleList or LineList for now
			if (this._PrimitivesType==PrimitiveType.LineList) step=2;
			if (this._PrimitivesType==PrimitiveType.TriangleList) step=3;

			this.UpdateNoOfVertices();
			this.UpdateNoOfIndices();
			this.UpdateNeighbourList();
		} 


		//Class Constructor 2
		public GeometryIndexed(Device device, int numVertices, int numIndices, Type vertexType, VertexFormats vertexFormat, PrimitiveType primitivesType) :
			base()
		{ 
			this.Device = device; 
			this._PrimitivesType = primitivesType; 
			this._VertexType = vertexType; 
			this._VertexFormat = vertexFormat; 
			this._VB = new VertexBuffer(this._VertexType, numVertices, this.Device, Usage.SoftwareProcessing, this._VertexFormat, Pool.Default); 
			this._IB = new IndexBuffer(typeof(short), numIndices * 3, this.Device, Usage.SoftwareProcessing, Pool.Default);

			//can only use TriangleList or LineList for now
			if (this._PrimitivesType==PrimitiveType.LineList) step=2;
			if (this._PrimitivesType==PrimitiveType.TriangleList) step=3;
		
			this.UpdateNoOfVertices();
			this.UpdateNoOfIndices();
			this.UpdateNeighbourList();
		} 

		//Class Constructor 3
		public GeometryIndexed(Device device, Type vertexType, VertexFormats vertexFormat, PrimitiveType primitivesType) :
			base()
		{ 
			this.Device = device; 
			this._PrimitivesType = primitivesType; 
			this._VertexType = vertexType; 
			this._VertexFormat = vertexFormat; 
			this._VB = null; 
			this._IB = null;

			//can only use TriangleList or LineList for now
			if (this._PrimitivesType==PrimitiveType.LineList) step=2;
			if (this._PrimitivesType==PrimitiveType.TriangleList) step=3;
		} 









	


		//Reset - Variant 1
		//Changes the vertices (position, normal, color, texture coord)
		//has an array of modifications for most used components in the vertex type (see above)
		//if a certain element is not conatined in this vertex type (or no vertex needs a change), a null reference should be given
		public void ResetVertices (VerticesModifications.Vector3Modification[] posMod, VerticesModifications.ColorModification[] colorMod, VerticesModifications.Vector3Modification[] normalMod, VerticesModifications.TextureCoordinatesModification[] texCoordMod)
		{
			//retrieve the vertice in an array (of objects) since we don't know what kind of vertex type we have
			System.Array verts;
			verts = this._VB.Lock(0,0);

			IVertex o; //temp general vertex obj used to change propreties
        
			//reset position (if is the case and if the vertex format of this geometry allows it!)
			if (!(posMod == null) && ((this.VertexFormat & VertexFormats.Position) == VertexFormats.Position))
			{
				foreach (VerticesModifications.Vector3Modification v3m in posMod)
				{
					o = (IVertex)verts.GetValue((long)v3m.Index);
					o.Pos=v3m.NewVector;
					verts.SetValue(o,(long)v3m.Index);
				}
			}

			//reset color (if is the case and if the vertex format of this geometry allows it!)
			if (!(colorMod == null) && ((this.VertexFormat & VertexFormats.Diffuse) == VertexFormats.Diffuse))
			{
				foreach (VerticesModifications.ColorModification cm in colorMod)
				{
					o = (IVertex)verts.GetValue((long)cm.Index);
					if (cm.AlphaOnly==false)
					{
						o.Col=cm.NewColor;
					}
					else
					{
						o.Col=Color.FromArgb(Color.FromArgb(cm.NewColor).A,Color.FromArgb(o.Col).R,Color.FromArgb(o.Col).G,Color.FromArgb(o.Col).B).ToArgb();
					}
					verts.SetValue(o,(long)cm.Index);
				}
			}

			//reset normal (if is the case and if the vertex format of this geometry allows it!)
			if (!(normalMod == null) && ((this.VertexFormat & VertexFormats.Normal) == VertexFormats.Normal))
			{
				foreach (VerticesModifications.Vector3Modification v3m in normalMod)
				{
					o = (IVertex)verts.GetValue((long)v3m.Index);
					o.Nor=v3m.NewVector;
					verts.SetValue(o,(long)v3m.Index);
				}
			}

			//reset texture coord (if is the case and if the vertex format of this geometry allows it!)
			if (!(texCoordMod== null) && ((this.VertexFormat & VertexFormats.Texture1) == VertexFormats.Texture1))
			{
				foreach (VerticesModifications.TextureCoordinatesModification tcm in texCoordMod)
				{
					o = (IVertex)verts.GetValue((long)tcm.Index);
					o.Tex=tcm.NewTextureCoord;
					verts.SetValue(o,(long)tcm.Index);
				}
			}


			this._VB.Unlock();
		}





		//Reset - Variant 2
		//Changes the vertices' texture coordinates with a given 'Texture Fit' method (i.e. a UVMap object)
		public void ResetVertices (UVMap map)
		{
			map.FitOverVB(this._VB);
		}










	
		//Resets the neighbour list of this geometry (both vertex and primitves neighbours)
		public void UpdateNeighbourList()
		{
			//open the geometry information for use
			short[] indices;
			indices = (short[])this._IB.Lock(0,0);

			//search neighbours in the geometry declaration of triangles/lines (this part works only for LISTS of primitives (no fans or strips))

			//can only use TriangleList or LineList for now, if other is used then announce the user
			if (step==0) throw new ExceptionGeometry("Cannot handle other primitive type than TriangleList or LineList for now. CANNOT UPDATE NEIGHBOUR LIST! (change the primitve type to one of these)");

			int i,j,k;



			//search vertex-neighbours for each vertex FIRST
			this._VertexNeighbourList = new VertexN[this.NumVertices];

			//initialize the vertexNeighbourList's members with a new VertexN, with no neighbours;
			for (i=0; i<=this.NumVertices-1; i++)
			{
				this.VertexNeighbourList[i] = new VertexN(i);
			}

			//search through all primitves and add vertices from the same primitve as neighbours to eachother
			for (i=0; i<=this.NumIndices-1; i+=step)
			{
				//for each vertex from this primitive, add the rest as it's neighbours
				for (j=0; j<=step-1; j++)
				{
					for (k=0; k<=step-1; k++)
					{
						if (k!=j) this.VertexNeighbourList[indices[i+j]].Neighbours.Add(this.VertexNeighbourList[indices[i+k]]);
					}
				}
			}

			//some vertices might contain the same neighbour multiple times. minimalizing each array will fix this
			for (i=0; i<=this.NumVertices-1; i++)
			{
				this.VertexNeighbourList[i].Neighbours = Misc.MinimalizeArrayList(this.VertexNeighbourList[i].Neighbours);
			}


        
		
			//search primitive-neighbours for each vertex SECOND
			this._PrimitiveNeighbourList = new VertexN[this.NumVertices];

			//initialize the primitiveNeighbourList's members with a new VertexN[3], with no neighbours;
			for (i=0; i<=this.NumVertices-1; i++)
			{
				this.PrimitiveNeighbourList[i] = new VertexN(i);
			}

			//search through all primitves and add a current primitve as neighbour to all the vertices that it contains
			for (i=0; i<=this.NumIndices-1; i+=step)
			{
				//create an object to contain all vertices from this primitve
				VertexN[] pr;
				pr = new VertexN[step];
				for (j=0; j<=step-1; j++)
				{
					pr[j] = this.PrimitiveNeighbourList[indices[i+j]];
				}

				//for each vertex from this primitive, add this primitive as it's neighbour
				for (j=0; j<=step-1; j++)
				{
					this._PrimitiveNeighbourList[indices[i+j]].Neighbours.Add(pr);
				}
			}



			//close the data needed
			this._IB.Unlock();
		}










		//Updates the number of vertices
		public void UpdateNoOfVertices()
		{
			this._NumVertices = Misc.GetNoOfVertices(this._VB);
		}

		//Updates the number of indices
		public void UpdateNoOfIndices()
		{
			this._NumIndices = Misc.GetNoOfIndices(this._IB);
		}








		//Normalizes vertices of this geometry (only works for vertices that include normal information)
		//uses default scale value of 1.0F
		public void Normalize()
		{
			this.Normalize(1.0F);
		}

		//Normalizes vertices of this geometry (only works for vertices that include normal information)
		//uses a specified scale value (each normal vector will have an absolute value equal to this value)
		//only works for TriangleList for now
		public void Normalize(float scale)
		{
			//Alert - Normalizing
			if (this.Normalizing != null)
				this.Normalizing(new NormalizingInfo(NormalizingStage.Starting, -1, GettingPrimitiveNormalsInfo.Empty));

			//Cannot normalize if the vertex format of this geometry doesn't allow it to
			if ((this.VertexFormat & VertexFormats.Normal) != VertexFormats.Normal)
				throw new ExceptionGeometry ("Cannot normalize this indexed geometry because the vertex format doesn't contain information about normal");


			//retrieve a list with generated normals of each primitve (scaled to 1)
			Vector3[] pNorms;

			if (this.Normalizing != null)
			{
				//Alerts in separate method (for when inside), so it must add a handler method to the event that is raised when inside the GettingPrimitiveNormals method. After this is over, it must remove the handler
				GettingPrimitiveNormalsHandler h = new GettingPrimitiveNormalsHandler(OnGettingPrimitiveNormals);
				this.GettingPrimitiveNormals += h;
				pNorms = this.GetPrimitiveNormals();
				this.GettingPrimitiveNormals -= h;
			}
			else 
			{
				//just call the method
				pNorms = this.GetPrimitiveNormals();
			}

			//retrieve the vertice in an array (of objects) since we don't know what kind of vertex type we have
			System.Array verts;
			verts = this._VB.Lock(0,0);
			IVertex o; //temp general vertex obj used to change propreties

			if (step!=3) throw new ExceptionGeometry("Normalization is only allowed for TriangleList geometries.");
       
			int i,j,k;


			//Alert - Normalizing
			if (this.Normalizing != null)
				this.Normalizing(new NormalizingInfo(NormalizingStage.GettingPrimitiveNeighbourNormals, -1, GettingPrimitiveNormalsInfo.Empty));

			//for each vertex
			for (i=0; i<=this.NumVertices-1; i++)
			{
				//Alert - Normalizing
				if ((this.Normalizing != null) && (this.FrequentAlerts))
					this.Normalizing(new NormalizingInfo(NormalizingStage.GettingPrimitiveNeighbourNormals, i, GettingPrimitiveNormalsInfo.Empty));

				o = (IVertex)verts.GetValue(i);

				Vector3[] nor; //will hold normals for each neighbour primitive
				nor = new Vector3[this.PrimitiveNeighbourList[i].Neighbours.Count];
				
				//for each of it's primitve neighbours
				k=0;
				foreach (VertexN[] pn in this.PrimitiveNeighbourList[i].Neighbours) //pn=primitive neighbour
				{	
					//construct a structure to hold vertex indices of this primitive
					int[] p;
					p = new int[pn.Length];
					for (j=0; j<=step-1; j++)
					{
						p[j]=pn[j].IndexValue;
					}

					//set this primitive's normal, from the list of the pre-determinded normals in pNorms array; uses GetPrimitiveIndex to retrieve the index of this primitive by giving the indices of the 3 vertices of a triangle
					nor[k]=pNorms[this.GetPrimitiveIndex(p)];
					k++;
				}

				//Alert - Normalizing
				if ((this.Normalizing != null) && (this.FrequentAlerts))
					this.Normalizing(new NormalizingInfo(NormalizingStage.SettingVertexNormal, i, GettingPrimitiveNormalsInfo.Empty));
				
				//determine the average of all of this vertex neighbours primitive normals (that will be the vertex normal)
				Vector3 avgnor;
				avgnor = Vector3.Empty;

				foreach (Vector3 v3 in nor)
				{
					avgnor.Add(v3);
				}

				avgnor.Scale((float)(scale/nor.Length));
			
			
				//set the actual normal (to the vertex)
				o.Nor=avgnor;
				verts.SetValue(o,i);
			}

	
			this._VB.Unlock();
		}

		//Handles events that are raised from when inside the GettingPrimitiveNormals method. Only active when inside Normalize method and at the calling of the GettingPrimitiveNormals method from there
		private void OnGettingPrimitiveNormals(GettingPrimitiveNormalsInfo info)
		{
			//Alert - Normalizing  (no need for checks, because this method is only used after checking that the event is used)
			this.Normalizing(new NormalizingInfo(NormalizingStage.GettingPrimitiveNormals, -1, info));
		}



		//Enum that describes a stage in the normalizing process
		public enum NormalizingStage
		{
            NotBegun = -1,
			Starting = 0,
			GettingPrimitiveNormals = 1,
			GettingPrimitiveNeighbourNormals = 2,
			SettingVertexNormal = 3
		}

		//Struct that holds information for the location in the normalizing process
		public struct NormalizingInfo
		{
            public NormalizingStage Stage;
			public int CurrentVertex;
			public GettingPrimitiveNormalsInfo GettingPrimitiveNormalsInfo;

			public static NormalizingInfo Empty
			{
				get
				{
					return new NormalizingInfo(NormalizingStage.NotBegun, -1, GettingPrimitiveNormalsInfo.Empty);
				}
			}

			public NormalizingInfo(NormalizingStage stage, int currentVertex, GettingPrimitiveNormalsInfo gettingPrimitiveNormalsInfo)
			{
				this.Stage = stage;
				this.CurrentVertex = currentVertex;
				this.GettingPrimitiveNormalsInfo = gettingPrimitiveNormalsInfo;
			}
		}

		//Alerts on updating and processing
		public delegate void NormalizingHandler(NormalizingInfo info);
		public event NormalizingHandler Normalizing;









	
	
		//Retrieves a list with normals for each primitive (only works for triangles)
		public Vector3[] GetPrimitiveNormals()
		{
			//Alert - GettingPrimitiveNormals
			if (this.GettingPrimitiveNormals != null)
				this.GettingPrimitiveNormals(new GettingPrimitiveNormalsInfo(GettingPrimitiveNormalsStage.Starting, -1));

			//retrieve the vertice in an array (of objects) since we don't know what kind of vertex type we have
			System.Array verts;
			verts = this._VB.Lock(0,0);
			IVertex o; //temp general vertex obj used to change propreties

			//open the geometry information for use
			short[] indices;
			indices = (short[])this._IB.Lock(0,0);

			//can only use TriangleList, if other is used then announce the user
			if (step!=3) throw new ExceptionGeometry("Cannot handle other primitive type than TriangleList or LineList for now. CANNOT GENERATE DIVISION! (change the primitve type to one of these)");

			int i,j;


			Vector3[] pns; //primitive normals - object to hold returned information
			pns = new Vector3[this.NumIndices/step]; //length = num of primitves

			Vector3[] p; //to hold positions of one's primitve's points
			p = new Vector3[step]; 


			//Alert - GettingPrimitiveNormals
			if (this.GettingPrimitiveNormals != null)
				this.GettingPrimitiveNormals(new GettingPrimitiveNormalsInfo(GettingPrimitiveNormalsStage.GettingNormal, -1));

			//for each primitive
			for (i=0; i<=this.NumIndices-1; i+=step)
			{
				//Alert - GettingPrimitiveNormals
				if ((this.GettingPrimitiveNormals != null) && (this.FrequentAlerts))
					this.GettingPrimitiveNormals(new GettingPrimitiveNormalsInfo(GettingPrimitiveNormalsStage.GettingNormal, i/step));

				//for each point of this triangle
				for (j=0; j<=step-1; j++)
				{
					o = (IVertex)verts.GetValue(indices[i+j]);
					p[j]=o.Pos;
				}

				//set this primitive's normal
				pns[i/step] = Misc.GetTriangleNormal(p[0],p[1],p[2]);
			}


			this._IB.Unlock();
			this._VB.Unlock();

			return pns;
		}



		//Enum that describes a stage in the process of getting the primitive normals
		public enum GettingPrimitiveNormalsStage
		{
			NotBegun = -1,
			Starting = 0,
            GettingNormal = 1
		}

		//Struct that holds information for the location in the process of getting the primitive normals
		public struct GettingPrimitiveNormalsInfo
		{
			public GettingPrimitiveNormalsStage Stage;
			public int CurrentPrimitive;

			public static GettingPrimitiveNormalsInfo Empty
			{
				get
				{
                    return new GettingPrimitiveNormalsInfo(GettingPrimitiveNormalsStage.NotBegun, -1);
				}
			}

			public GettingPrimitiveNormalsInfo(GettingPrimitiveNormalsStage stage, int currentPrimitive)
			{
				this.Stage = stage;
				this.CurrentPrimitive = currentPrimitive;
			}
		}

		//Alerts on updating and processing
		public delegate void GettingPrimitiveNormalsHandler(GettingPrimitiveNormalsInfo info);
		public event GettingPrimitiveNormalsHandler GettingPrimitiveNormals;
















		//Retrieves the index of a primitive from 2 or 3 vertices (lines or triangles) (the order considered is the order in which the primitives are described in the IB); -1 returned if no such primitive is found
		//returns the first match only
		public int GetPrimitiveIndex (int[] primIndices)
		{
			if (primIndices.Length!=step) throw new ExceptionGeometry("The number of indices for this primitive you must provide for this function must coincide with the primitive type you are using. Only works for LineList -> 2 and TriangleList -> 3. Please provide a compatible number of vertex indices");

			//open the geometry information for use
			short[] indices;
			indices = (short[])this._IB.Lock(0,0);

			int i,j;


			//search through primitive information (ib)
			for (i=0; i<=this.NumIndices-1; i+=step)
			{
				bool found = true;
            
				//see if this current primitive is a match
				for (j=0; j<=step-1; j++)
				{
					if (indices[i+j]!=primIndices[j]) 
					{
						found=false;
						break;
					}
				}

				if (found==true) return i/step;
			}
        

			this._IB.Unlock();

			return -1;
		}















		//Gets a division IB, generated by an array of vertices
		//if exclusive is on then only the geometry generated exclusively with the vertices is returned (only primitves that are made up entirely by vertices from vertices parameter)
		//vertices must hold short objects that hold vertices indices
		public IndexBuffer GetGeneratedDivisionIB(System.Collections.ArrayList vertices, bool exclusive)
		{
			//Alert - GettingGeneratedDivisionIB
			if (this.GettingGeneratedDivisionIB != null)
				this.GettingGeneratedDivisionIB(new GettingGeneratedDivisionIBInfo(GettingGeneratedDivisionIBStage.Starting, -1));

			//open the geometry information for use
			short[] indices;
			indices = (short[])this._IB.Lock(0,0);

			//can only use TriangleList or LineList for now, if other is used then announce the user
			if (step==0) throw new ExceptionGeometry("Cannot handle other primitive type than TriangleList or LineList for now. CANNOT GENERATE DIVISION! (change the primitve type to one of these)");

			//holds all the primitves found
			System.Collections.ArrayList primitives;
			primitives = new System.Collections.ArrayList();


			int i,k;

			//Alert - GettingGeneratedDivisionIB
			if (this.GettingGeneratedDivisionIB != null)
				this.GettingGeneratedDivisionIB(new GettingGeneratedDivisionIBInfo(GettingGeneratedDivisionIBStage.CheckingPrimitive, -1));

			//for each primitve
			for (i=0; i<=this.NumIndices-1; i+=step)
			{
				//Alert - GettingGeneratedDivisionIB
				if ((this.GettingGeneratedDivisionIB != null) && (this.FrequentAlerts))
					this.GettingGeneratedDivisionIB(new GettingGeneratedDivisionIBInfo(GettingGeneratedDivisionIBStage.CheckingPrimitive, i/step));

				short[] prim; //primitve to hold this current triangle/array in case of needing to be added later
				prim = new short[step];

				bool foundOne = false, foundAll = true;
				//for each vertex of this current primitve
				for (k=0; k<=step-1; k++)
				{
					prim[k]=indices[i+k];
					if (Misc.ArrayListContainsInt(vertices, indices[i+k]))
					{
						foundOne=true;
					}
					else
					{
						foundAll=false;
					}
				}

				//see if this primitive should be added or not
				bool add=false;
				if (exclusive==true)
				{
					if (foundAll==true) add=true;
				}
				else
				{
					if (foundOne==true) add=true;
				}

				//add this primitve (which was hold in prim) if case
				if (add==true)
				{
					primitives.Add(prim);
				}
			}

			this._IB.Unlock();

			//Alert - GettingGeneratedDivisionIB
			if (this.GettingGeneratedDivisionIB != null)
				this.GettingGeneratedDivisionIB(new GettingGeneratedDivisionIBInfo(GettingGeneratedDivisionIBStage.WriteNewIB, -1));
		
			//an IB that will hold our division - must transfer the primitves found to it
			IndexBuffer genIB; 
			genIB = new IndexBuffer(typeof(short), primitives.Count*step,this.Device,Usage.SoftwareProcessing,Pool.Default);
		
			indices = (short[])genIB.Lock(0,0);
        
			i=0;
			foreach (short[] prim in primitives)
			{
				for (k=0; k<=step-1; k++)
				{
					indices[i+k]=prim[k];
				}
           
				i+=step;
			}

			genIB.Unlock();

			return genIB;
		}



		//Enum that describes a stage in the process of getting the primitive normals
		public enum GettingGeneratedDivisionIBStage
		{
			NotBegun = -1,
			Starting = 0,
			CheckingPrimitive = 1,
			WriteNewIB = 2
		}

		//Struct that holds information for the location in the process of getting the primitive normals
		public struct GettingGeneratedDivisionIBInfo
		{
			public GettingGeneratedDivisionIBStage Stage;
			public int CurrentPrimitive;

			public static GettingGeneratedDivisionIBInfo Empty
			{
				get
				{
					return new GettingGeneratedDivisionIBInfo(GettingGeneratedDivisionIBStage.NotBegun, -1);
				}
			}

			public GettingGeneratedDivisionIBInfo(GettingGeneratedDivisionIBStage stage, int currentPrimitive)
			{
				this.Stage = stage;
				this.CurrentPrimitive = currentPrimitive;
			}
		}

		//Alerts on updating and processing
		public delegate void GettingGeneratedDivisionIBHandler(GettingGeneratedDivisionIBInfo info);
		public event GettingGeneratedDivisionIBHandler GettingGeneratedDivisionIB;











		//Renders this indexed geometry
		public override void Render() 
		{ 
			if (this.Device == null) throw new ExceptionGeometry("Device not set. Cannot render geometry");  

			//Vital settings for the device and drawing
			this.Device.VertexFormat=this.VertexFormat;
			this.Device.SetStreamSource(0, this._VB, 0); 
			this.Device.Indices = this._IB; 
	
			//can only use TriangleList or LineList for now, if other is used then announce the user
			if (step==0) throw new ExceptionGeometry("Cannot handle other primitive type than TriangleList or LineList for now. CANNOT RENDER! (change the primitve type to one of these)");

			this.Device.DrawIndexedPrimitives(this.PrimitivesType, 0, 0, this.NumVertices, 0, this.NumIndices/step);  
		} 








		//***
		//(VERY MUCH LATER!!!) =))
		//Methods to create simple basic 3D geometry
		//Maybe put those in a generic basic geometry space object
		//+load from a file (maybe in constructor, maybe static members)
		//***
	}
}
