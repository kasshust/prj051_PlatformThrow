using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionInitializer : MonoBehaviour
{
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingLayerName = "transition";
    }

    private void Update()
    {
        if (GetComponent<Canvas>().worldCamera == null) {
            GetComponent<Canvas>().worldCamera = Camera.main;
            GetComponent<Canvas>().sortingLayerName = "transition";
        }
    }

}
