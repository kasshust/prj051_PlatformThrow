using UnityEngine;
using System.Collections;
using RedBlueGames.Tools;

public class EnemyController2D : Controller2D
{
    public LayerMask m_CharMask;
    public float    m_CircleRadius;

    public override void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false, float motionSpeed = 1.0f)
    {
        UpdateRaycastOrigins();

        collisions.Reset();
        collisions.moveAmountOld = moveAmount;
        playerInput = input;

        CircleCharCollision(ref moveAmount);

        if (moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount);
        }

        if (moveAmount.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
        }

        HorizontalCollisions(ref moveAmount);
        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount, motionSpeed);
        }

        transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    Vector2 m_HitBodyWithDir;
    Vector2 m_HitBodyBound;
    Vector2 m_HitBodyCenterToHitPoint;
    Vector2 m_Reverse;
    CircleCollider2D m_CircleCollider2D;
    protected void CircleCharCollision(ref Vector2 moveAmount)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, m_CircleRadius, Vector2.zero, Mathf.Infinity, m_CharMask);
        

        if (hit)
        {
            m_CircleCollider2D = hit.collider as CircleCollider2D;
            if (m_CircleCollider2D == null) return;

            m_HitBodyWithDir    = (transform.position - hit.transform.position);
            m_HitBodyBound      = m_HitBodyWithDir.normalized * m_CircleCollider2D.radius;
            m_HitBodyCenterToHitPoint = (hit.point - (Vector2)hit.transform.position);

            // Debug.DrawRay(hit.point, m_HitBodyBound, Color.blue, 0.0f);

            // DebugUtility.DrawCircle(hit.point, 0.01f, Color.blue, 10);
            // DebugUtility.DrawCircle(hit.transform.position, 0.01f, Color.green, 10);

            m_Reverse = m_HitBodyBound;
            // Debug.DrawRay(hit.point, m_Reverse, Color.yellow, 0.0f);

            /*
            float Xpower = 1.0f - Mathf.Abs(Vector2.Dot(m_Reverse, Vector2.up));
            float Xsum = Mathf.Sign(m_Reverse.x) * m_Reverse.magnitude * Xpower;
            m_Reverse.x += Xsum;
            Debug.DrawRay(hit.point, m_Reverse, Color.cyan, 0.0f);
            */

            moveAmount.x = (m_Reverse.x)/2.0f;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // DebugUtility.DrawCircle(transform.position, m_CircleRadius, Color.blue, 6);
    }
#endif
}
