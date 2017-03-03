using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Direct3DAid.Geometries;
using Direct3DAid.Surfaces;


namespace Direct3DAid
{
	/// <summary>
	/// Summary description for frmTest3.
	/// </summary>
	public class frmTest3 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox pathT;
		private System.Windows.Forms.Button startB;
		private System.Windows.Forms.Label processingL;
		private System.Windows.Forms.ProgressBar progressB;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.PictureBox colorB;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox fullScreenCB;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox myHeightT;
		private System.ComponentModel.IContainer components;

		public frmTest3()
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
				if(components != null)
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
			this.pathT = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.startB = new System.Windows.Forms.Button();
			this.processingL = new System.Windows.Forms.Label();
			this.progressB = new System.Windows.Forms.ProgressBar();
			this.colorB = new System.Windows.Forms.PictureBox();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.label2 = new System.Windows.Forms.Label();
			this.fullScreenCB = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.myHeightT = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// pathT
			// 
			this.pathT.Location = new System.Drawing.Point(104, 8);
			this.pathT.Name = "pathT";
			this.pathT.Size = new System.Drawing.Size(256, 20);
			this.pathT.TabIndex = 0;
			this.pathT.Text = "maps//HeightMap1.bmp";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "Height Map Path";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// startB
			// 
			this.startB.Location = new System.Drawing.Point(8, 32);
			this.startB.Name = "startB";
			this.startB.Size = new System.Drawing.Size(96, 32);
			this.startB.TabIndex = 2;
			this.startB.Text = "Start Simulation";
			this.startB.Click += new System.EventHandler(this.startB_Click);
			// 
			// processingL
			// 
			this.processingL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.processingL.ForeColor = System.Drawing.Color.Red;
			this.processingL.Location = new System.Drawing.Point(8, 72);
			this.processingL.Name = "processingL";
			this.processingL.Size = new System.Drawing.Size(456, 64);
			this.processingL.TabIndex = 3;
			this.processingL.Text = "When ready, press \"Sart Simulation\"";
			this.processingL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// progressB
			// 
			this.progressB.Location = new System.Drawing.Point(8, 144);
			this.progressB.Maximum = 22;
			this.progressB.Name = "progressB";
			this.progressB.Size = new System.Drawing.Size(456, 23);
			this.progressB.Step = 1;
			this.progressB.TabIndex = 4;
			// 
			// colorB
			// 
			this.colorB.BackColor = System.Drawing.Color.AliceBlue;
			this.colorB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.colorB.Location = new System.Drawing.Point(440, 8);
			this.colorB.Name = "colorB";
			this.colorB.Size = new System.Drawing.Size(24, 24);
			this.colorB.TabIndex = 5;
			this.colorB.TabStop = false;
			this.colorB.Click += new System.EventHandler(this.colorB_Click);
			// 
			// colorDialog
			// 
			this.colorDialog.Color = System.Drawing.Color.AliceBlue;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(376, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 24);
			this.label2.TabIndex = 1;
			this.label2.Text = "Sky Color";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// fullScreenCB
			// 
			this.fullScreenCB.Location = new System.Drawing.Point(368, 40);
			this.fullScreenCB.Name = "fullScreenCB";
			this.fullScreenCB.Size = new System.Drawing.Size(96, 24);
			this.fullScreenCB.TabIndex = 6;
			this.fullScreenCB.Text = "Full Screen";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(160, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 24);
			this.label3.TabIndex = 1;
			this.label3.Text = "Distance From Ground";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// myHeightT
			// 
			this.myHeightT.Location = new System.Drawing.Point(288, 40);
			this.myHeightT.Name = "myHeightT";
			this.myHeightT.Size = new System.Drawing.Size(72, 20);
			this.myHeightT.TabIndex = 7;
			this.myHeightT.Text = "0,5";
			// 
			// frmTest3
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(472, 174);
			this.Controls.Add(this.myHeightT);
			this.Controls.Add(this.fullScreenCB);
			this.Controls.Add(this.colorB);
			this.Controls.Add(this.progressB);
			this.Controls.Add(this.processingL);
			this.Controls.Add(this.startB);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pathT);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Name = "frmTest3";
			this.Text = "frmTest3";
			this.ResumeLayout(false);

		}
		#endregion




		private void startB_Click(object sender, System.EventArgs e)
		{
            Bitmap b = (Bitmap)Image.FromFile(this.pathT.Text);
            
			frmTest2 f;
			f = new frmTest2(b.Width, b.Height, this.pathT.Text, this.colorB.BackColor, this.fullScreenCB.Checked, float.Parse(this.myHeightT.Text, System.Globalization.NumberStyles.Number));

			f.UpdatingSurfaceGeometry += new frmTest2.UpdatingSurfaceGeometryHandler(OnSurfUpdatingGeometry);

			f.Show();

			this.progressB.Value = 0;
		}

		
		private void colorB_Click(object sender, System.EventArgs e)
		{
			if (this.colorDialog.ShowDialog() == DialogResult.OK) this.colorB.BackColor = this.colorDialog.Color;
		}





		private void OnSurfUpdatingGeometry(Surfaces.SurfaceSplatted.UpdatingGeometryInfo info)
		{
			if (this.progressB.Value < this.progressB.Maximum)
				this.progressB.Value += 1;
			else
				this.progressB.Value = 0;


			string t = "Surface -> Updating Geometry - Stage: " + info.Stage.ToString();
			
			if ((info.CurrentPoint.X >= 0) && (info.CurrentPoint.Y >= 0))
				t = t + " (Current Point = [" + info.CurrentPoint.X + ", " + info.CurrentPoint.Y + "])";

			if (info.NormalizingInfo.Stage >= 0)
			{
				t = t + ", " + info.NormalizingInfo.Stage.ToString();

				if (info.NormalizingInfo.CurrentVertex >= 0)
					t = t + " (Current Vertex = " + info.NormalizingInfo.CurrentVertex + ")";

				if (info.NormalizingInfo.GettingPrimitiveNormalsInfo.Stage >= 0)
				{
					t = t + ", " + info.NormalizingInfo.GettingPrimitiveNormalsInfo.Stage.ToString();

					if (info.NormalizingInfo.GettingPrimitiveNormalsInfo.CurrentPrimitive >= 0)
						t = t + " (Current Primitive = " + info.NormalizingInfo.GettingPrimitiveNormalsInfo.CurrentPrimitive + ")";
				}
			}

			if (info.UpdatingDivisionedGeometryInfo.Stage >= 0)
			{
				t = t + ", " + info.UpdatingDivisionedGeometryInfo.Stage.ToString();

				if (info.UpdatingDivisionedGeometryInfo.CurrentDivision >= 0)
					t = t + " (Current Division = " + info.UpdatingDivisionedGeometryInfo.CurrentDivision + ")";

				if (info.UpdatingDivisionedGeometryInfo.CurrentVertex >= 0)
					t = t + " (Current Vertex = " + info.UpdatingDivisionedGeometryInfo.CurrentVertex + ")";

				if (info.UpdatingDivisionedGeometryInfo.GettingGeneratedDivisionIBInfo.Stage >= 0)
				{
					t = t + ", " + info.UpdatingDivisionedGeometryInfo.GettingGeneratedDivisionIBInfo.Stage.ToString();

					if (info.UpdatingDivisionedGeometryInfo.GettingGeneratedDivisionIBInfo.CurrentPrimitive >= 0)
						t = t + " (Current Primitive = " + info.UpdatingDivisionedGeometryInfo.GettingGeneratedDivisionIBInfo.CurrentPrimitive + ")";
				}
			}


			this.processingL.Text = t;

			Application.DoEvents();
		}



		private void timer1_Tick(object sender, System.EventArgs e)
		{
			this.Refresh();
			Application.DoEvents();
		}



	}
}
