using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemList : ScriptableObject
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
		
		public int id;
		public string name;
		public string description;
		public int valuables;
		public int consume;
		public int typeA;
		public int typeB;
		public int valueA;
		public int valueB;
		public int valueC;
		public int valueD;
		public int valueE;
		public int valueF;
		public int valueG;
		public int icon;
		public int price;
	}
}

