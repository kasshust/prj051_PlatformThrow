using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedBlueGames.Tools;

public class TestBall : CatchableBall
{
    [SerializeField] Rigidbody2D m_Rigidbody2D;
    
    public float m_CircleRadius;
    public LayerMask m_CharMask;

    RaycastHit2D m_Hit;


    private void Update()
    {
        if (m_State == BallState.Bound || m_State == BallState.Throwed)
        {
            RaycastHit2D m_Hit = CollisionCheck();
            if (m_Hit)
            {
                BehaviorImpactReceiver receiver = m_Hit.collider.gameObject.GetComponent<BehaviorImpactReceiver>();
                if (receiver == null) return;
                
                m_AttackInfo.Direction =   m_Rigidbody2D.velocity.normalized;
                m_AttackInfo.ImpactValue = m_Rigidbody2D.velocity.magnitude;
                m_AttackInfo.DamageValue = 0;

                if (receiver.ReceiveImpactGetReply(ref m_AttackInfo, m_Hit, ref m_ReplyInfo)) {
                    m_Rigidbody2D.velocity *= -1;
                };
            }
        }
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
        transform.position = m_Parent.GetHandPosition() ;
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.angularVelocity = 0.0f;
        m_Rigidbody2D.Sleep();
    }

    override public void Catched(ICatcher Parent)
    {
        m_Parent = Parent;
        m_State  = BallState.Carried;
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
        Hit(collision.contacts[0].normal);
    }

    private void Hit(Vector2 normal)
    {
        float angle = Vector2.SignedAngle(Vector2.up, normal.normalized);

        if (Mathf.Abs(angle) < 20.0f)   HitGround();
        else                            HitWall();
        
    }

    private void HitWall() {
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

    private void HitGround() {
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

    private void LevelUp() {
        m_Level++;
    }

    private void LevelReset() {
        m_Level = 0;
    }

}
