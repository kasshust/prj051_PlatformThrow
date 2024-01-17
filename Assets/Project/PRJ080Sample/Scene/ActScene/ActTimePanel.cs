using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActTimePanel : MonoBehaviour
{
    [SerializeField] Text Day;
    [SerializeField] Text Date;
    [SerializeField] Text Time;
    [SerializeField] Text Field;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        int day              = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Day;
        PRJ080Data.Date date = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.GetDate(day);
        PRJ080Data.Time time = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Time;
        PRJ080Data.Field field = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Field;

        Day.text = "Day" + day.ToString();
        Date.text = date.ToString();
        Time.text = Time2String(time);
        Field.text = GameManager.Instance.m_Preset.m_FieldSprite[field].m_Name;
    }

    private string Time2String(PRJ080Data.Time t) {
        switch (t)
        {
            case PRJ080Data.Time.Morning:
                return "’©";
            case PRJ080Data.Time.AfterNoon:
                return "’‹";
            case PRJ080Data.Time.Evening:
                return "—[";
            case PRJ080Data.Time.Night:
                return "–é";
            default:
                return "–³";
        }
    } 
}
