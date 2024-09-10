using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
public abstract class PlatformCharacterAI : ActionGameCharacterAI
{
    [SerializeField, Foldout("PlatformCharacterAI Param")]
    protected PlatformCharacterBase m_CharacterBase;

    [SerializeField, ReadOnly, Foldout("PlatformCharacterAI Param")]
    protected Vector2 m_RockOnVec;

    protected float GetTargetDistance() {
        if (m_CharacterBase.m_RockOnTarget == null) return float.NaN;
        m_RockOnVec = m_CharacterBase.m_RockOnTarget.transform.position - m_CharacterBase.transform.position;
        return m_RockOnVec.magnitude;
    }

}
