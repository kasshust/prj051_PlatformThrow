using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TimeProcessManager : SingletonMonoBehaviourFast<TimeProcessManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    int             m_PreDay;
    PRJ080Data.Time m_PreTime;

    int             m_NextDay;
    PRJ080Data.Time m_NextTime;

    [SerializeField] LoopScrollRect m_ScrollRect;

    public Transform       m_CurrentTimeTrans;
    public List<Transform> m_TimeTransform;

    private void Start()
    {
        m_PreDay    = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Day;
        m_PreTime   = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Time;


        InitUI(m_PreDay, m_PreTime);
    }


    private bool CheckRecorder()
    {
        return StoryManager.Instance.GetState() == PhasePlayState.Recoeder;
    }

    private void InitUI(int preday, PRJ080Data.Time preTime) {
        m_CurrentTimeTrans.position = m_TimeTransform[(int)preTime].position;
    }

    public void MoveUI() {
        ProcessTime();
        m_NextDay = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Day;
        m_NextTime = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Time;

        Vector3 pos = m_TimeTransform[(int)m_NextTime].position;
        m_CurrentTimeTrans.DOMove(pos, 1.0f);

        // if(m_NextDay != m_PreDay) 
        m_ScrollRect.SrollToCellWithinTime(m_NextDay, 0.1f);
    }

    public void StartGame()
    {
        LoadAct();
    }

    public void SetEventFlag(string key, bool b)
    {
        if (CheckRecorder()) return;
        GameManager.Instance.SetEventFlag(key, b);
    }

    public void LoadRecorderScene() {
        GameManager.Instance.LoadRecorder();
    }


    public void LoadAct()
    {
        if (CheckRecorder()) {
            LoadRecorderScene();
            return;
        }
        GameManager.Instance.LoadActRapid();
    }

    public void LoadTimeProcess()
    {
        if (CheckRecorder())
        {
            LoadRecorderScene();
            return;
        }
        GameManager.instance.LoadTimeProcess();
    }

    public void RecoveryPoint()
    {
        if (CheckRecorder()) return;

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_MovePoint.Recovery();
    }

    public void LoadTitle()
    {
        if (CheckRecorder())
        {
            LoadRecorderScene();
            return;
        }

        GameManager.Instance.LoadTitle();
    }

    public void LoadConversation()
    {
        if (CheckRecorder())
        {
            LoadRecorderScene();
            return;
        }

        GameManager.instance.LoadConversation();
    }



    public void ReturnFirstDay() {
        if (CheckRecorder())
        {
            LoadRecorderScene();
            return;
        }

        GameManager.Instance.ReturnFirstDay();
    }

    // ŽŸ‚ÌTime
    public void ProcessTime()
    {
        PRJ080Data data = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData;
        data.m_Time++;
        if (data.m_Time == PRJ080Data.Time.End)
        {
            data.m_Time = PRJ080Data.Time.Morning;
            data.m_Day++;
        }
    }
}
