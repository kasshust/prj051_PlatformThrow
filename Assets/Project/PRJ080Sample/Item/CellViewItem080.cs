using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;



public class CellViewItem080 : EnhancedScrollerCellView
{
    [SerializeField]
    public Text m_nameTextUI;

    [SerializeField]
    public Text m_numTextUI;

    [SerializeField]
    protected Item080Button m_button;
    ScrollerDataItem080 m_DataInfo;

    public void SetData(ScrollerDataItem080 data)
    {
        m_DataInfo = data;

        m_nameTextUI.text = m_DataInfo.m_name;

        var num = GameDataBase.Instance.m_CurrentSaveData.m_ItemBug.getItemNum(m_DataInfo.m_id);
        m_numTextUI.text = num.ToString();

        m_button.SetItemDataInfo(m_DataInfo);
    }

    override public void RefreshCellView()
    {
        var num = GameDataBase.Instance.m_CurrentSaveData.m_ItemBug.getItemNum(m_DataInfo.m_id);
        m_numTextUI.text = num.ToString();
    }
}

