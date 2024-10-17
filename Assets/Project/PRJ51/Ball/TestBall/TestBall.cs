using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedBlueGames.Tools;

public class TestBall : NormalBall
{
    [SerializeField] private FMODUnity.EventReference m_HitSound;
    [SerializeField] private FMODUnity.EventReference m_HitSound2;

    [SerializeField] private TrailRenderer m_TrailRenderer;

    protected override void HitCollisionTarget()
    {
        BehaviorImpactReceiver receiver = m_Hit.collider.gameObject.GetComponent<BehaviorImpactReceiver>();
        if (receiver == null) return;

        m_AttackInfo.Direction = m_Rigidbody2D.velocity.normalized;
        m_AttackInfo.ImpactValue = m_Rigidbody2D.velocity.magnitude;
        m_AttackInfo.DamageValue = 1;

        if (receiver.ReceiveImpactGetReply(ref m_AttackInfo, m_Hit, ref m_ReplyInfo))
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_HitSound2, transform.position);
            m_Rigidbody2D.velocity *= -1;
        };
    }

    override protected void HitWall(Collision2D collision)
    {
        // Test
        // PlatformActionManager.Instance.CreateImpactSender(m_SenderData, this.transform);
        // FMODUnity.RuntimeManager.PlayOneShot(m_HitSound, transform.position);

        switch (m_State)
        {
            case BallState.Throwed:
                LevelUp();
                m_State = BallState.Bound;
                break;
            default:
                break;
        }
    }

    override protected void HitGround(Collision2D collision)
    {
        switch (m_State)
        {
            case BallState.Throwed:
                LevelUp();
                m_State = BallState.Bound;
                break;

            case BallState.Bound:
                LevelReset();
                m_AttackInfo.AttackSet = PlatformActionManager.AttackSet.All;
                m_State = BallState.Default;
                break;
            default:
                break;
        }
    }


    private void LevelUp()
    {
        m_Level++;
    }

    private void LevelReset()
    {
        m_Level = 0;
    }

    protected override void Step()
    {
        if (m_State == BallState.Default) m_TrailRenderer.enabled = false;
        else m_TrailRenderer.enabled = true;
    }
}
