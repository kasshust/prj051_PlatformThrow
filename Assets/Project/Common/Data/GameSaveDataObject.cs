using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "PRJ080SaveDataObject", menuName = "PRJ_080/PRJ080SaveDataObject")]
public class GameSaveDataObject : ScriptableObject
{
    [SerializeField] public GamePreset         m_Preset;
    [SerializeField] public StringBoolDictionary m_EventFlag;
}