using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record080ScrollerController : RecordScrollerController<Scroller080SaveData, Record080CellView>
{
    public override void CreateElements()
    {
        _data = new List<Scroller080SaveData>();
        List<GameSaveData> l = GameDataBase.Instance.m_SaveDataList;

        for (int i = 0; i < l.Count; i++)
        {
            int id              = i;
            _data.Add(new Scroller080SaveData
            {
                m_Controller    = this,
                m_id            = id,
                m_SaveDataInfo  = l[i],
                m_Operation     = m_Operation,
            });
        }

        m_scroller.cellViewVisibilityChanged += view =>
        {
            if (view.active)
            {
                // ÉZÉãÇ™ï\é¶èÛë‘Ç…Ç»Ç¡ÇΩéûÇÃèàóù
                var cellView = (Record080CellView)view;

                int id          = _data[view.dataIndex].m_id;
                cellView.SetData(new Scroller080SaveData
                {
                    m_Controller    = this,
                    m_id            = id,
                    m_SaveDataInfo  = l[id],
                    m_Operation     = m_Operation,
                });
            }
        };
    }
}

public class Scroller080SaveData
{
    public Record080ScrollerController  m_Controller;
    public GameSaveData               m_SaveDataInfo;
    public int                          m_id;
    public Record080ScrollerController.Operation m_Operation;
}
