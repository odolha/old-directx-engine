//Class: GeometryIndexedSplatted <- GeometryIndexed
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//               Like an indexed geometry, only uses a method called texture splatting. Uses only vertex type with texture and can contain multiple divisions of the geometry, for each another texture. Uses alpha blenging to blend textures.
//				 Divisions have priority: from 0 to numDivisions-1, 0 being the first division to render
//				 Only handles triangles (solid geometry)


using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Direct3DAid.Geometries
{
	public class GeometryIndexedSplatted : GeometryIndexed
	{
		//Number of divisions
		//Modifing this resets the division information
		private int _NumDivisions;
		public int NumDivisions
		{
			get
			{
				return this._NumDivisions;
			}
			set
			{
				this._NumDivisions = value;
				this._DivisionTexture = new Texture[value];
				this._DivisionUVMap = new UVMap[value];
				this._NumDivisionIndices = new int[value];
				this._NumDivisionVertices = new int[value];
				this._DivisionIB = new IndexBuffer[value];
				this._DivisionVB = new VertexBuffer[value]; //although it might not use, create the array with empty pointers
				this._DivisionAlphaSets = new VerticesModifications.ColorModification[value][]; //although it might not use, create the array with empty pointers
			}
		}




		//Division distribution; each vertex is part of one division
		//Divisions have priority: from 0 to numDivision-1, 0 being the first division to render
		//WARNING: if this is not respected, then there are unexpected results;
		private uint[] _DivisionDistribution;
		public uint[] DivisionDistribution
		{
			set
			{
				this._DivisionDistribution = value;
			}
		}



		//Division textures (each division has a different texture)
		private Texture[] _DivisionTexture;
		public Texture[] DivisionTexture
		{
			get
			{
				return this._DivisionTexture;
			}
		}


		//Holds optional UVMap's for each division (WARNING: maps are applied to the whole geometry, not on local vertices from a certain division)
		private UVMap[] _DivisionUVMap;
		public UVMap[] DivisionUVMap
		{
			get
			{
				return this._DivisionUVMap;
			}
		}





		//Number of vertices per each division
		private int[] _NumDivisionVertices;
		private int[] NumDivisionVertices 
		{ 
			get 
			{ 
				return this._NumDivisionVertices;
			} 
		}
		public int GetNumDivisionVertices(int index)
		{
            return this.NumDivisionVertices[index];
		}


		//Number of indices per each division
		private int[] _NumDivisionIndices;
		private int[] NumDivisionIndices 
		{ 
			get 
			{ 
				return this._NumDivisionIndices;
			} 
		} 
		public int GetNumDivisionIndices(int index)
		{
			return this.NumDivisionIndices[index];
		}



		//Local index buffer used for each division
		private IndexBuffer[] _DivisionIB;
		private IndexBuffer[] DivisionIB
		{
			get
			{
				return this._DivisionIB;
			}
			set
			{
				this._DivisionIB = value;
			}
		}



		//Local vertex buffer used for each division
		//NOTE: this is only used if specified in the geometry construction
		private VertexBuffer[] _DivisionVB;
		private VertexBuffer[] DivisionVB
		{
			get
			{
				return this._DivisionVB;
			}
			set
			{
				this._DivisionVB = value;
			}
		}





		//Local modifications to be made (diffuse alpha is 0 for some vertices) for each division
		//NOTE: only used if DivisionVB is specified not to use (in the geometry construction) and it helps calculating new vertices (with alpha modifications for each division) at render time
		private VerticesModifications.ColorModification[][] _DivisionAlphaSets;
		private VerticesModifications.ColorModification[][] DivisionAlphaSets
		{
			get
			{
				return this._DivisionAlphaSets;
			}
			set
			{
				this._DivisionAlphaSets = value;
			}
		}





		
		//Keeps track of this argument from UpdateSplattedGeometry method
		//if changed, you must usually update the geometry manually
		private bool _UseDivisionVB; 
		public bool UseDivisionVB
		{
			get
			{
				return this._UseDivisionVB;
			}
			set
			{
				this._UseDivisionVB = value;
			}
		}





	



		
	

	
		//Class Constructor 1
		public GeometryIndexedSplatted(Device device, VertexBuffer vB, IndexBuffer iB, Type vertexType, VertexFormats vertexFormat, PrimitiveType primitivesType, int numDivision, uint[] divisionDistribution):
			base(device, vB, iB, vertexType, vertexFormat, primitivesType)
		{
			this.UseDivisionVB = true; //implicitly

			this.NumDivisions = numDivision;
			this.DivisionDistribution = divisionDistribution;

			this.VBModified += new VBModifiedHandler(this.OnVBModified);
			this.IBModified += new IBModifiedHandler(this.OnIBModified);
		}


		//Class Constructor 2
		public GeometryIndexedSplatted(Device device, int numVertices, int numIndices, Type vertexType, VertexFormats vertexFormat, PrimitiveType primitivesType, int numDivision, uint[] divisionDistribution) :
			base(device, numVertices, numIndices, vertexType, vertexFormat, primitivesType)
		{ 
			this.UseDivisionVB = true; //implicitly

			this.NumDivisions = numDivision;
			this.DivisionDistribution = divisionDistribution;

			this.VBModified += new VBModifiedHandler(this.OnVBModified);
			this.IBModified += new IBModifiedHandler(this.OnIBModified);
		}

		//Class Constructor 3
		public GeometryIndexedSplatted(Device device, Type vertexType, VertexFormats vertexFormat, PrimitiveType primitivesType, int numDivision, uint[] divisionDistribution):
			base(device, vertexType, vertexFormat, primitivesType)
		{
			this.UseDivisionVB = true; //implicitly

			this.NumDivisions = numDivision;
			this.DivisionDistribution = divisionDistribution;

			this.VBModified += new VBModifiedHandler(this.OnVBModified);
			this.IBModified += new IBModifiedHandler(this.OnIBModified);
		}

		//Class Constructor 4
		public GeometryIndexedSplatted(Device device, Type vertexType, VertexFormats vertexFormat, PrimitiveType primitivesType, int numDivision):
			base(device, vertexType, vertexFormat, primitivesType)
		{
			this.UseDivisionVB = true; //implicitly

			this.NumDivisions = numDivision;
			this.DivisionDistribution = null;

			this.VBModified += new VBModifiedHandler(this.OnVBModified);
			this.IBModified += new IBModifiedHandler(this.OnIBModified);
		}








		//Updates IB for each division and the VerticesModifications that are used for splatting with alpha on the vertices from the edge of each division, and if chosed, also updates the Division VB's
		//This update is not auto, the user must call it whenever it needs the Splatted Geometry reconstructed
		//Note: for slight changes of vertices, is better to use the GemeteryIndexed's ResetVertices method. For indices modifications is usually needed an update of division
		public void UpdateDivisionedGeometry()
		{
			//Alert - UpdatingDivisionedGeometry
			if (this.UpdatingDivisionedGeometry != null)
				this.UpdatingDivisionedGeometry(new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.Starting, -1, -1, GettingGeneratedDivisionIBInfo.Empty));

			//can only use TriangleList for now, if other is used then announce the user
			if (step!=3) throw new ExceptionGeometry("Cannot handle other primitive type than TriangleList for now. Cannot update divisioned geometry");

			System.Array vertsAll; //all vertices from the main VB
			vertsAll = this._VB.Lock(0,0);

			int i,j;



			//update buffers and alpha sets - vertex modifications
			//updates for each division
			for (i=0; i<=this.NumDivisions-1; i++)
			{
				//get all vertices that belong to this division (with neighbours)
				//at the end we have all vertices that generate the division with neighbours (the division IB mask)
				System.Collections.ArrayList vertices;
				vertices = new System.Collections.ArrayList();

				//Alert - UpdatingDivisionedGeometry
				if (this.UpdatingDivisionedGeometry != null)
					this.UpdatingDivisionedGeometry(new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.GettingDivisionVertices, i, -1, GettingGeneratedDivisionIBInfo.Empty));

				for (j=0; j<=this.NumVertices-1; j++)
				{
					//Alert - UpdatingDivisionedGeometry
					if ((this.UpdatingDivisionedGeometry != null) && (this.FrequentAlerts))
						this.UpdatingDivisionedGeometry(new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.GettingDivisionVertices, i, j, GettingGeneratedDivisionIBInfo.Empty));

					if (this._DivisionDistribution[j]==i) vertices.Add(j);
					else
					{
						//also check for vertices who have at least one neighbour in the division (margin vertices)
						foreach (VertexN vn in this.GetVertexNeighbour(j).Neighbours)
						{
							if (this._DivisionDistribution[vn.IndexValue]==i) 
							{
								vertices.Add(j);
								break;
							}
						}
                    }
				}

				//calculate initial IB
				//generate the IB with those vertices from this division, only if at least one vertex was found
				if (vertices.Count==0)
				{
					this.DivisionIB[i] = null; //if it has no vertices is usless so let it be null so other functional units might not try to use this geometry
					this.NumDivisionIndices[i]=0;
				}
				else
				{
					if ((this.UpdatingDivisionedGeometry != null) && (this.FrequentAlerts))
					{
						//Alerts from a metohod that handles the events that are raised from the GetGeneratedDivisionIB method
						GettingGeneratedDivisionIBHandler h = new GettingGeneratedDivisionIBHandler(OnGettingGeneratedDivisionIB);
						this.paramInt = i; //make sure the passing parameter is set (the method that handles the event will require to know the value of i. it will take it from a general field)
						this.GettingGeneratedDivisionIB += h;
						this.DivisionIB[i] = this.GetGeneratedDivisionIB(vertices,true);
						this.GettingGeneratedDivisionIB -= h;
					}
					else
					{
						//just call the method
						this.DivisionIB[i] = this.GetGeneratedDivisionIB(vertices,true);
					}
					
					//update the counts for division IBs
					this.UpdateNoOfDivisionIndices(i);
				}

				//If division VB is chosen not to use then the geometry must use the same VB by modifying it's vertices's alpha color for each division in rendertime! So it must calculate which vertices what alpha to have for each division by using color modifications
				if (!this.UseDivisionVB)
				{
					if (this.UpdatingDivisionedGeometry != null)
						this.UpdatingDivisionedGeometry(new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.SettingVertexAlphaValue, i, -1, GettingGeneratedDivisionIBInfo.Empty));

					//set the alpha values for each vertex in geometry
					this.DivisionAlphaSets[i]=new VerticesModifications.ColorModification[this.NumVertices];
					for (j=0; j<=this.NumVertices-1; j++)
					{ 
						//Alert - UpdatingDivisionedGeometry
						if ((this.UpdatingDivisionedGeometry != null) && (this.FrequentAlerts))
							this.UpdatingDivisionedGeometry(new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.SettingVertexAlphaValue, i, j, GettingGeneratedDivisionIBInfo.Empty));

						//set the inital value of alpha to 255 (max)
						this.DivisionAlphaSets[i][j] = new VerticesModifications.ColorModification(j,Color.FromArgb(255,0,0,0).ToArgb(),true);

						//see if it's the rare case when alpha should be 0 (when the vertex is on the edge of this division and has a higher priority - distribution number is lower)
						if (this._DivisionDistribution[j]<i)
						{
							//see if it's on the edge of this division (has a neighbour of this division)
							bool hasNeighbourInDiv = false;
							System.Collections.ArrayList vn;
							vn = this.GetVertexNeighbour(j).Neighbours;

							foreach (VertexN v in vn)
							{
								if (this._DivisionDistribution[v.IndexValue] == i) hasNeighbourInDiv = true;
							}

							//if the vertex is on the edge, need to change alpha
							if (hasNeighbourInDiv==true) this.DivisionAlphaSets[i][j].NewColor = Color.FromArgb(0,0,0,0).ToArgb();
						}
					}
				}
				//If division VB is chosen to use then it must calculate the division VBs and rewrite the division IBs so they point to vertices from the division VBs recently calculated
				else
				{
					if (vertices.Count==0)
					{
						//division is empty - if no vertices are used in this division, then there is no point on making it
						this.DivisionVB[i] = null;
					}
					else
					{
						//division is not empty, so it needs filling of the division VB with vertices and the IB's values to modify

						//calculate VB
						this.DivisionVB[i] = new VertexBuffer(this.VertexType,vertices.Count,this.Device,this._VB.Description.Usage,this._VB.Description.VertexFormat,this._VB.Description.Pool);
                
						System.Array vertsDiv; //vertices from division's buffer
						vertsDiv = this.DivisionVB[i].Lock(0,0);
					
						IVertex od,o; //temp general vertex obj used
						System.Collections.ArrayList coresp = new System.Collections.ArrayList(); //will be used to store corespondence between the old indices(that pointed to the general VB) and the new indices that point to the division's VB; it will be used when rewriting the IB's

						//Alert - UpdatingDivisionedGeometry
						if (this.UpdatingDivisionedGeometry != null) this.UpdatingDivisionedGeometry(new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.ProcessingNewVB, i, -1, GettingGeneratedDivisionIBInfo.Empty));

						//copy vertices to this partial VB, from the complete one
						for (j=0; j<=vertices.Count-1; j++)
						{
							//Alert - UpdatingDivisionedGeometry
							if ((this.UpdatingDivisionedGeometry != null) && (this.FrequentAlerts))
								this.UpdatingDivisionedGeometry(new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.ProcessingNewVB, i, j, GettingGeneratedDivisionIBInfo.Empty));

							coresp.Add(vertices[j]);

							od = (IVertex)vertsDiv.GetValue(j); //get the vertex object from this current division VB
							o = (IVertex)vertsAll.GetValue((int)vertices[j]); //get the corresponding vertex object from the general VB

							od = o; //set the vertex from the division to be the same as the one coresponding from the general VB (copies all properties)

							//set alpha color to 0 if this vertex is an edge vertex (the ones that are margin vertices are the ones that don't have the division set to this division)
							if (this._DivisionDistribution[(int)vertices[j]]<i)
							{
								od.Col = Color.FromArgb(0,Color.FromArgb(od.Col)).ToArgb();
							}

							//apply coresponding texture fit (if requested), directly to each vertex in this partial VB
							if (this.DivisionUVMap[i]!=null)
							{
								od.Tex = this.DivisionUVMap[i].Fit(od.Pos);
							}

							//apply changes
							vertsDiv.SetValue(od,j);
						}
						this.DivisionVB[i].Unlock();



						//rewrite IB

						//Alert - UpdatingDivisionedGeometry
						if (this.UpdatingDivisionedGeometry != null) this.UpdatingDivisionedGeometry(new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.RearrangingIB, i, -1, GettingGeneratedDivisionIBInfo.Empty));
						
						//open the current IB from division  modify values
						short[] inds;
						inds = (short[])this.DivisionIB[i].Lock(0,0);

						for (j=0; j<=inds.Length-1; j++)
						{
							inds[j] = (short)coresp.IndexOf((Int32)(inds[j])); //Int32 used for manual boxing
						}
						this.DivisionIB[i].Unlock();

						//update the counts for division VBs
						this.NumDivisionVertices[i]=vertices.Count;
					}
				}
			} //NEXT i (division)
			
			//unlock the main VB
			this._VB.Unlock();
		}

		//Handles events that are raised from when inside the GettingGeneratedDivisionIB method. Only active when inside UpdateDivisionedGeometry method and at the calling of the GettingGeneratedDivisionIB  method from there
		private void OnGettingGeneratedDivisionIB(GettingGeneratedDivisionIBInfo info)
		{
			//Alert - UpdatingDivisionedGeometry (no need for checks because this method is only used after checking that the event is used)
			this.UpdatingDivisionedGeometry(new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.GettingGeneratedDivisionIB, this.paramInt, -1, info)); //this.paramInt is the number of division that is updated
		}



		//Enum that describes a stage in the normalizing process
		public enum UpdatingDivisionedGeometryStage
		{
			NotBegun = -1,
			Starting = 0,
			GettingDivisionVertices = 1,
			GettingGeneratedDivisionIB = 2,
			SettingVertexAlphaValue = 3,
			ProcessingNewVB = 4,
            RearrangingIB = 5
		}

		//Struct that holds information for the location in the normalizing process
		public struct UpdatingDivisionedGeometryInfo
		{
			public UpdatingDivisionedGeometryStage Stage;
			public int CurrentDivision;
			public int CurrentVertex;
			public GettingGeneratedDivisionIBInfo GettingGeneratedDivisionIBInfo;

			public static UpdatingDivisionedGeometryInfo Empty
			{
				get
				{
					return new UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage.NotBegun, -1, -1, GettingGeneratedDivisionIBInfo.Empty);
				}
			}

			public UpdatingDivisionedGeometryInfo(UpdatingDivisionedGeometryStage stage, int currentDivision, int currentVertex, GettingGeneratedDivisionIBInfo gettingGeneratedDivisionIBInfo)
			{
				this.Stage = stage;
				this.CurrentDivision = currentDivision;
				this.CurrentVertex = currentVertex;
				this.GettingGeneratedDivisionIBInfo = gettingGeneratedDivisionIBInfo;
			}
		}

		//Alerts on updating and processing
		public delegate void UpdatingDivisionedGeometryHandler(UpdatingDivisionedGeometryInfo info);
		public event UpdatingDivisionedGeometryHandler UpdatingDivisionedGeometry;














		//Updates the number of indices for each division
		public void UpdateNoOfDivisionIndices(int division)
		{
			if (this.DivisionIB[division]!=null) this._NumDivisionIndices[division] = Misc.GetNoOfIndices(this.DivisionIB[division]);
		}

		//Updates the number of vertices for each division
		public void UpdateNoOfDivisionVertices(int division)
		{
			if (this.DivisionVB[division]!=null) this._NumDivisionVertices[division] = Misc.GetNoOfVertices(this.DivisionVB[division]);
		}








	
		//Renders this splatted indexed geometry
		//(Splatted Rendering)
		public override void Render()
		{
			if (this.Device == null) throw new ExceptionGeometry("Device not set. Cannot render geometry");  

			//first set the division non-related stuff
			this.Device.VertexFormat = this.VertexFormat;
			this.Device.SetStreamSource(0, this._VB, 0);

			//draw each division
			for (int i=0; i<=this.NumDivisions-1; i++)
			{
				//render division (only if there is anything to draw - i.e. the IB of this division is not empty = this works in both cases, if UseDivisionVB is on or off)
				if (this.DivisionIB[i] != null)
				{
					//there are 2 ways on render: one, with no division VB's (kind of slow) and another with a VB for each division (faster)
					if (this.UseDivisionVB)
					{
						this.Device.SetStreamSource(0, this.DivisionVB[i], 0);
					}
					else
					{
						//set alpha values across the VB (on vertices that are on the edge of this division)
						if (this.NumDivisions>1) //optimize for single divisions - not splatted
							this.ResetVertices(null,this.DivisionAlphaSets[i],null,null);

						//aplies uvmaps for this division (if case)
						if (this._DivisionUVMap[i]!=null) this.ResetVertices(this.DivisionUVMap[i]);
					}

					//various settings for texture and alpha
					if (this.DivisionTexture[i] != null) 
					{
						//set texture and blending with diffuse and alpha
						this.Device.SetTexture(0, this.DivisionTexture[i]); 
						this.Device.TextureState[0].ColorOperation = TextureOperation.Modulate; 
						this.Device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor; 
						this.Device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse; 
					
						if (this.NumDivisions>1) //optimize for single divisions - not splatted
							this.Device.TextureState[0].AlphaArgument1 = TextureArgument.Diffuse;
					}
					else
					{
						this.Device.SetTexture(0,null); 
					}


					//can only use TriangleList for now, if other is used then announce the user
					if (step!=3) throw new ExceptionGeometry("Cannot handle other primitive type than TriangleList for now; CANNOT RENDER!");

					this.Device.Indices = this.DivisionIB[i]; //set the IB of this division

					//draw division
					if (this.UseDivisionVB)
					{
						//uses only the number of vertices from this division
						this.Device.DrawIndexedPrimitives(this.PrimitivesType, 0, 0, this.NumDivisionVertices[i], 0, this.NumDivisionIndices[i]/step);  
					}
					else
					{
						//usess all the vertices
						this.Device.DrawIndexedPrimitives(this.PrimitivesType, 0, 0, this.NumVertices, 0, this.NumDivisionIndices[i]/step);  
					}
				}
			} //NEXT i (division)
		}









		//EVENTS ATTACHED METHODS
		
		//when the base VB is modified
		private void OnVBModified()
		{
			//this.UpdateDivisionedGeometry();
		}

		//when the base IB is modified
		private void OnIBModified()
		{
			//this.UpdateDivisionedGeometry();
		}



		

		
		//Fields used privately for holding small amount of info when some variables run out of scope
		private int paramInt;
	}
}
