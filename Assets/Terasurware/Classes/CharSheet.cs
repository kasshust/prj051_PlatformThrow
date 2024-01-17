using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharSheet : ScriptableObject
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
		
		public string name;
		public string standard;
		public string glad;
		public string depressed;
		public string puzzled;
		public string surprise;
		public string angry;
	}
}

