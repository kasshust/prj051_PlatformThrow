using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item080Button : MonoBehaviour
{
    EventTrigger trigger;
    ScrollerDataItem080 m_DataInfo;

    void Start()
    {
        trigger = gameObject.AddComponent<EventTrigger>();

        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;

        entry.callback.AddListener((data) => {
            if (this.GetComponent<Button>().interactable)
            {
                if (Item080Panel.Instance != null) Item080Panel.Instance.SetItemDescription(m_DataInfo);
            }
        });

        var entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => {
            if (this.GetComponent<Button>().interactable)
            {
                if (Item080Panel.Instance != null) Item080Panel.Instance.ResetItemDescription();
            }
        });


        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry2);
    }

    public void ItemOperation()
    {
        Item080Panel.Instance.ItemOperationCall(m_DataInfo);
    }

    public void SetItemDataInfo(ScrollerDataItem080 data)
    {
        m_DataInfo = data;
    }
}
