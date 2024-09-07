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
        Entry,                      // �V�[���J�n
        Default,                    // �f�t�H���g���
        Item,                       // �A�C�e������
        Event,                      // GUI����z�[�� 
        Save,                       // �Z�[�u
        Option,                     // �I�v�V����
        Phase,                      // �t�F�[�Y�Ăяo��
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
        return "�ړ�����";
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

    // Inspector�����悤����΂������Y��ɂł���
    private void SetAct(PRJ080Data data)
    {
        List<PRJ080Data.Act> m_ActList = new List<PRJ080Data.Act>();

        // �Z�[�u/�A�C�e��/�ړ�
        // m_ActList.Add(PRJ080Data.Act.Act000);
        m_ActList.Add(PRJ080Data.Act.Act001);
        m_ActList.Add(PRJ080Data.Act.Act002);

        //�@����
        // �J��/�x�e
        if (data.m_Field == PRJ080Data.Field.Field00)
        {
            m_ActList.Add(PRJ080Data.Act.Act004);
            m_ActList.Add(PRJ080Data.Act.Act005);
        }

        //�@�w�O
        // �o�
        if (data.m_Field == PRJ080Data.Field.Field01)
        {
            m_ActList.Add(PRJ080Data.Act.Act007);
            m_ActList.Add(PRJ080Data.Act.Act010);
            m_ActList.Add(PRJ080Data.Act.Act011);
        }

        // ����O
        if (data.m_Field == PRJ080Data.Field.Field02)
        {
            m_ActList.Add(PRJ080Data.Act.Act008);
        }

        // �R���r�j�O
        if (data.m_Field == PRJ080Data.Field.Field03)
        {
            m_ActList.Add(PRJ080Data.Act.Act006);
        }

        // ����
        if (data.m_Field == PRJ080Data.Field.Field04)
        {
            m_ActList.Add(PRJ080Data.Act.Act009);
            m_ActList.Add(PRJ080Data.Act.Act008);
        }

        // ����
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
            // �C�x���g
            /*
            if (Context.CheckEvent()) {
                Context.stateMachine.SendEvent((int)StateEventId.Phase);
                return;
            }
            */

            // FlowChart�ɏ��𑗂�
            int day = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Day;
            PRJ080Data.Time time = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Time;
            Context.m_FlowChart.SetIntegerVariable("Day", day);
            Context.m_FlowChart.SetIntegerVariable("Time", (int)time);
            Context.m_FlowChart.SetBooleanVariable("Meetable", Context.Meetable());

            // Act�{�^�����쐬
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
            Debug.LogWarning(eventName + " : ���̃C�x���g�͑��݂��܂���");
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
            SetShortMessageValue("�s���|�C���g������܂���");
            FireEvent("ActFault");
            return false;
        }
        return true;
    }

    public bool Meetable() {
        // int day = PRJ080DataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Day;
        // PRJ080Data.Time time = PRJ080DataBase.Instance.m_CurrentSaveData.m_SimulationData.m_Time;

        // �[���͖�����
        // if (time == PRJ080Data.Time.Evening) return true;
        // ����Vacation�������Ă���ꍇ�̂�
        // if (time == PRJ080Data.Time.Morning && PRJ080Manager.Instance.CheckEventFlag("Vacation")) return true;

        return false;
    }

    public bool HpCheck(int needHp)
    {
        int point = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_HP.value;
        if (point <= needHp)
        {
            SetShortMessageValue("�̗͂�����܂���");
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
                SetShortMessageValue("�s���|�C���g������܂���");
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
        NoticeController.Instance.SendNotice("�̗͂�" + hp.ToString() +  "�񕜂���", NoticePanel.SendColor.Blue);

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress.Cal(stress);
        NoticeController.Instance.SendNotice("�X�g���X��" + stress.ToString() + "��������", NoticePanel.SendColor.Blue);
    }

    public void GetMoney() {
        int ef      = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_WorkEfficiency.value;
        int st      = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress.value;
        int st_max  = GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress.max_value;

        double stressLev = (double)st / (double)st_max;
        double decreaseRate = Math.Pow(stressLev, 2.0f);

        double mulRate = Math.Pow(1.2, ef-1);

        Debug.Log("�X�g���X���x��:" + stressLev.ToString() + " ������:" + decreaseRate.ToString() + " ������:" + mulRate.ToString());

        int sum = (int)(10000 * mulRate * (1.0f-decreaseRate));

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Money += sum;
        NoticeController.Instance.SendNotice("�g�p�\����"+sum.ToString()+ "�~������", NoticePanel.SendColor.Blue);

        int hp = 25;
        int stress = 10;

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_HP.value -= hp;
        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_MovePoint.value--;
        NoticeController.Instance.SendNotice("�̗͂�" + hp.ToString() + "�����", NoticePanel.SendColor.Red);

        GameDataBase.Instance.m_CurrentSaveData.m_SimulationData.m_PlayerInfo.m_Stress.Cal(stress);
        NoticeController.Instance.SendNotice("�X�g���X��" + stress.ToString() + "��������", NoticePanel.SendColor.Red);
    }


    //UI�������܂�

    // �A�C�e���֘A
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

    // �d�����Ă�
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

            NoticeController.Instance.SendNotice("������" + item.m_buyPrice.ToString() + "�~�����", NoticePanel.SendColor.Red);
            NoticeController.Instance.SendNotice(item.m_name + "����ɓ��ꂽ", NoticePanel.SendColor.Blue);

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
                Debug.LogError("�s���ȑ���");
                break;
            case ItemOperation.Bug:
                FireEvent("Item000");
                break;
            case ItemOperation.Shop:
                FireEvent("Item001");
                break;
            case ItemOperation.Give:
                Debug.LogError("Give�͂ł��Ȃ��͂�");
                break;
            default:
                Debug.LogError("�s���ȑ���");
                break;
        }
        */
    }

    public void PlayBGAnimation(string animationName)
    {
        m_BGAnimator.Play(animationName);
    }

}
