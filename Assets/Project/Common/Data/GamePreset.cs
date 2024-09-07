using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
[CreateAssetMenu(fileName = "PRJ080Preset", menuName = "PRJ_080/PRJ080Preset")]
public class GamePreset : ScriptableObject
{
    [SerializeField] public ItemList              m_ItemList;
    [SerializeField] public ShopList              m_ShopList;
}

