using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayIndexCallback : MonoBehaviour
{
    public Text text;

    void ScrollCellIndex(int idx)
    {
        string name = (idx).ToString();
        if (text != null)
        {
            text.text = "Day " + name;
        }
        gameObject.name = name;
    }
}


