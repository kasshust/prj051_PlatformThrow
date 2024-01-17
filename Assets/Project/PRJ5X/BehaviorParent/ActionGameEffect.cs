using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionGameEffect : ActionGameBehavior<ActionGameEffect> {
    public FactoryManager.EFFECT m_EffectEnum;

    [ReadOnly] public Vector2 m_HitPoint;
    [ReadOnly] public Vector2 m_Direction;
    [ReadOnly] public float   m_Strength;

    public void SetEffectInfo(Vector2 dir, Vector2 hitpoint, float impactStrength = 0.0f) {
        m_Direction = dir;
        m_HitPoint = hitpoint;
        m_Strength = impactStrength;
        UpdateEffectInfo();
    }
    
    // ここで情報を更新しないとCreate時に適応されないので注意
    virtual protected void UpdateEffectInfo() {
    
    }

    protected override void AdditonalCreateInit()
    {
        InitStatus();
    }

    private void InitStatus() {
    }

    override public void ReleaseObject()
    {
        m_FactoryManager.ReleaseObject(m_EffectEnum, this.gameObject);
    }
}
