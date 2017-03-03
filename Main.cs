using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;


namespace Direct3DAid
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class MainClass
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			//Application.Run(new frmTest2(80,80,"maps//HeightMap4.bmp",Color.SkyBlue));
			Application.Run(new frmTest3());
		}
	}
}
