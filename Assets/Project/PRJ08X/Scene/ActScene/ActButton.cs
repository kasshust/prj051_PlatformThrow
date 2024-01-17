using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActButton : MonoBehaviour
{
    [SerializeField] Text           m_Text;
    [SerializeField] Text           m_NeedPoint;

    [SerializeField] private FMODUnity.EventReference m_ClickSound;

    PRJ080Data.Act m_Act;
    public void Init(PRJ080Data.Act act) {
        m_Act = act;

        PRJ080Data.ActInfo d = GameManager.Instance.m_Preset.m_ActDict[act];
        m_Text.text = d.m_ActName;
        if (d.m_NeedPoint > 0) {
            m_NeedPoint.text = d.m_NeedPoint.ToString();
        } else {
            m_NeedPoint.text = "-";
        }
    }

    public void PlayClickSound() {
        FMODUnity.RuntimeManager.PlayOneShot(m_ClickSound, transform.position);
    }

    public void CallFungus() {
        if (ActManager.Instance.IsState<ActManager.DefaultState>()) {
            ActManager.Instance.CallFungusEvent((int)m_Act);
        }
    }
}


