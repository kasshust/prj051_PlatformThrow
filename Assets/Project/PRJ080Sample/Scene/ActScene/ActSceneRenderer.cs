using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class ActSceneRenderer : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_Renderer;

    public void SetSprite(PRJ080Data.Field field, PRJ080Data.Time t) {


        GamePreset.FieldInfo info = GameManager.Instance.m_Preset.m_FieldSprite[field];

        if (info.m_Sprites.Count < (int)t) Debug.LogError(field.ToString() + " : �X�v���C�g���ݒ肳��Ă��܂���");
        else m_Renderer.sprite = info.m_Sprites[(int)t];
    }
}