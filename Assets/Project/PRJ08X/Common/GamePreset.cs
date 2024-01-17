using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
[CreateAssetMenu(fileName = "PRJ080Preset", menuName = "PRJ_080/PRJ080Preset")]
public class GamePreset : ScriptableObject
{
    [Serializable]
    public struct FieldInfo {
        public string       m_Name; 
        public List<Sprite> m_Sprites;
    }
    [SerializeField] public FieldSpriteDictionary m_FieldSprite;
    [SerializeField] public ActDictionary         m_ActDict;
    [SerializeField] public Dictionary<PRJ080Data.Conv, string> m_ConvDict;

    [SerializeField] public ItemList              m_ItemList;
    [SerializeField] public ShopList              m_ShopList;
}

[Serializable]
public class FieldSpriteDictionary : SerializableDictionary<PRJ080Data.Field, GamePreset.FieldInfo>
{

}

[Serializable]
public class ActDictionary : SerializableDictionary<PRJ080Data.Act, PRJ080Data.ActInfo>
{

}