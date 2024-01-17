using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderSceneManager : SingletonMonoBehaviourFast<RecorderSceneManager>
{

    [SerializeField] RecordablePhaseObject m_Object;
    [SerializeField] GameObject       m_ScrollContent;
    [SerializeField] RecorderButton   m_RecorderButton;



    override protected void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init() {
        PreparePanel();
    }

    private void PreparePanel() {
        foreach (RecordablePhase p in m_Object.m_RecordablePhase)
        {
            RecorderButton g = Instantiate(m_RecorderButton, m_ScrollContent.transform);
            g.Init(p.Phase, p.Name);
        }    
    }

    public void ReturnActScene() {
        GameManager.Instance.LoadAct();
    }

    public void LoadEvent(StoryPhase p) {
        GameManager.Instance.LoadRecorderEvent(p);
    }
}
