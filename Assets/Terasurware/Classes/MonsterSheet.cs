using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSheet : ScriptableObject
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
		public string  description;
		public int icon;
		public int attack;
		public int defence;
		public int speed;
		public int value;
		public int drop1;
		public int possible1;
		public int drop2;
		public int possible2;
		public int drop3;
		public int possible3;
	}
}

