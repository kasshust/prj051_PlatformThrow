using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Flags_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/NotAlwaysIncludeResources/SimulatioData/Flags.xls";
	private static readonly string exportPath = "Assets/NotAlwaysIncludeResources/SimulatioData/Flags.asset";
	private static readonly string[] sheetNames = { "Flags", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			FlagList data = (FlagList)AssetDatabase.LoadAssetAtPath (exportPath, typeof(FlagList));
			if (data == null) {
				data = ScriptableObject.CreateInstance<FlagList> ();
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

					FlagList.Sheet s = new FlagList.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						FlagList.Param p = new FlagList.Param ();
						
					cell = row.GetCell(0); p.id = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.output = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.day = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.time = (cell == null ? 0.0 : cell.NumericCellValue);
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
