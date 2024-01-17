using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

public abstract class PlatformEnemyAI : PlatformCharacterAI
{
    public enum AIType
    {
        Type00,
        Type01,
        Type02,
        Type03,
        Type04,
        Type05,
        Type06,
        Type07,
        Type08,
        Type09,
    }

    public enum ActionType
    {
        None,
        Action
    }

    [SerializeField, Foldout("PlatformEnemyAI Param")]
    protected AIType m_AIType;

    [SerializeField, Foldout("PlatformEnemyAI Param")]
    protected LayerMask m_PlayerMask;

    [SerializeField, ReadOnly, Foldout("PlatformEnemyAI Param")]
    protected ActionType m_ActionType;

    protected void RockOnTarget()
    {
        if (m_CharacterBase.m_RockOnTarget == null) m_CharacterBase.RockOnTarget(transform.position, 50.0f, m_PlayerMask);
    }

    protected bool IsRigid()
    {
        return m_CharacterBase.GetRigid();
    }

    protected bool IsMotion()
    {
        return m_CharacterBase.GetIsMotion();
    }
}
