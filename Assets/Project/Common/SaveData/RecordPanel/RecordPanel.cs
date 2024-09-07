using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecordPanel<Controller> : MonoBehaviour where Controller : ScrollerController
{
    public GameObject Scroller;
    private Controller scrollerController;
    private CanvasGroup scrollerCanvasGroup;

    [SerializeField]
    private int selectedID;

    private void Awake()
    {
        if (Scroller == null) Debug.LogWarning("Scroller is null");

        scrollerCanvasGroup = Scroller.GetComponent<CanvasGroup>();
        scrollerController = GetComponent<Controller>();
        selectedID = -1;
    }

    private void setButtonActive(bool active)
    {
        scrollerCanvasGroup.interactable = active;
        scrollerCanvasGroup.blocksRaycasts = active;
    }


    private void reCreateScroller(bool reCreate)
    {
        if (reCreate) scrollerController.reCreateEnhancedScroller();
    }
}
