using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlagList : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public double id;
		public string output;
		public double day;
		public double time;
	}
}

