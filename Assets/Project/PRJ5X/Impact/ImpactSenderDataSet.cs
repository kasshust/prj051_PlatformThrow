using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImpactSenderDataSet", menuName = "ActionGameObject/AnimationEvent/ImpactSenderDataSet")]
public class ImpactSenderDataSet : ScriptableObject
{
    [SerializeField] public bool IsChild                           = false;
    [SerializeField, Range(0, 20)] public int ParentNum = 0;
    [SerializeField] public bool AdjustDirXDirection               = true;
    [SerializeField] public bool AdjustPosXDirection               = true;
    [SerializeField] private PlatformActionManager.AttackInfo attackinfo;
    [SerializeField] private PlatformActionManager.BaseSenderInfo basesenderInfo;

    public PlatformActionManager.AttackInfo         AttackInfo { get => attackinfo; set => attackinfo = value; }
    public PlatformActionManager.BaseSenderInfo     BaseSenderInfo { get => basesenderInfo; set => basesenderInfo = value; }
}

