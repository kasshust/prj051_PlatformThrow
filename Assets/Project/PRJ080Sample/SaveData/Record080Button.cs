using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record080Button : MonoBehaviour
{
    [ReadOnly] public Record080ScrollerController m_Controller;
    [ReadOnly] public int m_index;
    [ReadOnly] public bool m_loadable;
    [ReadOnly] public Record080ScrollerController.Operation op;

    [SerializeField] private FMODUnity.EventReference m_LoadSound;
    [SerializeField] private FMODUnity.EventReference m_ErrorSound;

    public void fire()
    {
        switch (op)
        {
            case Record080ScrollerController.Operation.Save:
                CallSaveMessage(m_index);
                break;
            case Record080ScrollerController.Operation.Load:
                Load(m_index);
                break;
        }
    }

    private void CallSaveMessage(int index)
    {
        if (ActManager.Instance != null)
        {
            if (ActManager.Instance.IsState<ActManager.SaveState>()) {
                ActManager.Instance.m_SaveTargetIndex = index;
                ActManager.Instance.FireEvent("Act000_0");
            }
        }
    }

    private void Load(int index)
    {
        if (m_loadable)
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_LoadSound, transform.position);
            if (GameManager.Instance != null) { GameManager.Instance.ContinueGame(index); }
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_ErrorSound, transform.position);
            Debug.Log("正常なデータではないためロードできません");
        }
        m_Controller.reCreateEnhancedScroller();        //　表示を更新
    }
}
