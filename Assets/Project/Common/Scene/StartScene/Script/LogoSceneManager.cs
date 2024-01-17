using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogoSceneManager : MonoBehaviour
{

    public GameObject imageObj;
    public float fadeInTime   = 1f;
    public float fadeWaitTime = 1f;
    public float fadeOutTime  = 1f;

    private float currentRemainTime;
    private Image image;

    private int state = 0;

    // Use this for initialization
    void Start()
    {
        // 初期化
        currentRemainTime = fadeInTime;
        image = imageObj.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float alpha;
        var   color = image.color;
        switch (state) {
            case 0:
                currentRemainTime -= Time.deltaTime;

                alpha = 1.0f - (float)(currentRemainTime / fadeInTime);
                color.a = alpha;
                image.color = color;

                if (currentRemainTime <= 0f)
                {
                    currentRemainTime = fadeWaitTime;
                    state++;
                }

                break;

            case 1:

                currentRemainTime -= Time.deltaTime;

                if (currentRemainTime <= 0f)
                {
                    currentRemainTime = fadeOutTime;
                    state++;
                }
                break;
            case 2:
                currentRemainTime -= Time.deltaTime;

                alpha = currentRemainTime / fadeOutTime;
                color.a = alpha;
                image.color = color;

                if (currentRemainTime <= 0f)
                {
                    state++;
                }
                break;
            case 3:
                
                TransitionManager tm = TransitionManager.Instance;
                tm.changeScene("TitleScene",0);
                
                break;
        }
    }
}
