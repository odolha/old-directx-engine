//LIBRARY EXCEPTIONS ARE DESCRIBED HERE


//Class of exceptions for the general library
class ExceptionDirectXAid : System.Exception
{
	public ExceptionDirectXAid(string message):
		base(message)
	{
	}
}



//Class of exceptions for the Geometry part
class ExceptionGeometry : ExceptionDirectXAid 
{
	public ExceptionGeometry(string message):
		base(message)
	{
	}
}

//Class of exceptions for the Mobility part
class ExceptionMobility : ExceptionDirectXAid 
{
	public ExceptionMobility(string message):
		base(message)
	{
	}
}

//Class of exceptions for the SpaceObject part
class ExceptionSpaceObject : ExceptionDirectXAid 
{
	public ExceptionSpaceObject(string message):
		base(message)
	{
	}
}

//Class of exceptions for the Surface part
class ExceptionSurface : ExceptionDirectXAid 
{
	public ExceptionSurface(string message):
		base(message)
	{
	}
}

//Class of exceptions for the UVMap part
class ExceptionUVMap : ExceptionDirectXAid 
{
	public ExceptionUVMap(string message):
		base(message)
	{
	}
}
