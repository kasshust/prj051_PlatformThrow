using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkObject : MonoBehaviour
{
    void Start()
    {
        // �t���X�^���J�����O������������
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.bounds = new Bounds(renderer.bounds.center, new Vector3(1000, 1000, 1000));
    }

}
