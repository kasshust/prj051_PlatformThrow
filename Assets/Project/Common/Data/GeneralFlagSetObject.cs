using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "GeneralFlagSet", menuName = "Flag/GeneralFlagSet")]
public class GeneralFlagSetObject : ScriptableObject
{

    [SerializeField]
    GeneralFlagSet m_GeneralFlagSet;

    public void InitAllFlag()
    {
        GeneralFlagSet d = new GeneralFlagSet();
        d.CopyFrom(m_GeneralFlagSet);

        foreach (KeyValuePair<string, GeneralFlagDict> pair in d)
        {
            GeneralFlagDict i = m_GeneralFlagSet[pair.Key];
            i.InitAllFlag();
            m_GeneralFlagSet[pair.Key] = i;
        }
    }

    public void save(string key)
    {

        foreach (KeyValuePair<string, GeneralFlagDict> pair in m_GeneralFlagSet)
        {
            m_GeneralFlagSet[pair.Key].save(key + "_" + pair.Key);
        }
    }
    public void load(string key)
    {
        foreach (KeyValuePair<string, GeneralFlagDict> pair in m_GeneralFlagSet)
        {
            m_GeneralFlagSet[pair.Key].load(key + "_" + pair.Key);
        }
    }

    private GeneralFlagDict GetDict(string key)
    {
        if (m_GeneralFlagSet.ContainsKey(key))
        {
            return m_GeneralFlagSet[key];
        }
        else
        {
            Debug.LogError(key + "keyに対応する値が存在しません");
            return new GeneralFlagDict();
        }
    }

    public void InitDict(string key) {
        if (m_GeneralFlagSet.ContainsKey(key))
        {
            m_GeneralFlagSet[key].InitAllFlag();
            Debug.Log("キーネーム : " + key + "の一般フラグを初期化しました");
        }
        else
        {
            Debug.LogError(key + "keyに対応する値が存在しません");
        }
    }

    public void SetValue(string key1, string key2, GeneralFlag f) {
        GeneralFlagDict d = GetDict(key1);
        d.SetValue(key2, f);
    }

    public GeneralFlag GetValue(string key1, string key2)
    {
        GeneralFlagDict d = GetDict(key1);
        return d.GetValue(key2);
    }

}


[Serializable]
public class GeneralFlagSet : SerializableDictionary<string, GeneralFlagDict>
{

}

[Serializable]
public struct GeneralFlag
{
    [SerializeField]
    public string Explain;
    [SerializeField, ReadOnly]
    public bool m_isRequesting;
    [SerializeField, ReadOnly]
    public bool m_Finished;

    public void Init()
    {
        m_isRequesting = false;
        m_Finished = false;
    }
}

[Serializable]
public class GeneralFlagDict
{
    [SerializeField]
    private GeneralFlagDictionary m_GeneralFlagDict;

    string savekey1 = "_isRequesting";
    string savekey2 = "_Count";

    public void InitAllFlag()
    {

        GeneralFlagDictionary d = new GeneralFlagDictionary();
        d.CopyFrom(m_GeneralFlagDict);

        foreach (KeyValuePair<string, GeneralFlag> pair in d)
        {
            GeneralFlag i = m_GeneralFlagDict[pair.Key];
            i.Init();
            m_GeneralFlagDict[pair.Key] = i;
        }
    }

    public void save(string key)
    {
        Dictionary<string, bool> saveDict1 = new Dictionary<string, bool>();
        Dictionary<string, bool> saveDict2 = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, GeneralFlag> pair in m_GeneralFlagDict)
        {
            GeneralFlag i = m_GeneralFlagDict[pair.Key];
            bool b = i.m_isRequesting;
            bool c = i.m_Finished;

            saveDict1.Add(pair.Key, b);
            saveDict2.Add(pair.Key, c);
        }

        JsonDataManager.SaveDict<string, bool>(saveDict1, key + savekey1);
        JsonDataManager.SaveDict<string, bool>(saveDict2, key + savekey2);
    }
    public void load(string key)
    {
        Dictionary<string, bool> loadDict1 = JsonDataManager.LoadDict<string, bool>(key + savekey1);
        Dictionary<string, bool> loadDict2 = JsonDataManager.LoadDict<string, bool>(key + savekey2);

        if (loadDict1 == null || loadDict2 == null)
        {
            return;
        }

        GeneralFlagDictionary d = new GeneralFlagDictionary();
        d.CopyFrom(m_GeneralFlagDict);
        foreach (KeyValuePair<string, GeneralFlag> pair in d)
        {
            GeneralFlag i = m_GeneralFlagDict[pair.Key];

            if (!loadDict1.ContainsKey(pair.Key))
            {
                Debug.LogWarning("セーブデータのバージョンが異なります : " + key + " " + pair.Key + " に対応するフラグを作成します");
                i.m_isRequesting = false;
            } else i.m_isRequesting = loadDict1[pair.Key];

            if (!loadDict2.ContainsKey(pair.Key))
            {
                Debug.LogWarning("セーブデータのバージョンが異なります : " + key + " " + pair.Key + " に対応するフラグを作成します");

            } else i.m_Finished = loadDict2[pair.Key];

            m_GeneralFlagDict[pair.Key] = i;
        }
    }

    public GeneralFlag GetValue(string key) {
        if (m_GeneralFlagDict.ContainsKey(key))
        {
            return m_GeneralFlagDict[key];
        }
        else
        {
            Debug.LogError(key + "keyに対応する値が存在しません");
            return new GeneralFlag();
        }
        
    }

    public void SetValue(string key, GeneralFlag flag) {
        if (m_GeneralFlagDict.ContainsKey(key))
        {
            m_GeneralFlagDict[key] = flag;
        }
        else
        {
            Debug.LogError(key + "keyに対応する値が存在しません");
        }
    }
}

[Serializable]
public class GeneralFlagDictionary : SerializableDictionary<string, GeneralFlag>
{

}