using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemDescription : MonoBehaviour
{
    public Image image;
    public Text  text;

    public void SetImage(Sprite _spr) {
        image.sprite = _spr;
    }

    public void SetText(string _text)
    {
        text.text = _text;
    }
}
