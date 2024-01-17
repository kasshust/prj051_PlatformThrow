using EnhancedUI.EnhancedScroller;
using UnityEngine;
using System.Collections.Generic;

public class Item080ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller             m_scroller;
    public EnhancedScrollerCellView     m_cellPrefab;
    private List<ScrollerDataItem080>      _data;

    [ReadOnly] public ItemBuggage<ItemList.Param>          m_Bug;
    [ReadOnly] public List<ShopList.Param>                 m_Shop;

    private float m_ScrollPosition = 0.0f;

    private void Init()
    {
        m_Bug   = null;
        m_Shop  = null;
        m_scroller.cellViewVisibilityChanged = null;
    }

    public void Reload()
    {
        m_scroller.Delegate = this;
        if (m_scroller.ScrollSize != 0) m_scroller.ReloadData(m_scroller.ScrollPosition / m_scroller.ScrollSize);
        else m_scroller.ReloadData();
    }

    public void ClearEnhancedScroller() {
        m_scroller.ClearAll();
        m_Bug       = null;
        m_Shop      = null;
        m_scroller.cellViewVisibilityChanged = null;
    }

    // 未使用
    public void RecreateEnhancedScroller()
    {
        if(m_Bug != null)       createElements(m_Bug);
        else if(m_Shop != null) createElements(m_Shop);

        m_scroller.Delegate = this;
        if (m_scroller.ScrollSize != 0) m_scroller.ReloadData(m_scroller.ScrollPosition / m_scroller.ScrollSize);
        else m_scroller.ReloadData();
    }

    public void InitEnhancedScroller(ItemBuggage<ItemList.Param> bug)
    {
        if (bug == null) return;
        Init();

        createElements(bug);
        Reload();
    }

    public void InitEnhancedScroller(List<ShopList.Param> shoplist)
    {
        if (shoplist == null) return;
        Init();
        createElements(shoplist);
        Reload();
    }

    public void createElements(ItemBuggage<ItemList.Param> bug)
    {
        m_Bug = bug;
        List<int> l = bug.buggage;
        List<ItemList.Param> itemList = GameManager.Instance.m_Preset.m_ItemList.sheets[0].list;

        _data = new List<ScrollerDataItem080>();
        for (int i = 0; i < l.Count; i++){
            int id                  = l[i];
            string name             = itemList[i].name;
            var num                 = bug.getItemNum(id);
            _data.Add(new ScrollerDataItem080 { m_name = name, m_id = id, m_num = num});
        };

        // スクロールからはみ出た後に再表示する処理
        m_scroller.cellViewVisibilityChanged += view =>
        {
            if (view.active){
                var cellView        = (CellViewItem080)view;
                int id              = _data[view.dataIndex].m_id;
                string name         = itemList[id].name;
                var num             = bug.getItemNum(id);
                cellView.SetData(new ScrollerDataItem080 { m_name = name, m_id = id, m_num = num});
            }
        };
    }

    public void createElements(List<ShopList.Param> shoplist)
    {
        m_Shop = shoplist;
        List<ItemList.Param> itemList = GameManager.Instance.m_Preset.m_ItemList.sheets[0].list;

        _data = new List<ScrollerDataItem080>();
        for (int i = 0; i < shoplist.Count; i++)
        {
            int itemId = shoplist[i].ItemId;

            if (itemId < 0 || itemId >= itemList.Count) {
                Debug.LogError("アイテムIDが不正です");
                return;
            }

            string name = itemList[itemId].name;
            var price = shoplist[i].buyPrice;
            _data.Add(new ScrollerDataItem080 { m_name = name, m_id = itemId, m_buyPrice = price});
        };

        // スクロールからはみ出た後に再表示する処理
        m_scroller.cellViewVisibilityChanged += view =>
        {
            if (view.active)
            {
                var cellView = (CellViewItem080)view;
                int itemId = _data[view.dataIndex].m_id;
                string name = itemList[itemId].name;
                var price = shoplist[view.dataIndex].buyPrice;
                cellView.SetData(new ScrollerDataItem080 { m_name = name, m_id = itemId, m_buyPrice = price });
            }
        };
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 20f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        return scroller.GetCellView(m_cellPrefab);
    }

    public void UpdateEnhancedScroller()
    {
        m_scroller.RefreshActiveCellViews();
    }
}
