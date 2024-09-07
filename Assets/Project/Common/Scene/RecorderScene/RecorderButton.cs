using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecorderButton : MonoBehaviour
{
    [SerializeField] StoryPhase m_Phase;
    [SerializeField] Text m_Text;
    [SerializeField] Button m_Button;

    public void Init(StoryPhase p, string Name)
    {
        m_Phase = p;
        m_Text.text = Name;
        if (!GameDataBase.Instance.m_CurrentSaveData.m_PhaseFlag[(int)p]) { m_Button.interactable = false; }
    }

    public void LoadRecord() {
        RecorderSceneManager.Instance.LoadEvent(m_Phase);
    }
}
