//Structure: TextureCoordinates
//Author: Ovidiu Dolha
//Description:	 -
//				 This strucutre provides info about a vertex texture coordinate UV


namespace Direct3DAid
{
	public struct TextureCoordinates
	{ 
		public float u; 
		public float v; 



		public static TextureCoordinates Empty
		{
			get
			{
				return new TextureCoordinates(0,0);
			}
		}



		public TextureCoordinates(float u, float v) 
		{ 
			this.u = u; 
			this.v = v; 
		} 
	}
}