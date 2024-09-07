using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniSearchableEnumAttribute;

[System.Serializable]
public struct RecordablePhase
{

    [SearchableEnum] public StoryPhase Phase;
    public string Name;
}

[Serializable]
[CreateAssetMenu(fileName = "RecordablePhase", menuName = "PRJ_080/RecordablePhase")]
public class RecordablePhaseObject : ScriptableObject
{
    
    public List<RecordablePhase> m_RecordablePhase;
}
