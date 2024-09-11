using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEnemy : EnemyBase
{
    [SerializeField] Rigidbody2D m_RigidBody;
    float maxVelocity = 2.0f;

    override protected void Wake()
    {
        base.Wake();
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();
        UpdateAI();
    }

    public override void ChatchImpactReply(ref PlatformActionManager.ReplyInfo replyInfo)
    {
    }

    public override void Damage(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, GameObject g = null)
    {
        m_RigidBody.velocity = attackInfo.Direction * attackInfo.ImpactValue; 
    }

    public override void Dead()
    {
    }

    public override void Move(Vector2 value)
    {
        m_RigidBody.velocity += value;

        if (m_RigidBody.velocity.magnitude >= maxVelocity) m_RigidBody.velocity = m_RigidBody.velocity.normalized * maxVelocity;
    }

    public override void ReleaseObject()
    {
    }
}
