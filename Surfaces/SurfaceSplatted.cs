//Class: SurfaceSplatted <- Surface
//Author: Ovidiu Dolha
//Description:   Using DirectX 9.0c
//               This is a surface that functions with IndexedGeometrySplatted class. It handles surface issues and can generate a splatted surface and render


using System;
using System.Drawing;
using Microsoft.DirectX; 
using Microsoft.DirectX.Direct3D; 

using Direct3DAid.Geometries;
using Direct3DAid.SpaceObjects;


namespace Direct3DAid.Surfaces
{
	public class SurfaceSplatted : Surface,
		IUsingD3DDevice, ISpaceObject
	{ 
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


		
		//Device to use
		private Device _Device; 
		public Device Device
		{ 
			get 
			{ 
				return this._Device; 
			} 
			set 
			{ 
				this._Device = value; 
			} 
		} 



		//Space Object of this surface (used to represent this surface in a real space system). OPTIONAL property!
		private SpaceObject _SpaceObject; 
		public SpaceObject SpaceObject 
		{ 
			get 
			{ 
				return this._SpaceObject; 
			} 
			set 
			{ 
				this._SpaceObject = value; 

				//set the geometry to the space object (for render)
				value.Geometry = this.Geometry;
			} 
		} 


	

		
		//Number of texture divisions on the surface
		private int _NumDivisions;
		public int NumDivisions
		{
			get
			{
				return this._NumDivisions;
			}
			set
			{
				this._NumDivisions=value;
				if (this.Geometry!=null) this.Geometry.NumDivisions=value;
			}
		}

		//Division textures (each division has a different texture) - it refers to the Texture collection from the geometry
		//accesses textures of geometry
		public Texture[] DivisionTexture
		{
			get
			{
				if (this.Geometry==null) throw new ExceptionSurface("Cannot access Texture array, because Geometry is not initiated");
				return this.Geometry.DivisionTexture;
			}
		}


		//Holds optional UVMap's for each division (WARNING: maps are applied to the whole geometry, not on local vertices from a certain division) - it refers to the UVMap collection from the geometry
		//accesses uvmaps of geometry
		public UVMap[] DivisionUVMap
		{
			get
			{
				if (this.Geometry==null) throw new ExceptionSurface("Cannot access UVMap array, because Geometry is not initiated");
				return this.Geometry.DivisionUVMap;
			}
		}






		//Geometry attached to this surface -> is used to hold physical information about it and it is GENERATED
		//Modifying this object's properties is highly unrecommended and can have unsuspected results!!!
		private GeometryIndexedSplatted _Geometry; 
		public GeometryIndexedSplatted Geometry
		{ 
			get 
			{ 
				return this._Geometry; 
			} 
		} 











		//Class Constructor 1
		public SurfaceSplatted(Size size, Technique surfaceTechnique, int numDivisions, Device device) :
			base(size, surfaceTechnique)
		{ 
			this._SpaceObject = null; //no space object initially attached

			this.Device = device; 
			this._NumDivisions = numDivisions;
			this._Geometry = new GeometryIndexedSplatted(this.Device,typeof(FVF_PosNorColTex),FVF_PosNorColTex.Format,PrimitiveType.TriangleList,this.NumDivisions); //an empty geometry
		} 

		//Class Constructor 2
		public SurfaceSplatted(Size size, ControlPoint initValue, Technique surfaceTechnique, int numDivisions, Device device) :
			base(size, initValue, surfaceTechnique)
		{ 
			this._SpaceObject = null; //no space object initially attached

			this.Device = device; 
			this._NumDivisions = numDivisions;
			this._Geometry = new GeometryIndexedSplatted(this.Device,typeof(FVF_PosNorColTex),FVF_PosNorColTex.Format,PrimitiveType.TriangleList,this.NumDivisions); //an empty geometry
		} 









		//(Re)Generates the VB and IB that hold out the surface by recreating the geometry property. The new surface appearence can be customizable in this class' properties, like surfaceTechnique property;
		public void UpdateGeometry(bool normalize, float normalizeScale) 
		{
			//Alert - UpdatingGeometry
			if (this.UpdatingGeometry != null)
				this.UpdatingGeometry(new UpdatingGeometryInfo(UpdatingGeometryStage.Starting, new Point(-1,-1),	GeometryIndexed.NormalizingInfo.Empty, GeometryIndexedSplatted.UpdatingDivisionedGeometryInfo.Empty));

			VertexBuffer vb;
			IndexBuffer ib;

			vb=new VertexBuffer(typeof(FVF_PosNorColTex),this.Size.Width*this.Size.Height,this.Device,Usage.SoftwareProcessing,FVF_PosNorColTex.Format,Pool.Default);
			ib=new IndexBuffer(typeof(short),(this.Size.Width-1)*(this.Size.Height-1)*2*3,this.Device,Usage.SoftwareProcessing,Pool.Default);

			uint[] divs; //distribution of divisions made out to index values from the geometry
			divs = new uint[this.Size.Width*this.Size.Height];


			//create geometry
			FVF_PosNorColTex[] verts; //retrieve the vertices in an array
			verts = (FVF_PosNorColTex[])vb.Lock(0,0);

			short[] indices; //open the geometry information for use
			indices = (short[])ib.Lock(0,0);


			int k,l; //keep track of the current vertex in vb, and the current index in ib
			k=0; l=0;

			//Alert - UpdatingGeometry
			if (this.UpdatingGeometry != null) this.UpdatingGeometry(new UpdatingGeometryInfo(UpdatingGeometryStage.ProcessingPoint, new Point(-1, -1),	GeometryIndexed.NormalizingInfo.Empty, GeometryIndexedSplatted.UpdatingDivisionedGeometryInfo.Empty));

			int i,j;
			for (j=0; j<=this.Size.Height-1; j++) 
			{ 
				for (i=0; i<=this.Size.Width-1; i++) 
				{ 
					//Alert - UpdatingGeometry
					if ((this.UpdatingGeometry != null) && (this.FrequentAlerts)) this.UpdatingGeometry(new UpdatingGeometryInfo(UpdatingGeometryStage.ProcessingPoint, new Point(i, j),	GeometryIndexed.NormalizingInfo.Empty, GeometryIndexedSplatted.UpdatingDivisionedGeometryInfo.Empty));

					//set vertex properties for point (i,j) -> [k]
					Vector3 p = this.GetPointRealCoordinatesXYHeight(i,j,true);
					verts[k].Pos = new Vector3(p.X,p.Z,p.Y);
					verts[k].Col = this.GetControlPoint(i,j).Color;

					divs[k] = this.GetControlPoint(i,j).Division;
					
					k++;
						
					//create triangles for tile (i,j)
					if ((j<this.Size.Height-1) && (i<this.Size.Width-1))
					{	
						short i00,i01,i10,i11; //hold indices of points from this current tile
						i00=(short)Misc.GetIndexFromCoordinates(this.Size.Width,new Point(i,j));
						i01=(short)Misc.GetIndexFromCoordinates(this.Size.Width,new Point(i,j+1));
						i10=(short)Misc.GetIndexFromCoordinates(this.Size.Width,new Point(i+1,j));
						i11=(short)Misc.GetIndexFromCoordinates(this.Size.Width,new Point(i+1,j+1));

						if (this.GetTileHatching(i,j)==HatchingMode.NESW)
						{
							indices[l+0]=i00; indices[l+1]=i01; indices[l+2]=i11;
							indices[l+3]=i11; indices[l+4]=i10; indices[l+5]=i00;
						}
						else
						{
							indices[l+0]=i01; indices[l+1]=i11; indices[l+2]=i10;
							indices[l+3]=i10; indices[l+4]=i00; indices[l+5]=i01;
						}

                        l+=6;
					}
				} 
			} 

			vb.Unlock();
			ib.Unlock();
			//

			//usually an empty geometry was created at constructor, all it needs now is to fill information for it (the VB and the IB)
			this.Geometry.DivisionDistribution=divs;
			this.Geometry.SetVBAndIB(vb,ib);

			//and update it
			if (this.UpdatingGeometry != null)
			{
				if (normalize)
				{
					//Adding event handler to a method described below, in order to catch the events raised by the Normalize method of this surface's geometry
					GeometryIndexed.NormalizingHandler hn = new Direct3DAid.Geometries.GeometryIndexed.NormalizingHandler(OnNormalizingGeometry);
					this._Geometry.Normalizing += hn;
					this._Geometry.Normalize(normalizeScale);
					this._Geometry.Normalizing -= hn;
				}
				//Adding event handler to a method described below, in order to catch the events raised by the UpdateDivisionedGeometry method of this surface's geometry
				GeometryIndexedSplatted.UpdatingDivisionedGeometryHandler hu = new Direct3DAid.Geometries.GeometryIndexedSplatted.UpdatingDivisionedGeometryHandler(OnUpdatingDivisionedGeometry);
				this._Geometry.UpdatingDivisionedGeometry += hu;
				this._Geometry.UpdateDivisionedGeometry();
				this._Geometry.UpdatingDivisionedGeometry -= hu;
			}
			else
			{
				//just call the methods
				if (normalize)
				{
					this._Geometry.Normalize(normalizeScale);
				}
				this._Geometry.UpdateDivisionedGeometry();
			}


			//set the geometry to the space object (if case) (for render)
			if (this.SpaceObject!=null) this.SpaceObject.Geometry = this.Geometry;
		}

		//Handles events that are raised from when inside the above method. Only active at a certain location
		private void OnNormalizingGeometry(GeometryIndexed.NormalizingInfo info)
		{
			//Alert - UpdatingGeometry (no need for checks because this method is only used after checking that the event is used)
			this.UpdatingGeometry(new UpdatingGeometryInfo(UpdatingGeometryStage.NormalizingGeometry, new Point(-1,-1),	info, GeometryIndexedSplatted.UpdatingDivisionedGeometryInfo.Empty));
		}

		//Handles events that are raised from when inside the above method. Only active at a certain location
		private void OnUpdatingDivisionedGeometry(GeometryIndexedSplatted.UpdatingDivisionedGeometryInfo info)
		{
			//Alert - UpdatingGeometry (no need for checks because this method is only used after checking that the event is used)
			this.UpdatingGeometry(new UpdatingGeometryInfo(UpdatingGeometryStage.UpdatingDivisionedGeometry, new Point(-1,-1),	GeometryIndexed.NormalizingInfo.Empty, info));
		}


		
		//Enum that describes a stage in the normalizing process
		public enum UpdatingGeometryStage
		{
			NotBegun = -1,
			Starting = 0,
			ProcessingPoint = 1,
			NormalizingGeometry = 2,
			UpdatingDivisionedGeometry = 3
		}

		//Struct that holds information for the location in the normalizing process
		public struct UpdatingGeometryInfo
		{
			public UpdatingGeometryStage Stage;
			public Point CurrentPoint;
			public GeometryIndexed.NormalizingInfo NormalizingInfo;
			public GeometryIndexedSplatted.UpdatingDivisionedGeometryInfo UpdatingDivisionedGeometryInfo;

			public static UpdatingGeometryInfo Empty
			{
				get
				{
					return new UpdatingGeometryInfo(UpdatingGeometryStage.NotBegun, new Point(-1,-1), GeometryIndexed.NormalizingInfo.Empty, GeometryIndexedSplatted.UpdatingDivisionedGeometryInfo.Empty);
				}
			}

			public UpdatingGeometryInfo(UpdatingGeometryStage stage, Point currentPoint, GeometryIndexed.NormalizingInfo normalizingInfo, GeometryIndexedSplatted.UpdatingDivisionedGeometryInfo updatingDivisionedGeometryInfo)
			{
				this.Stage = stage;
				this.CurrentPoint = currentPoint;
				this.NormalizingInfo = normalizingInfo;
				this.UpdatingDivisionedGeometryInfo = updatingDivisionedGeometryInfo;
			}
		}

		//Alerts on updating and processing
		public delegate void UpdatingGeometryHandler(UpdatingGeometryInfo info);
		public event UpdatingGeometryHandler UpdatingGeometry;

















		
		//Renders this surface
		public void Render() 
		{ 
			if (this.Device != this.Geometry.Device) throw new ExceptionSurface("The device on this surface and the one from it's generated geometry are different!");

			if (this.SpaceObject == null)
			{
				this.Geometry.Render();
			}
			else
			{
				if (this.Device != this.SpaceObject.Device) throw new ExceptionSurface("The device on this surface and the one from it's space object are different!");
				this.SpaceObject.Render();
			}
		} 
	}
}