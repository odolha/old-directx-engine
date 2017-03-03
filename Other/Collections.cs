//LYBRARY SPECIALIZED COLLECTIONS ARE DESCRIBED HERE


using System;
using System.Collections;


public class InputAssignementCollection : ArrayList 
{
	//Item property shadowed from the base (ArrayList)
	public new Direct3DAid.Mobility.Controllers.InputAssignement this[int index]
	{ 
		get 
		{ 
			return ((Direct3DAid.Mobility.Controllers.InputAssignement)(base[index])); 
		} 
		set 
		{ 
			base[index] = value;
		} 
	} 




	//Class Constructor
	public InputAssignementCollection() :
		base()
	{ 
	} 




	//Add method overriding the base method
	public int Add(Direct3DAid.Mobility.Controllers.InputAssignement value) 
	{ 
		return base.Add(value);
	} 

	//Remove method overriding the base method
	public void Remove(Direct3DAid.Mobility.Controllers.InputAssignement obj) 
	{ 
		base.Remove(obj); 
	} 
}
