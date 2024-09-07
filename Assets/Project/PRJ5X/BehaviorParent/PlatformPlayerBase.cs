using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pixeye.Unity;

public abstract class PlatformPlayerBase : PlatformCharacterBase
{
    [SerializeField, Foldout("PlatformPlayerBase Param")]            public FactoryManager.PLAYER    m_PlayerEnum;
    [SerializeField, ReadOnly, Foldout("PlatformPlayerBase Param")]  protected int                   m_SavedRockOnXDir;
    [SerializeField, ReadOnly, Foldout("PlatformPlayerBase Param")]  protected float                 m_SpeedMultiplier = 1.0f;
    // [SerializeField, ReadOnly, Foldout("PlatformPlayerBase Param")]  public bool m_Strengthen;
    
    protected override void Update()
    {
        base.Update();
    }

    public override ActionGameCharacterBase CreateInit()
    {
        base.CreateInit();
        // m_SpriteObj.InitAllParts();
        return this;
    }

    protected override void ReRockOnTarget()
    {
        RockOn();
    }

    virtual public void RockOn() {

        if (m_RockOnTarget == null)
        {
            RockOnTarget(
                transform.position,
                10,
                LayerMask.GetMask("enemy"),
                true
            );
        }

        SetControlState(ControlState.RockOn);
        m_SavedRockOnXDir = m_XDirection;
    }

    public void RockOff()
    {
        m_RockOnTarget = null;
        SetControlState(ControlState.Default);
    }

    public void SetControlState(ControlState state)
    {
        m_ControlState = state;
    }

    private void UpdateControlState()
    {
        if (m_RockOnTarget != null) m_ControlState = ControlState.RockOn;
        else m_ControlState = ControlState.Default;
    }

    override public void ReleaseObject()
    {
        // m_SpriteObj.InitAllParts();
        m_FactoryManager.ReleaseObject(m_PlayerEnum, this.gameObject);
    }
}
