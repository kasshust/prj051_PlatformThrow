using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class SceneWindow : MonoBehaviour
{

    public Vector2 activePosition;
    public Vector2 deactivePosition;

    public bool b_Variablesize = false;
    public Vector3 activeSize;
    public Vector3 deactiveSize;

    RectTransform m_RectTransform;
    Tween         m_Tween;

    public void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_RectTransform.DOAnchorPos(deactivePosition, 0.0f).SetEase(Ease.OutCubic);
        if (b_Variablesize) m_RectTransform.DOSizeDelta(deactiveSize, 0.0f).SetEase(Ease.OutCubic);
    }

    public void Tween(bool active, float speed = 0.5f,float delay = 0.0f) {
        if(m_RectTransform == null) m_RectTransform = GetComponent<RectTransform>();

        if (active)
        {
            m_RectTransform.DOAnchorPos(activePosition, speed).SetEase(Ease.OutCubic).SetDelay(delay);
            if (b_Variablesize) m_RectTransform.DOSizeDelta(activeSize, speed).SetEase(Ease.OutCubic).SetDelay(0.1f + delay);
        }
        else
        {
            m_RectTransform.DOAnchorPos(deactivePosition, speed).SetEase(Ease.OutCubic).SetDelay(delay);
            if (b_Variablesize) m_RectTransform.DOSizeDelta(deactiveSize, speed).SetEase(Ease.OutCubic).SetDelay(0.1f + delay);
        }
    }
}
