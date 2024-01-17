using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ItemList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Project/PRJ080/Item/ItemList.xls";
	private static readonly string exportPath = "Assets/Project/PRJ080/Item/ItemList.asset";
	private static readonly string[] sheetNames = { "item", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			ItemList data = (ItemList)AssetDatabase.LoadAssetAtPath (exportPath, typeof(ItemList));
			if (data == null) {
				data = ScriptableObject.CreateInstance<ItemList> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					ItemList.Sheet s = new ItemList.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						ItemList.Param p = new ItemList.Param ();
						
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.description = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.valuables = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.consume = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.typeA = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.typeB = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.valueA = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.valueB = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.valueC = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.valueD = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.valueE = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.valueF = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.valueG = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.icon = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.price = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
