using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataBase : DataBase<GameSaveData>
{
    [SerializeField] public CommonSaveData          m_CommonData;
    [SerializeField] public GameSaveDataObject      m_SaveDataObject;

    public override void Load(int index)
    {
        GameSaveData s = new GameSaveData(m_SaveDataObject);
        s.Load(index.ToString());
    }

    public override void Save(int index)
    {
        m_CurrentSaveData.Save(index.ToString());
        m_SaveDataList[index].Load(index.ToString());
    }

    public void LoadCommon()
    {
        m_CommonData.Load("Common");
    }

    public void SaveCommon() 
    {
        m_CommonData.Save("Common");
    }

    protected void Start()
    {
        InitAllData();
        LoadAllData();
    }

    private void InitAllData()
    {
        m_CurrentSaveData = new GameSaveData(m_SaveDataObject);

        LoadCommon();
        m_SaveDataList.Clear();
        for (int i = 0; i < SAVEDATANUM; i++)
        {
            GameSaveData data = new GameSaveData(m_SaveDataObject);
            m_SaveDataList.Add(data);
        }
        
        Debug.Log($@"<a href=""{Application.persistentDataPath}"">""{Application.persistentDataPath}""</a>");
    }
}

[System.Serializable]
public class GameSaveData : ISaveData
{
    GameSaveDataObject m_SaveDataObject;
    [SerializeField] public PRJ080Data                  m_SimulationData;
    [SerializeField] public List<bool>                  m_PhaseFlag;
    [SerializeField] public StringBoolDictionary        m_EventFlag;
    [SerializeField] public ItemBuggage<ItemList.Param> m_ItemBug;

    public GameSaveData(GameSaveDataObject saveDataObject)
    {
        m_SaveDataObject = saveDataObject;
        m_SimulationData = new PRJ080Data();
        m_PhaseFlag = new List<bool>(new bool[1001]);
        m_EventFlag = new StringBoolDictionary(saveDataObject.m_EventFlag);
        // m_ItemBug   = new ItemBuggage<ItemList.Param>(saveDataObject.m_Preset.m_ItemList.sheets[0].list);
    }

    public void Save(string key)
    {
        // JsonDataManager.Save(m_SimulationData, key + "_Simulation");
        if (m_PhaseFlag != null) JsonDataManager.SaveArray(m_PhaseFlag.ToArray(), key + "_PhaseFlag");

        Dictionary<string, bool> temp = m_EventFlag;
        if (temp != null) JsonDataManager.SaveDict(temp, key + "_EventFlag");


        // m_ItemBug.Save(key + "_ItemBug");
    }

    public void Load(string key)
    {
        m_SimulationData    = JsonDataManager.Load<PRJ080Data>(key + "_Simulation");

        bool[] b = JsonDataManager.LoadArray<bool>(key + "_PhaseFlag");
        if (b != null) m_PhaseFlag = new List<bool>(b);

        Dictionary<string, bool> temp         = JsonDataManager.LoadDict<string,bool>(key + "_EventFlag");
        if(temp != null)m_EventFlag.CopyFrom(temp);

        // m_ItemBug.Load(key + "_ItemBug");
    }

    public void Init()
    {
        if (m_SaveDataObject == null)
        {
            Debug.LogError("セーブデータオブジェクトが事前に設定されていないため初期化できません");
            return;
        }

        m_SimulationData.Init();
        m_EventFlag = new StringBoolDictionary(m_SaveDataObject.m_EventFlag);
        // m_ItemBug.Init();
    }

    public void FirstDay() {
        if (m_SaveDataObject == null)
        {
            Debug.LogError("セーブデータオブジェクトが事前に設定されていないため初期化できません");
            return;
        }

        m_SimulationData.m_HeroineInfo.Init();
        m_SimulationData.m_Day      = 1;
        m_SimulationData.m_Time     = PRJ080Data.Time.Morning;
        m_SimulationData.m_Field    = PRJ080Data.Field.Field00;

        // イベント情報初期化
        m_EventFlag = new StringBoolDictionary(m_SaveDataObject.m_EventFlag);
    }

}

[System.Serializable]
public class CommonSaveData : ISaveData
{

    public int test = 0;

    public void Save(string key)
    {
        JsonDataManager.Save(test, key + "_test");
    }

    public void Load(string key)
    {
        test = JsonDataManager.Load<int>(key + "_test");
    }

    public void Init() {
    
    }
}


[System.Serializable]
public class PRJ080Data
{
    [System.Serializable]
    public enum Date
    {
        Mon = 0,
        Tue = 1,
        Wed = 2,
        Thurs = 3,
        Fri = 4,
        Sat = 5,
        Sun = 6
    }

    [System.Serializable]
    public enum Time
    {
        Morning,
        AfterNoon,
        Evening,
        Night,
        End
    }

    [System.Serializable]
    public enum Field
    {
        DebugField = 0,
        Field00         ,        // 自宅
        Field01         ,        // 駅前
        Field02         ,        // 自宅前
        Field03         ,　　　　// コンビニ
        Field04         ,　　　　// 公園
        Field05         ,　　　　// 裏路地
    }

    [System.Serializable]
    public enum Act {
        Act000,
        Act001,
        Act002,
        Act003,
        Act004,
        Act005,
        Act006,
        Act007,
        Act008,
        Act009,
        Act010,
        Act011,
        Act012,
        Act013,
        Act014,
        Act015,
        Act016,
        Act017,
        Act018,
        Act019,
    }

    [System.Serializable]
    public enum Conv
    {
        Conv000 = 0,
        Conv001,
        Conv002,
        Conv003,
        Conv004,
        Conv005,
        Conv006,
        Conv007,
        Conv008,
        Conv009,
        Conv010,
        Conv011,
        Conv012,
        Conv013,
        Conv014,
        Conv015,
        Conv016,
        Conv017,
        Conv018,
        Conv019,
        Conv020,
        Conv021,
        Conv022,
        Conv023,
        Conv024,
        Conv025,
        Conv026,
        Conv027,
        Conv028,
        Conv029,
        Conv030,
        Conv031,
        Conv032,
        Conv033,
        Conv034,
        Conv035,
        Conv036,
        Conv037,
        Conv038,
        Conv039,
        Conv040,
        Conv041,
        Conv042,
        Conv043,
        Conv044,
        Conv045,
        Conv046,
        Conv047,
        Conv048,
        Conv049,
        Conv050,
        Conv051,
        Conv052,
        Conv053,
        Conv054,
        Conv055,
        Conv056,
        Conv057,
        Conv058,
        Conv059,
        Conv060,
        Conv061,
        Conv062,
        Conv063,
        Conv064,
        Conv065,
        Conv066,
        Conv067,
        Conv068,
        Conv069,
        Conv070,
        Conv071,
        Conv072,
        Conv073,
        Conv074,
        Conv075,
        Conv076,
        Conv077,
        Conv078,
        Conv079,
        Conv080,
        Conv081,
        Conv082,
        Conv083,
        Conv084,
        Conv085,
        Conv086,
        Conv087,
        Conv088,
        Conv089,
        Conv090,
        Conv091,
        Conv092,
        Conv093,
        Conv094,
        Conv095,
        Conv096,
        Conv097,
        Conv098,
    }

    [System.Serializable]
    public struct PlayerInfo 
    {
        [SerializeField] public int     m_Money;
        [SerializeField] public Param   m_HP;
        [SerializeField] public Param   m_Stress;
        [SerializeField] public Param   m_MovePoint;
        [SerializeField] public Param   m_WorkEfficiency;
        [SerializeField] public Param   m_SlaveryPoint;

        public void Init()
        {
            m_Money = 0;
            m_HP.value = m_HP.max_value = 100;
            m_Stress.value = 0;
            m_Stress.max_value = 100;
            m_SlaveryPoint.value = m_SlaveryPoint.max_value = 0;
            m_MovePoint.value = m_MovePoint.max_value = 3;

            m_WorkEfficiency.value = 1;
            m_WorkEfficiency.max_value = 10;
        }
    }

    [System.Serializable]
    public struct CharInfo
    {
        public int      m_Money;
        public int      m_MeetCount;

        public Param    m_Confidence;       // 信用度
        public Param 　 m_Alertness;        // 警戒度
        public Param    m_Dependence;       // 依存度
        public Param    m_Normalcy;         // 正常性バイアス

        public Param    m_Favorite;
        public Param    m_Stress;
        public Param    m_Sadism;
        public Param    m_Masochism;

        public void Init() {
            m_Money = 0;
            m_MeetCount = 0;

            m_Favorite.value = 0;
            m_Favorite.max_value = 10;

            m_Stress.value = 0;
            m_Stress.max_value = 100;

            m_Sadism.value = 0;
            m_Sadism.max_value = 100;

            m_Masochism.value = 0;
            m_Masochism.max_value = 100;


            m_Confidence.value = 0;
            m_Confidence.max_value = 100;

            m_Alertness.value = 0;
            m_Alertness.max_value = 100;

            m_Dependence.value = 50;
            m_Dependence.max_value = 100;

            m_Normalcy.value = 100;
            m_Normalcy.max_value = 100;

        }
    }

    [System.Serializable]
    public struct ActInfo {
        public string m_ActName;
        public int    m_NeedPoint;
        public bool   m_NotEqualFungusName;
    }

    [System.Serializable]
    public struct Param
    {
        [SerializeField] public int value;
        [SerializeField] public int max_value;

        public void  Cal(int v) {
            value += v;
            value = Mathf.Clamp(value, 0, max_value);
        }

        public void Recovery() {
            value = max_value;
        }

        public bool IsMax() {
            if (value >= max_value) return true;
            else return false;
        }
    }

    [System.Serializable]
    public enum Party {
        Solo,
        Meet,
        Partner,
    }

    public Date GetDate(int day)
    {
        return (Date)((day-1) % 7);
    }

    public PRJ080Data() {
        m_PlayTime = 0.0;
        m_Loadable  = false;
        m_Day       = 1;
        m_Time      = Time.Morning;
        m_Field     = Field.DebugField;
        m_Party     = Party.Solo;
    }

    public void Init() {
        m_PlayTime      = 0.0;
        m_Loadable      = true;

        m_Day           = 1;
        m_Time          = Time.Morning;
        m_Field         = Field.Field00;

        m_PlayerInfo.Init();
        m_HeroineInfo.Init();

        m_Party         = Party.Solo;
    }

    // システム情報
    [SerializeField] public double      m_PlayTime;
    [SerializeField] public bool        m_Loadable;

    [SerializeField] public int         m_Day;
    [SerializeField] public Time        m_Time;
    [SerializeField] public Field       m_Field;
    [SerializeField] public PlayerInfo  m_PlayerInfo;
    [SerializeField] public CharInfo    m_HeroineInfo;
    [SerializeField] public Party       m_Party;
}