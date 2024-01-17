using IceMilkTea.Core;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;

public class Item080Panel : SingletonMonoBehaviourFast<Item080Panel>
{
    private ImtStateMachine<Item080Panel> stateMachine;
    public enum StateEventId
    {
        Idle,
        Open,
        Close,
    }

    [SerializeField] private Flowchart      m_FlowChart;

    [SerializeField] private MapCommonPanel             m_MapCommonPanel;
    [SerializeField] private Item080ScrollerController  scrollerController;
    [SerializeField] private CanvasGroup                scrollerCanvasGroup;
    [SerializeField] private Button                     m_ReturnButton;

    [SerializeField] public ItemDescription m_ItemDescription;
    // public SelectHomeItemOperationPanel m_ItemOperationPanel;

    private ScrollerDataItem080 m_HandleItemDataInfo;       //　操作対象のアイテム情報
    public ScrollerDataItem080 GetHandleItemDataInfo() { return m_HandleItemDataInfo; }

    public void ItemOperationCall(ScrollerDataItem080 data) {
        m_HandleItemDataInfo = data;
        if (m_FlowChart != null) {
            FireEvent("ItemOperationCall");
        }
        else Debug.LogError("ItemPanelにFlowChartが設定されていません");
    }

    private bool HasEvent(string eventName)
    {
        return m_FlowChart.HasBlock(eventName);
    }

    private bool FireEvent(string eventName)
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

    private void Awake()
    {
        SetUpState();
        // DebugItemMax();
    }

    /*
    private void DebugItemMax()
    {
        ItemBuggage<ItemList.Param> bug = PRJ080DataBase.Instance.m_CurrentSaveData.m_ItemBug;
        bug.setMaxItem(1);
    }
    */


    private void SetUpState()
    {
        stateMachine = new ImtStateMachine<Item080Panel>(this);
        stateMachine.AddTransition<SelectState, OpenOperationState>((int)StateEventId.Open);
        stateMachine.AddTransition<OpenOperationState, SelectState>((int)StateEventId.Idle);
        stateMachine.SetStartState<SelectState>();
    }

    private void Start()
    {
        stateMachine.Update();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    #region 外部からの呼び出し
    public void Open(ItemBuggage<ItemList.Param> bug)
    {
        scrollerController.InitEnhancedScroller(bug);
        m_MapCommonPanel.Open();
    }

    public void Open(List<ShopList.Param> shop)
    {
        scrollerController.InitEnhancedScroller(shop);
        m_MapCommonPanel.Open();
    }

    public void Close()
    {
        scrollerController.ClearEnhancedScroller();
        m_MapCommonPanel.Close();
    }
    #endregion

    public class SelectState : ImtStateMachine<Item080Panel>.State
    {
        protected override void Enter()
        {
            // Context.m_ItemOperationPanel.Close();
            Context.SetActive(true);
        }
        protected override void Update()
        {
        }
        protected override void Exit()
        {
            Context.SetActive(false);
        }
    }

    public class OpenOperationState : ImtStateMachine<Item080Panel>.State
    {
        protected override void Enter()
        {
            Context.SetActive(false);
        }

        protected override void Update()
        {
            bool rightPush = Input.GetMouseButtonDown(1);
            if (rightPush) Context.stateMachine.SendEvent((int)StateEventId.Close);
        }

        protected override void Exit()
        {
            Context.SetActive(true);
        }
    }

    public void SetActive(bool active)
    {
        scrollerCanvasGroup.interactable = active;
        scrollerCanvasGroup.blocksRaycasts = active;
        m_ReturnButton.interactable = active;
    }

    public void SetItemDescription(ScrollerDataItem080 data)
    {
        string descript = GameManager.Instance.m_Preset.m_ItemList.sheets[0].list[data.m_id].description;
        m_ItemDescription.SetText(descript);
    }

    public void ResetItemDescription()
    {
        m_ItemDescription.SetText("");
    }

    /*
    public void UseItem(ScrollerDataItem080 datainfo) {
        PRJ080DataBase.Instance.m_CurrentSaveData.m_ItemBug.sub(datainfo.m_id);
        // scrollerController.Reload();
        scrollerController.RecreateEnhancedScroller();
    }
    */

    public void ReCreateScroller()
    {
        scrollerController.RecreateEnhancedScroller();
    }
}
