using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using Direct3DAid;
using Direct3DAid.Geometries;
using Direct3DAid.Geometries.VerticesModifications;
using Direct3DAid.Surfaces;


namespace Direct3DAid
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmTest : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;

		public frmTest()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.button1.Text = "test modi";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// frmTest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(592, 566);
			this.Controls.Add(this.button1);
			this.Name = "frmTest";
			this.Text = "Test Form";
			this.Load += new System.EventHandler(this.frmTest_Load);
			this.ResumeLayout(false);

		}
		#endregion




		public GeometryIndexedSplatted g;
		public Device dev;
		public VertexBuffer vb;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button button1;
		public IndexBuffer ib;
		public Texture tex;


		private void frmTest_Load(object sender, System.EventArgs e)
		{
			PresentParameters pp;
			pp = new PresentParameters();
			pp.Windowed=true; // We don't want to run fullscreen
			pp.SwapEffect = SwapEffect.Discard; // Discard the frames 
			pp.EnableAutoDepthStencil = true; // Turn on a Depth stencil
			pp.AutoDepthStencilFormat = DepthFormat.D16; // And the stencil format

			dev = new Device(0, DeviceType.Hardware,this ,CreateFlags.SoftwareVertexProcessing,pp);
			dev.RenderState.CullMode = Cull.None;
			dev.RenderState.ZBufferEnable = true;
			dev.RenderState.Lighting = false;

			//enable alpha blending
			dev.RenderState.AlphaBlendEnable = true;
			dev.RenderState.SourceBlend = Blend.SourceAlpha;
		    dev.RenderState.DestinationBlend = Blend.InvSourceAlpha;
			dev.RenderState.BlendOperation = BlendOperation.Add;




			vb = new VertexBuffer(typeof(FVF_PosColTex),25,dev,Usage.SoftwareProcessing, FVF_PosColTex.Format , Pool.Default);
			ib = new IndexBuffer(typeof(short),28*3,dev,Usage.SoftwareProcessing,Pool.Default);


			//defining vertices
			FVF_PosColTex[] verts;
			verts = (FVF_PosColTex[])vb.Lock(0,0);

			/*
			verts[0].Pos = new Vector3(0,1,0);
			verts[1].Pos = new Vector3(1,1,0);
			verts[2].Pos = new Vector3(0,0,0);
			verts[3].Pos = new Vector3(1,0,0);

			verts[0].Col = Color.Red.ToArgb();
			verts[1].Col = Color.White.ToArgb();
			verts[2].Col = Color.Red.ToArgb();
			verts[3].Col = Color.Blue.ToArgb();
			*/

			verts[0].Pos = new Vector3(4,0,0);
			verts[1].Pos = new Vector3(0,0,0);
			verts[2].Pos = new Vector3(1,0,0);
			verts[3].Pos = new Vector3(1,1,0);
			verts[4].Pos = new Vector3(1,2,0);
			verts[5].Pos = new Vector3(2,0,0);
			verts[6].Pos = new Vector3(3,1,0);
			verts[7].Pos = new Vector3(2,2,0);
			verts[8].Pos = new Vector3(2,3,0);
			verts[9].Pos = new Vector3(0,3,0);
			verts[10].Pos = new Vector3(1,4,0);
			verts[11].Pos = new Vector3(1,5,0);
			verts[12].Pos = new Vector3(2,4,0);
			verts[13].Pos = new Vector3(3,5,0);
			verts[14].Pos = new Vector3(4,5,0);
			verts[15].Pos = new Vector3(5,0,0);
			verts[16].Pos = new Vector3(5,1,0);
			verts[17].Pos = new Vector3(4,1,0);
			verts[18].Pos = new Vector3(3,3,0);
			verts[19].Pos = new Vector3(4,4,0);
			verts[20].Pos = new Vector3(5,4,0);
			verts[21].Pos = new Vector3(4,3,0);
			verts[22].Pos = new Vector3(5,3,0);
			verts[23].Pos = new Vector3(4,2,0);
			verts[24].Pos = new Vector3(5,2,0);

			for (int i=0; i<=24; i++)
			{
				verts[i].Col=Color.FromArgb(255,255,255).ToArgb();
			}
            

			vb.Unlock();


			//defining indices
			short[] indices;
			indices = (short[])ib.Lock(0,0);

			/*
			indices[0]=0;indices[1]=1;indices[2]=3;
			indices[3]=0;indices[3]=3;indices[4]=2;
			*/

			indices[0]=1;indices[1]=3;indices[2]=2;
			indices[3]=2;indices[4]=3;indices[5]=5;
			indices[6]=3;indices[7]=7;indices[8]=5;
			indices[9]=5;indices[10]=7;indices[11]=6;
			indices[12]=7;indices[13]=23;indices[14]=6;
			indices[15]=23;indices[16]=17;indices[17]=6;
			indices[18]=23;indices[19]=24;indices[20]=17;
			indices[21]=17;indices[22]=24;indices[23]=16;
			indices[24]=17;indices[25]=16;indices[26]=0;
			indices[27]=0;indices[28]=16;indices[29]=15;
			indices[30]=9;indices[31]=11;indices[32]=10;
			indices[33]=9;indices[34]=10;indices[35]=8;
			indices[36]=9;indices[37]=8;indices[38]=4;
			indices[39]=11;indices[40]=12;indices[41]=10;
			indices[42]=10;indices[43]=12;indices[44]=18;
			indices[45]=10;indices[46]=18;indices[47]=8;
			indices[48]=8;indices[49]=7;indices[50]=4;
			indices[51]=8;indices[52]=18;indices[53]=7;
			indices[54]=23;indices[55]=7;indices[56]=18;
			indices[57]=18;indices[58]=21;indices[59]=23;
			indices[60]=21;indices[61]=24;indices[62]=23;
			indices[63]=21;indices[64]=22;indices[65]=24;
			indices[66]=11;indices[67]=13;indices[68]=12;
			indices[69]=12;indices[70]=13;indices[71]=18;
			indices[72]=13;indices[73]=14;indices[74]=18;
			indices[75]=14;indices[76]=20;indices[77]=19;
			indices[78]=20;indices[79]=21;indices[80]=19;
			indices[81]=22;indices[82]=21;indices[83]=20;


			ib.Unlock();
			
			tex = TextureLoader.FromFile(dev,"c:\\test.jpg");

			//////////////////////////////////////////////////////////////

			//div distrib:
			uint[] dd;
			dd = new uint[25];
			dd[0]=2;
			dd[1]=2;
			dd[2]=2;
			dd[3]=2;
			dd[4]=0;
			dd[5]=2;
			dd[6]=1;
			dd[7]=1;
			dd[8]=0;
			dd[9]=0;
			dd[10]=1;
			dd[11]=0;
			dd[12]=0;
			dd[13]=1;
			dd[14]=0;
			dd[15]=2;
			dd[16]=2;
			dd[17]=2;
			dd[18]=1;
			dd[19]=0;
			dd[20]=0;
			dd[21]=0;
			dd[22]=2;
			dd[23]=1;
			dd[24]=2;

			g = new GeometryIndexedSplatted(this.dev,this.vb,this.ib,typeof(FVF_PosColTex),FVF_PosColTex.Format, PrimitiveType.TriangleList,3,dd);
			g.DivisionTexture[0] = TextureLoader.FromFile(dev,"c:\\t0.jpg");
			g.DivisionTexture[1] = TextureLoader.FromFile(dev,"c:\\t1.jpg");
			g.DivisionTexture[2] = TextureLoader.FromFile(dev,"c:\\t2.jpg");
			
			g.UpdateDivisionedGeometry();




			this.timer1.Enabled=true;
		}
	


		float k=0;
		

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			//k+=0.02F;

			this.dev.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.DarkCyan, 1.0F, 0);
			this.dev.BeginScene();

			this.dev.Transform.World=Matrix.Multiply(Matrix.Multiply(Matrix.Translation(-2.5F,-2.5F,0),Matrix.RotationY(k)),Matrix.Translation(2.5F,2.5F,0));
			this.dev.Transform.View=Matrix.LookAtLH(new Vector3(0,4,-8.5f),new Vector3(2.5F,2.0F,0),new Vector3(0,1,0));
			this.dev.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI/4, 1.0F, 1.0F, 100.0F);

			this.g.Render();

			this.dev.EndScene();
			this.dev.Present();
		}


		
		
		
		private void button1_Click(object sender, System.EventArgs e)
		{
			Vector3Modification[] pm;
			pm = new Vector3Modification[2];
			ColorModification[] cm;
			cm = new ColorModification[2];

			cm[0] = new ColorModification(0,Color.Brown.ToArgb());
			cm[1] = new ColorModification(3,Color.Cyan.ToArgb());
           
			pm[0] = new Vector3Modification(1,new Vector3(-1,2,2));
			pm[1] = new Vector3Modification(0,new Vector3(3,2,2));



			UVMapPlaneXY uvM;
			uvM = new UVMapPlaneXY(new PointF(0,0),new SizeF(5,5),new SizeF(1,1));


			this.g.ResetVertices(uvM);
		
			//this.g.ResetVertices(pm,cm,null,null);		
		}
	}
}
