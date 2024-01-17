using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTransition : Transition
{

    public GameObject imageObj;
    private Image     image;

    protected override void Awake()
    {
        base.Awake();
        image = imageObj.GetComponent<Image>();
    }

    protected override void InProcess()
    {
        var color = image.color;
        var alpha = ((float)this.currentTime) / ((float)(this.inTime));
        color.a = alpha;
        image.color = color;
    }

    protected override void OutProcess()
    {
        var color = image.color;
        var alpha = 1.0f - ((float)this.currentTime) / ((float)(this.outTime));
        color.a = alpha;
        image.color = color;
    }

    protected override void Finish()
    {
    }
    protected override void Interval()
    {
        
    }
}
