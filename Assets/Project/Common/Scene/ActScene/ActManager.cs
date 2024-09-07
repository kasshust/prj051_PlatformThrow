using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Pixeye.Unity;
using IceMilkTea.Core;
using System;

public class ActManager : SingletonMonoBehaviourFast<ActManager>
{
    [SerializeField, Foldout("PreSetting")] public Flowchart        m_FlowChart;


    
    [SerializeField, Foldout("PreSetting UI Panel")] public ActSceneRenderer     m_Renderer;
    [SerializeField, Foldout("PreSetting UI Panel")] Record080ScrollerController m_Controller;
    [SerializeField, Foldout("PreSetting UI Panel")] public MapCommonPanel   m_SavePanel;
    [SerializeField, Foldout("PreSetting UI Panel")] public ActPanel         m_ActPanel;
    [SerializeField, Foldout("PreSetting UI Panel")] public MapCommonPanel   m_ButtonPanel;
    [SerializeField, Foldout("PreSetting UI Panel")] public ActTimePanel     m_ActTimePanel;
    [SerializeField, Foldout("PreSetting UI Panel")] public Item080Panel     m_ItemPanel;
    [SerializeField, Foldout("PreSetting UI Panel")] public MapCommonPanel   m_PlayerStatusPanel;
    [SerializeField, Foldout("PreSetting UI Panel")] public MapCommonPanel   m_OptionPanel;
    [SerializeField, Foldout("PreSetting UI Panel")] public Animator         m_BGAnimator;

    public int m_SaveTargetIndex = 0;
    private StoryPhase m_CallPhase;

    [SerializeField] private FMODUnity.EventReference m_UseSound;

    private ImtStateMachine<ActManager> stateMachine;
    public bool IsState<T>() where T : ImtStateMachine<ActManager>.State { return stateMachine.IsCurrentState<T>(); }

    public enum StateEventId
    {
        Entry,                      // シーン開始
        Default,                    // デフォルト状態
        Item,                       // アイテム操作
        Event,                      // GUI操作ホーム 
        Save,                       // セーブ
        Option,                     // オプション
        Phase,                      // フェーズ呼び出し
    }

    override protected void Awake()
    {
        base.Awake();
        SetUpState();
    }

    private void SetUpState()
    {
        stateMachine = new ImtStateMachine<ActManager>(this);
        stateMachine.AddTransition<EntryState, DefaultState>((int)StateEventId.Default);

        stateMachine.AddTransition<DefaultState, EventState>((int)StateEventId.Event);
        stateMachine.AddTransition<DefaultState, SaveState>((int)StateEventId.Save);
        stateMachine.AddTransition<DefaultState, PhaseState>((int)StateEventId.Phase);
        stateMachine.AddTransition<DefaultState, ItemState>((int)StateEventId.Item);
        stateMachine.AddTransition<DefaultState, OptionState>((int)StateEventId.Option);

        stateMachine.AddTransition<EventState, DefaultState>((int)StateEventId.Default);
        stateMachine.AddTransition<EventState, ItemState>((int)StateEventId.Item);
        stateMachine.AddTransition<EventState, SaveState>((int)StateEventId.Save);

        stateMachine.AddTransition<OptionState, DefaultState>((int)StateEventId.Default);

        stateMachine.AddTransition<SaveState, DefaultState>((int)StateEventId.Default);
        stateMachine.AddTransition<SaveState, EventState>((int)StateEventId.Event);

        stateMachine.AddTransition<ItemState, DefaultState>((int)StateEventId.Default);
        stateMachine.AddTransition<ItemState, EventState>((int)StateEventId.Event);

        stateMachine.SetStartState<EntryState>();
    }

    private void Start()
    {
        stateMachine.Update();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void OpenItemState()
    {
        stateMachine.SendEvent((int)StateEventId.Item);
    }

    public void OpenEventState()
    {
        stateMachine.SendEvent((int)StateEventId.Event);
    }

    public void OpenDefaultState()
    {
        stateMachine.SendEvent((int)StateEventId.Default);
    }

    public void OpenSaveState()
    {
        stateMachine.SendEvent((int)StateEventId.Save);
    }

    public void OpenOptionState()
    {
        stateMachine.SendEvent((int)StateEventId.Option);
    }

    public void OpenRecordScene()
    {
        FireEvent("Recorder000");
        stateMachine.SendEvent((int)StateEventId.Event);
    }

    public void OpenReturnHome()
    {
        FireEvent("Home000");
        stateMachine.SendEvent((int)StateEventId.Event);
    }

    public void ReturnTitle() {
        GameManager.Instance.LoadTitle();
    }

    public void LoadRecoeder()
    {
        GameManager.Instance.LoadRecorder();
    }

    public void TurnEnd() {

        int day = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Day;
        PRJ080Data.Time time = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Time;
        LoadTimeProcess();
    }

    public void StartConversation() {
        LoadConversation();
    }

    private void LoadTimeProcess()
    {
        stateMachine.SendEvent((int)StateEventId.Event);
        GameManager.instance.LoadTimeProcess();
    }

    private void LoadConversation()
    {
        stateMachine.SendEvent((int)StateEventId.Event);
        GameManager.instance.LoadConversation();
    }

    public void LoadEvent(StoryPhase phase) {
        GameManager.instance.LoadEvent(phase);
    }

    public PRJ080Data.Field GetField() {
        return GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Field;
    }

    List<PRJ080Data.Field> m_MoveList = new List<PRJ080Data.Field>() {
            PRJ080Data.Field.Field00,
            PRJ080Data.Field.Field01,
            PRJ080Data.Field.Field03,
            PRJ080Data.Field.Field04,
            PRJ080Data.Field.Field05,
    };
    public List<PRJ080Data.Field> GetMoveFieldList() {
        
        return m_MoveList;
    }

    public string GetFieldMoveText(PRJ080Data.Field f) {
        return "移動する";
    }




    public void Move(PRJ080Data.Field field)
    {
        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Field = field;
        ChangeField(field);
    }

    public void Move(int i)
    {
        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Field = (PRJ080Data.Field)i;
        ChangeField((PRJ080Data.Field)i);
    }

    public void FireMoveAnimation(PRJ080Data.Field field) 
    {
        FireEvent("MoveAnimation");
        m_FlowChart.SetIntegerVariable("TargetField", (int)field);
    }

    public void MoveMenuCallback()
    {
        OpenDefaultState();
    }

    public bool CheckField(PRJ080Data.Field field)
    {
        return field.ToString() == GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Field.ToString();
    }

    // Inspectorをしようすればもっと綺麗にできる
    private void SetAct(PRJ080Data data)
    {
        List<PRJ080Data.Act> m_ActList = new List<PRJ080Data.Act>();

        // セーブ/アイテム/移動
        // m_ActList.Add(PRJ080Data.Act.Act000);
        m_ActList.Add(PRJ080Data.Act.Act001);
        m_ActList.Add(PRJ080Data.Act.Act002);

        //　自宅
        // 労働/休憩
        if (data.m_Field == PRJ080Data.Field.Field00)
        {
            m_ActList.Add(PRJ080Data.Act.Act004);
            m_ActList.Add(PRJ080Data.Act.Act005);
        }

        //　駅前
        // 出会う
        if (data.m_Field == PRJ080Data.Field.Field01)
        {
            m_ActList.Add(PRJ080Data.Act.Act007);
            m_ActList.Add(PRJ080Data.Act.Act010);
            m_ActList.Add(PRJ080Data.Act.Act011);
        }

        // 自宅前
        if (data.m_Field == PRJ080Data.Field.Field02)
        {
            m_ActList.Add(PRJ080Data.Act.Act008);
        }

        // コンビニ前
        if (data.m_Field == PRJ080Data.Field.Field03)
        {
            m_ActList.Add(PRJ080Data.Act.Act006);
        }

        // 公園
        if (data.m_Field == PRJ080Data.Field.Field04)
        {
            m_ActList.Add(PRJ080Data.Act.Act009);
            m_ActList.Add(PRJ080Data.Act.Act008);
        }

        // 公園
        if (data.m_Field == PRJ080Data.Field.Field05)
        {
            m_ActList.Add(PRJ080Data.Act.Act012);
        }

        m_ActList.Add(PRJ080Data.Act.Act003);

        m_ActPanel.SetAct(m_ActList);
    }

    private bool CheckEvent() {
        if (CheckMainEvent()) return true;
        if (CheckSubEvent()) return true;
        return false;
    }

    private bool CheckMainEvent() {

        StoryPhase p = GameManager.Instance.CheckMainEvent();
        if (p == StoryPhase.PhaseDefault) {
            return false;
        }
        m_CallPhase = p;
        return true;
    }

    private bool CheckSubEvent() {
        StoryPhase p = GameManager.Instance.CheckSubEvent();
        if (p == StoryPhase.PhaseDefault)
        {
            return false;
        }
        m_CallPhase = p;
        return true;
    }


    public class EntryState : ImtStateMachine<ActManager>.State
    {
        protected override void Enter()
        {
            Context.stateMachine.SendEvent((int)StateEventId.Default);
            Context.ChangeField(GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Field);
        }
    }

    public class DefaultState : ImtStateMachine<ActManager>.State
    {
        private void PrepareAct() {
            Context.m_ActPanel.Tween(true);
            Context.m_PlayerStatusPanel.Open();
            Context.m_ActTimePanel.Init();
            Context.SetAct(GameDataBase.Instance.m_CurrentSaveData.m_SimulationData);
        }

        protected override void Enter()
        {
            // イベント
            /*
            if (Context.CheckEvent()) {
                Context.stateMachine.SendEvent((int)StateEventId.Phase);
                return;
            }
            */

            // FlowChartに情報を送る
            int day = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Day;
            PRJ080Data.Time time = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Time;
            Context.m_FlowChart.SetIntegerVariable("Day", day);
            Context.m_FlowChart.SetIntegerVariable("Time", (int)time);
            Context.m_FlowChart.SetBooleanVariable("Meetable", Context.Meetable());

            // Actボタンを作成
            PrepareAct();
            Context.m_ButtonPanel.Open();
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            Context.m_ActPanel.Tween(false);
            Context.m_ButtonPanel.Close();
        }
    }

    public class EventState : ImtStateMachine<ActManager>.State
    {
        protected override void Enter()
        {
            Context.m_PlayerStatusPanel.Close();
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
        }
    }

    public class ItemState : ImtStateMachine<ActManager>.State 
    {
        protected override void Enter()
        {
            Context.m_PlayerStatusPanel.Open();
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            Context.m_ItemPanel.Close();
        }
    }

    public class SaveState : ImtStateMachine<ActManager>.State
    {
        protected override void Enter()
        {
            Context.m_SavePanel.Open();
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            Context.m_SavePanel.Close();
        }
    }

    public class OptionState : ImtStateMachine<ActManager>.State
    {
        protected override void Enter()
        {
            Context.m_OptionPanel.Open();
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            Context.m_OptionPanel.Close();
        }
    }

    public class PhaseState : ImtStateMachine<ActManager>.State
    {
        protected override void Enter()
        {
            Context.LoadEvent(Context.m_CallPhase);
        }

        protected override void Update()
        {
            if (!TransitionManager.instance.isPlayTransition()) {
                Context.LoadEvent(Context.m_CallPhase);
            }
        }

        protected override void Exit()
        {
        }
    }



    public void CallFungusEvent(int act)
    {
        if (FireEvent(((PRJ080Data.Act)act).ToString()))
        {
            OpenEventState();
        }
    }

    public bool HasEvent(string eventName)
    {
        return m_FlowChart.HasBlock(eventName);
    }

    public bool FireEvent(string eventName)
    {
        if (m_FlowChart.HasBlock(eventName))
        {
            m_FlowChart.ExecuteBlock(eventName);
            return true;
        }
        else
        {
            Debug.LogWarning(eventName + " : このイベントは存在しません");
            return false;
        }
    }

    /// <summary>
    /// Act
    /// </summary>
    /// 

    private void SetShortMessageValue(string s)
    {
        m_FlowChart.SetStringVariable("Message", s);
    }


    public bool MovePointCheck() {
        int point = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_MovePoint.value;
        if (point <= 0)
        {
            SetShortMessageValue("行動ポイントが足りません");
            FireEvent("ActFault");
            return false;
        }
        return true;
    }

    public bool Meetable() {
        // int day = PRJ080DataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Day;
        // PRJ080Data.Time time = PRJ080DataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Time;

        // 夕方は無条件
        // if (time == PRJ080Data.Time.Evening) return true;
        // 朝はVacationが立っている場合のみ
        // if (time == PRJ080Data.Time.Morning && PRJ080Manager.Instance.CheckEventFlag("Vacation")) return true;

        return false;
    }

    public bool HpCheck(int needHp)
    {
        int point = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_HP.value;
        if (point <= needHp)
        {
            SetShortMessageValue("体力が足りません");
            FireEvent("ActFault");
            return false;
        }
        return true;
    }



    public void Rest()
    {
        if (MovePointCheck())
        {
            if (!GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_HP.IsMax())
            {
                FireEvent("Rest000");
                GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_MovePoint.value--;
            }
            else {
                SetShortMessageValue("行動ポイントが足りません");
                FireEvent("ActFault");
            }
            
        }
    }

    public void Work() {
        int hp = 25;
        int stress = 10;

        if (MovePointCheck()) {
            if (HpCheck(hp))
            {
                FireEvent("Work000");

            }
        }
    }

    public void Recovery() {
        
        int hp = 50;
        int stress = -10;

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_HP.Cal(hp);
        NoticeController.Instance.SendNotice("体力が" + hp.ToString() +  "回復した", NoticePanel.SendColor.Blue);

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress.Cal(stress);
        NoticeController.Instance.SendNotice("ストレスが" + stress.ToString() + "減少した", NoticePanel.SendColor.Blue);
    }

    public void GetMoney() {
        int ef      = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_WorkEfficiency.value;
        int st      = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress.value;
        int st_max  = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress.max_value;

        double stressLev = (double)st / (double)st_max;
        double decreaseRate = Math.Pow(stressLev, 2.0f);

        double mulRate = Math.Pow(1.2, ef-1);

        Debug.Log("ストレスレベル:" + stressLev.ToString() + " 減少率:" + decreaseRate.ToString() + " 増加率:" + mulRate.ToString());

        int sum = (int)(10000 * mulRate * (1.0f-decreaseRate));

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Money += sum;
        NoticeController.Instance.SendNotice("使用可能金が"+sum.ToString()+ "円増えた", NoticePanel.SendColor.Blue);

        int hp = 25;
        int stress = 10;

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_HP.value -= hp;
        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_MovePoint.value--;
        NoticeController.Instance.SendNotice("体力を" + hp.ToString() + "消費した", NoticePanel.SendColor.Red);

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress.Cal(stress);
        NoticeController.Instance.SendNotice("ストレスが" + stress.ToString() + "増加した", NoticePanel.SendColor.Red);
    }


    //UI処理を含む

    // アイテム関連
    public void OpenItemBug()
    {
        m_ItemPanel.Open(GameDataBase.Instance.m_CurrentSaveData.m_ItemBug);
        OpenItemState();
    }
    public void OpenItemGive()
    {
        m_ItemPanel.Open(GameDataBase.Instance.m_CurrentSaveData.m_ItemBug);
        OpenItemState();
    }
    public void OpenItemShop(int index)
    {
        m_ItemPanel.Open(GameManager.Instance.m_Preset.m_ShopList.sheets[index].list);
        OpenItemState();
    }

    // 重複してる
    public void UseItem()
    {
        ScrollerDataItem080 item = Item080Panel.Instance.GetHandleItemDataInfo();
        bool sucess = GameManager.Instance.UseItem(item);
        if (sucess) FMODUnity.RuntimeManager.PlayOneShot(m_UseSound, transform.position);

        Item080Panel.Instance.ReCreateScroller();
    }

    public void BuyItem()
    {
        int money = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Money;
        ScrollerDataItem080 item = Item080Panel.Instance.GetHandleItemDataInfo();
        if (item.m_buyPrice > money)
        {
            FireEvent("ShopFault");
        }
        else
        {
            GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Money -= item.m_buyPrice;
            GameDataBase.Instance.m_CurrentSaveData.m_ItemBug.add(item.m_id);

            NoticeController.Instance.SendNotice("お金を" + item.m_buyPrice.ToString() + "円消費した", NoticePanel.SendColor.Red);
            NoticeController.Instance.SendNotice(item.m_name + "を手に入れた", NoticePanel.SendColor.Blue);

            FireEvent("ShopSucess");
        }

        Item080Panel.Instance.ReCreateScroller();
    }

    public void ReactiveItemPanel()
    {
        Item080Panel.Instance.SetActive(true);
    }

    public void Save()
    {
        GameDataBase.Instance.Save(m_SaveTargetIndex);
        m_Controller.reCreateEnhancedScroller();
    }

    private void ChangeField(PRJ080Data.Field field)
    {
        PRJ080Data.Time t = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Time;
        m_Renderer.SetSprite(field, t);
    }

    public void OperateItem()
    {

        Item080Panel.Instance.SetActive(false);
        ScrollerDataItem080 data = Item080Panel.Instance.GetHandleItemDataInfo();

        /*
        switch (data.m_Operation)
        {
            case ItemOperation.None:
                Debug.LogError("不正な操作");
                break;
            case ItemOperation.Bug:
                FireEvent("Item000");
                break;
            case ItemOperation.Shop:
                FireEvent("Item001");
                break;
            case ItemOperation.Give:
                Debug.LogError("Giveはできないはず");
                break;
            default:
                Debug.LogError("不正な操作");
                break;
        }
        */
    }

    public void PlayBGAnimation(string animationName)
    {
        m_BGAnimator.Play(animationName);
    }

}
