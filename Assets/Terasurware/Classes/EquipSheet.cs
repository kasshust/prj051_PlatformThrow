using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipSheet : ScriptableObject
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
		public string name;
		public string  description;
		public int icon;
		public int type;
		public int attack;
		public int defence;
		public int value;
		public int price;
	}
}

