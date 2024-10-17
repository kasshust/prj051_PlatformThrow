using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBomb : NormalBall
{
    [SerializeField] private FMODUnity.EventReference m_ExplosionSound;
    [SerializeField] ImpactSenderDataSet m_SenderData;

    protected void Explosion() {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_SenderData.BaseSenderInfo.CircleSenderInfo.Radius);
        foreach (Collider2D collider in colliders)
        {
            ApplyExplosionForce(collider);
        }

        PlatformActionManager.Instance.CreateImpactSender(m_SenderData, this.transform);
        FMODUnity.RuntimeManager.PlayOneShot(m_ExplosionSound, transform.position);
        Destroy(this.gameObject);
    }

    void ApplyExplosionForce(Collider2D targetCollider)
    {
        Rigidbody2D targetRigidbody = targetCollider.GetComponent<Rigidbody2D>();
        float explosionRadius = m_SenderData.BaseSenderInfo.CircleSenderInfo.Radius;
        float explosionForce = m_SenderData.AttackInfo.ImpactValue;
        if (targetRigidbody != null)
        {
            // îöêSÇ©ÇÁÇÃãóó£Ç…âûÇ∂ÇƒóÕÇåvéZ
            Vector2 explosionDirection = targetCollider.transform.position - transform.position;
            float distance = explosionDirection.magnitude;
            float normalizedDistance = distance / explosionRadius;
            float force = Mathf.Lerp(explosionForce, 0f, normalizedDistance);

            // óÕÇâ¡Ç¶ÇÈ
            targetRigidbody.AddForce(explosionDirection.normalized * force, ForceMode2D.Impulse);
        }
    }

    protected override void HitCollisionTarget()
    {

        BehaviorImpactReceiver receiver = m_Hit.collider.gameObject.GetComponent<BehaviorImpactReceiver>();
        if (receiver == null) return;

        m_AttackInfo.Direction = m_Rigidbody2D.velocity.normalized;
        m_AttackInfo.ImpactValue = 0;
        m_AttackInfo.DamageValue = 0;

        if (receiver.ReceiveImpactGetReply(ref m_AttackInfo, m_Hit, ref m_ReplyInfo))
        {
            if (m_State == BallState.Throwed) Explosion();
        };
    }

    override protected void HitWall(Collision2D collision)
    {
        if (m_State == BallState.Throwed) Explosion();
    }

    override protected void HitGround(Collision2D collision)
    {
        if (m_State == BallState.Throwed) Explosion();
    }

}
