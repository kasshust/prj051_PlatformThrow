using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class EquipList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/xls/EquipList.xls";
	private static readonly string exportPath = "Assets/Resources/xls/EquipList.asset";
	private static readonly string[] sheetNames = { "sword","shield", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			EquipSheet data = (EquipSheet)AssetDatabase.LoadAssetAtPath (exportPath, typeof(EquipSheet));
			if (data == null) {
				data = ScriptableObject.CreateInstance<EquipSheet> ();
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

					EquipSheet.Sheet s = new EquipSheet.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						EquipSheet.Param p = new EquipSheet.Param ();
						
					cell = row.GetCell(0); p.id = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p. description = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.icon = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.type = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.attack = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.defence = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.value = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.price = (int)(cell == null ? 0 : cell.NumericCellValue);
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
