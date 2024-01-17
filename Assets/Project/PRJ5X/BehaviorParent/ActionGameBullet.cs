using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionGameBullet : ActionGameBehavior<ActionGameBullet>
{

    [ReadOnly]  public Vector2  m_Accel;
    [ReadOnly]  public Vector2  m_Velocity;

    [SerializeField]
    protected PlatformActionManager.AttackInfo m_AttackInfo;

    protected float m_Time;
    public float    m_Endtime = 1.4f;

    public FactoryManager.BALL m_BallEnum;

    [ReadOnly]
    public PlatformActionManager.ReplyInfo m_ReplyInfo;

    override public ActionGameBullet CreateInit()
    {
        m_Time = 0.0f;
        return this;
    }

    override public void ReleaseObject() {
        m_FactoryManager.ReleaseObject(m_BallEnum , this.gameObject);   
    }

    public void SetVelocity(float amount, Vector2 direction)
    {
        m_Velocity = amount * direction * Time.deltaTime;
    }

    protected bool CheckEndTime() {
        m_Time += Time.deltaTime;
        if (m_Time > m_Endtime) return true;
        else return false;
    }

    protected virtual void CalculatePos()
    {
        transform.Translate(m_Velocity);
    }

    protected virtual void CalculateFixedPos()
    {
        transform.Translate(m_Velocity / Time.deltaTime);
    }

    protected virtual void SendImpact(RaycastHit2D hit, Vector2 dir)
    {
        if (hit.collider.TryGetComponent(out BehaviorImpactReceiver receiver))
        {
            receiver.ReceiveImpactGetReply(ref m_AttackInfo, hit, null, ref m_ReplyInfo , this.gameObject);
        }
    }
}
