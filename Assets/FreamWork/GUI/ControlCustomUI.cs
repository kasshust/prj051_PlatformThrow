using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlCustomUI : MonoBehaviour
{
    // Scroll main texture based on time

    public Vector2 scrollSpeed;
    
    [SerializeField]
    private GameObject obj;
    private Material   mat;



    void Start()
    {
        mat = obj.GetComponent<CanvasRenderer>().GetMaterial();
    }

    void Update()
    {
        
        Vector2 offset = Time.time * scrollSpeed;
        if(mat != null) mat.SetTextureOffset("_MainTex", offset);

        
    }
}
