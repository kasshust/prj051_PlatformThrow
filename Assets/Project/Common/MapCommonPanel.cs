using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System;

[Serializable]
public class MapCommonPanel : MonoBehaviour
{
    public enum OpenPattern { 
        Pattern00,
        Pattern01,
        Pattern02,
        Pattern03,
        Pattern04,
    }
    public enum ClosePattern
    {
        Pattern00,
        Pattern01,
        Pattern02,
        Pattern03,
        Pattern04,
    }

    private Tween MoveTween;

    [Header("FadeIn")]

    [Tooltip("" +
    "Fadeパターンの説明\n\n" +
    "Pattern00 : \n\n" +
    "Pattern01 : \n\n" +
    "Pattern02 : \n\n" +
    "Pattern03 : \n\n" +
    "Pattern04 : \n\n" +
    "" +
    "")]

    public OpenPattern  m_OpenPattern;
    public Vector3 FadeINPos;
    [Range(0, 5)]
    public float   FadeInTime = 1.0f;
    public Ease    FadeInEase = Ease.OutCubic;

    [Header("FadeOut")]

    public ClosePattern m_ClosePattern;
    public Vector3 FadeOutPos;
    [Range(0, 5)]
    public float   FadeOutTime = 1.0f;
    public Ease    FadeOutEase = Ease.OutCubic;


    public void Open() {
        switch (m_OpenPattern)
        {
            case OpenPattern.Pattern00:

                RectTransform trans = GetComponent<RectTransform>();
                MoveTween = trans.DOAnchorPos(FadeINPos, FadeInTime).SetEase(FadeInEase);
                break;
            
            
            default:
                break;
        }
    }

    public void Close()
    {
        switch (m_ClosePattern)
        {
            case ClosePattern.Pattern00:
                RectTransform trans = GetComponent<RectTransform>();
                MoveTween = trans.DOAnchorPos(FadeOutPos, FadeInTime).SetEase(FadeOutEase);

                break;


            default:
                break;
        }
    }

}
