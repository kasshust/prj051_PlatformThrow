using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TitleEnum;

public class TitleSceneWindow : MonoBehaviour
{

    public GameObject TitleSceneManager;
    public Status Status_Set;

    public Vector2 activePosition;
    public Vector2 deactivePosition;

    public bool isShake = false;
    public Vector2 shakeSize;
    public float shakeSpeed = 1.0f;

    RectTransform trans;


    private bool tweening = false;


    public void Tween() {
        Status status = TitleSceneManager.GetComponent<TitleManager>().getStatus();
        trans = GetComponent<RectTransform>();

        tweening = true;

        if (status == Status_Set)
        {
            var tween = trans.DOAnchorPos(activePosition, 0.5f).SetEase(Ease.OutCubic);

            tween.OnComplete(() => {
                tweening = false;
            });
        }
        else
        {
            var tween = trans.DOAnchorPos(deactivePosition, 0.5f).SetEase(Ease.OutCubic);

            tween.OnComplete(() => {
                tweening = false;
            });
        }
    }

    public void Start()
    {
        trans = GetComponent<RectTransform>();
    }

    public void Update()
    {
        if(TitleSceneManager != null) { 
            Status status = TitleSceneManager.GetComponent<TitleManager>().getStatus();
            if (status == Status_Set)
            {
                if (isShake && !tweening)
                {
                    trans.anchoredPosition = activePosition + shakeSize * new Vector2(Mathf.Cos(Time.time * shakeSpeed), Mathf.Cos(Time.time*shakeSpeed));
                }
            }
        }
    }
}
