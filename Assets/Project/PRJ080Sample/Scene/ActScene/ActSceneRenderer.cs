using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class ActSceneRenderer : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_Renderer;

    public void SetSprite(PRJ080Data.Field field, PRJ080Data.Time t) {


        GamePreset.FieldInfo info = GameManager.Instance.m_Preset.m_FieldSprite[field];

        if (info.m_Sprites.Count < (int)t) Debug.LogError(field.ToString() + " : スプライトが設定されていません");
        else m_Renderer.sprite = info.m_Sprites[(int)t];
    }
}