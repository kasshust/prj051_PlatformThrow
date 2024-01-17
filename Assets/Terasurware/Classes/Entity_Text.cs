using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Text : ScriptableObject
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
		public string text1;
		public string text2;
		public string text3;
		public string text4;
		public string text5;
		public string text6;
		public string text7;
		public string text8;
		public string text9;
		public string text10;
	}
}

