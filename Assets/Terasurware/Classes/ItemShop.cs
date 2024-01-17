using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemShop : ScriptableObject
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
		public int ItemId;
		public int num;
		public int buyPrice;
		public int sellPrice;
	}
}

