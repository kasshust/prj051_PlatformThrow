using UnityEngine;
using System.Collections;
using RedBlueGames.Tools;

public class PlayerController2D : Controller2D
{
    public LayerMask m_CharMask;
    public float m_CircleRadius;
    public float m_EnemyStepAddOffset;
    [SerializeField,ReadOnly] public RaycastHit2D m_SubCollisionWithEnemy;

    public override void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false, float motionSpeed = 1.0f)
    {
        UpdateRaycastOrigins();

        collisions.Reset();
        collisions.moveAmountOld = moveAmount;
        playerInput = input;

        CircleCharCollision(ref moveAmount);
        m_SubCollisionWithEnemy = EnemyCollisionCheck(ref moveAmount);

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
        // DebugUtility.DrawCircle(transform.position, m_CircleRadius, Color.red, 10);

        if (!hit) return;

        /*

        if (hit.collider.gameObject.CompareTag("Boss"))
        {
            m_CircleCollider2D = hit.collider as CircleCollider2D;
            if (m_CircleCollider2D == null) return;

            m_HitBodyWithDir    = (transform.position - hit.transform.position);
            m_HitBodyBound      = m_HitBodyWithDir.normalized * m_CircleCollider2D.radius;
            m_HitBodyCenterToHitPoint = (hit.point - (Vector2)hit.transform.position);

            Debug.DrawRay(hit.point, m_HitBodyBound, Color.blue, 0.0f);
            // DebugUtility.DrawCircle(hit.point, 0.01f, Color.blue, 10);
            // DebugUtility.DrawCircle(hit.transform.position, 0.01f, Color.green, 10);

            m_Reverse = m_HitBodyBound - m_HitBodyCenterToHitPoint;
            // Debug.DrawRay(hit.point, m_Reverse, Color.yellow, 0.0f);

            float Xpower = 1.0f - Mathf.Abs(Vector2.Dot(m_Reverse, Vector2.up));
            float Xsum = Mathf.Sign(m_Reverse.x) * m_Reverse.magnitude * Xpower;
            m_Reverse.x += Xsum;
            // Debug.DrawRay(hit.point, m_Reverse, Color.cyan, 0.0f);

            moveAmount = (m_Reverse);
        }

        */
    }

    protected RaycastHit2D EnemyCollisionCheck(ref Vector2 moveAmount) {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, m_CircleRadius + m_EnemyStepAddOffset, Vector2.zero, Mathf.Infinity, m_CharMask);
        // DebugUtility.DrawCircle(transform.position, m_CircleRadius + m_EnemyStepAddOffset, Color.red, 10);

        return hit;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // DebugUtility.DrawCircle(transform.position, m_CircleRadius, Color.blue, 6);
    }
#endif
}
