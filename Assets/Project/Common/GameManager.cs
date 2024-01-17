using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : SingletonMonoBehaviourFast<GameManager>
{
    [SerializeField] public bool m_Debug;
    [SerializeField] public GamePreset m_Preset;
    [SerializeField] public bool m_IsPlaying;

    override protected void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (m_IsPlaying) {
            GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayTime += Time.deltaTime;
        }
        
    }

    public bool CheckEventFlag(string key)
    {
        if (GameDataBase.Instance.m_CurrentSaveData.m_EventFlag.ContainsKey(key))
        {
            return GameDataBase.Instance.m_CurrentSaveData.m_EventFlag[key];
        }
        else
        {
            Debug.LogError("キーが不正");
            return false;
        }
    }

    public void SetEventFlag(string key, bool b)
    {
        if (GameDataBase.Instance.m_CurrentSaveData.m_EventFlag.ContainsKey(key))
        {
            GameDataBase.Instance.m_CurrentSaveData.m_EventFlag[key] = b;
        }
        else
        {
            Debug.LogError("キーが不正");
        }
    }

    public void StartStory()
    {
        m_IsPlaying = true;
        GameDataBase.Instance.m_CurrentSaveData.Init();
        StoryManager.Instance.LoadPhase(StoryPhase.Phase000, 0);
    }

    public void ReturnFirstDay() {
        GameDataBase.Instance.m_CurrentSaveData.FirstDay();
        StoryManager.Instance.LoadPhase(StoryPhase.Phase400, 0);
    }

    public void ContinueGame(int index)
    {
        m_IsPlaying = true; 
        GameDataBase.Instance.SelectSaveData(index);
        TransitionManager.Instance.changeScene("ActScene080");
    }

    public void LoadTitle()
    {
        m_IsPlaying = false;
        TransitionManager.Instance.changeScene("TitleScene");
    }

    public void LoadAct() {
        TransitionManager.Instance.changeScene("ActScene080");
    }

    public void LoadActRapid()
    {
        TransitionManager.Instance.changeScene("ActScene080", "Rapid");
    }

    public void LoadTimeProcess() {
        StoryManager.Instance.LoadPhase(StoryPhase.Phase500, "Rapid");
    }

    public void LoadConversation()
    {
        TransitionManager.Instance.changeScene("ConversationScene");
    }

    public void LoadEvent(StoryPhase phase)
    {
        StoryManager.Instance.LoadPhase(phase);
    }

    public void LoadRecorderEvent(StoryPhase phase)
    {
        StoryManager.Instance.LoadPhase(phase,0,PhasePlayState.Recoeder);
    }

    public void LoadRecorder() {
        TransitionManager.Instance.changeScene("RecorderScene");
    }

    public bool UseItem(ScrollerDataItem080 item) {
        try
        {
            ItemList.Param p = GameManager.Instance.m_Preset.m_ItemList.sheets[0].list[item.m_id];

            switch (p.typeA)
            {
                case 0:
                    NoticeController.Instance.SendNotice(item.m_name + "を使用した", NoticePanel.SendColor.Normal);
                    GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_HP.Cal(p.valueA);
                    GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress.Cal(p.valueB);

                    if (p.valueA != 0) {
                        int v = Mathf.Abs(p.valueA);
                        if (p.valueA > 0 ) NoticeController.Instance.SendNotice("体力が" + v.ToString() + "回復した", NoticePanel.SendColor.Blue);
                        else              NoticeController.Instance.SendNotice("体力が"  +  v.ToString() + "減少した", NoticePanel.SendColor.Red);
                    }

                    if (p.valueB != 0){
                        int v = Mathf.Abs(p.valueB);
                        if (p.valueB < 0) NoticeController.Instance.SendNotice("ストレスが" + v.ToString() + "減少した", NoticePanel.SendColor.Blue);
                        else NoticeController.Instance.SendNotice("ストレスが" + v.ToString() + "増加した", NoticePanel.SendColor.Red);
                    }
                    GameDataBase.Instance.m_CurrentSaveData.m_ItemBug.sub(item.m_id);
                    
                    
                    return true;
                case 1:
                    NoticeController.Instance.SendNotice(item.m_name + "を使用した", NoticePanel.SendColor.Normal);
                    GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_WorkEfficiency.Cal(p.valueA);

                    if (p.valueA != 0)
                    {
                        int v = Mathf.Abs(p.valueA);
                        if (p.valueA > 0) NoticeController.Instance.SendNotice("労働効率が" + v.ToString() + "上昇した", NoticePanel.SendColor.Blue);
                        else NoticeController.Instance.SendNotice("労働効率が" + v.ToString() + "減少した", NoticePanel.SendColor.Red);
                    }
                    GameDataBase.Instance.m_CurrentSaveData.m_ItemBug.sub(item.m_id);
                    
                    
                    return true;
                case 2:

                    NoticeController.Instance.SendNotice("このアイテムは使用できません");
                    
                    
                    return false;
                default:
                    NoticeController.Instance.SendNotice("このアイテムを使用できません");
                    return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }

    public void GiveItem(ScrollerDataItem080 item) {

        try
        {
            NoticeController.Instance.SendNotice(item.m_name + "を渡した", NoticePanel.SendColor.Normal);

            ItemList.Param p = GameManager.Instance.m_Preset.m_ItemList.sheets[0].list[item.m_id];

            switch (p.typeB)
            {
                case 0:


                    break;
                case 1:
                    GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_HeroineInfo.m_Confidence.Cal(p.valueD);
                    GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_HeroineInfo.m_Alertness.Cal(p.valueE);
                    GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_HeroineInfo.m_Dependence.Cal(p.valueF);
                    GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_HeroineInfo.m_Normalcy.Cal(p.valueG);

                    if (p.valueD != 0)
                    {
                        int v = Mathf.Abs(p.valueD);
                        if (p.valueD > 0) NoticeController.Instance.SendNotice("信用度が" + v.ToString() + "増加した", NoticePanel.SendColor.Blue);
                        else NoticeController.Instance.SendNotice("信用度が" + v.ToString() + "減少した", NoticePanel.SendColor.Red);
                    }

                    if (p.valueE != 0)
                    {
                        int v = Mathf.Abs(p.valueE);
                        if (p.valueE > 0) NoticeController.Instance.SendNotice("警戒度が" + v.ToString() + "増加した", NoticePanel.SendColor.Red);
                        else NoticeController.Instance.SendNotice("警戒度が" + v.ToString() + "減少した", NoticePanel.SendColor.Blue);
                    }

                    if (p.valueF != 0)
                    {
                        int v = Mathf.Abs(p.valueF);
                        if (p.valueF > 0) NoticeController.Instance.SendNotice("依存度が" + v.ToString() + "増加した", NoticePanel.SendColor.Normal);
                        else NoticeController.Instance.SendNotice("依存度が" + v.ToString() + "減少した", NoticePanel.SendColor.Normal);
                    }

                    if (p.valueG != 0)
                    {
                        int v = Mathf.Abs(p.valueG);
                        if (p.valueG > 0) NoticeController.Instance.SendNotice("正常性が" + v.ToString() + "増加した", NoticePanel.SendColor.Normal);
                        else NoticeController.Instance.SendNotice("正常性が" + v.ToString() + "減少した", NoticePanel.SendColor.Normal);
                    }
                    break;
                default:
                    break;
            }

            GameDataBase.Instance.m_CurrentSaveData.m_ItemBug.sub(item.m_id);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    // Event条件
    public StoryPhase CheckMainEvent() {
        int day = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Day;

        if (day == 10) {
            return StoryPhase.PhaseDefault;
        }

        if (day == 20)
        {
            return StoryPhase.PhaseDefault;
        }

        return StoryPhase.PhaseDefault;
    }

    public StoryPhase CheckSubEvent()
    {
        return StoryPhase.PhaseDefault;
    }
}
