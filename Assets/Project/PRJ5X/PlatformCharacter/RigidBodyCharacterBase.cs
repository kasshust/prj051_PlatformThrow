using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RigidBodyEnemyBase : PlatformEnemyBase
{
    [SerializeField]
    protected Rigidbody2D m_Rigidbody2D;

    private void FixedUpdate()
    {
        if (m_ZeroGravity) m_Rigidbody2D.gravityScale = 0.0f;
        else m_Rigidbody2D.gravityScale = 1.0f;
    }

    override protected void EndHitStop()
    {
        m_Rigidbody2D.Resume(gameObject);
    }

    public override void StartHitStop(float time)
    {
        base.StartHitStop(time);
        m_Rigidbody2D.Pause(gameObject);
        m_Rigidbody2D.velocity *= 0.0f;
    }

    public override void ForceSetVelocityX(float velocityX)
    {
        m_Rigidbody2D.velocity.Set(velocityX, m_Rigidbody2D.velocity.y);
    }

    public override void ForceSetVelocityY(float velocityY)
    {
        m_Rigidbody2D.velocity.Set(m_Rigidbody2D.velocity.x, velocityY);
    }

    public override void ForceSetVelocity(Vector2 velocity)
    {
        m_Rigidbody2D.velocity = velocity;
    }

    override public void ForceSetAngularVelocity(float velocity)
    {
        m_Rigidbody2D.angularVelocity = velocity;
    }

    public override void AddVelocity(Vector2 velocity)
    {
        m_Rigidbody2D.velocity += velocity;
    }

}
