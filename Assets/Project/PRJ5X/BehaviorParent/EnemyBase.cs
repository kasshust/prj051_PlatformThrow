using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

// 非プラットフォーマーのエネミー
public abstract class EnemyBase : ActionGameCharacterBase
{
    [SerializeField, ReadOnly]    private ActionGameCharacterAI m_CharAI;

    override protected void Wake()
    {
        base.Wake();
        if(m_CharAI == null) m_CharAI = GetComponent<ActionGameCharacterAI>();
    }

    public override ActionGameCharacterBase CreateInit()
    {
        base.CreateInit();
        if (m_CharAI != null) m_CharAI.ResetAI();
        m_Velocity.Set(0.0f, 0.0f, 0.0f);
        return this;
    }

    public void ResumeAI()
    {
        if (m_CharAI != null) m_CharAI.Resume();
    }

    public void ResetStopAI()
    {
        if (m_CharAI != null)
        {
            m_CharAI.ResetAI();
            m_CharAI.Stop();
        }
    }

    protected void UpdateAI()
    {
        if (m_CharAI != null) m_CharAI.UpdateAI();
    }


    //　Force系、必要か？
    public override void ForceSetAngularVelocity(float velocity)
    {
    }

    public override void ForceSetVelocity(Vector2 velocity)
    {
    }

    public override void ForceSetVelocityX(float velocityX)
    {
    }

    public override void ForceSetVelocityY(float velocityY)
    {
    }

    public abstract void Move(Vector2 value);
}
