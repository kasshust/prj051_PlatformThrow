using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using Kogane;
using System.Linq;

public static class JsonDataManager
{
    /// パスを取得 & セーブファイル名記録
    private static string getFilePath() { return Application.persistentDataPath + "/saveData"; }

    public static void Save<T>(T saveData, string key, bool pretty = false)
    {
        string jsonSerializedData = JsonUtility.ToJson(saveData, pretty);
        save(jsonSerializedData, key);

    }

    public static void SaveArray<T>(T[] array, string key)
    {
        string jsonSerializedData = JsonHelper.ToJson(array);
        save(jsonSerializedData, key);
    }

    public static void SaveDict<TKey, TValue>(Dictionary<TKey, TValue> dictionary, string key)
    {
        var jsonDictionary = new JsonDictionary<TKey, TValue>(dictionary);
        var jsonSerializedData = JsonUtility.ToJson(jsonDictionary, true);
        save(jsonSerializedData, key);
    }

    private static void save(string jsonSerializedData, string key)
    {
        using (var sw = new StreamWriter(getFilePath() + "_" + key + ".json", false))
        {
            try
            {
                sw.Write(jsonSerializedData);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public static T Load<T>(string key)
    {
        T jsonDeserializedData;

        try
        {
            using (FileStream fs = new FileStream(getFilePath() + "_" + key + ".json", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                string result = sr.ReadToEnd();
                return jsonDeserializedData = JsonUtility.FromJson<T>(result);
            }
        }
        catch (Exception e)
        {
            Debug.Log("データが存在しません");
            Debug.LogWarning(e);
            return default(T);
        }
    }

    public static T[] LoadArray<T>(string key)
    {
        T[] jsonDeserializedData;

        try
        {
            using (FileStream fs = new FileStream(getFilePath() + "_" + key + ".json", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                string result = sr.ReadToEnd();
                return jsonDeserializedData = JsonHelper.FromJson<T>(result);
            }
        }
        catch (Exception e)
        {
            // Debug.Log("セーブデータが存在しません");
            Debug.LogWarning(e);
            return default(T[]);
        }
    }

    public static Dictionary<TKey, TValue> LoadDict<TKey, TValue>(string key)
    {
        try
        {
            using (FileStream fs = new FileStream(getFilePath() + "_" + key + ".json", FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                string result = sr.ReadToEnd();
                var jsonDictionary = JsonUtility.FromJson<JsonDictionary<TKey, TValue>>(result);
                var dictionary = jsonDictionary.Dictionary;
                return dictionary;
            }
        }
        catch (Exception e)
        {
            // Debug.Log("セーブデータが存在しません");
            Debug.LogWarning(e);
            return null;
        }
    }

}

/// <summary>
/// <see cref="JsonUtility"/> に不足している機能を提供します。
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// 指定した string を Root オブジェクトを持たない JSON 配列と仮定してデシリアライズします。
    /// </summary>
    public static T[] FromJson<T>(string json)
    {
        // ルート要素があれば変換できるので
        // 入力されたJSONに対して(★)の行を追加する
        //
        // e.g.
        // ★ {
        // ★     "array":
        //        [
        //            ...
        //        ]
        // ★ }
        //
        string dummy_json = $"{{\"{DummyNode<T>.ROOT_NAME}\": {json}}}";

        // ダミーのルートにデシリアライズしてから中身の配列を返す
        var obj = JsonUtility.FromJson<DummyNode<T>>(dummy_json);
        return obj.array;
    }

    /// <summary>
    /// 指定した配列やリストなどのコレクションを Root オブジェクトを持たない JSON 配列に変換します。
    /// </summary>
    /// <remarks>
    /// 'prettyPrint' には非対応。整形したかったら別途変換して。
    /// </remarks>
    public static string ToJson<T>(IEnumerable<T> collection)
    {
        string json = JsonUtility.ToJson(new DummyNode<T>(collection)); // ダミールートごとシリアル化する
        int start = DummyNode<T>.ROOT_NAME.Length + 4;
        int len = json.Length - start - 1;
        return json.Substring(start, len); // 追加ルートの文字を取り除いて返す
    }

    // 内部で使用するダミーのルート要素
    [Serializable]
    private struct DummyNode<T>
    {
        // 補足:
        // 処理中に一時使用する非公開クラスのため多少設計が変でも気にしない

        // JSONに付与するダミールートの名称
        public const string ROOT_NAME = nameof(array);
        // 疑似的な子要素
        public T[] array;
        // コレクション要素を指定してオブジェクトを作成する
        public DummyNode(IEnumerable<T> collection) => this.array = collection.ToArray();
    }
}