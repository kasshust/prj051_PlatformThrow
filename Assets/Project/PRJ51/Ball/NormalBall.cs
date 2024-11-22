using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedBlueGames.Tools;
using System;

public abstract class NormalBall : CatchableBall
{
    [SerializeField] protected Rigidbody2D m_Rigidbody2D;
    public float                           m_CircleRadius;
    public LayerMask                       m_CharMask;
    protected RaycastHit2D                 m_Hit;

    protected abstract void HitCollisionTarget();
    protected abstract void HitWall(Collision2D collision);
    protected abstract void HitGround(Collision2D collision);
    protected virtual void  Step() { }

    public event Action CatchedAction;

    private void Update()
    {
        if (m_State == BallState.Bound || m_State == BallState.Throwed)
        {
            m_Hit = CollisionCheck();
            if (m_Hit)
            {
                HitCollisionTarget();
            }
        }

        Step();
    }
    
    protected RaycastHit2D CollisionCheck()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, m_CircleRadius, Vector2.zero, Mathf.Infinity, m_CharMask);
        DebugUtility.DrawCircle(transform.position, m_CircleRadius, Color.red, 10);
        return hit;
    }

    public override bool IsCatchable()
    {
        if (m_State == BallState.Carried) return false;
        return true;
    }

    override public void Carried()
    {
        transform.position = m_Parent.GetHandPosition();
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.angularVelocity = 0.0f;
        m_Rigidbody2D.Sleep();
    }

    override public void Catched(ICatcher parent)
    {
        m_Parent = parent;
        m_State = BallState.Carried;
        if (CatchedAction != null) CatchedAction.Invoke();
    }

    override public void Throwed(ref ThrowProperty throwProperty)
    {
        m_Rigidbody2D.velocity = throwProperty.Velocity;
        m_AttackInfo.AttackSet = throwProperty.AttackSet;

        m_State         = BallState.Throwed;
        m_Parent        = null;
        m_Rigidbody2D.WakeUp();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitTerrain(collision);
    }

    private void HitTerrain(Collision2D collision)
    {
        float angle = Vector2.SignedAngle(Vector2.up, collision.contacts[0].normal.normalized);
        if (Mathf.Abs(angle) < 20.0f) HitGround(collision);
        else                          HitWall(collision);
    }



}
