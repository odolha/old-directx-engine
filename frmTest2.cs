using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Direct3DAid;
using Direct3DAid.SpaceObjects;
using Direct3DAid.Geometries;
using Direct3DAid.Geometries.VerticesModifications;
using Direct3DAid.Surfaces;
using Direct3DAid.Mobility;
using Direct3DAid.Mobility.Controllers;
using Direct3DAid.Mobility.Navigators;


namespace Direct3DAid
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmTest2 : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;

		public frmTest2(int surfWidth, int surfHeight, string surfPath, Color skyColor, bool fullScreen, float myHeight)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.SurfPath = surfPath;

			this.w = surfWidth;
			this.h = surfHeight;

			this.SkyCol = skyColor;

			this.FullScreen = fullScreen;

			this.MyHeight = myHeight;
		}



		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.button1 = new System.Windows.Forms.Button();
			this.FPS = new System.Windows.Forms.Label();
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.info = new System.Windows.Forms.Label();
			this.newCheckT = new System.Windows.Forms.Timer(this.components);
			this.startT = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 10;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(528, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(56, 48);
			this.button1.TabIndex = 0;
			this.button1.Text = "START FREE SPIRIT";
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// FPS
			// 
			this.FPS.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FPS.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(0)), ((System.Byte)(192)));
			this.FPS.Location = new System.Drawing.Point(8, 8);
			this.FPS.Name = "FPS";
			this.FPS.TabIndex = 1;
			this.FPS.Text = "---";
			this.FPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// timer2
			// 
			this.timer2.Enabled = true;
			this.timer2.Interval = 1000;
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// textBox1
			// 
			this.textBox1.Enabled = false;
			this.textBox1.Location = new System.Drawing.Point(536, 64);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(40, 20);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "MOVE";
			this.textBox1.Visible = false;
			// 
			// info
			// 
			this.info.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.info.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(192)));
			this.info.Location = new System.Drawing.Point(120, 8);
			this.info.Name = "info";
			this.info.Size = new System.Drawing.Size(120, 23);
			this.info.TabIndex = 1;
			this.info.Text = "---";
			this.info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// newCheckT
			// 
			this.newCheckT.Interval = 400;
			this.newCheckT.Tick += new System.EventHandler(this.newCheckT_Tick);
			// 
			// startT
			// 
			this.startT.Interval = 50;
			this.startT.Tick += new System.EventHandler(this.startT_Tick);
			// 
			// frmTest2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.FPS);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.info);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmTest2";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Test Form";
			this.Load += new System.EventHandler(this.frmTest_Load);
			this.ResumeLayout(false);

		}
		#endregion


		private bool CloseNow = false;


		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label FPS;
		private System.Windows.Forms.Timer timer2;


		private string SurfPath;
		private int w;
		private int h;
		private Color SkyCol;
		private bool FullScreen;
		private float MyHeight;

		private float wSMax;
		private float hSMax;


		public Direct3DAid.Surfaces.SurfaceSplatted s;
		public Device dev;
		public Texture tex;

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label info; // Materials for our mesh


		//other fields
		private Camera cam;
		private InputController ic;
		private OnSurfaceNavigator10 n;
		private System.Windows.Forms.Timer newCheckT;
		private System.Windows.Forms.Timer startT;
		private Matrix viewM;


		public delegate void UpdatingSurfaceGeometryHandler(SurfaceSplatted.UpdatingGeometryInfo info);
		public event UpdatingSurfaceGeometryHandler UpdatingSurfaceGeometry;


		private void frmTest_Load(object sender, System.EventArgs e)
		{
			int i,j;
		
			
			PresentParameters pp;
			pp = new PresentParameters();
			pp.Windowed					= true;
			pp.EnableAutoDepthStencil	= true;
			pp.AutoDepthStencilFormat	= DepthFormat.D16;
			pp.SwapEffect				= SwapEffect.Discard;
			pp.MultiSample				= MultiSampleType.TwoSamples;

			if (this.FullScreen)
			{
				pp.Windowed						= false;
				pp.BackBufferWidth				= 800;
				pp.BackBufferHeight				= 600;
				pp.FullScreenRefreshRateInHz	= 85;
				pp.BackBufferFormat				= Format.X8R8G8B8;
				pp.PresentationInterval			= PresentInterval.Immediate; 
			}

			dev = new Device(0, DeviceType.Hardware,this ,CreateFlags.SoftwareVertexProcessing,pp);
			dev.RenderState.CullMode = Cull.None;
			dev.RenderState.ZBufferEnable = true;
			dev.RenderState.Lighting = true;

			dev.RenderState.MultiSampleAntiAlias = true;

			//fog
			dev.RenderState.FogEnable = true;
			dev.RenderState.FogColor = Color.FromArgb(this.SkyCol.ToArgb());
			dev.RenderState.FogVertexMode = FogMode.Linear;
			dev.RenderState.FogStart = 14;
			dev.RenderState.FogEnd = 25;

			dev.SamplerState[0].MinFilter = TextureFilter.None;
			dev.SamplerState[0].MagFilter = TextureFilter.Linear;

			//enable alpha blending
			dev.RenderState.AlphaBlendEnable = true;
			dev.RenderState.SourceBlend = Blend.SourceAlpha;
			dev.RenderState.DestinationBlend = Blend.InvSourceAlpha;
			dev.RenderState.BlendOperation = BlendOperation.Add;



			Direct3DAid.Surfaces.Technique t;
			//t = Technique.DefaultRectangledSimple(new SizeF(1.0F,1.0F),HatchingMode.NWSE);
			t = Technique.DefaultRectangledAlternated(new SizeF(1.0F,1.0F),HatchingMode.NWSE );
			//t = Technique.DefaultTriangledEquilateral(1.0F,true);

			s = new Direct3DAid.Surfaces.SurfaceSplatted(new Size(w,h),new ControlPoint(0.0F,Color.LightGray.ToArgb(),0),t,5,dev);

			this.wSMax = (this.w - 1) * s.SurfaceTechnique.TileSize.Width;
			this.hSMax = (this.h - 1) * s.SurfaceTechnique.TileSize.Height;

			s.UpdatingGeometry += new Direct3DAid.Surfaces.SurfaceSplatted.UpdatingGeometryHandler(OnUpdatingGeometry);


			s.SpaceObject=new SpaceObject(s.Device); //make it's space object


			//load from file
			s.SetHeightsFromHeightMap(this.SurfPath,0.045F,-0.4F);

			//sets from heights (divisions according to height)
			float[] levs = new float[] {0.0F, 1.3F, 1.3F, 2.0F};
			s.SetDivisionsAccordingToHeight(levs);


			//make random grass
			Random r = new Random();
			for (i=0; i<=s.Size.Width-1; i++)
			{
				for (j=0; j<=s.Size.Height-1; j++)
				{
					if (s.GetControlPoint(i,j).Division == 1)
					{
						if (r.NextDouble() > 0.5) s.SetControlPointDivision(i,j,2);
					}
				}
			}

			//make random colored surf
			for (i=0; i<=s.Size.Width-1; i++)
			{
				for (j=0; j<=s.Size.Height-1; j++)
				{
					if (s.GetControlPoint(i,j).Division == 0)
					{
						s.SetControlPointColor(i, j, Misc.GetRandomColor(Color.YellowGreen, 10, r).ToArgb());
					}
					if ((s.GetControlPoint(i,j).Division == 1) || (s.GetControlPoint(i,j).Division == 2))
					{
						s.SetControlPointColor(i, j, Misc.GetRandomColor(Color.LightGreen, 20, r).ToArgb());
					}
					if (s.GetControlPoint(i,j).Division == 3)
					{
						s.SetControlPointColor(i, j, Misc.GetRandomColor(Color.MediumAquamarine, 15, r).ToArgb());
					}
					if (s.GetControlPoint(i,j).Division == 4)
					{
						s.SetControlPointColor(i, j, Misc.GetRandomColor(Color.LightGray, 5, r).ToArgb());
					}
				}
			}


			UVMap uvm,uvm1;
			uvm = new UVMapPlaneXZ(new PointF(0.0F,0.0F),new SizeF(1.0F,1.0F),new SizeF(0.5F,0.5F));
			uvm1 = new UVMapPlaneXZ(new PointF(0.0F,0.0F),new SizeF(1.0F,1.0F),new SizeF(1.2F,1.2F));

			s.DivisionUVMap[0]=uvm;
			s.DivisionUVMap[1]=uvm1;
			s.DivisionUVMap[2]=uvm1;
			s.DivisionUVMap[3]=uvm;
			s.DivisionUVMap[4]=uvm;
            

			s.UpdateGeometry(true,1.2F);


			s.DivisionTexture[0] = TextureLoader.FromFile(dev,"tex\\b0.jpg");
			s.DivisionTexture[1] = TextureLoader.FromFile(dev,"tex\\b1.jpg");
			s.DivisionTexture[2] = TextureLoader.FromFile(dev,"tex\\b1Alt.jpg");
			s.DivisionTexture[3] = TextureLoader.FromFile(dev,"tex\\b2.jpg");
			s.DivisionTexture[4] = TextureLoader.FromFile(dev,"tex\\b3.jpg");
		
 
			//this.viewM = Matrix.Multiply(Matrix.Multiply(Matrix.Multiply(Matrix.Translation(-2.5F,-2.5F,0),Matrix.RotationZ(k)),Matrix.Translation(2.5F,2.5F,0)),Matrix.LookAtLH(new Vector3(0,-5,-4.5f),new Vector3(2.5F,2.0F,0),new Vector3(0,0,-1)));
			this.viewM = Matrix.LookAtLH(new Vector3(1,2,1),new Vector3(1,0,1),new Vector3(0,0,1));
			this.timer1.Enabled=true;


			//CAMERA:

			this.cam = new Camera(this.dev,(float)(Math.PI/3), 1.3333F,0.01F,100F);
			this.cam.SelfMatrix = viewM;


			this.startT.Start();
		}


	


		private void timer1_Tick(object sender, System.EventArgs e)
		{
			if (ic != null)	this.ic.Process();
			if (n != null) this.n.Process();
			if (cam !=null)
			{
				this.cam.UpdateView();
				this.cam.UpdateProjection();
			}

			f++;

			//k+=0.02F;

			this.dev.Clear(ClearFlags.Target | ClearFlags.ZBuffer, this.SkyCol, 1.0F, 0);
			this.dev.BeginScene();


			//---------lights
			dev.Lights[1].Type = LightType.Directional;
			dev.Lights[1].Diffuse = System.Drawing.Color.White;
			dev.Lights[1].Direction = new Vector3(0.2F, -1, 0.2F);

			dev.Lights[1].Enabled = true;
			//---------lights;


			this.s.Render();
/*			this.s.SpaceObject.SelfMatrix = Matrix.Translation(-this.wSMax,0,0);
			this.s.Render();
			this.s.SpaceObject.SelfMatrix = Matrix.Identity;
*/

			this.dev.EndScene();
			this.dev.Present();


			if (this.CloseNow) this.Close();
		}


		
		
		
		private void button1_Click(object sender, System.EventArgs e)
		{
			//NAVIGATOR:
			Microsoft.DirectX.DirectInput.Device md; 
			
			md = Misc.GetDefaultMouseDevice(this, 0); 
			md.Unacquire(); 
			md.SetCooperativeLevel(this, Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Exclusive | Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Foreground); 
			md.Properties.AxisModeAbsolute = false; 
			md.Acquire(); 


			StringCollection ca = new StringCollection(); 
			AssignementsMap ia = new AssignementsMap(); 

			Controller c; 
			c = FreeSpiritNavigator10.GetDefaultController(); 

			ca = c.ControlActions;
 
			ca.Add("EXIT"); 
			ca.Add("FillMode"); 
			ia.KeyboardAssignements.Add(new ButtonInputAssignement(ca.IndexOf("EXIT"), (byte)Microsoft.DirectX.DirectInput.Key.Escape, 0, 0)); 
			ia.KeyboardAssignements.Add(new ButtonInputAssignement(ca.IndexOf("FillMode"), (byte)Microsoft.DirectX.DirectInput.Key.F, 0, 0)); 

			ia.MouseAssignements.Add(new AxisInputAssignement(ca.IndexOf("TurnLeft"), Axis3.X, false)); 
			ia.MouseAssignements.Add(new AxisInputAssignement(ca.IndexOf("TurnRight"), Axis3.X, false)); 
			ia.MouseAssignements.Add(new AxisInputAssignement(ca.IndexOf("TurnDown"), Axis3.Y, false)); 
			ia.MouseAssignements.Add(new AxisInputAssignement(ca.IndexOf("TurnUp"), Axis3.Y, false)); 
			
			ia.KeyboardAssignements.Add(new ButtonInputAssignement(ca.IndexOf("RollLeft"), (byte)Microsoft.DirectX.DirectInput.Key.Delete, 0, 0)); 
			ia.KeyboardAssignements.Add(new ButtonInputAssignement(ca.IndexOf("RollRight"), (byte)Microsoft.DirectX.DirectInput.Key.PageDown, 0, 0)); 
			ia.KeyboardAssignements.Add(new ButtonInputAssignement(ca.IndexOf("Foreward"), (byte)Microsoft.DirectX.DirectInput.Key.UpArrow, 0, 0)); 
			ia.KeyboardAssignements.Add(new ButtonInputAssignement(ca.IndexOf("Backward"), (byte)Microsoft.DirectX.DirectInput.Key.DownArrow, 0, 0)); 
			
			this.ic = new InputController(Misc.GetDefaultKeyboardDevice(this, 0), md, ca, ia); 

			this.n = new OnSurfaceNavigator10(this.cam, ic, this.s, this.MyHeight);


			this.ic.ActionPerformed += new Direct3DAid.Mobility.Controllers.Controller.ActionPerformedHandler(this.OnControllerAction);
		}



		private void OnControllerAction(int index, string name, double val)
		{
			if (name == "EXIT")
			{
				this.CloseNow = true;
			}
			else if (name == "FillMode") 
			{
				if (!this.newCheckT.Enabled)
				{
					if (dev.RenderState.FillMode == FillMode.Solid)
						dev.RenderState.FillMode = FillMode.WireFrame;
					else 
						dev.RenderState.FillMode = FillMode.Solid;

					this.newCheckT.Start();
				}
			}
		}




		private int f =0;
		private void timer2_Tick(object sender, System.EventArgs e)
		{
			this.FPS.Text=f.ToString();
			f=0;
		}

		

		//handles surface updating of geometry
		private void OnUpdatingGeometry(Surfaces.SurfaceSplatted.UpdatingGeometryInfo info)
		{
			if (this.UpdatingSurfaceGeometry != null) this.UpdatingSurfaceGeometry(info);
		}

		private void newCheckT_Tick(object sender, System.EventArgs e)
		{
			this.newCheckT.Stop();
		}


		
		private void startT_Tick(object sender, System.EventArgs e)
		{
			this.startT.Stop();
			this.button1_Click(this, null);
		}



	
	}
}










//mesh settings (sample for how to use mesh)
/*
FIELDS:
	Mesh mesh = null; // Our mesh object in sysmem
	Material[] meshMaterials;
	SpaceObject meshSO = null; //space obj for mesh

INCODE (LOADING):
	ExtendedMaterial[] materials = null;

	this.mesh = Mesh.FromFile("mesh//cursor2.x", MeshFlags.SystemMemory, this.dev, out materials);

	this.meshMaterials = new Material[materials.Length];
	for(i=0; i<materials.Length; i++)
	{
		this.meshMaterials[i] = materials[i].Material3D;
		// Set the ambient color for the material (D3DX does not do this)
		this.meshMaterials[i].Ambient = this.meshMaterials[i].Diffuse;
	}

	this.meshSO = new SpaceObject(this.dev);
	this.meshSO.SelfMatrix = Matrix.Translation(1.2F,2.8F,1.0F);
*/
//mesh

