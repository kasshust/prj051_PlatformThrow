using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class CharList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/xls/CharList.xls";
	private static readonly string exportPath = "Assets/Resources/xls/CharList.asset";
	private static readonly string[] sheetNames = { "charSheet", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			CharSheet data = (CharSheet)AssetDatabase.LoadAssetAtPath (exportPath, typeof(CharSheet));
			if (data == null) {
				data = ScriptableObject.CreateInstance<CharSheet> ();
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

					CharSheet.Sheet s = new CharSheet.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						CharSheet.Param p = new CharSheet.Param ();
						
					cell = row.GetCell(0); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(1); p.standard = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.glad = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.depressed = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.puzzled = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.surprise = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.angry = (cell == null ? "" : cell.StringCellValue);
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
