using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

public  class Record080CellView : EnhancedScrollerCellView
{
    [SerializeField]
    public Text m_nameTextUI;

    [SerializeField]
    public Text m_timeTextUI;

    [SerializeField]
    public Text m_DayTextUI;

    [SerializeField]
    public Button m_button;

    private int     m_id;
    private bool    m_loadable;

    public virtual void SetData(Scroller080SaveData data)
    {
        Record080Button script = m_button.GetComponent<Record080Button>();

        // セーブデータが存在しない
        if (data.m_SaveDataInfo.m_SimulationData == null)
        {
            m_nameTextUI.text   = "No Data";
            m_timeTextUI.text   = "--";
            m_DayTextUI.text    = "--";

            script.m_index          = data.m_id;
            script.m_loadable       = false;
            script.op               = data.m_Operation;
            script.m_Controller     = data.m_Controller;
        }
        else {

            m_id                = data.m_id;
            m_loadable          = data.m_SaveDataInfo.m_SimulationData.m_Loadable;
            PRJ080Data.Field f  = data.m_SaveDataInfo.m_SimulationData.m_Field;

            if (m_loadable)
            {
                // m_nameTextUI.text = GameManager.Instance.m_Preset.m_FieldSprite[f].m_Name;
                m_timeTextUI.text = GameMainSystem.Instance.SecondsToHMS(data.m_SaveDataInfo.m_SimulationData.m_PlayTime);
                m_DayTextUI.text = data.m_SaveDataInfo.m_SimulationData.m_Day.ToString() + "日目";
            }
            else
            {
                m_nameTextUI.text   = "No Data";
                m_timeTextUI.text   = "--";
                m_DayTextUI.text    = "--";
            }

            script.m_index      = data.m_id;
            script.m_loadable   = data.m_SaveDataInfo.m_SimulationData.m_Loadable;
            script.op           = data.m_Operation;
            script.m_Controller = data.m_Controller;
        }
        
    }

    override public void RefreshCellView()
    {
    }
}
