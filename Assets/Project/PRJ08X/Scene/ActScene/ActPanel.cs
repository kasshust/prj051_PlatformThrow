using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActPanel : MonoBehaviour
{
    SceneWindow m_Window;
    [SerializeField] ActButton   m_ActButton;

    private void Awake()
    {
        m_Window = GetComponent<SceneWindow>();
    }

    public void Tween(bool active, float speed = 0.5f, float delay = 0.0f)
    {
        m_Window.Tween(active, speed, delay);
    }

    public void ClearButton() {
        foreach (Transform n in transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }

    private void CreateButton(PRJ080Data.Act act) {
        ActButton b = Instantiate(m_ActButton, transform);
        b.Init(act);
    }

    public void SetAct(List<PRJ080Data.Act> l)
    {
        ClearButton();
        foreach (var item in l)
        {
            CreateButton(item);
        }
    }
}
