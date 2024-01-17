using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ItemShop_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Project/PRJ080/Item/ItemShop.xls";
	private static readonly string exportPath = "Assets/Project/PRJ080/Item/ItemShop.asset";
	private static readonly string[] sheetNames = { "Shop1","Shop2","Shop3","Shop4","Sheet5","Sheet6", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			ShopList data = (ShopList)AssetDatabase.LoadAssetAtPath (exportPath, typeof(ShopList));
			if (data == null) {
				data = ScriptableObject.CreateInstance<ShopList> ();
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

					ShopList.Sheet s = new ShopList.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						ShopList.Param p = new ShopList.Param ();
						
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.ItemId = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.num = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.buyPrice = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.sellPrice = (int)(cell == null ? 0 : cell.NumericCellValue);
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
