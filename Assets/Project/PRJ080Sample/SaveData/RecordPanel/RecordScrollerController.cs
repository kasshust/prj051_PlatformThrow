using EnhancedUI.EnhancedScroller;
using UnityEngine;
using System.Collections.Generic;

public abstract class RecordScrollerController<ScrollerData,CellView> : ScrollerController, IEnhancedScrollerDelegate where CellView : EnhancedScrollerCellView
{
    public EnhancedScroller         m_scroller;
    public CellView                 m_cellPrefab;
    protected List<ScrollerData>      _data;

    public enum Operation
    {
        Save,
        Load
    }

    [SerializeField]
    protected Operation m_Operation;

    private float m_ScrollPosition = 0.0f;

    private void Start()
    {
        InitEnhancedScroller();
    }
    public void InitEnhancedScroller()
    {
        CreateElements();
        m_scroller.Delegate = this;
        m_scroller.ReloadData();
    }

    override public void reCreateEnhancedScroller()
    {
        CreateElements();
        m_scroller.Delegate = this;
        m_scroller.ReloadData(m_scroller.ScrollPosition / m_scroller.ScrollSize);
    }
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 40f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        return scroller.GetCellView(m_cellPrefab);
    }

    public void updateEnhancedScroller()
    {
        m_scroller.RefreshActiveCellViews();
    }

    public abstract void CreateElements();
}
