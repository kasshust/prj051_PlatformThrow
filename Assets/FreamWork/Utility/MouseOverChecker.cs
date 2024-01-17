using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseOverChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public UnityEvent EnterEvent;
    public UnityEvent ExitEvent;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnterEvent.Invoke();
        Debug.Log("マウスが侵入");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ExitEvent.Invoke();
        Debug.Log("マウスがでてった");
    }

}
