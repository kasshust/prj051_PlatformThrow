using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataBase<T> : SingletonMonoBehaviourFast<DataBase<T>> where T : ISaveData
{
    protected int SAVEDATANUM = 10;

    [SerializeField] public T m_CurrentSaveData;
    [SerializeField, ReadOnly] public List<T> m_SaveDataList;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);

        m_SaveDataList = new List<T>();
    }

    public void CreateDummySaveAllData()
    {
        for (int i = 0; i < SAVEDATANUM; i++) {
            m_SaveDataList[i].Save(i.ToString());
        }
        Debug.Log("ダミー用データの作成");
    }

    public void LoadAllData()
    {
        for (int i = 0; i < SAVEDATANUM; i++)
        {
            m_SaveDataList[i].Load(i.ToString());
        }
    }

    public void SelectSaveData(int index)
    {
        m_CurrentSaveData = m_SaveDataList[index];
    }

    abstract public void Load(int index);
    abstract public void Save(int index);
}

public interface ISaveData{
    public void Save(string key);
    public void Load(string key);
    public void Init();                 //　はじめからの初期化処理
}

[System.Serializable]
public class KData<T> {
    [SerializeField] public string   m_Key;
    [SerializeField] public T        m_Data;
    public KData(string key, T data) {
        m_Key = key;
        m_Data = data;
    }
}