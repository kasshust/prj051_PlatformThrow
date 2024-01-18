using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : CatchableBall
{
    [SerializeField] Rigidbody2D m_Rigidbody2D;

    public override bool IsCatchable()
    {
        if (m_State == BallState.Throwed || m_State == BallState.Carried) return false;
        return true;
    }

    override public void Carried()
    {
        transform.position = m_Parent.transform.position;
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.angularVelocity = 0.0f;
        m_Rigidbody2D.Sleep();
    }

    override public void Catched(GameObject Parent)
    {
        m_Parent = Parent;
        m_State  = BallState.Carried;
    }

    override public void Throwed(ref ThrowProperty throwProperty)
    {
        GetComponent<Rigidbody2D>().velocity = throwProperty.Velocity;
        m_State = BallState.Throwed;
        m_Parent = null;
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

        Debug.Log("•ÇÕ“Ë");

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

        Debug.Log("’n–ÊÕ“Ë");

        switch (m_State)
        {
            case BallState.Throwed:
                LevelUp();
                m_State = BallState.Bound;
                break;
            
            case BallState.Bound:
                LevelReset();
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
